using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using WhetStone.Arrays;
using WhetStone.Comparison;
using WhetStone.Factory;
using WhetStone.Ports.AutoCommands;
using WhetStone.Random;
using WhetStone.Serializations;

namespace WhetStone.Ports
{
    namespace AutoCommands
    {
        public interface IConnectionAutoCommand
        {
            void OnRecieve(IConnection c);
        }
        [Serializable]
        public class ConnectionSendCommand : IConnectionAutoCommand
        {
            private string _expectedreply { get; }
            public ConnectionSendCommand(string expectedreply)
            {
                this._expectedreply = expectedreply;
            }
            public void OnRecieve(IConnection c)
            {
                c.Send(_expectedreply);
            }
        }
        [Serializable]
        public class ConnectionIgnoreCommand : IConnectionAutoCommand
        {
            public void OnRecieve(IConnection c)
            {}
        }
        [Serializable]
        public class ConnectionChangeTargetCommand : IConnectionAutoCommand
        {
            public ConnectionChangeTargetCommand(EndPoint target)
            {
                this.target = target;
            }
            public EndPoint target { get; }
            public void OnRecieve(IConnection c)
            {
                c.target = target;
            }
        }
        [Serializable]
        public class ConnectionChangeSourceCommand : IConnectionAutoCommand
        {
            public ConnectionChangeSourceCommand(EndPoint source)
            {
                this.source = source;
            }
            public EndPoint source { get; }
            public void OnRecieve(IConnection c)
            {
                c.source = source;
            }
        }
        [Serializable]
        // ReSharper disable once ClassNeverInstantiated.Global
        public class ConnectionGluedAutoCommand : IConnectionAutoCommand
        {
            public IConnectionAutoCommand[] commands { get; }
            public ConnectionGluedAutoCommand(params IConnectionAutoCommand[] commands)
            {
                this.commands = commands;
            }
            public void OnRecieve(IConnection c)
            {
                foreach (var autoCommand in commands)
                {
                    autoCommand.OnRecieve(c);
                }
            }
        }
    }
    public interface IPortBound: IDisposable
    {
        EndPoint source { get; set; }
    }
    public interface IConnection: IPortBound
    {
        EndPoint target { get; set; }
        ISet<Type> enabledAutoCommands { get; }
        int SendBytes(byte[] o);
        //receive without autocommands
        byte[] RecieveBytes(out EndPoint from, int buffersize);
    }
    public static class Connextention
    {
        private const int DEFAULTBUFFERSIZE = 1024;
        
        public static bool ping(this IConnection c,  out Exception ex)
        {
            try
            {
                const string pingstring = "ping0112358";
                ConnectionSendCommand p = new ConnectionSendCommand(pingstring);
                c.Send(p);
                object reply = c.recieve();
                ex = null;
                return pingstring.Equals(reply as string);
            }
            catch(Exception e)
            {
                ex = e;
                return false;
            }
        }
        public static bool ping(this IConnection c)
        {
            Exception e;
            return c.ping(out e);
        }
        public enum SourceStyle { Private, Public, None}
        public static EndPoint LocalEndPoint(int port)
        {
            return new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
        }
        public static void setLocalTarget(this IConnection c, int port)
        {
            c.target = LocalEndPoint(port);
        }
        public static void setLocalSource(this IPortBound c, int port, SourceStyle style = SourceStyle.Private)
        {
            switch (style)
            {
                case SourceStyle.Private:
                    c.source = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
                    return;
                case SourceStyle.Public:
                    c.source = new IPEndPoint(IPAddress.Any, port);
                    return;
                default:
                    c.source = new IPEndPoint(IPAddress.None, port);
                    return;
            }
        }
        public static EndPoint EnsureSource(this IPortBound c, SourceStyle style = SourceStyle.Private)
        {
            if (c.source == null)
            {
                c.setLocalSource(0, style);
            }
            return c.source;
        }
        public static bool AcceptsAutoCommand(this IConnection @this, IConnectionAutoCommand c)
        {
            if (c != null && @this.enabledAutoCommands != null && @this.enabledAutoCommands.Contains(c.GetType()))
                return true;
            var glued = c as ConnectionGluedAutoCommand;
            return glued != null && glued.commands.All(@this.AcceptsAutoCommand);
        }
        public static object silentrecieve(this IConnection @this, out EndPoint from, int buffersize)
        {
            return Serialization.Deserialize(@this.RecieveBytes(out from,buffersize));
        }
        public static object recieve(this IConnection c)
        {
            EndPoint p;
            return c.recieve(out p);
        }
        public static T recieve<T>(this IConnection c)
        {
            EndPoint from;
            return recieve<T>(c, out from);
        }
        public static T recieve<T>(this IConnection c, out EndPoint from, int buffersize = DEFAULTBUFFERSIZE)
        {
            return (T)recieve(c, out from,buffersize);
        }
        
