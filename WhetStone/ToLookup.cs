using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class toLookup
    {
        public static ILookup<T, T> ToLookup<T>(this IEnumerable<T> @this, IEqualityComparer<T> matcher)
        {
            return @this.ToLookup(a => a, matcher);
        }
    }
}
