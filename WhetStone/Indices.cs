using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    public static class indices
    {
        public static IList<int> Indices<T>(this IList<T> @this)
        {
            return range.Range(@this.Count);
        }
        public static IList<int[]> Indices(this Array @this)
        {
            return range.Range(@this.Rank).Select(a => range.Range(@this.GetLowerBound(a), @this.GetUpperBound(a)+1).AsList()).Join();
        }
        public static IList<int> Indices<T>(this T[] @this)
        {
            return range.Range(@this.GetLowerBound(0), @this.GetUpperBound(0)+1);
        }
    }
}
