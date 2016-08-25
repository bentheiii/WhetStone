using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Ports
{
    public static class setLocalTarget
    {
        public static void SetLocalTarget(this IConnection c, int port)
        {
            c.target = localEndPoint.LocalEndPoint(port);
        }
    }
}
