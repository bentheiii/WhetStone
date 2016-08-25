using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace WhetStone.Ports
{
    public class UdpConnection : IConnection
    {
        private readonly Socket _sock;
        public EndPoint target { get; set; }
        public ISet<Type> enabledAutoCommands { get; }
        public UdpConnection()
        {
            this.enabledAutoCommands = new HashSet<Type>();
            this.target = null;
            _sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
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
        public int SendBytes(byte[] o)
        {
            return _sock.SendTo(o, target);
        }
        public byte[] RecieveBytes(out EndPoint from, int bufferSize)
        {
            from = new IPEndPoint(0, 0);
            byte[] buffer = new byte[bufferSize];
            int l = _sock.ReceiveFrom(buffer, ref from);
            Array.Resize(ref buffer, l);
            return buffer;
        }
        ~UdpConnection()
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
