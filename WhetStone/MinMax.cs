using System.Collections.Generic;
using System.Numerics;
using WhetStone.Fielding;

namespace WhetStone.Comparison
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
        public static bool MinMax<T>(ref T min, ref T max, IComparer<T> comp = null)
        {
            comp = comp ?? Comparer<T>.Default;
            if (comp.Compare(min,max) <= 0)
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
