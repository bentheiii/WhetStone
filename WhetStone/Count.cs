using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Comparison;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class count
    {
        /// <summary>
        /// Counts the number of times <paramref name="target"/> appears in <paramref name="this"/>.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="this"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to count from.</param>
        /// <param name="target">The element, whose appearances in <paramref name="this"/> are to be counted.</param>
        /// <param name="comp">The <see cref="IEqualityComparer{T}"/> to use to equate <paramref name="this"/>'s elements to <paramref name="target"/>. <see langowrd="null"/> will set to default <see cref="IEqualityComparer{T}"/>.</param>
        /// <returns>The number of elements in <paramref name="this"/> that are equal to <paramref name="target"/> by <paramref name="comp"/>.</returns>
        public static int Count<T>(this IEnumerable<T> @this, T target, IEqualityComparer<T> comp = null)
        {
            @this.ThrowIfNull(nameof(@this));
            comp = comp ?? EqualityComparer<T>.Default;
            return @this.Count(a => comp.Equals(a, target));
        }
        /// <summary>
        /// Count the number of sub-enumerables in <paramref name="this"/> that are equal to <paramref name="target"/>.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="this"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to count from.</param>
        /// <param name="target">The sub-enumerables, whose appearances in <paramref name="this"/> are to be counted.</param>
        /// <param name="comp">The <see cref="IEqualityComparer{T}"/> to use to equate <paramref name="this"/>'s sub-enumerables to <paramref name="target"/>. <see langowrd="null"/> will set to default <see cref="IEqualityComparer{T}"/>.</param>
        /// <returns>The number of sub-enumerables in <paramref name="this"/> that are equal to <paramref name="target"/> by <paramref name="comp"/>.</returns>
        public static int Count<T>(this IEnumerable<T> @this, IEnumerable<T> target, IEqualityComparer<IEnumerable<T>> comp = null)
        {
            @this.ThrowIfNull(nameof(@this));
            target.ThrowIfNull(nameof(target));
            comp = comp ?? new EnumerableCompararer<T>();
            var qcount = target.Count();
            if (qcount == 0)
                throw new ArgumentException(nameof(target)+" is empty!");
            return @this.Trail(qcount).Count(a => comp.Equals(@this,a));
        }
    }
}
