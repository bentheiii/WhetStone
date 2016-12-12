using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    public static class yieldAggregate
    {
        public static IEnumerable<R> YieldAggregate<T, R>(this IEnumerable<T> @this, Func<T, R, R> aggregator, R seed = default(R))
        {
            foreach (T t in @this)
            {
                yield return seed;
                seed = aggregator(t, seed);
            }
            yield return seed;
        }
        public static IEnumerable<T> YieldAggregate<T>(Func<T, T> aggregator, T seed = default(T))
        {
            while (true)
            {
                yield return seed;
                seed = aggregator(seed);
            }
        }
    }
}
