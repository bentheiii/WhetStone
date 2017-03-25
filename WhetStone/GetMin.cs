using System;
using System.Collections.Generic;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class getMin
    {
        /// <overloads>Get the minimum element in an enumerable.</overloads>
        /// <summary>
        /// Get the smallest element in an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="tosearch">The <see cref="IEnumerable{T}"/> to search in.</param>
        /// <param name="compare">The <see cref="IComparer{T}"/> to find the smallest element.</param>
        /// <param name="index">The index of the largest element.</param>
        /// <returns>The smallest element in <paramref name="tosearch"/>.</returns>
        /// <exception cref="ArgumentException">If <paramref name="tosearch"/> is empty.</exception>
        public static T GetMin<T>(this IEnumerable<T> tosearch, IComparer<T> compare, out int index)
        {
            tosearch.ThrowIfNull(nameof(tosearch));
            compare.ThrowIfNull(nameof(compare));
            index = -1;
            var ret = default(T);
            foreach (var tu in tosearch.CountBind())
            {
                var t = tu.Item1;
                var ind = tu.Item2;
                if (index < 0 || compare.Compare(t, ret) < 0)
                {
                    index = ind;
                    ret = t;
                }
            }
            if (index == -1)
                throw new ArgumentException("enumerable cannot be empty!");
            return ret;
        }
        /// <overloads>Get the minimum element in an enumerable.</overloads>
        /// <summary>
        /// Get the smallest element in an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="tosearch">The <see cref="IEnumerable{T}"/> to search in.</param>
        /// <param name="compare">The <see cref="IComparer{T}"/> to find the smallest element. <see langword="null"/> means the default comparer will be used.</param>
        /// <returns>The smallest element in <paramref name="tosearch"/>.</returns>
        /// <exception cref="ArgumentException">If <paramref name="tosearch"/> is empty.</exception>
        public static T GetMin<T>(this IEnumerable<T> tosearch, IComparer<T> compare = null)
        {
            tosearch.ThrowIfNull(nameof(tosearch));
            int prox;
            return tosearch.GetMin(compare ?? Comparer<T>.Default, out prox);
        }
        /// <summary>
        /// Get the smallest element in an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="tosearch">The <see cref="IEnumerable{T}"/> to search in.</param>
        /// <param name="index">The index of the largest element.</param>
        /// <returns>The smallest element in <paramref name="tosearch"/>.</returns>
        /// <exception cref="ArgumentException">If <paramref name="tosearch"/> is empty.</exception>
        public static T GetMin<T>(this IEnumerable<T> tosearch, out int index)
        {
            tosearch.ThrowIfNull(nameof(tosearch));
            return tosearch.GetMin(Comparer<T>.Default, out index);
        }
    }
}
