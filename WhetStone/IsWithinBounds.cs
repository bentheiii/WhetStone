using System;
using System.Collections.Generic;
using System.Linq;
using NumberStone;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class isWithinBounds
    {
        /// <overloads>Check whether an index is valid for a random-access container.</overloads>
        /// <summary>
        /// Get whether a multi-dimensional index is valid for an <see cref="Array"/>.
        /// </summary>
        /// <param name="arr">The <see cref="Array"/> to check.</param>
        /// <param name="ind">The index to check for.</param>
        /// <returns>Whether <paramref name="ind"/> is a valid index for <paramref name="arr"/>.</returns>
        /// <exception cref="ArgumentException">If <paramref name="ind"/> is the wrong size for <paramref name="arr"/></exception>
        public static bool IsWithinBounds(this Array arr, params int[] ind)
        {
            arr.ThrowIfNull(nameof(arr));
            ind.ThrowIfNull(nameof(ind));
            if (arr.Rank != ind.Length)
                throw new ArgumentException("mismatch on index length");
            return arr.GetBounds().Zip(ind).All(a => a.Item2.iswithinPartialExclusive(a.Item1.Item1, a.Item1.Item2));
        }
        /// <summary>
        /// Get whether an index is valid for an <see cref="IList{T}"/>.
        /// </summary>
        /// <param name="arr">The <see cref="IList{T}"/> to check.</param>
        /// <param name="ind">The index to check for.</param>
        /// <returns>Whether <paramref name="ind"/> is a valid index for <paramref name="arr"/>.</returns>
        public static bool IsWithinBounds<T>(this IList<T> arr, int ind)
        {
            arr.ThrowIfNull(nameof(arr));
            return ind.iswithinPartialExclusive(0, arr.Count);
        }
        /// <summary>
        /// Get whether an index is valid for an <see cref="IList{T}"/>.
        /// </summary>
        /// <param name="arr">The <see cref="IList{T}"/> to check.</param>
        /// <param name="ind">The index to check for.</param>
        /// <returns>Whether <paramref name="ind"/> is a valid index for <paramref name="arr"/>.</returns>
        public static bool IsWithinBounds<T>(this T[] arr, int ind)
        {
            arr.ThrowIfNull(nameof(arr));
            return IsWithinBounds((Array)arr, ind);
        }
    }
}
