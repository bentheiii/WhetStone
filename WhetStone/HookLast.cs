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
            return @this.HookAggregate(sink, (a, b) => Tuple.Create(a));
        }
        public static IEnumerable<T> HookLast<T>(this IEnumerable<T> @this, IGuard<Tuple<T>> sink, Func<T, bool> critiria)
        {
            return @this.HookAggregate(sink, (a, b) => critiria(a) ? Tuple.Create(a) : b);
        }
        public static IEnumerable<T> HookLast<T>(this IEnumerable<T> @this, IGuard<T> sink)
        {
            return @this.HookAggregate(sink, (a, b) => b);
        }
        public static IEnumerable<T> HookLast<T>(this IEnumerable<T> @this, IGuard<T> sink, Func<T, bool> critiria)
        {
            return @this.HookAggregate(sink, (a, b) => critiria(a) ? a : b);
        }
    }
}
