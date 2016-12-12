using System;
using System.Collections.Generic;
using WhetStone.Guard;

namespace WhetStone.Looping
{
    public static class hookAggregate
    {
        public static IEnumerable<T> HookAggregate<T, R>(this IEnumerable<T> @this, IGuard<R> sink, Func<T, R, R> aggregator, R seed = default(R))
        {
            return @this.AttachAggregate(aggregator,seed).Detach(sink);
        }
    }
}
