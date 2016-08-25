using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Fielding;

namespace WhetStone.NumbersMagic
{
    public static class minmax
    {
        public static bool MinMax(ref int min, ref int max)
        {
            if (min < max)
            {
                return false;
            }
            var temp = max;
            max = min;
            min = temp;
            return true;
        }
        public static bool MinMax(ref long min, ref long max)
        {
            if (min < max)
            {
                return false;
            }
            var temp = max;
            max = min;
            min = temp;
            return true;
        }
        public static bool MinMax(ref BigInteger min, ref BigInteger max)
        {
            if (min < max)
            {
                return false;
            }
            var temp = max;
            max = min;
            min = temp;
            return true;
        }
        public static bool MinMax(ref double min, ref double max)
        {
            if (min < max)
            {
                return false;
            }
            var temp = max;
            max = min;
            min = temp;
            return true;
        }
        public static bool MinMax(ref float min, ref float max)
        {
            if (min < max)
            {
                return false;
            }
            var temp = max;
            max = min;
            min = temp;
            return true;
        }
        public static bool MinMax<T>(ref T min, ref T max)
        {
            if (min.ToFieldWrapper() < max)
            {
                return false;
            }
            var temp = max;
            max = min;
            min = temp;
            return true;
        }
    }
}
