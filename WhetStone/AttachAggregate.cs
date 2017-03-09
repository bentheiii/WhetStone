using System;
using System.Collections.Generic;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class attachAggregate
    {
        /// <summary>
        /// Attaches a new <see cref="IEnumerable{T}"/> to the original <see cref="IEnumerable{T}"/>, created with an aggregator function.
        /// </summary>
        /// <typeparam name="T">The type of the original <see cref="IEnumerable{T}"/>.</typeparam>
        /// <typeparam name="R">The type of the output of <paramref name="aggregator"/>.</typeparam>
        /// <param name="this">The original <see cref="IEnumerable{T}"/></param>
        /// <param name="aggregator">The aggregator function.</param>
        /// <param name="seed">The initial seed for the aggregator function.</param>
        /// <returns><paramref name="this"/> zipped to <paramref name="aggregator"/>'s output.</returns>
        /// <example>
        /// This can simplify complicated <see cref="IEnumerable{T}"/> expressions to a single line:
        /// <code>
        /// var indexedTriangleNumbers = range.Range(10).AttachAggregate((a,b)=>a+b);
        /// </code> 
        /// </example>
        public static IEnumerable<Tuple<T, R>> AttachAggregate<T, R>(this IEnumerable<T> @this, Func<T, R, R> aggregator, R seed = default(R))
        {
            @this.ThrowIfNull(nameof(@this));
            aggregator.ThrowIfNull(nameof(aggregator));
            foreach (var t in @this)
            {
                seed = aggregator(t, seed);
                yield return Tuple.Create(t, seed);
            }
        }
    }
}
