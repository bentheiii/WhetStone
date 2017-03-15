using System;
using System.Collections.Generic;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class anyAndAll
    {
        /// <overloads>Checks that an <see cref="IEnumerable{T}"/> is all true and also that it is not empty.</overloads>
        /// <summary>
        /// Checks that all members of the <see cref="IEnumerable{T}"/> are <see langword="true"/> (when subject to <paramref name="cond"/>) and that the <see cref="IEnumerable{T}"/> is not empty.
        /// </summary>
        /// <typeparam name="T">The <see cref="IEnumerable{T}"/>'s type.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to check.</param>
        /// <param name="cond">The selector to use on each element to check for trueness.</param>
        /// <returns>Whether <paramref name="cond"/> evaluates all elements to <see langword="true"/> and <paramref name="this"/> is not empty.</returns>
        public static bool AnyAndAll<T>(this IEnumerable<T> @this, Func<T,bool> cond)
        {
            @this.ThrowIfNull(nameof(@this));
            cond.ThrowIfNull(nameof(cond));
            using (var tor = @this.GetEnumerator())
            {
                if (!tor.MoveNext() || !cond(tor.Current))
                    return false;
                while (tor.MoveNext())
                {
                    if (!cond(tor.Current))
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks that all members of the <see cref="IEnumerable{T}"/> of type <see cref="bool"/> are <see langword="true"/> and that the <see cref="IEnumerable{T}"/> of type <see cref="bool"/> is not empty.
        /// </summary>
        /// <param name="this">The <see cref="IEnumerable{T}"/> of type <see cref="bool"/> to check.</param>
        /// <returns>Whether all elements are <see langword="true"/> and <paramref name="this"/> is not empty.</returns>
        public static bool AnyAndAll(this IEnumerable<bool> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            using (var tor = @this.GetEnumerator())
            {
                if (!tor.MoveNext() || !tor.Current)
                    return false;
                while (tor.MoveNext())
                {
                    if (!tor.Current)
                        return false;
                }
            }
            return true;
        }
    }
}
