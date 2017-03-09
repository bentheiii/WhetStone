using System.Collections.Generic;
using System.Linq;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class orderBy
    {
        /// <summary>
        /// Returns an <see cref="IOrderedEnumerable{TElement}"/> from an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="this">The elements to sort.</param>
        /// <param name="comp">The <see cref="IComparer{T}"/> to sort the elements by.</param>
        /// <returns>A new <see cref="IEnumerable{T}"/> with <paramref name="this"/>'s element's sorted.</returns>
        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> @this, IComparer<T> comp = null)
        {
            @this.ThrowIfNull(nameof(@this));
            comp = comp ?? Comparer<T>.Default;
            return @this.OrderBy(a => a, comp);
        }
        /// <summary>
        /// Returns an <see cref="IOrderedEnumerable{TElement}"/> in descending order from an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="this">The elements to sort in descending order.</param>
        /// <param name="comp">The <see cref="IComparer{T}"/> to sort the elements by.</param>
        /// <returns>A new <see cref="IEnumerable{T}"/> with <paramref name="this"/>'s element's sorted in descending order.</returns>
        public static IOrderedEnumerable<T> OrderByDescending<T>(this IEnumerable<T> @this, IComparer<T> comp = null)
        {
            @this.ThrowIfNull(nameof(@this));
            comp = comp ?? Comparer<T>.Default;
            return @this.OrderByDescending(a => a, comp);
        }
    }
}
