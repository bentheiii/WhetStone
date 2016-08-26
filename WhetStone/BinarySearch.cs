using System;
using System.Collections.Generic;
using WhetStone.NumbersMagic;

namespace WhetStone.Looping
{
    public static class binarySearch
    {
        public static int BinarySearch(Func<int, int> searcher, int min, int max, int failvalue = -1)
        {
            if (failvalue.iswithin(min, max))
                throw new ArgumentException("failval cannot be within searched values");
            while (min < max)
            {
                int i = (min + max) / 2;
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
        public static int BinarySearch(Func<int, bool> searcher, int min, int max, int failvalue = -1)
        {
            if (failvalue.iswithin(min, max))
                throw new ArgumentException("failval cannot be within searched values");
            int rangemax = max;
            while (min < max)
            {
                int i = (min + max) / 2;
                var res = searcher(i);
                if (res == false)
                    max = i;
                else
                {
                    if (i + 1 >= rangemax || !searcher(i + 1))
                        return i;
                    min = i + 1;
                }
            }
            return failvalue;
        }
        public static int BinarySearch<T>(this IList<T> sortedarr, Func<T, bool> searcher)
        {
            return BinarySearch(i => searcher(sortedarr[i]), 0, sortedarr.Count);
        }
        public static int BinarySearch<T>(this IList<T> sortedarr, Func<T, int> searcher)
        {
            return BinarySearch(i => searcher(sortedarr[i]), 0, sortedarr.Count);
        }
        public static int BinarySearch<T>(this IList<T> sortedarr, T tofind, IComparer<T> comp)
        {
            return BinarySearch(sortedarr, a => comp.Compare(tofind, a));
        }
        public static int BinarySearch<T>(this IList<T> sortedarr, T tofind)
        {
            return BinarySearch(sortedarr, tofind, Comparer<T>.Default);
        }
    }
}
