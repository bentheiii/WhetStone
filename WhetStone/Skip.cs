using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class skip
    {
        /// <summary>
        /// Get an <see cref="IList{T}"/> that is the same as the original <see cref="IList{T}"/> with the first elements skipped.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to use.</param>
        /// <param name="skipCount">The number of elements to skip.</param>
        /// <returns>A mutability-passing <see cref="IList{T}"/> that skips the first <paramref name="skipCount"/> elements in <paramref name="this"/>.</returns>
        public static IList<T> Skip<T>(this IList<T> @this, int skipCount)
        {
            if (skipCount == 0)
                return @this;
            return @this.Slice(Math.Min(skipCount, @this.Count));
        }
    }
}
