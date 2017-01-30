using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    public static class binarySearch
    {
        private static int BinarySearchBoundsUnknown(Func<int, int> searcher, int failval = -1)
        {
            var s = searcher(0);
            if (s == 0)
                return 0;
            if (s > 0)
                return BinarySearchMaxUnknown(searcher, 0, failval);
            return BinarySearchMinUnknown(searcher, 0, failval);
        }
        private static int BinarySearchMaxUnknown(Func<int, int> searcher, int min, int failval = -1)
        {
            if (searcher(min) == 0)
                return min;
            int range = 1;
            int practmin = min;
            while (true)
            {
                int max = min + range;
                var s = searcher(max);
                if (s == 0)
                    return max;
                if (s < 0)
                    return BinarySearch(searcher, practmin, max, failval);
                practmin = max;
                range *= 2;
            }
        }
        private static int BinarySearchMinUnknown(Func<int, int> searcher, int max, int failval = -1)
        {
            if (searcher(max) == 0)
                return max;
            int range = 1;
            int practmax = max;
            while (true)
            {
                int min = max - range;
                var s = searcher(min);
                if (s == 0)
                    return max;
                if (s > 0)
                    return BinarySearch(searcher, min, practmax, failval);
                practmax = min;
                range *= 2;
            }
        }
        private static int BinarySearchAllKnown(Func<int, int> searcher, int min, int max, int failvalue = -1)
        {
            while (min < max)
            {
                int i = (min + max)/2;
                int res = searcher(i);
                if (res == 0)
                    return i;
                if (i == min)
                    break;
                if (res > 0)
                    min = i;
                else
                    max = i;
            }
            return failvalue;
        }
        public static int BinarySearch(Func<int, int> searcher, int? min = null, int? max = null, int failvalue = -1)
        {
            if (min == null)
            {
                if (max == null)
                    return BinarySearchBoundsUnknown(searcher, failvalue);
                return BinarySearchMinUnknown(searcher, max.Value, failvalue);
            }
            if (max == null)
                return BinarySearchMaxUnknown(searcher, min.Value, failvalue);
            return BinarySearchAllKnown(searcher, min.Value, max.Value, failvalue);
        }
        public enum BooleanBinSearchStyle
        {
            GetLastTrue, GetFirstTrue
        }
        public static int BinarySearch(Func<int, bool> searcher, int? min = null, int? max = null, int failvalue = -1, BooleanBinSearchStyle style = BooleanBinSearchStyle.GetLastTrue)
        {
            Func<int, int> s;
            if (style == BooleanBinSearchStyle.GetLastTrue)
                s = i =>
                {
                    if (searcher(i))
                    {
                        bool ks;
                        try
                        {
                            ks = searcher(i + 1);
                        }
                        catch (Exception)
                        {
                            ks = false;
                        }
                        return !ks ? 0 : 1;
                    }
                    return -1;
                };
            else
                s = i =>
                {
                    if (searcher(i))
                    {
                        bool ks;
                        try
                        {
                            ks = searcher(i - 1);
                        }
                        catch (Exception)
                        {
                            ks = false;
                        }
                        return !ks ? 0 : -1;
                    }
                    return 1;
                };
            return BinarySearch(s, min, max, failvalue);
        }
        public static int BinarySearch<T>(this IList<T> sortedarr, Func<T, bool> searcher, BooleanBinSearchStyle style = BooleanBinSearchStyle.GetLastTrue)
        {
            return BinarySearch(i => searcher(sortedarr[i]), 0, sortedarr.Count, style:style);
        }
        public static int BinarySearch<T>(this IList<T> sortedarr, Func<T, int> searcher)
        {
            return BinarySearch(i => searcher(sortedarr[i]), 0, sortedarr.Count);
        }
        public static int BinarySearch<T>(this IList<T> sortedarr, T tofind, IComparer<T> comp = null)
        {
            comp = comp ?? Comparer<T>.Default;
            return BinarySearch(sortedarr, a => comp.Compare(tofind, a));
        }
        public static int BinarySearchStartBias<T>(this IList<T> sortedarr, Func<T, bool> searcher, BooleanBinSearchStyle style = BooleanBinSearchStyle.GetLastTrue)
        {
            return BinarySearch(i => i >= sortedarr.Count ? style == BooleanBinSearchStyle.GetFirstTrue : searcher(sortedarr[i]), 0, style: style);
        }
        public static int BinarySearchStartBias<T>(this IList<T> sortedarr, Func<T, int> searcher)
        {
            return BinarySearch(i => i >= sortedarr.Count ? -1 : searcher(sortedarr[i]), 0, null);
        }
        public static int BinarySearchStartBias<T>(this IList<T> sortedarr, T tofind, IComparer<T> comp = null)
        {
            comp = comp ?? Comparer<T>.Default;
            return BinarySearchStartBias(sortedarr, a => comp.Compare(tofind, a));
        }
    }
}
