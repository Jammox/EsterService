using System;
using System.Threading;

namespace EsterService.Scheduling
{
	/// <summary>
	/// Manager for schedulable timed threads.
	/// </summary>
	public interface IThreadScheduler
	{
		void LoadSchedule(Action test, AutoResetEvent autoEvent, TimeSpan delay, TimeSpan interval, bool active);
		void LoadSchedule(Action test, TimeSpan delay, TimeSpan interval, bool active);
		void StartAll();
		void StopAll();
		void PauseAll();
		void ContinueAll();
	}
}
