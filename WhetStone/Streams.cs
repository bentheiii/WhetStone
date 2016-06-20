using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WhetStone.Looping;

namespace WhetStone.Streams
{
	public class SplitWriter : IDisposable
	{
		private readonly IDictionary<TextWriter,bool> _subscribers = new Dictionary<TextWriter, bool>();
		public bool AddWriter(TextWriter w,bool manage = false)
		{
			if (_subscribers.ContainsKey(w))
				return false;
			 _subscribers.Add(w,manage);
			return true;
		}
		public bool RemoveWriter(TextWriter w,bool disposeIfManaged = true)
		{
		    if (!_subscribers.ContainsKey(w))
		        return false;
		    bool dispose = disposeIfManaged && _subscribers[w];
		    var ret= _subscribers.Remove(w);
            if (dispose)
                w.Dispose();
		    return ret;
		}
		public void Write(object x)
		{
			Write(x.ToString());
		}
		public void Write(string x)
		{
			foreach (var subscriber in _subscribers)
			{
				subscriber.Key.Write(x);
			}
		}
		public void WriteLine(string x)
		{
			this.Write(x+Environment.NewLine);
		}
		public void WriteLine(object x)
		{
			this.WriteLine(x.ToString());
		}
		private bool _disposed = false;
		protected virtual void Dispose(bool disposing)
		{
			if (!this._disposed)
			{
				if (disposing)
				{
					foreach (var subscriber in _subscribers.Where(a=>a.Value))
					{
						subscriber.Key.Dispose();
					}
				}

				this._disposed = true;
			}
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		~SplitWriter()
		{
			this.Dispose(false);
		}
	}
	public static class StreamExtentions
	{
		public static IEnumerable<string> Loop(this TextReader @this)
		{
		    return Loops.Generate(@this.ReadLine).TakeWhile(a => a != null);
		}
	    public static byte[] ReadAll(this Stream @this, int initialchunksize = 256)
	    {
	        if (!@this.CanRead)
                throw new ArgumentException("stream is unreadable");
            byte[] buffer = new byte[initialchunksize/2];
	        int written = 0;
	        int r = int.MaxValue;
	        while (r > 0)
	        {
                Array.Resize(ref buffer, buffer.Length*2);
	            r = @this.Read(buffer, written, buffer.Length-written);
	            written += r;
	        }
            Array.Resize(ref buffer,written);
	        return buffer;
	    }
	    public static IEnumerable<byte> Loop(this Stream @this)
	    {
            if (!@this.CanRead)
                throw new ArgumentException("stream is unreadable");
	        return Loops.Generate(@this.ReadByte).TakeWhile(a => a > 0).Select(a=>(byte)a);

	    }
	}
}
