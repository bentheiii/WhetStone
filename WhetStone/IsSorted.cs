using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class isSorted
    {
        /// <summary>
        /// Get whether an <see cref="IEnumerable{T}"/> is sorted.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to check.</param>
        /// <param name="comp">The <see cref="IComparer{T}"/> to check with. <see langword="null"/> will use the default <see cref="Comparer{T}"/></param>
        /// <param name="allowEquals">Whether to allow for equalities in the <see cref="IEnumerable{T}"/></param>
        /// <returns>Whether <paramref name="this"/> is sorted according to <paramref name="comp"/>.</returns>
        public static bool IsSorted<T>(this IEnumerable<T> @this, IComparer<T> comp = null, bool allowEquals = true)
        {
            @this.ThrowIfNull(nameof(@this));
            comp = comp ?? Comparer<T>.Default;
            return
                @this.Trail(2).All(allowEquals
                    ? (Func<T[], bool>)(a => comp.Compare(a[0], a[1]) <= 0)
                    :                  (a => comp.Compare(a[0], a[1]) <  0));
        }
    }
}
