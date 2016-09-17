using System;
using System.Collections.Generic;
using WhetStone.Guard;

namespace WhetStone.Looping
{
    public static class hookCount
    {
        public static IEnumerable<T> HookCount<T>(this IEnumerable<T> @this, IGuard<int> sink)
        {
            return @this.HookAggregate(sink, (i, t) => i + 1, 0);
        }
        public static IEnumerable<T> HookCount<T>(this IEnumerable<T> @this, IGuard<int> sink, Func<T, bool> critiria)
        {
            return @this.HookAggregate(sink, (i, t) => critiria(t) ? i + 1 : i, 0);
        }
    }
}
