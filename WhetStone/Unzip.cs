using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class unZip
    {
        /// <summary>
        /// Splits an <see cref="IEnumerable{T}"/> of <see cref="Tuple"/>s to separate <see cref="IEnumerable{T}"/>s.
        /// </summary>
        /// <typeparam name="T1">The first type of the <see cref="Tuple"/>.</typeparam>
        /// <typeparam name="T2">The second type of the <see cref="Tuple"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to split.</param>
        /// <returns>The individual <see cref="IEnumerable{T}"/>s.</returns>
        public static Tuple<IEnumerable<T1>, IEnumerable<T2>> UnZip<T1, T2>(this IEnumerable<Tuple<T1, T2>> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return Tuple.Create(@this.Select(a => a.Item1), @this.Select(a => a.Item2));
        }
        /// <summary>
        /// Splits an <see cref="IList{T}"/> of <see cref="Tuple"/>s to separate <see cref="IList{T}"/>s.
        /// </summary>
        /// <typeparam name="T1">The first type of the <see cref="Tuple"/>.</typeparam>
        /// <typeparam name="T2">The second type of the <see cref="Tuple"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to split.</param>
        /// <returns>The individual <see cref="IList{T}"/>s.</returns>
        public static Tuple<IList<T1>, IList<T2>> UnZip<T1, T2>(this IList<Tuple<T1, T2>> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return Tuple.Create(@this.Select(a => a.Item1).AsList(), @this.Select(a => a.Item2).AsList());
        }
        /// <summary>
        /// Splits an <see cref="IEnumerable{T}"/> of <see cref="Tuple"/>s to separate <see cref="IEnumerable{T}"/>s.
        /// </summary>
        /// <typeparam name="T1">The first type of the <see cref="Tuple"/>.</typeparam>
        /// <typeparam name="T2">The second type of the <see cref="Tuple"/>.</typeparam>
        /// <typeparam name="T3">The third type of the <see cref="Tuple"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to split.</param>
        /// <returns>The individual <see cref="IEnumerable{T}"/>s.</returns>
        public static Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> UnZip<T1, T2, T3>(this IEnumerable<Tuple<T1, T2, T3>> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return Tuple.Create(@this.Select(a => a.Item1), @this.Select(a => a.Item2), @this.Select(a => a.Item3));
        }
        /// <summary>
        /// Splits an <see cref="IList{T}"/> of <see cref="Tuple"/>s to separate <see cref="IList{T}"/>s.
        /// </summary>
        /// <typeparam name="T1">The first type of the <see cref="Tuple"/>.</typeparam>
        /// <typeparam name="T2">The second type of the <see cref="Tuple"/>.</typeparam>
        /// <typeparam name="T3">The third type of the <see cref="Tuple"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to split.</param>
        /// <returns>The individual <see cref="IList{T}"/>s.</returns>
        public static Tuple<IList<T1>, IList<T2>, IList<T3>> UnZip<T1, T2, T3>(this IList<Tuple<T1, T2, T3>> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return Tuple.Create(@this.Select(a => a.Item1).AsList(), @this.Select(a => a.Item2).AsList(), @this.Select(a => a.Item3).AsList());
        }
        /// <summary>
        /// Splits an <see cref="IEnumerable{T}"/> of <see cref="Tuple"/>s to separate <see cref="IEnumerable{T}"/>s.
        /// </summary>
        /// <typeparam name="T1">The first type of the <see cref="Tuple"/>.</typeparam>
        /// <typeparam name="T2">The second type of the <see cref="Tuple"/>.</typeparam>
        /// <typeparam name="T3">The third type of the <see cref="Tuple"/>.</typeparam>
        /// <typeparam name="T4">The fourth type of the <see cref="Tuple"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to split.</param>
        /// <returns>The individual <see cref="IEnumerable{T}"/>s.</returns>
        public static Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>> UnZip<T1, T2, T3, T4>(this IEnumerable<Tuple<T1, T2, T3, T4>> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return Tuple.Create(@this.Select(a => a.Item1), @this.Select(a => a.Item2), @this.Select(a => a.Item3), @this.Select(a=>a.Item4));
        }
        /// <summary>
        /// Splits an <see cref="IList{T}"/> of <see cref="Tuple"/>s to separate <see cref="IList{T}"/>s.
        /// </summary>
        /// <typeparam name="T1">The first type of the <see cref="Tuple"/>.</typeparam>
        /// <typeparam name="T2">The second type of the <see cref="Tuple"/>.</typeparam>
        /// <typeparam name="T3">The third type of the <see cref="Tuple"/>.</typeparam>
        /// <typeparam name="T4">The fourth type of the <see cref="Tuple"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to split.</param>
        /// <returns>The individual <see cref="IList{T}"/>s.</returns>
        public static Tuple<IList<T1>, IList<T2>, IList<T3>, IList<T4>> UnZip<T1, T2, T3, T4>(this IList<Tuple<T1, T2, T3, T4>> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return Tuple.Create(@this.Select(a => a.Item1).AsList(), @this.Select(a => a.Item2).AsList(), @this.Select(a => a.Item3).AsList(), @this.Select(a => a.Item4).AsList());
        }
        /// <summary>
        /// Splits an <see cref="IEnumerable{T}"/> of <see cref="Tuple"/>s to separate <see cref="IEnumerable{T}"/>s.
        /// </summary>
        /// <typeparam name="T1">The first type of the <see cref="Tuple"/>.</typeparam>
        /// <typeparam name="T2">The second type of the <see cref="Tuple"/>.</typeparam>
        /// <typeparam name="T3">The third type of the <see cref="Tuple"/>.</typeparam>
        /// <typeparam name="T4">The fourth type of the <see cref="Tuple"/>.</typeparam>
        /// <typeparam name="T5">The fifth type of the <see cref="Tuple"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to split.</param>
        /// <returns>The individual <see cref="IEnumerable{T}"/>s.</returns>
        public static Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>> UnZip<T1, T2, T3, T4, T5>(this IEnumerable<Tuple<T1, T2, T3, T4, T5>> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return Tuple.Create(@this.Select(a => a.Item1), @this.Select(a => a.Item2), @this.Select(a => a.Item3), @this.Select(a => a.Item4), @this.Select(a => a.Item5));
        }
        /// <summary>
        /// Splits an <see cref="IList{T}"/> of <see cref="Tuple"/>s to separate <see cref="IList{T}"/>s.
        /// </summary>
        /// <typeparam name="T1">The first type of the <see cref="Tuple"/>.</typeparam>
        /// <typeparam name="T2">The second type of the <see cref="Tuple"/>.</typeparam>
        /// <typeparam name="T3">The third type of the <see cref="Tuple"/>.</typeparam>
        /// <typeparam name="T4">The fourth type of the <see cref="Tuple"/>.</typeparam>
        /// <typeparam name="T5">The fifth type of the <see cref="Tuple"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to split.</param>
        /// <returns>The individual <see cref="IList{T}"/>s.</returns>
        public static Tuple<IList<T1>, IList<T2>, IList<T3>, IList<T4>, IList<T5>> UnZip<T1, T2, T3, T4, T5>(this IList<Tuple<T1, T2, T3, T4, T5>> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return Tuple.Create(@this.Select(a => a.Item1).AsList(), @this.Select(a => a.Item2).AsList(), @this.Select(a => a.Item3).AsList(), @this.Select(a => a.Item4).AsList(), @this.Select(a => a.Item5).AsList());
        }
    }
}
