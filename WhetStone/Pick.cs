using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Looping;
using WhetStone.SystemExtensions;

namespace WhetStone.Random
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class pick
    {
        /// <summary>
        /// Get a random element in an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to search in.</param>
        /// <param name="gen">The <see cref="RandomGenerator"/> to use to roll for a random element. <see langword="null"/> for <see cref="GlobalRandomGenerator"/>.</param>
        /// <returns>A random element from <paramref name="this"/>.</returns>
        /// <remarks>Running time: O(|<paramref name="this"/>|)</remarks>
        public static T Pick<T>(this IEnumerable<T> @this, RandomGenerator gen = null)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.Pick(1, gen).Single();
        }
        /// <summary>
        /// Get random elements in an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to search in.</param>
        /// <param name="count">The number of elements to return.</param>
        /// <param name="gen">The <see cref="RandomGenerator"/> to use to roll for a random element. <see langword="null"/> for <see cref="GlobalRandomGenerator"/>.</param>
        /// <returns><paramref name="count"/> random elements from <paramref name="this"/>.</returns>
        /// <remarks>Running time: O(|<paramref name="this"/>| * <paramref name="count"/> / (<paramref name="count"/> + 1))</remarks>
        public static IEnumerable<T> Pick<T>(this IEnumerable<T> @this, int count, RandomGenerator gen = null)
        {
            @this.ThrowIfNull(nameof(@this));
            count.ThrowIfAbsurd(nameof(count));
            gen = gen ?? new GlobalRandomGenerator();
            int nom = count;
            int denom = @this.Count();
            if (nom > denom || nom < 0)
                throw new ArgumentException();
            foreach (var t in @this)
            {
                if (nom == 0)
                    yield break;
                if (gen.success(nom / (double)denom))
                {
                    yield return t;
                    nom--;
                }
                denom--;
            }
        }
        /// <summary>
        /// Get a random element in an <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to search in.</param>
        /// <param name="gen">The <see cref="RandomGenerator"/> to use to roll for a random element. <see langword="null"/> for <see cref="GlobalRandomGenerator"/>.</param>
        /// <returns>A random element from <paramref name="this"/>.</returns>
        public static T Pick<T>(this IList<T> @this, RandomGenerator gen = null)
        {
            @this.ThrowIfNull(nameof(@this));
            gen = gen ?? new GlobalRandomGenerator();
            return @this[gen.Int(@this.Count)];
        }
        /// <summary>
        /// Get random elements in an <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to search in.</param>
        /// <param name="count">The number of elements to return.</param>
        /// <param name="gen">The <see cref="RandomGenerator"/> to use to roll for a random element. <see langword="null"/> for <see cref="GlobalRandomGenerator"/>.</param>
        /// <returns><paramref name="count"/> random elements from <paramref name="this"/>.</returns>
        public static IList<T> Pick<T>(this IList<T> @this, int count, RandomGenerator gen = null)
        {
            @this.ThrowIfNull(nameof(@this));
            count.ThrowIfAbsurd(nameof(count));
            return range.Range(@this.Count).Join(count, join.CartesianType.NoReflexive | join.CartesianType.NoSymmatry).Pick(gen).Select(a => @this[a]).Reverse();
        }
    }
}
