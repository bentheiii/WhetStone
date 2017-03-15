using System.Collections.Generic;
using System.Linq;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class duplicates
    {
        /// <summary>
        /// Filters out all elements in an <see cref="IEnumerable{T}"/> that don't appear enough times.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="arr">The <see cref="IEnumerable{T}"/> to filter.</param>
        /// <param name="comp">The <see cref="IEqualityComparer{T}"/> to use to compare elements. <see langword="null"/> will use the default <see cref="IEqualityComparer{T}"/></param>
        /// <param name="minoccurances">The minimum number of times an element must appear in order to not be filtered out.</param>
        /// <returns>A filtered <see cref="IEnumerable{T}"/> with only elements in <paramref name="arr"/> that appear at least <paramref name="minoccurances"/> times.</returns>
        /// <remarks>Space complexity: O(|arr|)</remarks>
        public static IEnumerable<T> Duplicates<T>(this IEnumerable<T> arr, IEqualityComparer<T> comp = null, int minoccurances = 2)
        {
            arr.ThrowIfNull(nameof(arr));
            minoccurances.ThrowIfAbsurd(nameof(minoccurances), false);
            comp = comp ?? EqualityComparer<T>.Default;
            var occurances = new Dictionary<T, int>(comp);
            foreach (var t in arr)
            {
                int olval;
                bool exists = occurances.TryGetValue(t, out olval);
                int newval;
                if (!exists)
                {
                    newval = 1;
                }
                else
                {
                    if (olval == 0)
                        continue;
                    newval = olval + 1;
                }
                if (newval >= minoccurances)
                {
                    yield return t;
                    newval = 0;
                }
                occurances[t] = newval;
            }
        }
        /// <summary>
        /// Filters out all elements in a sorted <see cref="IEnumerable{T}"/> that don't appear enough times.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="arr">The sorted <see cref="IEnumerable{T}"/> to filter.</param>
        /// <param name="comp">The <see cref="IEqualityComparer{T}"/> to use to compare elements. <see langword="null"/> will use the default <see cref="IEqualityComparer{T}"/></param>
        /// <param name="minoccurances">The minimum number of times an element must appear in order to not be filtered out.</param>
        /// <returns>A filtered <see cref="IEnumerable{T}"/> with only elements in <paramref name="arr"/> that appear at least <paramref name="minoccurances"/> times.</returns>
        /// <remarks>
        /// <para><paramref name="arr"/> doesn't have to be sorted, it just has to have all elements equal to each other adjacent.</para>
        /// <para>Alternately, all non-adjacent equal elements will be treated as non-equal.</para>
        /// </remarks>
        public static IEnumerable<T> DuplicatesSorted<T>(this IEnumerable<T> arr, IEqualityComparer<T> comp = null, int minoccurances = 2)
        {
            arr.ThrowIfNull(nameof(arr));
            minoccurances.ThrowIfAbsurd(nameof(minoccurances), false);

            comp = comp ?? EqualityComparer<T>.Default;
            return arr.ToOccurancesSorted(comp).Where(a => a.Item2 >= minoccurances).Select(a => a.Item1);
        }
    }
}
