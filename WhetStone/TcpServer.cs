using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using WhetStone.Factory;

namespace WhetStone.Ports
{
    public class TcpServer : IPortBound, ICreator<IConnection>
    {
        private readonly Socket _sock;
        public int Backlog { get; }
        public TcpServer()
        {
            _sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Backlog = 10;
        }
        public EndPoint source
        {
            get
            {
                return _sock.LocalEndPoint;
            }
            set
            {
                _sock.Bind(value);
            }
        }
        public IConnection Create()
        {
            _sock.Listen(Backlog);
            return new PrivateTcpServerConnection(_sock.Accept());
        }
        ~TcpServer()
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
        private class PrivateTcpServerConnection : IConnection
        {
            public ISet<Type> enabledAutoCommands { get; }
            private readonly Socket _sock;
            public PrivateTcpServerConnection(Socket sock)
            {
                this.enabledAutoCommands = new HashSet<Type>();
                this._sock = sock;
            }
            public EndPoint source
            {
                get
                {
                    return _sock.LocalEndPoint;
                }
                set
                {
                    throw new Exception("cannot change source of dedicated TCP connection");
                }
            }
            public EndPoint target
            {
                get
                {
                    return _sock.RemoteEndPoint;
                }
                set
                {
                    throw new Exception("cannot change target of dedicated TCP connection");
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
            ~PrivateTcpServerConnection()
            {
                this.Dispose();
            }
            public void Dispose()
            {
                _sock.Dispose();
            }
        }
    }
}
