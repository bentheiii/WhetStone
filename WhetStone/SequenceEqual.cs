using System.Collections.Generic;
using System.Linq;
using WhetStone.Comparison;

namespace WhetStone.Looping
{
    public static class sequenceEqual
    {
        public static bool SequenceEqual<T>(this IEnumerable<T> @this, params T[] seq)
        {
            return @this.SequenceEqual(seq.AsEnumerable());
        }
        public static bool SequenceEqualIndices<T>(this IList<T> @this, IList<T> other, IEqualityComparer<T> comp = null)
        {
            return new SequenceIndexEquator<T>(comp).Equals(@this,other);
        }
        public static bool SequenceEqualIndices<T>(this IList<T> @this, params T[] other)
        {
            return @this.SequenceEqualIndices(other.AsList());
        }
    }
}
