using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Guard;

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
            index = -1;
            var ret = default(T);
            IGuard<int> ind = new Guard<int>();
            foreach (T t in tosearch.CountBind().Detach(ind))
            {
                if (index < 0 || compare.Compare(t, ret) < 0)
                {
                    index = ind.value;
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
            return tosearch.GetMin(Comparer<T>.Default, out index);
        }
    }
}
