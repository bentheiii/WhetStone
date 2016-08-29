using System.Collections.Generic;
using WhetStone.Fielding;

namespace WhetStone.Looping
{
    public static class partialProducts
    {
        public static IEnumerable<T> PartialProducts<T>(this IEnumerable<T> @this)
        {
            var f = Fields.getField<T>();
            return @this.YieldAggregate(f.multiply, f.one);
        }
    }
}
