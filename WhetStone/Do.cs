using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class @do
    {
        /// <summary>
        /// Enumerates the entire <see cref="IEnumerable{T}"/> and applies a method to it.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to enumerate.</param>
        /// <param name="action">The <see cref="Action{T}"/> to invoke on each element. If null, no action will be invoked, but the <paramref name="this"/> will still be enumerated.</param>
        public static void Do<T>(this IEnumerable<T> @this, Action<T> action = null)
        {
            foreach (T t in @this)
            {
                action?.Invoke(t);
            }
        }
    }
}
