using System.Collections.Generic;

namespace WhetStone.Looping
{
    public static class columns
    {
        public static IEnumerable<IEnumerable<T>> Columns<T>(this T[,] @this)
        {
            return range.Range(@this.GetLength(1)).Select(a => range.Range(@this.GetLength(0)).Select(x => @this[x, a]));
        }
    }
}
