using System;
using System.Collections.Generic;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class allEqual
    {
        /// <summary>
        /// Checks whether all members of an <see cref="IEnumerable{T}"/> are equal.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to check.</param>
        /// <param name="comp">The <see cref="IEqualityComparer{T}"/> to use, to check all the elements all are the same.</param>
        /// <returns>Whether all the elements in <paramref name="this"/> are equal.</returns>
        /// <remarks><para>This compares every element to <paramref name="this"/>'s first element.</para><para>If <paramref name="this"/> is empty, <see langword="true"/> is returned.</para></remarks>
        public static bool AllEqual<T>(this IEnumerable<T> @this, IEqualityComparer<T> comp = null)
        {
            @this.ThrowIfNull(nameof(@this));
            comp = comp ?? EqualityComparer<T>.Default;
            using (IEnumerator<T> tor = @this.GetEnumerator())
            {
                if (!tor.MoveNext())
                    return true;
                T mem = tor.Current;
                while (tor.MoveNext())
                {
                    if (!comp.Equals(mem, tor.Current))
                        return false;
                }
            }
            return true;
        }
    }
}
