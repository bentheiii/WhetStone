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
    public static class searchForSub
    {
        /// <summary>
        /// search for sub-enumerables in <paramref name="this"/> that are equal to <paramref name="target"/>.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="this"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to count from.</param>
        /// <param name="target">The sub-enumerables, whose appearances in <paramref name="this"/> are to be counted.</param>
        /// <param name="comp">The <see cref="IEqualityComparer{T}"/> to use to equate <paramref name="this"/>'s sub-enumerables to <paramref name="target"/>.</param>
        /// <param name="innerComp">If <paramref name="comp"/> is null, a new <see cref="IEqualityComparer{T}"/> for sub-enumerables is created with this <see cref="IEqualityComparer{T}"/> as its inner. <see langword="null"/> for default.</param>
        /// <returns>The indices and values of sub-enumerables in <paramref name="this"/> that are equal to <paramref name="target"/> by <paramref name="comp"/>.</returns>
        public static IEnumerable<(int startIndex, T[] subSequence)> SearchForSub<T>(this IEnumerable<T> @this, IEnumerable<T> target, IEqualityComparer<IEnumerable<T>> comp = null, IEqualityComparer<T> innerComp = null)
        {
            @this.ThrowIfNull(nameof(@this));
            target.ThrowIfNull(nameof(target));
            if (comp == null)
            {
                comp = new EnumerableCompararer<T>(eq: innerComp);
            }
            else if (innerComp != null)
            {
                throw new ArgumentException($"either {nameof(comp)} or {nameof(innerComp)} must be null");
            }
            var qcount = target.Count();
            if (qcount == 0)
                throw new ArgumentException(nameof(target) + " is empty!");
            var trail = @this.Trail(qcount).CountBind();
            var found = trail.Where(a => comp.Equals(target, a.element));
            return found.Select(a=>(a.index, a.element));
        }
        /// <summary>
        /// search for sub-lists in <paramref name="this"/> that are equal to <paramref name="target"/>.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="this"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to count from.</param>
        /// <param name="target">The sub-enumerables, whose appearances in <paramref name="this"/> are to be counted.</param>
        /// <param name="comp">The <see cref="IEqualityComparer{T}"/> to use to equate <paramref name="this"/>'s sub-lists to <paramref name="target"/>.</param>
        /// <param name="innerComp">If <paramref name="comp"/> is null, a new <see cref="IEqualityComparer{T}"/> for sub-enumerables is created with this <see cref="IEqualityComparer{T}"/> as its inner. <see langword="null"/> for default.</param>
        /// <returns>The indices and values of sub-lists in <paramref name="this"/> that are equal to <paramref name="target"/> by <paramref name="comp"/>.</returns>
        public static IEnumerable<(int startIndex, IList<T> subSequence)> SearchForSub<T>(this IList<T> @this, IList<T> target, IEqualityComparer<IList<T>> comp = null, IEqualityComparer<T> innerComp = null)
        {
            @this.ThrowIfNull(nameof(@this));
            target.ThrowIfNull(nameof(target));
            if (comp == null)
            {
                comp = new EnumerableCompararer<T>(eq: innerComp);
            }
            else if (innerComp != null)
            {
                throw new ArgumentException($"either {nameof(comp)} or {nameof(innerComp)} must be null");
            }
            var qcount = target.Count;
            if (qcount == 0)
                throw new ArgumentException(nameof(target) + " is empty!");
            var trail = @this.Trail(qcount).CountBind();
            var found = trail.Where(a => comp.Equals(target, a.element));
            return found.Select(a => (a.index, a.element));
        }
    }
}
