using System;
using System.Collections.Generic;
using WhetStone.Guard;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class hookFirst
    {
        /// <overloads>Hooks an <see cref="IGuard{T}"/> to an <see cref="IEnumerable{T}"/>'s first value.</overloads>
        /// <summary>
        /// Hooks an <see cref="IGuard{T}"/> to an <see cref="IEnumerable{T}"/>'s first value.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to hook to.</param>
        /// <param name="sink">The <see cref="IGuard{T}"/> to update.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that sets <paramref name="sink"/> to its first enumerated value.</returns>
        /// <remarks>If <paramref name="sink"/> contains <see langword="null"/>, a first value has not yet been set.</remarks>
        public static IEnumerable<T> HookFirst<T>(this IEnumerable<T> @this, IGuard<Tuple<T>> sink)
        {
            sink.value = null;
            using (var tor = @this.GetEnumerator())
            {
                if (!tor.MoveNext())
                    yield break;
                sink.value = Tuple.Create(tor.Current);
                yield return tor.Current;
                while (tor.MoveNext())
                {
                    yield return tor.Current;
                }
            }
        }
        /// <summary>
        /// Hooks an <see cref="IGuard{T}"/> to an <see cref="IEnumerable{T}"/>'s first value that matches a criteria.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to hook to.</param>
        /// <param name="sink">The <see cref="IGuard{T}"/> to update.</param>
        /// <param name="critiria">The criteria that finds the first element</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that sets <paramref name="sink"/> to its first enumerated value that matches <paramref name="critiria"/>.</returns>
        /// <remarks>If <paramref name="sink"/> contains <see langword="null"/>, a first value has not yet been set.</remarks>
        public static IEnumerable<T> HookFirst<T>(this IEnumerable<T> @this, IGuard<Tuple<T>> sink, Func<T, bool> critiria)
        {
            sink.value = null;
            using (var tor = @this.GetEnumerator())
            {
                while (true)
                {
                    if (!tor.MoveNext())
                        yield break;
                    var yes = critiria(tor.Current);
                    if (yes)
                        sink.value = Tuple.Create(tor.Current);
                    yield return tor.Current;
                    if (yes)
                        break;
                }
                while (tor.MoveNext())
                {
                    yield return tor.Current;
                }
            }
        }
        /// <summary>
        /// Hooks an <see cref="IGuard{T}"/> to an <see cref="IEnumerable{T}"/>'s first value.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to hook to.</param>
        /// <param name="sink">The <see cref="IGuard{T}"/> to update.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that sets <paramref name="sink"/> to its first enumerated value.</returns>
        public static IEnumerable<T> HookFirst<T>(this IEnumerable<T> @this, IGuard<T> sink)
        {
            using (var tor = @this.GetEnumerator())
            {
                if (!tor.MoveNext())
                    yield break;
                sink.value = tor.Current;
                yield return tor.Current;
                while (tor.MoveNext())
                {
                    yield return tor.Current;
                }
            }
        }
        /// <summary>
        /// Hooks an <see cref="IGuard{T}"/> to an <see cref="IEnumerable{T}"/>'s first value that matches a criteria.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to hook to.</param>
        /// <param name="sink">The <see cref="IGuard{T}"/> to update.</param>
        /// <param name="critiria">The criteria that finds the first element</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that sets <paramref name="sink"/> to its first enumerated value that matches <paramref name="critiria"/>.</returns>
        public static IEnumerable<T> HookFirst<T>(this IEnumerable<T> @this, IGuard<T> sink, Func<T, bool> critiria)
        {
            using (var tor = @this.GetEnumerator())
            {
                while (true)
                {
                    if (!tor.MoveNext())
                        yield break;
                    var yes = critiria(tor.Current);
                    if (yes)
                        sink.value = tor.Current;
                    yield return tor.Current;
                    if (yes)
                        break;
                }
                while (tor.MoveNext())
                {
                    yield return tor.Current;
                }
            }
        }
    }
}
