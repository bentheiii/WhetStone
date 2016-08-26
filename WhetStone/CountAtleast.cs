using System;
using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class countAtleast
    {
        public static bool CountAtLeast<T>(this IEnumerable<T> @this, int minCount, Func<T, bool> predicate = null)
        {
            if (predicate != null)
                @this = @this.Where(predicate);
            var rec = @this.RecommendSize();
            if (rec.HasValue)
                return rec.Value >= minCount;
            return @this.Skip(minCount - 1).Any();
        }
    }
}
