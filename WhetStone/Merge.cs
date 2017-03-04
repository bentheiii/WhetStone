using System;
using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class merge
    {
        /// <summary>
        /// Combines multiple sorted <see cref="IEnumerable{T}"/>s to a single sorted <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>s</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/>s to combine.</param>
        /// <param name="chooser">The <see cref="IComparer{T}"/> to sort the members. <see langword="null"/> means the default comparer will be used.</param>
        /// <returns>A new <see cref="IEnumerable{T}"/> composed of <paramref name="this"/>'s members combined.</returns>
        public static IEnumerable<T> Merge<T>(this IEnumerable<IEnumerable<T>> @this, IComparer<T> chooser = null)
        {
            chooser = chooser ?? Comparer<T>.Default;
            var numerators = new List<IEnumerator<T>>(@this.Select(a => a.GetEnumerator()));
            numerators.RemoveAll(a => !a.MoveNext());
            while (numerators.Any())
            {
                int index;
                numerators.Select(a => a.Current).ToArray().GetMin(chooser, out index);
                yield return numerators[index].Current;
                if (!numerators[index].MoveNext())
                    numerators.RemoveAt(index);
            }
        }
        /// <summary>
        /// Combines two sorted <see cref="IEnumerable{T}"/>s to a single sorted <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>s</typeparam>
        /// <param name="this">The first <see cref="IEnumerable{T}"/> to combine.</param>
        /// <param name="other">The second <see cref="IEnumerable{T}"/> to combine.</param>
        /// <param name="chooser">The <see cref="IComparer{T}"/> to sort the members. <see langword="null"/> means the default comparer will be used.</param>
        /// <returns></returns>
        public static IEnumerable<T> Merge<T>(this IEnumerable<T> @this, IEnumerable<T> other, IComparer<T> chooser = null)
        {
            return new[] { @this, other }.Merge(chooser);
        }
    }
}
