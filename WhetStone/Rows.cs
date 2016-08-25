using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Looping;

namespace WhetStone.Arrays
{
    public static class rows
    {
        public static IEnumerable<IEnumerable<T>> Rows<T>(this T[,] @this)
        {
            return range.Range(@this.GetLength(0)).Select(a => range.Range(@this.GetLength(1)).Select(x => @this[a, x]));
        }
    }
}
