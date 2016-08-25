using System;
using System.Collections.Generic;
using System.Threading;
using WhetStone.Factory;

namespace WhetStone.Ports
{
    public class ConnectionCreatorThread : IDisposable
    {
        public class NewConnectionEventArgs : EventArgs
        {
            public NewConnectionEventArgs(IConnection connection, int instanceIndex)
            {
                this.connection = connection;
                this.instanceIndex = instanceIndex;
            }
            public IConnection connection { get; }
            public int instanceIndex { get; }
        }
        public delegate void NewConnectionHandler(object sender, NewConnectionEventArgs e);
        private int _instancecount = 1;
        public ConnectionCreatorThread(ICreator<IConnection> creator, ThreadPriority mainpriority = ThreadPriority.Normal)
        {
            this._mainthread = new Thread(this.threadmain);
            _mainthread.Priority = mainpriority;
            _mainthread.IsBackground = true;
            this.creator = creator;
        }
        public ICreator<IConnection> creator { get; }
        public event NewConnectionHandler onCreate;
        private readonly Thread _mainthread;
        private readonly List<IDisposable> _dependants = new List<IDisposable>();
        public void AddDependant(IDisposable d)
        {
            _dependants.Add(d);
        }
        public void start()
        {
            _mainthread.Start();
        }
        public void stop()
        {
            if (_mainthread.IsAlive)
                _mainthread.Abort();
        }
        private void threadmain()
        {
            while (true)
            {
                IConnection conn = creator.Create();
                onCreate.Invoke(this, new NewConnectionEventArgs(conn, _instancecount++));
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            stop();
            if (disposing)
            {
                foreach (IDisposable iDisposable in _dependants)
                {
                    iDisposable.Dispose();
                }
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
