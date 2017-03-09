using System.Collections.Generic;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class recommendSize
    {
        /// <summary>
        /// Attempt to extract the size of an <see cref="IEnumerable{T}"/> without enumerating it.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to check.</param>
        /// <returns>The length of <paramref name="this"/> or <see langword="null"/> if unsuccessful.</returns>
        public static int? RecommendCount<T>(this IEnumerable<T> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.AsCollection(false)?.Count;
        }
    }
}
