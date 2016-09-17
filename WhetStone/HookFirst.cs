using System;
using System.Collections.Generic;
using WhetStone.Guard;

namespace WhetStone.Looping
{
    public static class hookFirst
    {
        public static IEnumerable<T> HookFirst<T>(this IEnumerable<T> @this, IGuard<Tuple<T>> sink)
        {
            return @this.HookAggregate(sink, (t0, t1) => t0 ?? Tuple.Create(t1), null);
        }
        public static IEnumerable<T> HookFirst<T>(this IEnumerable<T> @this, IGuard<Tuple<T>> sink, Func<T, bool> critiria)
        {
            return @this.HookAggregate(sink, (t0, t1) => (t0 == null && critiria(t1)) ? Tuple.Create(t1) : t0, null);
        }
    }
}
