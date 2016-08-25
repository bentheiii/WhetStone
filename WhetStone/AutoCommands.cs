using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Ports.AutoCommands
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
        { }
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
