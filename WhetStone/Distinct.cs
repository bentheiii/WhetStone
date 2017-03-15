using System.Collections.Generic;
using System.Linq;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class distinct
    {
        /// <summary>
        /// Filters the sorted <see cref="IEnumerable{T}"/>  of any duplicates. 
        /// </summary>
        /// <typeparam name="T">The type of the sorted <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="this">The sorted <see cref="IEnumerable{T}"/></param>
        /// <param name="comp">The <see cref="IEqualityComparer{T}"/> to check for equality. <see langword="null"/> means default <see cref="IEqualityComparer{T}"/>.</param>
        /// <returns>A new <see cref="IEnumerable{T}"/> that only contains one element for every equal sub-enumerable in <paramref name="this"/>.</returns>
        /// <remarks>
        /// <para><paramref name="this"/> doesn't have to be sorted, it just has to have all elements equal to each other adjacent.</para>
        /// <para>Alternately, all non-adjacent equal elements will be treated as non-equal.</para>
        /// </remarks>
        public static IEnumerable<T> DistinctSorted<T>(this IEnumerable<T> @this, IEqualityComparer<T> comp = null)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.ToOccurancesSorted(comp).Select(a => a.Item1);
        }
    }
}
