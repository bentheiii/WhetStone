using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WhetStone.Arrays;
using WhetStone.Comparison;
using WhetStone.Enviroment;
using WhetStone.Factory;
using WhetStone.Random;

namespace WhetStone.Ports
{
    public class PeerTcpGenerator : IPortBound, ICreator<EndPoint, RandomGenerator, IConnection>, ICreator<EndPoint, IConnection>
    {
        [Serializable]
        private class PeerTcpGeneratorConnectionMessage
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
        public enum ConnectionOutcome { Fail = 0, Server = -1, Client = 1 }
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
            IEnumerable<byte> bytes = macAddress.MacAddress();
            var port = ((IPEndPoint)placeholder.source).Port;
            bytes = bytes.Concat(BitConverter.GetBytes(port)).ToArray();
            if (bytes.Count() < PeerTcpGeneratorConnectionMessage.SEED_LENGTH)
                bytes = gen.Bytes(PeerTcpGeneratorConnectionMessage.SEED_LENGTH - bytes.Count()).Concat(bytes);
            PeerTcpGeneratorConnectionMessage mes =
                new PeerTcpGeneratorConnectionMessage(bytes.ToArray(PeerTcpGeneratorConnectionMessage.SEED_LENGTH), placeholder.source);
            _int.Send(mes);
            EndPoint from;
            var peermes = _int.Recieve<PeerTcpGeneratorConnectionMessage>(out from);
            if (!from.Equals(target))
                throw new Exception("peer target mismatch");
            var comp = new EnumerableCompararer<byte>().Compare(mes.seeds, peermes.seeds);
            if (comp == 0)
                throw new Exception($"seed perfect match, 2^-{8 * PeerTcpGeneratorConnectionMessage.SEED_LENGTH} chance!");
            if (comp < 0)
            {
                placeholder.Dispose();
                //we are server
                var ret = new TcpServer { source = mes.ConnEndPoint }.Create();
                outcome = ConnectionOutcome.Server;
                return ret;
            }
            else
            {
                Thread.Sleep(TimeSpan.FromSeconds(0.1));
                placeholder.Dispose();
                //we are client
                var ret = new TcpClient { target = peermes.ConnEndPoint };
                outcome = ConnectionOutcome.Client;
                return ret;
            }
        }
    }
}
