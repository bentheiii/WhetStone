using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Ports
{
    public interface IPortBound : IDisposable
    {
        EndPoint source { get; set; }
    }
}
