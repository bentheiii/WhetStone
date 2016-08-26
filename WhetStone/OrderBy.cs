using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class orderBy
    {
        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> @this)
        {
            return OrderBy(@this, Comparer<T>.Default);
        }
        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> @this, IComparer<T> comp)
        {
            return @this.OrderBy(a => a, comp);
        }
        public static IOrderedEnumerable<T> OrderByDescending<T>(this IEnumerable<T> @this)
        {
            return OrderByDescending(@this, Comparer<T>.Default);
        }
        public static IOrderedEnumerable<T> OrderByDescending<T>(this IEnumerable<T> @this, IComparer<T> comp)
        {
            return @this.OrderByDescending(a => a, comp);
        }
    }
}
