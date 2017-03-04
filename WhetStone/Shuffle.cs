using System;
using System.Collections.Generic;
using WhetStone.Random;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class shuffle
    {
        /// <summary>
        /// Shuffles an <see cref="IList{T}"/>'s elements using algorithm P.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="arr">The <see cref="IList{T}"/> to shuffle.</param>
        /// <param name="gen">The <see cref="RandomGenerator"/> to use. <see langword="null"/> for <see cref="GlobalRandomGenerator"/>.</param>
        /// <exception cref="ArgumentException"><paramref name="arr"/> is read-only</exception>
        public static void Shuffle<T>(this IList<T> arr, RandomGenerator gen = null)
        {
            if (arr.IsReadOnly)
                throw new ArgumentException("array is read-only");
            gen = gen ?? new GlobalRandomGenerator();
            foreach (int i in range.Range(arr.Count))
            {
                int j = gen.Int(i, arr.Count);
                if (i == j)
                    continue;
                T temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
            }
        }
    }
}
