using System.Collections.Generic;
using System.Linq;
using WhetStone.Fielding;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class getProduct
    {
        /// <summary>
        /// Get the product of all elements in an <see cref="IEnumerable{T}"/> using fielding.
        /// </summary>
        /// <typeparam name="T">The element to multiply</typeparam>
        /// <param name="toAdd">The <see cref="IEnumerable{T}"/> to get the product of.</param>
        /// <returns>The product of all elements in <paramref name="toAdd"/></returns>
        /// <remarks>Uses fielding, use <see cref="Enumerable.Aggregate{TSource}"/> for non-generic equivalent.</remarks>
        public static T GetProduct<T>(this IEnumerable<T> toAdd)
        {
            toAdd.ThrowIfNull(nameof(toAdd));
            var f = Fields.getField<T>();
            return toAdd.Aggregate(f.one, f.Product);
        }
    }
}
