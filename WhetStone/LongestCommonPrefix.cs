using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class longestCommonPrefix
    {
        /// <summary>
        /// Get the longest common prefix of an <see cref="IEnumerable{T}"/> or <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>s.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/>s to check.</param>
        /// <returns>The Longest common prefix shared by all of <paramref name="this"/>.</returns>
        public static IEnumerable<T> LongestCommonPrefix<T>(this IEnumerable<IEnumerable<T>> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return LongestCommonPrefix<T>(@this, null).First();
        }
        /// <summary>
        /// Get the longest common prefixes of an <see cref="IEnumerable{T}"/> or <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>s.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/>s to check.</param>
        /// <param name="comp">The <see cref="IEqualityComparer{T}"/> to compare prefix elements. <see langword="null"/> for the default <see cref="IEqualityComparer{T}"/>.</param>
        /// <returns>The Longest common prefixes shared by all of <paramref name="this"/> under <paramref name="comp"/>.</returns>
        public static IEnumerable<IEnumerable<T>> LongestCommonPrefix<T>(this IEnumerable<IEnumerable<T>> @this, IEqualityComparer<T> comp)
        {
            @this.ThrowIfNull(nameof(@this));
            comp = comp ?? EqualityComparer<T>.Default;
            return @this.Zip().TakeWhile(a => a.AllEqual(comp));
        }
        /// <summary>
        /// Get the longest common prefix of two <see cref="IEnumerable{T}"/>s.
        /// </summary>
        /// <typeparam name="T">The elements of the <see cref="IEnumerable{T}"/>s.</typeparam>
        /// <param name="this">The first <see cref="IEnumerable{T}"/>.</param>
        /// <param name="other">The second <see cref="IEnumerable{T}"/>.</param>
        /// <returns>The Longest common prefixes shared by <paramref name="this"/> and <paramref name="other"/>.</returns>
        public static IEnumerable<T> LongestCommonPrefix<T>(this IEnumerable<T> @this, IEnumerable<T> other)
        {
            @this.ThrowIfNull(nameof(@this));
            other.ThrowIfNull(nameof(other));
            return LongestCommonPrefix(@this, other, null).Select(a=>a.Item1);
        }
        /// <summary>
        /// Get the longest common prefixes of two <see cref="IEnumerable{T}"/>s.
        /// </summary>
        /// <typeparam name="T">The elements of the <see cref="IEnumerable{T}"/>s.</typeparam>
        /// <param name="this">The first <see cref="IEnumerable{T}"/>.</param>
        /// <param name="other">The second <see cref="IEnumerable{T}"/>.</param>
        /// <param name="comp">The <see cref="IEqualityComparer{T}"/> to compare prefix elements. <see langword="null"/> for the default <see cref="IEqualityComparer{T}"/>.</param>
        /// <returns>The Longest common prefixes shared by <paramref name="this"/> and <paramref name="other"/> under <paramref name="comp"/>.</returns>
        public static IEnumerable<Tuple<T, T>> LongestCommonPrefix<T>(this IEnumerable<T> @this, IEnumerable<T> other, IEqualityComparer<T> comp)
        {
            @this.ThrowIfNull(nameof(@this));
            other.ThrowIfNull(nameof(other));
            comp = comp ?? EqualityComparer<T>.Default;
            return @this.Zip(other).TakeWhile(a => comp.Equals(a.Item1, a.Item2));
        }
    }
}
