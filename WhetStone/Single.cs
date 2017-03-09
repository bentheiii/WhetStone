using System;
using System.Collections.Generic;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class single
    {
        /// <summary>
        /// Get the element of an <see cref="IEnumerable{T}"/> if it is alone, a default element if it empty, or throw exception otherwise.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to use.</param>
        /// <param name="default">The value to return if <paramref name="this"/> is empty.</param>
        /// <returns>The single element of <paramref name="this"/>, or <paramref name="default"/> if none exist.</returns>
        /// <exception cref="InvalidOperationException">If the argument contains more than one element.</exception>
        public static T SingleOrDefault<T>(this IEnumerable<T> @this, T @default)
        {
            @this.ThrowIfNull(nameof(@this));
            using (var tor = @this.GetEnumerator())
            {
                if (!tor.MoveNext())
                    return @default;
                var ret = tor.Current;
                if (tor.MoveNext())
                    throw new InvalidOperationException("enumerable has more than one element");
                return ret;
            }
        }
    }
}
