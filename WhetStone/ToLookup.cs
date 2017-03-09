using System.Collections.Generic;
using System.Linq;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class toLookup
    {
        /// <summary>
        /// Convert an <see cref="IEnumerable{T}"/> to <see cref="ILookup{TKey,TElement}"/> with identity.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to convert.</param>
        /// <param name="matcher">The <see cref="IEqualityComparer{T}"/> to use.</param>
        /// <returns>The equivalence classes for <paramref name="this"/> under <paramref name="matcher"/></returns>
        public static ILookup<T, T> ToLookup<T>(this IEnumerable<T> @this, IEqualityComparer<T> matcher)
        {
            @this.ThrowIfNull(nameof(@this));
            matcher.ThrowIfNull(nameof(matcher));
            return @this.ToLookup(a => a, matcher);
        }
    }
}
