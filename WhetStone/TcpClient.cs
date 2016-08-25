using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace WhetStone.Ports
{
    public class TcpClient : IConnection
    {
        public ISet<Type> enabledAutoCommands { get; }
        private readonly Socket _sock;
        private EndPoint _target;
        public TcpClient()
        {
            this.enabledAutoCommands = new HashSet<Type>();
            _sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _target = null;
        }
        public EndPoint target
        {
            get
            {
                return _target;
            }
            set
            {
                if (target == null)
                {
                    _sock.Connect(value);
                    _target = value;
                }
                else
                    throw new Exception("the socket is connected and cannot be re-targeted");
            }
        }
        public EndPoint source
        {
            get
            {
                return _sock.LocalEndPoint;
            }
            set
            {
                if (target == null)
                    _sock.Bind(value);
                else
                    throw new Exception("the socket is connected and cannot be rebound");
            }
        }
        public int SendBytes(byte[] o)
        {
            return _sock.Send(o);
        }
        public byte[] RecieveBytes(out EndPoint from, int bufferSize)
        {
            from = new IPEndPoint(0, 0);
            byte[] buffer = new byte[bufferSize];
            int l = _sock.Receive(buffer);
            Array.Resize(ref buffer, l);
            return buffer;
        }
        ~TcpClient()
        {
            this.Dispose(false);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                _sock.Dispose();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
