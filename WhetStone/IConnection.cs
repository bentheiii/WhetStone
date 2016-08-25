using System;
using System.Collections.Generic;
using System.Net;

namespace WhetStone.Ports
{
    public interface IConnection : IPortBound
    {
        EndPoint target { get; set; }
        ISet<Type> enabledAutoCommands { get; }
        int SendBytes(byte[] o);
        //receive without autocommands
        byte[] RecieveBytes(out EndPoint from, int buffersize);

    }
}
