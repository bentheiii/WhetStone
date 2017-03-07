using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class columns
    {
        /// <summary>
        /// Returns the 2D <see cref="Array"/> arranged as an <see cref="IList{T}"/> of <see cref="IList{T}"/>, with the second dimension first.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Array"/>.</typeparam>
        /// <param name="this">The <see cref="Array"/> to transform.</param>
        /// <returns>An <see cref="IList{T}"/> of <see cref="IList{T}"/>, each element of the super-list containing a second-dimension slice of the original <see cref="Array"/>.</returns>
        /// <example>
        /// <code>
        /// var arr = new int[,]{{0,1,2,3},{4,5,6,7},{8,9,10,11}};
        /// arr.Columns().Select(a=>a.ToArray()).ToArray(); //int[,] {{0,4,8},{1,5,9},{2,6,10},{3,7,11}}
        /// </code>
        /// </example>
        public static IList<IList<T>> Columns<T>(this T[,] @this)
        {
            return range.Range(@this.GetLength(1)).Select(a => range.Range(@this.GetLength(0)).Select(x => @this[x, a]));
        }
    }
}
