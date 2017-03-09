using System;
using System.Collections.Generic;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class selectMany
    {
        /// <summary>
        /// get a 1-many mapping of an <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of the original <see cref="IList{T}"/>.</typeparam>
        /// <typeparam name="R">The type of the resultant <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to map.</param>
        /// <param name="selector">The selector function from <paramref name="this"/>'s element to multiple elements.</param>
        /// <param name="samecount">Whether it can be assured all the elements in <paramref name="this"/> map to the same amount of elements for optimization. If <see langword="null"/>, the resultant values will be checked.</param>
        /// <returns>A read-only <see cref="IList{T}"/> that concatenates the result of <paramref name="this"/> through <paramref name="selector"/>.</returns>
        public static IList<R> SelectMany<T, R>(this IList<T> @this, Func<T, IList<R>> selector, bool? samecount = false)
        {
            @this.ThrowIfNull(nameof(@this));
            selector.ThrowIfNull(nameof(selector));
            return @this.Select(selector).Concat(samecount);
        }
    }
}
