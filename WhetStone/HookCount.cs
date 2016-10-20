using System;
using System.Collections.Generic;
using WhetStone.Guard;

namespace WhetStone.Looping
{
    public static class hookCount
    {
        public static IEnumerable<T> HookCount<T>(this IEnumerable<T> @this, IGuard<int> sink)
        {
            sink.value = 0;
            foreach (var t in @this)
            {
                sink.value++;
                yield return t;
            }
        }
        public static IEnumerable<T> HookCount<T>(this IEnumerable<T> @this, IGuard<int> sink, Func<T, bool> critiria)
        {
            sink.value = 0;
            foreach (var t in @this)
            {
                if (critiria(t))
                    sink.value++;
                yield return t;
            }
        }
    }
}
