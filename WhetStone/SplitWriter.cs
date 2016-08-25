using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
}