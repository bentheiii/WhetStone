using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WhetStone.Ports
{
    public class RecieverThread : IDisposable
    {
        public class MessageRecieveEventArgs : EventArgs
        {
            public MessageRecieveEventArgs(object message, EndPoint source)
            {
                this.message = message;
                this.source = source;
            }
            public object message { get; }
            public EndPoint source { get; }
        }
        public delegate void MessageRecieveHandler(object sender, MessageRecieveEventArgs e);
        public RecieverThread(IConnection conn, ThreadPriority p = ThreadPriority.Normal)
        {
            this._thread = new Thread(this.threadmain);
            _thread.Priority = p;
            _thread.IsBackground = true;
            this.conn = conn;
        }
        public IConnection conn { get; }
        public event MessageRecieveHandler onRecieve;
        private readonly Thread _thread;
        public void start()
        {
            _thread.Start();
        }
        private void stop()
        {
            if (_thread.IsAlive)
                _thread.Abort();
        }
        public void threadmain()
        {
            while (true)
            {
                EndPoint p;
                object val = conn.Recieve(out p);
                this.onRecieve?.Invoke(this, new MessageRecieveEventArgs(val, p));
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                stop();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
