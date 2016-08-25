using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class where
    {
        public static IEnumerable<T> Where<T>(this IEnumerable<T> @this, params T[] toinclude)
        {
            return @this.Where(toinclude.Contains);
        }
    }
}
