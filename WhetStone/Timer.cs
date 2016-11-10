using System;

namespace WhetStone.Timer
{
	public class IdleTimer
	{
		private DateTime _startTime = DateTime.Now;
		private IdleTimer _timePaused;
		public IdleTimer(bool startpaused = false)
		{
			if (startpaused)
				this.Pause();
		}
		public bool paused => this._timePaused != null;
		public void Pause()
		{
			if (paused)
				throw new Exception("Timer already paused");
			_timePaused = new IdleTimer();
		}
		public void Resume()
		{
			if (!paused)
				throw new Exception("Timer already resumed");
			_startTime = _startTime.Add(_timePaused.timeSinceStart);
			_timePaused = null;
		}
		public void Reset()
		{
			_startTime = DateTime.Now;
			_timePaused = null;
		}
		public TimeSpan timeSinceStart
		{
			get
			{
                var ret =  DateTime.Now.Subtract(_startTime);
			    if (paused)
			        ret = ret.Subtract(_timePaused.timeSinceStart);
			    return ret;
			}
		}
	}
}
