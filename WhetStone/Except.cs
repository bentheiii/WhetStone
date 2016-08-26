using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class except
    {
        public static IEnumerable<T> Except<T>(this IEnumerable<T> @this, params T[] toexclude)
        {
            return @this.Where(a => !toexclude.Contains(a));
        }
    }
}
