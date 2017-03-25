using System.Collections.Generic;
using System.Linq;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class subsets
    {
        /// <summary>
        /// Get all the subsets from an <see cref="IEnumerable{T}"/>'s elements.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to use.</param>
        /// <returns>All the (element-wise) subsets of <paramref name="this"/>.</returns>
        public static IList<IEnumerable<T>> SubSets<T>(this IEnumerable<T> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return new[] {false, true}.Join(@this.Count()).Select(a => @this.Zip(a).Where(x => x.Item2).Select(x=>x.Item1));
        }
        /// <summary>
        /// Get all the subsets from an <see cref="IList{T}"/>'s elements.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to use.</param>
        /// <returns>All the (element-wise) subsets of <paramref name="this"/>.</returns>
        public static IList<IList<T>> SubSets<T>(this IList<T> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return range.IRange(@this.Count).Select(@this.SubSets).Concat();
        }
        /// <summary>
        /// Get all the subsets from an <see cref="IEnumerable{T}"/>'s elements of a specific size.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to use.</param>
        /// <param name="setSize">The size of the subsets.</param>
        /// <returns>All the (element-wise) subsets of <paramref name="this"/> of size <paramref name="setSize"/>.</returns>
        public static IEnumerable<IEnumerable<T>> SubSets<T>(this IEnumerable<T> @this, int setSize)
        {
            @this.ThrowIfNull(nameof(@this));
            setSize.ThrowIfAbsurd(nameof(setSize));
            return @this.Join(setSize, join.CartesianType.NoReflexive | join.CartesianType.NoSymmatry);
        }
        /// <summary>
        /// Get all the subsets from an <see cref="IList{T}"/>'s elements of a specific size.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to use.</param>
        /// <param name="setSize">The size of the subsets.</param>
        /// <returns>All the (element-wise) subsets of <paramref name="this"/> of size <paramref name="setSize"/>.</returns>
        public static IList<IList<T>> SubSets<T>(this IList<T> @this, int setSize)
        {
            @this.ThrowIfNull(nameof(@this));
            setSize.ThrowIfAbsurd(nameof(setSize));
            return @this.Join(setSize, join.CartesianType.NoReflexive | join.CartesianType.NoSymmatry);
        }
    }
}
