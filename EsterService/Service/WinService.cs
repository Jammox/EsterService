using Common.Logging;
using EsterService.Configuration;
using EsterService.Scheduling;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Topshelf;

namespace EsterService.Service
{
	class WinService
	{
		private readonly IThreadScheduler _threadScheduler;
		private readonly IProcessController _processController;
		private readonly IServiceConfig _serviceConfig;
		private readonly ISettings _settings;

		public ILog Log { get; private set; }

		public WinService(IThreadScheduler threadScheduler, IProcessController processController,
			IServiceConfig serviceConfig, ISettings settings, ILog logger)
		{
			Log = logger ??
				throw new ArgumentNullException(nameof(logger));

			FileVersionInfo fvi =
				FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);

			Log.Info($"{fvi.ProductName} v{fvi.FileVersion}");

			_threadScheduler = threadScheduler ??
				throw new ArgumentNullException(nameof(threadScheduler));

			_processController = processController ??
				throw new ArgumentNullException(nameof(processController));

			_serviceConfig = serviceConfig ??
				throw new ArgumentNullException(nameof(serviceConfig));

			_settings = settings ??
				throw new ArgumentNullException(nameof(settings));
		}

		#region Control Handlers

		public bool Start(HostControl hostControl)
		{
			try
			{
				Log.Info("Start command received.");

				// load from config
				_serviceConfig.Load();

				foreach (var item in _serviceConfig.Items)
				{
					Log.Info($"Scheduling {item.Name}, Delay {item.Delay}, Interval {item.Interval}, Active {item.Active}");

					// initiate schedules
					_threadScheduler.LoadSchedule(() =>
						{
							try
							{
								Log.Info($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}: Starting process {item.Name}");

								(new ProcessController { CreateNoWindow = true, UseShellExecute = false })
									.Start(item.File, item.Options);
							}
							catch (Exception ex)
							{
								Trace.TraceError($"Process {item.Name} failed!", ex);
							}
						},
						item.Delay,
						item.Interval,
						item.Active);
				}

				// Start SHMHLA (if specified and found)
				if (File.Exists(_settings.HlaCorePath))
				{
					_processController.Start(_settings.HlaCorePath);
				}
				else
				{
					Log.Warn($"SHMHLA not found at {_settings.HlaCorePath}");
				}

				_threadScheduler.StartAll();

			}
			catch (Exception ex)
			{
				Log.Error($"Error.", ex);
			}
			return true;
		}

		public bool Stop(HostControl hostControl)
		{
			Log.Info("Stop command received.");

			try
			{
				_threadScheduler.StopAll();
				_processController.Stop();
			}
			catch (Exception ex)
			{
				Log.Error("Exception!", ex);
			}
			return true;
		}

		public bool Pause(HostControl hostControl)
		{
			Log.Info("Pause command received.");

			try
			{
				_threadScheduler.PauseAll();
			}
			catch (Exception ex)
			{
				Log.Error("Exception!", ex);
			}
			return true;
		}

		public bool Continue(HostControl hostControl)
		{
			Log.Info("Continue command received.");
			try
			{
				_threadScheduler.ContinueAll();
			}
			catch (Exception ex)
			{
				Log.Error("Exception!", ex);
			}
			return true;
		}

		public bool Shutdown(HostControl hostControl)
		{
			Log.Info("Shutdown command received.");
			return Stop(hostControl);
		}

		#endregion

	}
}
