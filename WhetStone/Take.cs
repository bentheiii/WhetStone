using System;
using System.Collections.Generic;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class take
    {
        /// <summary>
        /// Get an <see cref="IList{T}"/> that is the same as the original <see cref="IList{T}"/> with the last elements skipped.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to use.</param>
        /// <param name="length">The number of elements take.</param>
        /// <returns>A mutability-passing <see cref="IList{T}"/> that takes the first <paramref name="length"/> elements in <paramref name="this"/>.</returns>
        public static IList<T> Take<T>(this IList<T> @this, int length)
        {
            @this.ThrowIfNull(nameof(@this));
            length.ThrowIfAbsurd(nameof(length));
            if (length >= @this.Count)
                return @this;
            return @this.Slice(0, length: Math.Min(length, @this.Count));
        }
    }
}
