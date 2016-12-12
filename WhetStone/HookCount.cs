using System;
using System.Collections.Generic;
using WhetStone.Guard;

namespace WhetStone.Looping
{
    public static class hookCount
    {
        public static IEnumerable<T> HookCount<T>(this IEnumerable<T> @this, IGuard<int> sink)
        {
            return @this.HookAggregate(sink, (a, b) => b + 1);
        }
        public static IEnumerable<T> HookCount<T>(this IEnumerable<T> @this, IGuard<int> sink, Func<T, bool> critiria)
        {
            return @this.HookAggregate(sink, (a, b) => critiria(a) ? b + 1 : b);
        }
    }
}
