using System.Collections.Generic;
using WhetStone.Fielding;

namespace WhetStone.Looping
{
    public static class partialSums
    {
        public static IEnumerable<T> PartialSums<T>(this IEnumerable<T> @this)
        {
            var f = Fields.getField<T>();
            return @this.YieldAggregate(f.add, f.zero);
        }
    }
}
