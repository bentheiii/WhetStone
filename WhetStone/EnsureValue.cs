using System;
using System.Collections.Generic;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class ensureValue
    {
        /// <summary>
        /// Ensures an <see cref="IDictionary{T,G}"/> has a value for a specific key, setting a default value is it doesn't.
        /// </summary>
        /// <typeparam name="T">The type of key of the <see cref="IDictionary{T,G}"/>.</typeparam>
        /// <typeparam name="G">The type of value of the <see cref="IDictionary{T,G}"/>.</typeparam>
        /// <param name="this">The <see cref="IDictionary{T,G}"/> to ensure a value on.</param>
        /// <param name="key">The key to ensure value for.</param>
        /// <param name="defaultval">The value to set to the key if none exists?</param>
        /// <returns>Whether a value existed before the method was called.</returns>
        public static bool EnsureValue<T, G>(this IDictionary<T, G> @this, T key, G defaultval = default(G))
        {
            @this.ThrowIfNull(nameof(@this));
            if (@this.ContainsKey(key))
                return true;
            @this[key] = defaultval;
            return false;
        }
    }
}
