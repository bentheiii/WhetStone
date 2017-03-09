using System;
using System.Collections.Generic;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class rows
    {
        /// <summary>
        /// Returns the 2D <see cref="Array"/> arranged as an <see cref="IList{T}"/> of <see cref="IList{T}"/>, with the first dimension first.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Array"/>.</typeparam>
        /// <param name="this">The <see cref="Array"/> to transform.</param>
        /// <returns>An <see cref="IList{T}"/> of <see cref="IList{T}"/>, each element of the super-list containing a first-dimension slice of the original <see cref="Array"/>.</returns>
        public static IList<IList<T>> Rows<T>(this T[,] @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return range.Range(@this.GetLength(0)).Select(a => range.Range(@this.GetLength(1)).Select(x => @this[a, x]));
        }
    }
}
