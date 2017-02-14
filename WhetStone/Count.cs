using System.Collections.Generic;
using System.Linq;
using WhetStone.Comparison;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class count
    {
        /// <summary>
        /// Counts the number of times <paramref name="query"/> appears in <paramref name="this"/>.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="this"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to count from.</param>
        /// <param name="query">The element, whose appearances in <paramref name="this"/> are to be counted.</param>
        /// <param name="comp">The <see cref="IEqualityComparer{T}"/> to use to equate <paramref name="this"/>'s elements to <paramref name="query"/>. <see langowrd="null"/> will set to default <see cref="IEqualityComparer{T}"/>.</param>
        /// <returns>The number of elements in <paramref name="this"/> that are equal to <paramref name="query"/> by <paramref name="comp"/>.</returns>
        public static int Count<T>(this IEnumerable<T> @this, T query, IEqualityComparer<T> comp = null)
        {
            comp = comp ?? EqualityComparer<T>.Default;
            return @this.Count(a => comp.Equals(a, query));
        }
        /// <summary>
        /// Count the number of sub-enumerables in <paramref name="this"/> that are equal to <paramref name="query"/>.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="this"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to count from.</param>
        /// <param name="query">The sub-enumerables, whose appearances in <paramref name="this"/> are to be counted.</param>
        /// <param name="comp">The <see cref="IEqualityComparer{T}"/> to use to equate <paramref name="this"/>'s sub-enumerables to <paramref name="query"/>. <see langowrd="null"/> will set to default <see cref="IEqualityComparer{T}"/>.</param>
        /// <returns>The number of sub-enumerables in <paramref name="this"/> that are equal to <paramref name="query"/> by <paramref name="comp"/>.</returns>
        public static int Count<T>(this IEnumerable<T> @this, IEnumerable<T> query, IEqualityComparer<IEnumerable<T>> comp = null)
        {
            comp = comp ?? new EnumerableEqualityCompararer<T>();
            return @this.Trail(query.Count()).Count(a => comp.Equals(@this,a));
        }
    }
}
