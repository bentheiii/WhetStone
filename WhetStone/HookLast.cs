using System;
using System.Collections.Generic;
using WhetStone.Guard;

namespace WhetStone
{
    public static class hookLast
    {
        public static IEnumerable<T> HookLast<T>(this IEnumerable<T> @this, IGuard<Tuple<T>> sink)
        {
            sink.value = null;
            foreach (var t in @this)
            {
                sink.value = Tuple.Create(t);
                yield return t;
            }
        }
        public static IEnumerable<T> HookLast<T>(this IEnumerable<T> @this, IGuard<Tuple<T>> sink, Func<T, bool> critiria)
        {
            sink.value = null;
            foreach (var t in @this)
            {
                if (critiria(t))
                    sink.value = Tuple.Create(t);
                yield return t;
            }
        }
        public static IEnumerable<T> HookLast<T>(this IEnumerable<T> @this, IGuard<T> sink)
        {
            foreach (var t in @this)
            {
                sink.value = t;
                yield return t;
            }
        }
        public static IEnumerable<T> HookLast<T>(this IEnumerable<T> @this, IGuard<T> sink, Func<T, bool> critiria)
        {
            foreach (var t in @this)
            {
                if (critiria(t))
                    sink.value = t;
                yield return t;
            }
        }
    }
}