        public static object recieve(this IConnection c, out EndPoint from, int buffersize = DEFAULTBUFFERSIZE)
        {
            object r = c.silentrecieve(out from, buffersize);
            IConnectionAutoCommand a = r as IConnectionAutoCommand;
            if (a != null && c.enabledAutoCommands!=null && c.AcceptsAutoCommand(a))
            {
                a.OnRecieve(c);
            }
            return r;
        }
        public static int Send<T>(this IConnection @this, T message, Func<T, byte[]> converter = null)
        {
            converter = converter ?? (Serialization.Serialize);
            return @this.SendBytes(converter(message));
        }
    }
    public class UdpConnection : IConnection
    {
        private readonly Socket _sock;
        public EndPoint target { get; set; }
        public ISet<Type> enabledAutoCommands{ get; }
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
            from = new IPEndPoint(0,0);
            byte[] buffer = new byte[bufferSize];
            int l = _sock.ReceiveFrom(buffer, ref from);
            Array.Resize(ref buffer,l);
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
                Array.Resize(ref buffer,l);
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
            Array.Resize(ref buffer,l);
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
    public class RecieverThread : IDisposable
    {
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
            if(_thread.IsAlive)
                _thread.Abort();
        }
        public void threadmain()
        {
            while (true)
            {
                EndPoint p;
                object val = conn.recieve(out p);
	            this.onRecieve?.Invoke(this, new MessageRecieveEventArgs(val,p));
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
    public class SocketListenerThread : IDisposable
    {
        private int _instancecount = 1;
        public SocketListenerThread(ICreator<IConnection> creator, ThreadPriority mainpriority = ThreadPriority.Normal)
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
                onCreate.Invoke(this,new NewConnectionEventArgs(conn, _instancecount++));
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
    namespace PeerTcp
    {
        [Serializable]
        public class PeerTcpGeneratorConnectionMessage
        {
            public const int SEED_LENGTH = 16;
            public byte[] seeds { get; }
            public EndPoint ConnEndPoint { get; }
            public PeerTcpGeneratorConnectionMessage(byte[] seeds, EndPoint connEndPoint)
            {
                this.seeds = seeds;
                ConnEndPoint = connEndPoint;
            }
        }
        public class PeerTcpGenerator : IPortBound, ICreator<EndPoint, RandomGenerator,IConnection>, ICreator<EndPoint, IConnection>
        {
            private readonly IConnection _int;
            public PeerTcpGenerator()
            {
                _int = new UdpConnection();
            }
            public void Dispose()
            {
                _int.Dispose();
            }
            public EndPoint source
            {
                get
                {
                    return _int.source;
                }
                set
                {
                    _int.source = value;
                }
            }
            public enum ConnectionOutcome { Fail = 0, Server = -1, Client = 1}
            public IConnection Create(EndPoint target)
            {
                ConnectionOutcome outcome;
                return Create(target, out outcome);
            }
            public IConnection Create(EndPoint target, out ConnectionOutcome outcome)
            {
                _int.EnsureSource();
                return Create(target, new LocalRandomGenerator(_int.source.GetHashCode()), out outcome);
            }
            public IConnection Create(EndPoint target, RandomGenerator gen)
            {
                ConnectionOutcome outcome;
                return Create(target, gen, out outcome);
            }
            public IConnection Create(EndPoint target, RandomGenerator gen, out ConnectionOutcome outcome)
            {
                outcome = ConnectionOutcome.Fail;
                _int.target = target;
                IConnection placeholder = new UdpConnection();
                placeholder.EnsureSource();
                IEnumerable<byte> bytes = Enviroment.Platform.getMacAddress();
                var port = ((IPEndPoint)placeholder.source).Port;
                bytes = bytes.Concat(BitConverter.GetBytes(port)).ToArray();
                if (bytes.Count() < PeerTcpGeneratorConnectionMessage.SEED_LENGTH)
                    bytes = gen.Bytes(PeerTcpGeneratorConnectionMessage.SEED_LENGTH - bytes.Count()).Concat(bytes);
                PeerTcpGeneratorConnectionMessage mes =
                    new PeerTcpGeneratorConnectionMessage( bytes.ToArray(PeerTcpGeneratorConnectionMessage.SEED_LENGTH), placeholder.source);
                _int.Send(mes);
                EndPoint from;
                var peermes = _int.recieve<PeerTcpGeneratorConnectionMessage>(out from);
                if (!from.Equals(target))
                    throw new Exception("peer target mismatch");
                var comp = new EnumerableCompararer<byte>().Compare(mes.seeds, peermes.seeds);
                if (comp == 0)
                    throw new Exception($"seed perfect match, 2^-{8*PeerTcpGeneratorConnectionMessage.SEED_LENGTH} chance!");
                if (comp < 0)
                {
                    placeholder.Dispose();
                    //we are server
                    var ret = new TcpServer {source = mes.ConnEndPoint}.Create();
                    outcome = ConnectionOutcome.Server;
                    return ret;
                }
                else
                {
                    Thread.Sleep(TimeSpan.FromSeconds(0.1));
                    placeholder.Dispose();
                    //we are client
                    var ret = new TcpClient {target = peermes.ConnEndPoint};
                    outcome = ConnectionOutcome.Client;
                    return ret;
                }
            }
        }
    }
}
