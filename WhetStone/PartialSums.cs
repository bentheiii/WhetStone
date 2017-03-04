using System.Collections.Generic;
using NumberStone;
using WhetStone.Fielding;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class partialSums
    {
        /// <summary>
        /// Get the partial sum of an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to add.</param>
        /// <returns>All the partial sums of <paramref name="this"/>.</returns>
        /// <remarks>
        /// <para>Uses fielding to add. Use <see cref="yieldAggregate.YieldAggregate{T,R}"/> for non-fielding version.</para>
        /// <para>Because it starts with a 0, Has 1 more element than <paramref name="this"/>.</para>
        /// </remarks>
        public static IEnumerable<T> PartialSums<T>(this IEnumerable<T> @this)
        {
            var f = Fields.getField<T>();
            return @this.YieldAggregate(f.add, f.zero);
        }
        /// <summary>
        /// Get the partial sums of an <see cref="IEnumerable{T}"/> while compensating for floating-point errors.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to add.</param>
        /// <returns>All the partial compensating sums of <paramref name="this"/>.</returns>
        /// <remarks>
        /// <para>Uses fielding to add.</para>
        /// <para>Because it starts with a 0, Has 1 more element than <paramref name="this"/>.</para>
        /// </remarks>
        public static IEnumerable<T> PartialCompensatingSums<T>(this IEnumerable<T> @this)
        {
            KahanSum<T> ret = new KahanSum<T>();
            yield return ret.Sum;
            foreach (T t in @this)
            {
                ret.Add(t);
                yield return ret.Sum;
            }
        }
        /// <summary>
        /// Get the partial sums of an <see cref="IEnumerable{T}"/> of doubles while compensating for floating-point errors.
        /// </summary>
        /// <param name="this">The <see cref="IEnumerable{T}"/> of doubles to add.</param>
        /// <returns>All the partial compensating sums of <paramref name="this"/>.</returns>
        /// <remarks>
        /// <para>Because it starts with a 0.0, Has 1 more element than <paramref name="this"/>.</para>
        /// </remarks>
        public static IEnumerable<double> PartialCompensatingSums(this IEnumerable<double> @this)
        {
            var ret = new KahanSum();
            yield return 0.0;
            foreach (var t in @this)
            {
                ret.Add(t);
                yield return ret.Sum;
            }
        }
    }
}
