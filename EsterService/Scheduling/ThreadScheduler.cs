using System;
using System.Collections.Generic;
using System.Threading;

namespace EsterService.Scheduling
{
	/// <summary>
	/// Manager for schedulable timed threads.
	/// </summary>
	/// <example>
	/// <code>
	///	IThreadScheduler ts = new ThreadScheduler();
	///	ts.LoadSchedule(() =>
	///		{
	///			Console.WriteLine("This is schedule 1");
	///		},
	///		new TimeSpan(0, 0, 5),
	///		new TimeSpan(0, 0, 10),
	///		true);
	///	ts.StartAll();
	/// </code>
	/// </example>
	public class ThreadScheduler : IThreadScheduler
	{
		#region Fields

		private List<IThreadSchedule> _threadSchedules = new List<IThreadSchedule>();

		// track locking objects to prevent a thread trampling itself.
		private Dictionary<string, object> _updateLocks = new Dictionary<string, object>();

		#endregion

		public ThreadScheduler() { }

		#region Methods

		/// <summary>
		/// Load a scheduled thread into the schedule collection.
		/// </summary>
		public void LoadSchedule(Action action, AutoResetEvent autoEvent, TimeSpan delay, TimeSpan interval, bool active)
		{
			if (action == null)
				throw new ArgumentException(nameof(action));

			if (delay == null)
				throw new ArgumentException(nameof(delay));

			if (interval == null)
				throw new ArgumentException(nameof(interval));

			string id = GetKey(16);
			_updateLocks.Add(id, new object());

			_threadSchedules.Add(new ThreadSchedule((obj) =>
				{
					if (Monitor.TryEnter(_updateLocks[id]))
					{
						try
						{
							action();
						}
						finally
						{
							Monitor.Exit(_updateLocks[id]);
						}
					}
				},
				autoEvent,
				delay,
				interval,
				active));
		}

		/// <summary>
		/// Load a scheduled thread into the schedule collection.
		/// </summary>
		public void LoadSchedule(Action action, TimeSpan delay, TimeSpan interval, bool active)
		{
			LoadSchedule(action, null, delay, interval, active);
		}

		private static string GetKey(int size)
		{
			return Guid.NewGuid().ToString("n").Substring(0, size);
		}

		#endregion

		#region Collection control

		/// <summary>
		/// Start all scheduled threads.
		/// </summary>
		public void StartAll()
		{
			foreach (var ts in _threadSchedules)
			{
				ts.Start();
			}
		}

		/// <summary>
		/// Stop all scheduled threads.
		/// </summary>
		public void StopAll()
		{
			foreach (var ts in _threadSchedules)
			{
				ts.Stop();
			}
		}

		/// <summary>
		/// Pause all scheduled threads.
		/// </summary>
		public void PauseAll()
		{
			foreach (var ts in _threadSchedules)
			{
				ts.Pause();
			}
		}

		/// <summary>
		/// Continue all scheduled threads.
		/// </summary>
		public void ContinueAll()
		{
			foreach (var ts in _threadSchedules)
			{
				ts.Continue();
			}
		}

		#endregion

	}
}
