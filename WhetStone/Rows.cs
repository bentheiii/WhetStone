using System.Collections.Generic;

namespace WhetStone.Looping
{
    public static class rows
    {
        public static IList<IList<T>> Rows<T>(this T[,] @this)
        {
            return range.Range(@this.GetLength(0)).Select(a => (IList<T>)range.Range(@this.GetLength(1)).Select(x => @this[a, x]));
        }
    }
}
