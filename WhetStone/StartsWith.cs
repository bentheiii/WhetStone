using System;
using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class startsWith
    {
        /// <summary>
        /// Get whether an <see cref="IEnumerable{T}"/> starts with another <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>s.</typeparam>
        /// <param name="this">The complete <see cref="IEnumerable{T}"/>.</param>
        /// <param name="prefix">The prefix <see cref="IEnumerable{T}"/>.</param>
        /// <param name="comp">The <see cref="IEqualityComparer{T}"/> to check for element equality. <see langword="null"/> for default.</param>
        /// <returns>Whether <paramref name="this"/> start with <paramref name="prefix"/>.</returns>
        public static bool StartsWith<T>(this IEnumerable<T> @this, IEnumerable<T> prefix, IEqualityComparer<T> comp = null)
        {
            //todo this looks like shit
            comp = comp ?? EqualityComparer<T>.Default;
            return @this.Select(a => Tuple.Create(a)).Zip(prefix.Select(a => Tuple.Create(a))).TakeWhile(x => x.Item2 != null).All(
                    x => x.Item1 != null && comp.Equals(x.Item1.Item1, x.Item2.Item1));
        }
    }
}
