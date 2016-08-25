using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.NumbersMagic
{
    public static class @base
    {
        public static long convertfrombase(IEnumerable<int> x, int originalbase)
        {
            long ret = 0;
            int p = 1;
            foreach (int t in x)
            {
                ret += t * p;
                p *= originalbase;
            }
            return ret;
        }
        public static IEnumerable<int> converttobase(int x, int tobase)
        {
            return converttobase((long)x, tobase);
        }
        public static IEnumerable<int> converttobase(long x, int tobase)
        {
            while (x > 0)
            {
                yield return (int)(x % tobase);
                x /= tobase;
            }
        }
    }
}
