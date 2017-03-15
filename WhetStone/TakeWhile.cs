using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class takeWhile
    {
        /// <summary>
        /// Take from an <see cref="IEnumerable{T}"/> until an element does not uphold a predicate. Then take that element as well.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to take from.</param>
        /// <param name="pred">The predicate all but the last element must uphold.</param>
        /// <returns>All the first-most elements that uphold <paramref name="pred"/>, and the first that does not.</returns>
        public static IEnumerable<T> TakeWhileInclusive<T>(this IEnumerable<T> @this, Func<T,bool> pred)
        {
            foreach (var t in @this)
            {
                yield return t;
                if (!pred(t))
                    yield break;
            }
        }
    }
}
