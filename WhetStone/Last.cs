using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class last
    {
        /// <overloads>Get the last element in an enumerable, or a default value if none found.</overloads>
        /// <summary>
        /// Get the last element in an <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to search.</param>
        /// <param name="def">The default value to return if no elements in <paramref name="this"/> are found.</param>
        /// <returns>The last element in <paramref name="this"/>.</returns>
        public static T LastOrDefault<T>(this IEnumerable<T> @this, T def)
        {
            var l = @this.AsList(false);
            if (l != null)
            {
                return l.Count == 0 ? def : l[l.Count-1];
            }
            var ret = def;
            foreach (T t in @this)
            {
                ret = t;
            }
            return ret;
        }
        /// <summary>
        /// Get the last element in an <see cref="IEnumerable{T}"/> to fulfill a condition.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to search.</param>
        /// <param name="cond">The condition to consider elements after.</param>
        /// <param name="def">The default value to return if no elements in <paramref name="this"/> are found.</param>
        /// <returns>The last element in <paramref name="this"/> to uphold <paramref name="cond"/>.</returns>
        public static T LastOrDefault<T>(this IEnumerable<T> @this, Func<T, bool> cond, T def = default(T))
        {
            var l = @this.AsList(false);
            if (l != null)
            {
                return l.Reverse().FirstOrDefault(cond, def);
            }
            var ret = def;
            foreach (T t in @this)
            {
                if (cond(t))
                    ret = t;
            }
            return ret;
        }
        /// <summary>
        /// Get the last element in an <see cref="IEnumerable{T}"/> to fulfill a condition.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to search.</param>
        /// <param name="cond">The condition to consider elements after.</param>
        /// <param name="any">Whether an element was found or not.</param>
        /// <param name="def">The default value to return if no elements in <paramref name="this"/> are found.</param>
        /// <returns>The last element in <paramref name="this"/> to uphold <paramref name="cond"/>.</returns>
        public static T LastOrDefault<T>(this IEnumerable<T> @this, Func<T, bool> cond, out bool any, T def = default(T))
        {
            var l = @this.AsList(false);
            if (l != null)
            {
                return l.Reverse().FirstOrDefault(cond, out any, def);
            }
            T ret;
            any = false;
            using (var tor = @this.GetEnumerator())
            {
                while (true)
                {
                    if (!tor.MoveNext())
                        return def;
                    if (cond(tor.Current))
                    {
                        any = true;
                        ret = tor.Current;
                        break;
                    }
                }
                while (!tor.MoveNext())
                {
                    if (cond(tor.Current))
                        ret = tor.Current;
                }
            }
            return ret;
        }
    }
}
