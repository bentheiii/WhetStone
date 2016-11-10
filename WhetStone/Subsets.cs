using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class subsets
    {
        public static IList<IEnumerable<T>> SubSets<T>(this IEnumerable<T> @this)
        {
            return new[] {false, true}.Join(@this.Count()).Select(a => @this.Zip(a).Where(x => x.Item2).Detach());
        }
        public static IList<IEnumerable<T>> SubSets<T>(this IList<T> @this)
        {
            return new[] {false, true}.Join(@this.Count).Select(a => @this.Zip(a).Where(x => x.Item2).Detach());
        }
        public static IEnumerable<IEnumerable<T>> SubSets<T>(this IEnumerable<T> @this, int setSize)
        {
            return @this.Join(setSize, join.CartesianType.NoReflexive | join.CartesianType.NoSymmatry);
        }
        public static IList<IList<T>> SubSets<T>(this IList<T> @this, int setSize)
        {
            return @this.Join(setSize, join.CartesianType.NoReflexive | join.CartesianType.NoSymmatry);
        }
    }
}
