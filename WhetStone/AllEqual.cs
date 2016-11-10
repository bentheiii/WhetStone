using System.Collections.Generic;
using System.Linq;
using WhetStone.Tuples;

namespace WhetStone.Looping
{
    public static class allEqual
    {
        public static bool AllEqual<T>(this IEnumerable<T> @this, IEqualityComparer<T> comp = null)
        {
            return @this.AllEqualToFirst(comp);
        }
        public static bool AllEqualToFirst<T>(this IEnumerable<T> @this, IEqualityComparer<T> comp = null)
        {
            comp = comp ?? EqualityComparer<T>.Default;
            using (IEnumerator<T> tor = @this.GetEnumerator())
            {
                if (!tor.MoveNext())
                    return true;
                T mem = tor.Current;
                while (tor.MoveNext())
                {
                    if (!comp.Equals(mem, tor.Current))
                        return false;
                }
            }
            return true;
        }
        public static bool AllEqualToPrevious<T>(this IEnumerable<T> @this, IEqualityComparer<T> comp = null)
        {
            comp = comp ?? EqualityComparer<T>.Default;
            return @this.Trail(2).Select(a => a.ToTuple2()).All(a => comp.Equals(a.Item1, a.Item2));
        }
        public static bool AllEqualToEachOther<T>(this IEnumerable<T> @this, IEqualityComparer<T> comp = null, @join.CartesianType type = @join.CartesianType.NoReflexive | @join.CartesianType.NoSymmatry)
        {
            comp = comp ?? EqualityComparer<T>.Default;
            return @this.Join(type).All(a => comp.Equals(a.Item1, a.Item2));
        }
    }
}
