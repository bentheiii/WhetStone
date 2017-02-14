using System;
using System.Collections.Generic;
using WhetStone.Guard;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class hookAggregate
    {
        /// <summary>
        /// Hooks an aggregate value to an <see cref="IGuard{T}"/>, to be recalculated upon enumeration.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/></typeparam>
        /// <typeparam name="R">The type of the aggregated value.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to hook to.</param>
        /// <param name="sink">The <see cref="IGuard{T}"/> to update with the aggregate value.</param>
        /// <param name="aggregator">The aggregator function.</param>
        /// <param name="seed">The initial seed for the aggregator function.</param>
        /// <returns>A new <see cref="IEnumerable{T}"/> that, when enumerated, will also aggregate <paramref name="sink"/>'s value.</returns>
        public static IEnumerable<T> HookAggregate<T, R>(this IEnumerable<T> @this, IGuard<R> sink, Func<T, R, R> aggregator, R seed = default(R))
        {
            return @this.AttachAggregate(aggregator,seed).Detach(sink);
        }
    }
}
