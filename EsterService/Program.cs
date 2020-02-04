using EsterService.Configuration;
using System;
using System.IO;
using Topshelf;
using Topshelf.Common.Logging;
using Topshelf.Ninject;

namespace EsterService
{
	/// <summary>
	/// *********** EsterService ***********
	/// * Copyright © 2019 James A.Fitches *
	/// ************************************
	/// </summary>
	class Program
	{
		static void Main(string[] args)
		{
			const string ServiceName = "EsterService";
			const string DisplayName = "Ester Windows Service";
			const string Description = "Run Ester/BMD processes at scheduled intervals.";

			Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

			HostFactory.Run(x =>
			{
				x.UseCommonLogging();

				x.UseNinject(
					new Ninject.Modules.INinjectModule[]
					{
						new IocModule(),
						new SettingsModule()
					});

				x.Service<Service.WinService>(sc =>
				{
					sc.ConstructUsingNinject();
					sc.WhenStarted((s, hostControl) => s.Start(hostControl));
					sc.WhenStopped((s, hostControl) => s.Stop(hostControl));
					sc.WhenPaused((s, hostControl) => s.Pause(hostControl));
					sc.WhenContinued((s, hostControl) => s.Continue(hostControl));
					sc.WhenShutdown((s, hostControl) => s.Shutdown(hostControl));
				});

				x.RunAsLocalSystem();
				x.EnablePauseAndContinue();
				x.EnableShutdown();
				x.SetDescription(Description);
				x.SetDisplayName(DisplayName);
				x.SetServiceName(ServiceName);
			});
		}
	}
}
