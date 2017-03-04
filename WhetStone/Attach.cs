using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class attach
    {
        /// <summary>
        /// Turns each element in an <see cref="IEnumerable{T}"/> to a tuple including itself and an output of itself and a selector function.
        /// </summary>
        /// <typeparam name="T1">The type of the original <see cref="IEnumerable{T}"/></typeparam>
        /// <typeparam name="T2">The type of <paramref name="selector"/>'s output.</typeparam>
        /// <param name="this">The original <see cref="IEnumerable{T}"/></param>
        /// <param name="selector">The function from which to get the output as the second member of the tuple.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of type <see cref="Tuple{T1,T2}"/> of <paramref name="this"/>'s elements with the output of the selector function applied to them.</returns>
        /// <remarks><c>enumerable.Attach(selector)</c> is functionally identical to <c>enumerable.Zip(enumerable.Select(selector))</c>. Although here, the enumerable is enumerated only once per output enumeration.</remarks>
        public static IEnumerable<Tuple<T1, T2>> Attach<T1, T2>(this IEnumerable<T1> @this, Func<T1, T2> selector)
        {
            return @this.Select(a => Tuple.Create(a, selector(a)));
        }
        /// <summary>
        /// Turns each element in an <see cref="IEnumerable{T}"/> of type <see cref="Tuple{T1,T2}"/> to a tuple including itself and an output of itself and a selector function.
        /// </summary>
        /// <typeparam name="T1">The first type of the original <see cref="IEnumerable{T}"/> of type <see cref="Tuple{T1,T2}"/></typeparam>
        /// <typeparam name="T2">The second type of the original <see cref="IEnumerable{T}"/> of type <see cref="Tuple{T1,T2}"/></typeparam>
        /// <typeparam name="T3">The type of <paramref name="selector"/>'s output.</typeparam>
        /// <param name="this">The original <see cref="IEnumerable{T}"/> of type <see cref="Tuple{T1,T2}"/></param>
        /// <param name="selector">The function from which to get the output as the third member of the tuple.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of type <see cref="Tuple{T1,T2,T3}"/> of <paramref name="this"/>'s elements with the output of the selector function applied to them.</returns>
        public static IEnumerable<Tuple<T1, T2, T3>> Attach<T1, T2, T3>(this IEnumerable<Tuple<T1, T2>> @this, Func<T1, T2, T3> selector)
        {
            return @this.Select(a => Tuple.Create(a.Item1, a.Item2, selector(a.Item1, a.Item2)));
        }
        /// <summary>
        /// Turns each element in an <see cref="IEnumerable{T}"/> of type <see cref="Tuple{T1,T2,T3}"/> to a tuple including itself and an output of itself and a selector function.
        /// </summary>
        /// <typeparam name="T1">The first type of the original <see cref="IEnumerable{T}"/> of type <see cref="Tuple{T1,T2,T3}"/></typeparam>
        /// <typeparam name="T2">The second type of the original <see cref="IEnumerable{T}"/> of type <see cref="Tuple{T1,T2,T3}"/></typeparam>
        /// <typeparam name="T3">The third type of the original <see cref="IEnumerable{T}"/> of type <see cref="Tuple{T1,T2,T3}"/></typeparam>
        /// <typeparam name="T4">The type of <paramref name="selector"/>'s output.</typeparam>
        /// <param name="this">The original <see cref="IEnumerable{T}"/> of type <see cref="Tuple{T1,T2,T3}"/></param>
        /// <param name="selector">The function from which to get the output as the fourth member of the tuple.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of type <see cref="Tuple{T1,T2,T3,T4}"/> of <paramref name="this"/>'s elements with the output of the selector function applied to them.</returns>
        public static IEnumerable<Tuple<T1, T2, T3, T4>> Attach<T1, T2, T3, T4>(this IEnumerable<Tuple<T1, T2, T3>> @this, Func<T1, T2, T3, T4> selector)
        {
            return @this.Select(a => Tuple.Create(a.Item1, a.Item2, a.Item3, selector(a.Item1, a.Item2, a.Item3)));
        }
        /// <summary>
        /// Turns each element in an <see cref="IEnumerable{T}"/> of type <see cref="Tuple{T1,T2,T3,T4}"/> to a tuple including itself and an output of itself and a selector function.
        /// </summary>
        /// <typeparam name="T1">The first type of the original <see cref="IEnumerable{T}"/> of type <see cref="Tuple{T1,T2,T3,T4}"/></typeparam>
        /// <typeparam name="T2">The second type of the original <see cref="IEnumerable{T}"/> of type <see cref="Tuple{T1,T2,T3,T4}"/></typeparam>
        /// <typeparam name="T3">The third type of the original <see cref="IEnumerable{T}"/> of type <see cref="Tuple{T1,T2,T3,T4}"/></typeparam>
        /// <typeparam name="T4">The fourth type of the original <see cref="IEnumerable{T}"/> of type <see cref="Tuple{T1,T2,T3,T4}"/></typeparam>
        /// <typeparam name="T5">The type of <paramref name="selector"/>'s output.</typeparam>
        /// <param name="this">The original <see cref="IEnumerable{T}"/> of type <see cref="Tuple{T1,T2,T3,T4}"/></param>
        /// <param name="selector">The function from which to get the output as the fifth member of the tuple.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of type <see cref="Tuple{T1,T2,T3,T4,T5}"/> of <paramref name="this"/>'s elements with the output of the selector function applied to them.</returns>
        public static IEnumerable<Tuple<T1, T2, T3, T4, T5>> Attach<T1, T2, T3, T4, T5>(this IEnumerable<Tuple<T1, T2, T3, T4>> @this, Func<T1, T2, T3, T4, T5> selector)
        {
            return @this.Select(a => Tuple.Create(a.Item1, a.Item2, a.Item3, a.Item4, selector(a.Item1, a.Item2, a.Item3, a.Item4)));
        }
        /// <summary>
        /// Turns each element in an <see cref="IList{T}"/> to a tuple including itself and an output of itself and a selector function.
        /// </summary>
        /// <typeparam name="T1">The type of the original <see cref="IList{T}"/></typeparam>
        /// <typeparam name="T2">The type of <paramref name="selector"/>'s output.</typeparam>
        /// <param name="this">The original <see cref="IList{T}"/></param>
        /// <param name="selector">The function from which to get the output as the second member of the tuple.</param>
        /// <returns>An <see cref="IList{T}"/> of type <see cref="Tuple{T1,T2}"/> of <paramref name="this"/>'s elements with the output of the selector function applied to them.</returns>
        /// <remarks><c>enumerable.Attach(selector)</c> is functionally identical to <c>enumerable.Zip(enumerable.Select(selector))</c>. Although here, the enumerable is enumerated only once per output enumeration, the <see cref="IList{T}.this"/> is invoked once per access.</remarks>
        public static IList<Tuple<T1, T2>> Attach<T1, T2>(this IList<T1> @this, Func<T1, T2> selector)
        {
            return @this.Select(a => Tuple.Create(a, selector(a)));
        }
        /// <summary>
        /// Turns each element in an <see cref="IList{T}"/> of type <see cref="Tuple{T1,T2}"/> to a tuple including itself and an output of itself and a selector function.
        /// </summary>
        /// <typeparam name="T1">The first type of the original <see cref="IList{T}"/> of type <see cref="Tuple{T1,T2}"/></typeparam>
        /// <typeparam name="T2">The second type of the original <see cref="IList{T}"/> of type <see cref="Tuple{T1,T2}"/></typeparam>
        /// <typeparam name="T3">The type of <paramref name="selector"/>'s output.</typeparam>
        /// <param name="this">The original <see cref="IList{T}"/> of type <see cref="Tuple{T1,T2}"/></param>
        /// <param name="selector">The function from which to get the output as the third member of the tuple.</param>
        /// <returns>An <see cref="IList{T}"/> of type <see cref="Tuple{T1,T2,T3}"/> of <paramref name="this"/>'s elements with the output of the selector function applied to them.</returns>
        public static IList<Tuple<T1, T2, T3>> Attach<T1, T2, T3>(this IList<Tuple<T1, T2>> @this, Func<T1, T2, T3> selector)
        {
            return @this.Select(a => Tuple.Create(a.Item1, a.Item2, selector(a.Item1, a.Item2)));
        }
        /// <summary>
        /// Turns each element in an <see cref="IList{T}"/> of type <see cref="Tuple{T1,T2,T3}"/> to a tuple including itself and an output of itself and a selector function.
        /// </summary>
        /// <typeparam name="T1">The first type of the original <see cref="IList{T}"/> of type <see cref="Tuple{T1,T2,T3}"/></typeparam>
        /// <typeparam name="T2">The second type of the original <see cref="IList{T}"/> of type <see cref="Tuple{T1,T2,T3}"/></typeparam>
        /// <typeparam name="T3">The third type of the original <see cref="IList{T}"/> of type <see cref="Tuple{T1,T2,T3}"/></typeparam>
        /// <typeparam name="T4">The type of <paramref name="selector"/>'s output.</typeparam>
        /// <param name="this">The original <see cref="IList{T}"/> of type <see cref="Tuple{T1,T2,T3}"/></param>
        /// <param name="selector">The function from which to get the output as the fourth member of the tuple.</param>
        /// <returns>An <see cref="IList{T}"/> of type <see cref="Tuple{T1,T2,T3,T4}"/> of <paramref name="this"/>'s elements with the output of the selector function applied to them.</returns>
        public static IList<Tuple<T1, T2, T3, T4>> Attach<T1, T2, T3, T4>(this IList<Tuple<T1, T2, T3>> @this, Func<T1, T2, T3, T4> selector)
        {
            return @this.Select(a => Tuple.Create(a.Item1, a.Item2, a.Item3, selector(a.Item1, a.Item2, a.Item3)));
        }
        /// <summary>
        /// Turns each element in an <see cref="IList{T}"/> of type <see cref="Tuple{T1,T2,T3,T4}"/> to a tuple including itself and an output of itself and a selector function.
        /// </summary>
        /// <typeparam name="T1">The first type of the original <see cref="IList{T}"/> of type <see cref="Tuple{T1,T2,T3,T4}"/></typeparam>
        /// <typeparam name="T2">The second type of the original <see cref="IList{T}"/> of type <see cref="Tuple{T1,T2,T3,T4}"/></typeparam>
        /// <typeparam name="T3">The third type of the original <see cref="IList{T}"/> of type <see cref="Tuple{T1,T2,T3,T4}"/></typeparam>
        /// <typeparam name="T4">The fourth type of the original <see cref="IList{T}"/> of type <see cref="Tuple{T1,T2,T3,T4}"/></typeparam>
        /// <typeparam name="T5">The type of <paramref name="selector"/>'s output.</typeparam>
        /// <param name="this">The original <see cref="IList{T}"/> of type <see cref="Tuple{T1,T2,T3,T4}"/></param>
        /// <param name="selector">The function from which to get the output as the fifth member of the tuple.</param>
        /// <returns>An <see cref="IList{T}"/> of type <see cref="Tuple{T1,T2,T3,T4,T5}"/> of <paramref name="this"/>'s elements with the output of the selector function applied to them.</returns>
        public static IList<Tuple<T1, T2, T3, T4, T5>> Attach<T1, T2, T3, T4, T5>(this IList<Tuple<T1, T2, T3, T4>> @this, Func<T1, T2, T3, T4, T5> selector)
        {
            return @this.Select(a => Tuple.Create(a.Item1, a.Item2, a.Item3, a.Item4, selector(a.Item1, a.Item2, a.Item3, a.Item4)));
        }
    }
}
