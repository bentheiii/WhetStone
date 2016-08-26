using System;
using System.Collections.Generic;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    public static class Sort
    {
        public static T[] sort<T>(this T[] tosort, IComparer<T> comparer = null)
        {
            T[] ret = tosort.Copy();
            Array.Sort(ret, comparer ?? Comparer<T>.Default);
            return ret;
        }
        public static T[] sort<T>(this T[] tosort, int startindex, int length, IComparer<T> comparer = null)
        {
            T[] ret = tosort.Copy();
            Array.Sort(ret, startindex, length, comparer ?? Comparer<T>.Default);
            return ret;
        }
    }
}
