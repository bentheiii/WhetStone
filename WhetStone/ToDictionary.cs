using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class toDictionary
    {
        /// <summary>
        /// Convert an <see cref="IEnumerable{T}"/> of <see cref="KeyValuePair{TKey,TValue}"/> into an <see cref="IDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <typeparam name="K">The type of the key.</typeparam>
        /// <typeparam name="V">The type of the value.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to convert.</param>
        /// <returns><paramref name="this"/> converted to an <see cref="IDictionary{TKey,TValue}"/>.</returns>
        public static IDictionary<K, V> ToDictionary<K, V>(this IEnumerable<KeyValuePair<K, V>> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.ToDictionary(a => a.Key, a => a.Value);
        }
        /// <summary>
        /// Convert an <see cref="IEnumerable{T}"/> of <see cref="Tuple{T0,T1}"/> into an <see cref="IDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <typeparam name="K">The type of the key.</typeparam>
        /// <typeparam name="V">The type of the value.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to convert.</param>
        /// <returns><paramref name="this"/> converted to an <see cref="IDictionary{TKey,TValue}"/>.</returns>
        public static IDictionary<K, V> ToDictionary<K, V>(this IEnumerable<Tuple<K, V>> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.ToDictionary(a => a.Item1, a => a.Item2);
        }
        /// <summary>
        /// Convert an <see cref="IEnumerable{T}"/> of <see cref="Tuple{T0,T1}"/> into an <see cref="IDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <typeparam name="K">The type of the key.</typeparam>
        /// <typeparam name="V">The type of the value.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to convert.</param>
        /// <returns><paramref name="this"/> converted to an <see cref="IDictionary{TKey,TValue}"/>.</returns>
        public static IDictionary<K, V> ToDictionary<K, V>(this IEnumerable<(K, V)> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.ToDictionary(a => a.Item1, a => a.Item2);
        }
    }
}
