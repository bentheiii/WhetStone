using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    public static class getBounds
    {
        public static IList<Tuple<int, int>> GetBounds(this Array @this)
        {
            return range.Range(@this.Rank).Select(a => Tuple.Create(@this.GetLowerBound(a), @this.GetUpperBound(a)+1));
        }
    }
}
