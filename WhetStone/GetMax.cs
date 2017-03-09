using System;
using System.Collections.Generic;
using WhetStone.Comparison;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class getMax
    {
        /// <overloads>Get the maximum element in an enumerable.</overloads>
        /// <summary>
        /// Get the largest element in an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="tosearch">The <see cref="IEnumerable{T}"/> to search in.</param>
        /// <param name="compare">The <see cref="IComparer{T}"/> to find the smallest element.</param>
        /// <param name="index">The index of the largest element.</param>
        /// <returns>The largest element in <paramref name="tosearch"/>.</returns>
        /// <exception cref="ArgumentException">If <paramref name="tosearch"/> is empty.</exception>
        public static T GetMax<T>(this IEnumerable<T> tosearch, IComparer<T> compare, out int index)
        {
            tosearch.ThrowIfNull(nameof(tosearch));
            compare.ThrowIfNull(nameof(compare));
            return tosearch.GetMin(compare.Reverse(), out index);
        }
        /// <overloads>Get the maximum element in an enumerable.</overloads>
        /// <summary>
        /// Get the largest element in an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="tosearch">The <see cref="IEnumerable{T}"/> to search in.</param>
        /// <param name="compare">The <see cref="IComparer{T}"/> to find the smallest element. <see langword="null"/> means the default comparer will be used.</param>
        /// <returns>The largest element in <paramref name="tosearch"/>.</returns>
        /// <exception cref="ArgumentException">If <paramref name="tosearch"/> is empty.</exception>
        public static T GetMax<T>(this IEnumerable<T> tosearch, IComparer<T> compare = null)
        {
            tosearch.ThrowIfNull(nameof(tosearch));
            int prox;
            return tosearch.GetMax(compare ?? Comparer<T>.Default, out prox);
        }
        /// <overloads>Get the maximum element in an enumerable.</overloads>
        /// <summary>
        /// Get the largest element in an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="tosearch">The <see cref="IEnumerable{T}"/> to search in.</param>
        /// <param name="index">The index of the largest element.</param>
        /// <returns>The largest element in <paramref name="tosearch"/>.</returns>
        /// <exception cref="ArgumentException">If <paramref name="tosearch"/> is empty.</exception>
        public static T GetMax<T>(this IEnumerable<T> tosearch, out int index)
        {
            tosearch.ThrowIfNull(nameof(tosearch));
            return tosearch.GetMax(Comparer<T>.Default, out index);
        }
    }
}
