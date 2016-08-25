using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.NumbersMagic
{
    public static class trueMod
    {
        public static double TrueMod(this double a, double b)
        {
            var ret = a;
            if (ret < 0)
                ret = b + a % b;
            if (ret >= b)
                ret = ret % b;
            return ret;
        }
        public static int TrueMod(this int a, int b)
        {
            var ret = a;
            if (ret < 0)
                ret = b + a % b;
            if (ret >= b)
                ret = ret % b;
            return ret;
        }
    }
}
