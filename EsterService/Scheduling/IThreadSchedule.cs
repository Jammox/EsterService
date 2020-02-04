using System;

namespace EsterService.Scheduling
{
	/// <summary>
	/// Schedulable timed thread.
	/// </summary>
	public interface IThreadSchedule
	{
		TimeSpan Delay { get; set; }
		TimeSpan Interval { get; set; }
		bool Active { get; set; }
		void Start();
		void Stop();
		void Pause();
		void Continue();
	}
}
