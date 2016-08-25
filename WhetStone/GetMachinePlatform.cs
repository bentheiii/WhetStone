using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Enviroment
{
    public static class getMachinePlatform
    {
        // ReSharper disable InconsistentNaming
        public enum PlatformArcitecture { x86 = 32, bit64 = 64, other = -1 }
        // ReSharper restore InconsistentNaming
        public static PlatformArcitecture GetMachinePlatform()
        {
            return !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432")) ? PlatformArcitecture.bit64 : PlatformArcitecture.x86;
        }
    }
}
