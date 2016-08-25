using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Ports
{
    public static class SetLocalSourcecs
    {
        public static void setLocalSource(this IPortBound c, int port, localEndPoint.SourceStyle style = localEndPoint.SourceStyle.Private)
        {
            c.source = localEndPoint.LocalEndPoint(port, style);
        }
    }
}
