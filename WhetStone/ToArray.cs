using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class toArray
    {
        /// <summary>
        /// Creates an <see cref="Array"/> and fills it with an <see cref="IEnumerable{T}"/>'s elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to take elements from.</param>
        /// <param name="capacity">The expected size of <paramref name="this"/>.</param>
        /// <param name="limitToCapacity">Whether to avoid expanding the array past its initial capacity.</param>
        /// <returns>An array with <paramref name="this"/>'s elements.</returns>
        /// <remarks>If <paramref name="limitToCapacity"/> is set to <see langword="true"/>, The resultant array will be returned as soon as the array is filled to capacity.</remarks>
        public static T[] ToArray<T>(this IEnumerable<T> @this, int capacity, bool limitToCapacity = false)
        {
            T[] ret = new T[capacity <= 0 ? 1 : capacity];
            int i = 0;
            foreach (T t in @this)
            {
                if (ret.Length <= i)
                {
                    if (limitToCapacity)
                        return ret;
                    Array.Resize(ref ret, ret.Length * 2);
                }
                ret[i] = t;
                i++;
            }
            Array.Resize(ref ret, i);
            return ret;
        }
    }
}
