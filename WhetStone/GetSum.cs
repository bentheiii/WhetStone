using System.Collections.Generic;
using System.Linq;
using NumberStone;
using WhetStone.Fielding;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class getSum
    {
        /// <summary>
        /// Get the sum of all elements in an <see cref="IEnumerable{T}"/> using fielding.
        /// </summary>
        /// <typeparam name="T">The element to add</typeparam>
        /// <param name="toAdd">The <see cref="IEnumerable{T}"/> to get the sum of.</param>
        /// <returns>The sum of all elements in <paramref name="toAdd"/></returns>
        /// <remarks>Uses fielding, use <see cref="Enumerable.Aggregate{TSource}"/> for non-generic equivalent.</remarks>
        public static T GetSum<T>(this IEnumerable<T> toAdd)
        {
            toAdd.ThrowIfNull(nameof(toAdd));
            var f = Fields.getField<T>();
            return toAdd.Aggregate(f.zero, f.add);
        }
        /// <summary>
        /// Get the sum of all elements in an <see cref="IEnumerable{T}"/> with floating point compensation.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="toAdd">The <see cref="IEnumerable{T}"/> to add.</param>
        /// <returns>A compensating sum of the <see cref="IEnumerable{T}"/></returns>
        /// <remarks>Uses fielding</remarks>
        public static T GetCompensatingSum<T>(this IEnumerable<T> toAdd)
        {
            toAdd.ThrowIfNull(nameof(toAdd));
            KahanSum<T> ret = new KahanSum<T>();
            foreach (T t in toAdd)
            {
                ret.Add(t);
            }
            return ret.Sum;
        }
        /// <summary>
        /// Get the sum of all elements in an <see cref="IEnumerable{T}"/> with floating point compensation.
        /// </summary>
        /// <param name="toAdd">The <see cref="IEnumerable{T}"/> to add.</param>
        /// <returns>A compensating sum of the <see cref="IEnumerable{T}"/></returns>
        public static double GetCompensatingSum(this IEnumerable<double> toAdd)
        {
            toAdd.ThrowIfNull(nameof(toAdd));
            var ret = new KahanSum();
            foreach (var t in toAdd)
            {
                ret.Add(t);
            }
            return ret.Sum;
        }
    }
}
