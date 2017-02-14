using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class isSymmetrical
    {
        /// <summary>
        /// Get whether an <see cref="IList{T}"/> is symmetrical.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to check.</param>
        /// <param name="c">The <see cref="IEqualityComparer{T}"/> to compare elements. <see langword="null"/> will use the default <see cref="EqualityComparer{T}"/></param>
        /// <returns>Whether <paramref name="this"/> is symmetrical.</returns>
        public static bool IsSymmetrical<T>(this IList<T> @this, IEqualityComparer<T> c = null)
        {
            c = c ?? EqualityComparer<T>.Default;
            return range.Range(@this.Count / 2).All(i => c.Equals(@this[i], @this[@this.Count -i - 1]));
        }
    }
}
