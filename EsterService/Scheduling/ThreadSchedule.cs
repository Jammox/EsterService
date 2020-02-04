using System;
using System.Threading;

namespace EsterService.Scheduling
{
	/// <summary>
	/// Schedulable timed thread.
	/// </summary>
	public class ThreadSchedule
		: IThreadSchedule, IDisposable
	{
		#region Fields

		private Timer _timer;
		private WaitHandle _waitHandle = new AutoResetEvent(false);
		private AutoResetEvent _autoEvent;
		private readonly TimerCallback _callBack;

		#endregion

		#region Constructors

		public ThreadSchedule(TimerCallback callBack, AutoResetEvent autoEvent, TimeSpan delay, TimeSpan interval, bool active)
		{
			_callBack = callBack;
			_autoEvent = autoEvent;
			Delay = delay;
			Interval = interval;
			Active = active;
		}

		public ThreadSchedule(TimerCallback callBack, TimeSpan delay, TimeSpan interval, bool active)
			: this(callBack, null, delay, interval, active) { }

		public ThreadSchedule(TimerCallback callBack, TimeSpan delay, TimeSpan interval)
			: this(callBack, delay, interval, true) { }

		public ThreadSchedule(TimerCallback callBack, TimeSpan interval)
			: this(callBack, new TimeSpan(0), interval) { }

		#endregion

		#region Properties

		/// <summary>
		/// TimeSpan value defining the time to delay before start.
		/// </summary>
		public TimeSpan Delay { get; set; }

		/// <summary>
		/// TimeSpan value defining the interval period between calls.
		/// </summary>
		public TimeSpan Interval { get; set; }

		/// <summary>
		/// Boolean value denoting if schedule is active.
		/// </summary>
		public bool Active { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Start scheduled thread.
		/// </summary>
		public virtual void Start()
		{
			if (Active)
				_timer = new Timer(_callBack, _autoEvent, Delay, Interval);
		}

		/// <summary>
		/// Stop scheduled thread.
		/// </summary>
		public virtual void Stop()
		{
			if (Active)
			{
				if (!_timer.Dispose(_waitHandle))
					throw new Exception("Timer already disposed.");

				_waitHandle.WaitOne();
			}
		}

		/// <summary>
		/// Pause scheduled thread.
		/// </summary>
		public virtual void Pause()
		{
			if (Active)
				_timer.Change(-1, -1);
		}

		/// <summary>
		/// Continue (paused) scheduled thread.
		/// </summary>
		public virtual void Continue()
		{
			if (Active)
				_timer.Change(Delay, Interval);
		}

		#endregion

		#region Implement IDisposable

		private bool _disposed = false;

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			if (disposing)
			{
				_timer.Dispose();
				_autoEvent.Dispose();
				_waitHandle.Dispose();
			}

			_disposed = true;
		}

		~ThreadSchedule()
		{
			Dispose(false);
		}

		#endregion

	}
}
