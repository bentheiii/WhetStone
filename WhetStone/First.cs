using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class first
    {
        /// <summary>
        /// Get the <see cref="IEnumerable{T}"/>'s first value or a default value.
        /// </summary>
        /// <typeparam name="T">The <see cref="IEnumerable{T}"/>'s type.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to check.</param>
        /// <param name="def">The value to return if <paramref name="this"/> is empty.</param>
        /// <returns>The first value in <paramref name="this"/> or <paramref name="def"/> if <paramref name="this"/> is empty.</returns>
        public static T FirstOrDefault<T>(this IEnumerable<T> @this, T def)
        {
            using (var tor = @this.GetEnumerator())
            {
                return !tor.MoveNext() ? def : tor.Current;
            }
        }
        /// <summary>
        /// Get the <see cref="IEnumerable{T}"/>'s first value to fulfill a condition or a default value.
        /// </summary>
        /// <typeparam name="T">The <see cref="IEnumerable{T}"/>'s type.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to check.</param>
        /// <param name="cond">The condition to qualify members for being returned.</param>
        /// <param name="def">The value to return if <paramref name="this"/> is empty of qualified members.</param>
        /// <returns>The first value in <paramref name="this"/> to pass <paramref name="cond"/> or <paramref name="def"/> if none to.</returns>
        public static T FirstOrDefault<T>(this IEnumerable<T> @this, Func<T, bool> cond, T def)
        {
            foreach (T t in @this)
            {
                if (cond(t))
                    return t;
            }
            return def;
        }
        /// <summary>
        /// Get the <see cref="IEnumerable{T}"/>'s first value to fulfill a condition or a default value.
        /// </summary>
        /// <typeparam name="T">The <see cref="IEnumerable{T}"/>'s type.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to check.</param>
        /// <param name="cond">The condition to qualify members for being returned.</param>
        /// <param name="any">Whether an element in <paramref name="this"/> was found or the default value was returned.</param>
        /// <param name="def">The value to return if <paramref name="this"/> is empty of qualified members.</param>
        /// <returns>The first value in <paramref name="this"/> to pass <paramref name="cond"/> or <paramref name="def"/> if none to.</returns>
        public static T FirstOrDefault<T>(this IEnumerable<T> @this, Func<T, bool> cond, out bool any, T def = default(T))
        {
            foreach (T t in @this)
            {
                if (cond(t))
                {
                    any = true;
                    return t;
                }
            }
            any = false;
            return def;
        }
    }
}
