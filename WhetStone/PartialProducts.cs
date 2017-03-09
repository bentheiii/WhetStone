using System.Collections.Generic;
using WhetStone.Fielding;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class partialProducts
    {
        /// <summary>
        /// Get the partial products of an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to multiply.</param>
        /// <returns>All the partial products of <paramref name="this"/>.</returns>
        /// <remarks>
        /// <para>Uses fielding to multiply. Use <see cref="yieldAggregate.YieldAggregate{T,R}"/> for non-fielding version.</para>
        /// <para>Because it starts with a 1, Has 1 more element than <paramref name="this"/>.</para>
        /// </remarks>
        public static IEnumerable<T> PartialProducts<T>(this IEnumerable<T> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            var f = Fields.getField<T>();
            return @this.YieldAggregate(f.multiply, f.one);
        }
    }
}
