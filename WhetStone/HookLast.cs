using System;
using System.Collections.Generic;
using WhetStone.Guard;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class hookLast
    {
        /// <overloads>Hooks an <see cref="IGuard{T}"/> to an <see cref="IEnumerable{T}"/>'s last value.</overloads>
        /// <summary>
        /// Hooks an <see cref="IGuard{T}"/> to an <see cref="IEnumerable{T}"/>'s last value.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to hook to.</param>
        /// <param name="sink">The <see cref="IGuard{T}"/> to update.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that sets <paramref name="sink"/> to its last enumerated value.</returns>
        /// <remarks>If <paramref name="sink"/> contains <see langword="null"/>, a last value has not yet been set.</remarks>
        public static IEnumerable<T> HookLast<T>(this IEnumerable<T> @this, IGuard<Tuple<T>> sink)
        {
            @this.ThrowIfNull(nameof(@this));
            sink.ThrowIfNull(nameof(sink));
            return @this.HookAggregate(sink, (a, b) => Tuple.Create(a));
        }
        /// <summary>
        /// Hooks an <see cref="IGuard{T}"/> to an <see cref="IEnumerable{T}"/>'s last value that matches a criteria.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to hook to.</param>
        /// <param name="sink">The <see cref="IGuard{T}"/> to update.</param>
        /// <param name="critiria">The criteria that finds the last element</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that sets <paramref name="sink"/> to its last enumerated value that matches <paramref name="critiria"/>.</returns>
        /// <remarks>If <paramref name="sink"/> contains <see langword="null"/>, a last value has not yet been set.</remarks>
        public static IEnumerable<T> HookLast<T>(this IEnumerable<T> @this, IGuard<Tuple<T>> sink, Func<T,bool> critiria)
        {
            @this.ThrowIfNull(nameof(@this));
            sink.ThrowIfNull(nameof(sink));
            critiria.ThrowIfNull(nameof(critiria));
            return @this.HookAggregate(sink, (a, b) => critiria(a) ? Tuple.Create(a) : b);
        }
        /// <overloads>Hooks an <see cref="IGuard{T}"/> to an <see cref="IEnumerable{T}"/>'s last value.</overloads>
        /// <summary>
        /// Hooks an <see cref="IGuard{T}"/> to an <see cref="IEnumerable{T}"/>'s last value.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to hook to.</param>
        /// <param name="sink">The <see cref="IGuard{T}"/> to update.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that sets <paramref name="sink"/> to its last enumerated value.</returns>
        public static IEnumerable<T> HookLast<T>(this IEnumerable<T> @this, IGuard<T> sink)
        {
            @this.ThrowIfNull(nameof(@this));
            sink.ThrowIfNull(nameof(sink));
            return @this.HookAggregate(sink, (a, b) => b);
        }
        /// <summary>
        /// Hooks an <see cref="IGuard{T}"/> to an <see cref="IEnumerable{T}"/>'s last value that matches a criteria.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to hook to.</param>
        /// <param name="sink">The <see cref="IGuard{T}"/> to update.</param>
        /// <param name="critiria">The criteria that finds the last element</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that sets <paramref name="sink"/> to its last enumerated value that matches <paramref name="critiria"/>.</returns>
        public static IEnumerable<T> HookLast<T>(this IEnumerable<T> @this, IGuard<T> sink, Func<T,bool> critiria)
        {
            @this.ThrowIfNull(nameof(@this));
            sink.ThrowIfNull(nameof(sink));
            critiria.ThrowIfNull(nameof(critiria));
            return @this.HookAggregate(sink, (a, b) => critiria(a) ? a : b);
        }
    }
}
