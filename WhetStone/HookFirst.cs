using System;
using System.Collections.Generic;
using WhetStone.Guard;

namespace WhetStone.Looping
{
    public static class hookFirst
    {
        public static IEnumerable<T> HookFirst<T>(this IEnumerable<T> @this, IGuard<Tuple<T>> sink)
        {
            sink.value = null;
            using (var tor = @this.GetEnumerator())
            {
                if (!tor.MoveNext())
                    yield break;
                sink.value = Tuple.Create(tor.Current);
                yield return tor.Current;
                while (tor.MoveNext())
                {
                    yield return tor.Current;
                }
            }
        }
        public static IEnumerable<T> HookFirst<T>(this IEnumerable<T> @this, IGuard<Tuple<T>> sink, Func<T, bool> critiria)
        {
            sink.value = null;
            using (var tor = @this.GetEnumerator())
            {
                while (true)
                {
                    if (!tor.MoveNext())
                        yield break;
                    var yes = critiria(tor.Current);
                    if (yes)
                        sink.value = Tuple.Create(tor.Current);
                    yield return tor.Current;
                    if (yes)
                        break;
                }
                while (tor.MoveNext())
                {
                    yield return tor.Current;
                }
            }
        }
        public static IEnumerable<T> HookFirst<T>(this IEnumerable<T> @this, IGuard<T> sink)
        {
            using (var tor = @this.GetEnumerator())
            {
                if (!tor.MoveNext())
                    yield break;
                sink.value = tor.Current;
                yield return tor.Current;
                while (tor.MoveNext())
                {
                    yield return tor.Current;
                }
            }
        }
        public static IEnumerable<T> HookFirst<T>(this IEnumerable<T> @this, IGuard<T> sink, Func<T, bool> critiria)
        {
            using (var tor = @this.GetEnumerator())
            {
                while (true)
                {
                    if (!tor.MoveNext())
                        yield break;
                    var yes = critiria(tor.Current);
                    if (yes)
                        sink.value = tor.Current;
                    yield return tor.Current;
                    if (yes)
                        break;
                }
                while (tor.MoveNext())
                {
                    yield return tor.Current;
                }
            }
        }
    }
}
