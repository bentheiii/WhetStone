using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Arrays
{
    public static class recommendSize
    {
        public static int? RecommendSize<T>(this IEnumerable<T> @this)
        {
            return (@this as IReadOnlyCollection<T>)?.Count ?? ((@this as ICollection<T>)?.Count);
        }
    }
}
