using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class yieldAggregate
    {
        /// <summary>
        /// Get all the partial aggregates of an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <typeparam name="R">The type of the aggregate value.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to aggregate.</param>
        /// <param name="aggregator">The aggregator function.</param>
        /// <param name="seed">The seed for the aggregator and the first item to return.</param>
        /// <returns>All the partial aggregates of <paramref name="this"/>.</returns>
        /// <remarks><para>The first element in the output is <paramref name="seed"/>, so the output will always include one element more than the input.</para></remarks>
        public static IEnumerable<R> YieldAggregate<T, R>(this IEnumerable<T> @this, Func<T, R, R> aggregator, R seed = default(R))
        {
            yield return seed;
            foreach (T t in @this)
            {
                seed = aggregator(t, seed);
                yield return seed;
            }
        }
        /// <summary>
        /// Get an infinite <see cref="IEnumerable{T}"/>, each element an aggregate of the previous element.
        /// </summary>
        /// <typeparam name="T">The type of aggregated elements.</typeparam>
        /// <param name="aggregator">the aggregator function.</param>
        /// <param name="seed">The initial element in the output.</param>
        /// <returns>An infinite <see cref="IEnumerable{T}"/>, starting with <paramref name="seed"/>, and every element is the product of <paramref name="aggregator"/> and the previous element.</returns>
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
