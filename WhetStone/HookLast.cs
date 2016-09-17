using System;
using System.Collections.Generic;
using WhetStone.Guard;
using WhetStone.Looping;

namespace WhetStone
{
    public static class hookLast
    {
        public static IEnumerable<T> HookLast<T>(this IEnumerable<T> @this, IGuard<Tuple<T>> sink)
        {
            return @this.HookAggregate(sink, (t0, t1) => Tuple.Create(t1), null);
        }
        public static IEnumerable<T> HookLast<T>(this IEnumerable<T> @this, IGuard<Tuple<T>> sink, Func<T, bool> critiria)
        {
            return @this.HookAggregate(sink, (t0, t1) => critiria(t1) ? Tuple.Create(t1) : t0, null);
        }
    }
}
