using System;
using System.Collections.Generic;
using WhetStone.Guard;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class hookCount
    {
        /// <summary>
        /// Hooks an <see cref="IGuard{T}"/>'s value to an <see cref="IEnumerable{T}"/>'s number of enumerated items that match a criteria.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to hook.</param>
        /// <param name="sink">The <see cref="IGuard{T}"/> to update.</param>
        /// <param name="criteria">The criteria to check. Or <see langword="null"/> for all elements.</param>
        /// <returns>A new <see cref="IEnumerable{T}"/> that updates <paramref name="sink"/>'s value when enumerated.</returns>
        public static IEnumerable<T> HookCount<T>(this IEnumerable<T> @this, IGuard<int> sink, Func<T, bool> criteria = null)
        {
            @this.ThrowIfNull(nameof(@this));
            sink.ThrowIfNull(nameof(sink));
            criteria.ThrowIfNull(nameof(criteria));
            var func = criteria == null ? ((a, b) => b + 1) : (Func<T,int,int>)((a, b) => criteria(a) ? b + 1 : b);
            return @this.HookAggregate(sink, func);
        }
    }
}
