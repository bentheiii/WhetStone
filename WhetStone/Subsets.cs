using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class subsets
    {
        public static IEnumerable<IEnumerable<T>> SubSets<T>(this IEnumerable<T> @this)
        {
            return range.IRange(0, @this.Count()).Select(@this.SubSets).Concat();
        }
        public static IList<T[]> SubSets<T>(this IList<T> @this)
        {
            return range.IRange(0,@this.Count).Select(@this.SubSets).Concat();
        }
        public static IEnumerable<IEnumerable<T>> SubSets<T>(this IEnumerable<T> @this, int setSize)
        {
            return @this.Join(setSize, join.CartesianType.NoReflexive | join.CartesianType.NoSymmatry);
        }
        public static IList<T[]> SubSets<T>(this IList<T> @this, int setSize)
        {
            return @this.Join(setSize, join.CartesianType.NoReflexive | join.CartesianType.NoSymmatry);
        }
    }
}
