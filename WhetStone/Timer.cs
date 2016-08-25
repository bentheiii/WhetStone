using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using WhetStone.Units.Frequencies;
using WhetStone.WordPlay;

namespace WhetStone.Timer
{
	public interface ITimer
	{
		bool paused { get; }
		void Pause();
		void Resume();
		TimeSpan timeSinceStart { get; }
	}
	public static class TimerExtentions
	{
		public static void togglePause(this ITimer @this)
		{
			if (@this.paused)
			{
				@this.Resume();
			}
			else
			{
				@this.Pause();
			}
		}
	}
	public class IdleTimer : ITimer
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
	public sealed class ActiveTimer : ITimer, IDisposable
	{
		public delegate void TimerTickHandler(object sender, EventArgs e);
		private readonly IdleTimer _idle = new IdleTimer();
		public TimeSpan tickLength { get; }
		public int ticks
		{
			get
			{
				return this._ticks;
			}
			private set
			{
				this._ticks = value;
			}
		}
		private readonly Thread _timerThread = new Thread(TimerThreadMain);
		private volatile int _ticks = 0;
 		public event TimerTickHandler onTick;
	    public ActiveTimer(Frequency f) : this(1.0/f) { }
	    public ActiveTimer(TimeSpan tickLength)
		{
			this.tickLength = tickLength;
			_timerThread.IsBackground = true;
		}
		private static void TimerThreadMain(object t)
		{
			ActiveTimer @this = t as ActiveTimer;
			while (true)
			{
			    try
			    {
			        Thread.Sleep(@this.paused ? Timeout.InfiniteTimeSpan : @this.tickLength);
			    }
			    catch (ThreadInterruptedException) { }
			    catch (ThreadAbortException)
			    {
			        break;
                }
                catch (Exception ex)
				{
					throw new Exception("sleep failed", ex);
				}
			    @this.Tick();
			}
		}
	    public void Tick()
	    {
	        this.ticks++;
	        this.onTick?.Invoke(this, EventArgs.Empty);
	    }
	    public void Start() => this._timerThread.Start(this);
		public bool paused { get; private set; } = false;
		public void Pause()
		{
			if (paused)
				throw new Exception("Timer already paused");
			this.paused = true;
		}
		public void Resume()
		{
			if (!paused)
				throw new Exception("Timer already resumed");
			paused = false;
			_timerThread.Interrupt();
		}
		public TimeSpan timeSinceStart => this._idle.timeSinceStart;
	    private bool _disposed = false;
        public void Dispose(bool disposed)
        {
            if (disposed && !this._disposed)
            {
                this._disposed = true;
                this._timerThread.Abort();
            }
        }
	    public void Dispose()
	    {
            if (_disposed)
	            Dispose(true);
	    }
	}
	public class ReaderTimer : TextReader, ITimer
	{
		private bool _disposed = false;
		private readonly Func<ITimer, string> _writeCalc;
		private readonly ActiveTimer _active;
		private volatile StringBuilder _buffer = new StringBuilder();
		private readonly ISet<Thread> _readerthreads = new HashSet<Thread>();
	    public ReaderTimer(Func<ITimer, string> writeCalc, Frequency f) : this(writeCalc, 1/f) { }
	    public ReaderTimer(Func<ITimer, string> writeCalc, TimeSpan ticklength)
		{
			this._writeCalc = writeCalc;
			_active = new ActiveTimer(ticklength);
			_active.onTick += _active_onTick;
			_active.Start();
		}
		private void WaitForNonNullBuffer()
		{
			if (this._disposed)
				throw new ObjectDisposedException("Timer Already Closed");
			if (_buffer.Length == 0)
			{
				_readerthreads.Add(Thread.CurrentThread);
				try
				{
					Thread.Sleep(Timeout.InfiniteTimeSpan);
				}
				catch (Exception ex)
				{
					if(!(ex is ThreadInterruptedException))
						throw new Exception("sleep threw and exception",ex);
				}
				_readerthreads.Remove(Thread.CurrentThread);
			}
		}
		public override int Peek()
		{
			this.WaitForNonNullBuffer();
			return _buffer[0];
		}
		public override int Read()
		{
			var ret= this.Peek();
			_buffer.Remove(0,1);
			return ret;
		}
		public override int Read(char[] buffer, int index, int count)
		{
			this.WaitForNonNullBuffer();
			int ret = _buffer.Length > count ? count : _buffer.Length;
			_buffer.CopyTo(0,buffer,index,ret);
			_buffer.Remove(0, ret);
			return ret;
		}
		public override string ReadLine()
		{
			this.WaitForNonNullBuffer();
			string s = this._buffer.ToString();
			int index = s.IndexOf(Environment.NewLine);
			char[] ret = new char[index];
			s.CopyTo(0,ret,0,index + Environment.NewLine.Length);
			_buffer.Remove(0, index+Environment.NewLine.Length);
			return ret.ConvertToString();
		}
		public override int ReadBlock(char[] buffer, int index, int count)
		{
			return Read(buffer, index, count);
		}
		public override string ReadToEnd()
		{
			this.WaitForNonNullBuffer();
			string ret = _buffer.ToString();
			_buffer.Clear();
			return ret;
		}
		public override void Close()
		{
			this.Dispose(true);
		}
		private void _active_onTick(object sender, EventArgs e)
		{
			this._buffer.Append(_writeCalc((ITimer)sender));
			_buffer.AppendLine();
			foreach (Thread thread in this._readerthreads)
			{
				thread.Interrupt();
			}
		}
		public bool paused => this._active.paused;
		public TimeSpan timeSinceStart => this._active.timeSinceStart;
		public void Pause()
		{
			_active.Pause();
		}
		public void Resume()
		{
			_active.Resume();
		}
		protected override void Dispose(bool disposing)
		{
			this._disposed = true;
			_active.Dispose();
			base.Dispose(disposing);
		}
	}
    public class WriterTimer : ITimer
    {
        private readonly ActiveTimer _inner;
        private readonly Action<string> _writeAction;
        public WriterTimer(Func<string> towrite, TimeSpan span, TextWriter writer = null, bool insertnewline = true)
        {
            writer = writer ?? Console.Out;
            _inner = new ActiveTimer(span);
            if (insertnewline)
                _writeAction = x => writer.WriteLine(x);
            else
                _writeAction = x => writer.Write(x);
            _inner.onTick += (sender, args) => _writeAction(towrite());
        }
        public bool paused
        {
            get
            {
                return _inner.paused;
            }
        }
        public void Pause()
        {
            _inner.Pause();
        }
        public void Resume()
        {
            _inner.Resume();
        }
        public TimeSpan timeSinceStart
        {
            get
            {
                return _inner.timeSinceStart;
            }
        }
    }
}
