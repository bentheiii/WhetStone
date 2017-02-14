using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class indices
    {
        /// <overloads>Get all valid indices on the list</overloads>
        /// <summary>
        /// Get all possible indices on the <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/></typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to get indices of.</param>
        /// <returns>A read-only <see cref="IList{T}"/> of all valid indices of <paramref name="this"/></returns>
        public static IList<int> Indices<T>(this IList<T> @this)
        {
            return range.Range(@this.Count);
        }
        /// <summary>
        /// Get all coordinates of an <see cref="Array"/>.
        /// </summary>
        /// <param name="this">The <see cref="Array"/> to get all coordinates of.</param>
        /// <returns>a read-only <see cref="IList{T}"/> of all valid coordinates of <paramref name="this"/></returns>
        public static IList<int[]> Indices(this Array @this)
        {
            return range.Range(@this.Rank).Select(a => range.Range(@this.GetLowerBound(a), @this.GetUpperBound(a)+1).AsList()).Join();
        }
        /// <summary>
        /// Get all indices of a 1D <see cref="Array"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Array"/></typeparam>
        /// <param name="this">The 1D <see cref="Array"/> to get all indices of.</param>
        /// <returns>a read-only <see cref="IList{T}"/> of all valid indices of <paramref name="this"/></returns>
        public static IList<int> Indices<T>(this T[] @this)
        {
            return range.Range(@this.GetLowerBound(0), @this.GetUpperBound(0)+1);
        }
    }
}
