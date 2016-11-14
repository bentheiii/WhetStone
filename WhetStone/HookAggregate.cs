using System;
using System.Collections.Generic;
using WhetStone.Guard;

namespace WhetStone.Looping
{
    public static class hookAggregate
    {
        public static IEnumerable<T> HookAggregate<T, R>(this IEnumerable<T> @this, IGuard<R> sink, Func<R, T, R> aggregator, R seed)
        {
            sink.value = seed;
            foreach (T t in @this)
            {
                sink.value = aggregator(sink.value, t);
                yield return t;
            }
        }
        public static IEnumerable<T> HookAggregate<T>(this IEnumerable<T> @this, IGuard<T> sink, Func<T, T, T> aggregator)
        {
            using (IEnumerator<T> tor = @this.GetEnumerator())
            {
                if (!tor.MoveNext())
                    yield break;
                sink.value = tor.Current;
                yield return tor.Current;
                while (tor.MoveNext())
                {
                    sink.value = aggregator(sink.value, tor.Current);
                    yield return tor.Current;
                }
            }
        }
    }
}
