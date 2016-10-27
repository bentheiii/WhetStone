using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.NumbersMagic;

namespace WhetStone.Looping
{
    public static class isWithinBounds
    {
        public static bool IsWithinBounds(this Array arr, params int[] ind)
        {
            if (arr.Rank != ind.Length)
                throw new ArgumentException("mismatch on indices");
            return arr.GetBounds().Zip(ind).All(a => a.Item2.iswithinPartialExclusive(a.Item1.Item1, a.Item1.Item2));
        }
        public static bool IsWithinBounds<T>(this IList<T> arr, int ind)
        {
            return ind.iswithinPartialExclusive(0, arr.Count);
        }
        public static bool IsWithinBounds<T>(this T[] arr, int ind)
        {
            return IsWithinBounds((Array)arr, ind);
        }
    }
}
