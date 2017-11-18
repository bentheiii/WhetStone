using System;
using System.Collections.Generic;
using System.Linq;

namespace WhetStone
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class asArray
    {
        /// <summary>
        /// Get an <see cref="IEnumerable{T}"/> as an array, or convert it to an array is unable
        /// </summary>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to convert.</param>
        /// <typeparam name="T">The inner type of <paramref name="this"/>.</typeparam>
        /// <returns><paramref name="this"/> casted to an <see cref="Array"/>, or converted to an array.</returns>
        public static T[] AsArray<T>(this IEnumerable<T> @this)
        {
            if (@this is T[] a)
                return a;
            return @this.ToArray();
        }
    }
}
