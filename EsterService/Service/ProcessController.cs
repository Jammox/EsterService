using System.Diagnostics;

namespace EsterService.Service
{
	/// <summary>
	/// Background process controller.
	/// </summary>
	public class ProcessController : IProcessController
	{
		private Process _proc;

		public bool CreateNoWindow { get; set; }
		public bool UseShellExecute { get; set; }

		/// <summary>
		/// Start process.
		/// </summary>
		/// <param name="procName">Process name.</param>
		/// <param name="procArgs">Process arguments.</param>
		public void Start(string procName, string procArgs)
		{
			try
			{
				_proc = new Process
				{
					StartInfo = new ProcessStartInfo(procName)
					{
						CreateNoWindow = CreateNoWindow,
						UseShellExecute = UseShellExecute,
						Arguments = procArgs ?? ""
					}
				};

				_proc?.Start();
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Start process.
		/// </summary>
		/// <param name="procName">Process name.</param>
		public void Start(string procName)
		{
			Start(procName, null);
		}

		/// <summary>
		/// Stop process.
		/// </summary>
		public void Stop()
		{
			try
			{
				_proc?.Kill();
			}
			catch
			{
				throw;
			}
		}
	}
}
