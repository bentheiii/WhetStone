using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class countAtleast
    {
        /// <summary>
        /// Checks whether the <see cref="IEnumerable{T}"/> has at least <paramref name="minCount"/> elements that satisfy <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to count.</param>
        /// <param name="minCount">The number of elements in <paramref name="this"/> that have to satisfy <paramref name="predicate"/>.</param>
        /// <param name="predicate">The predicate elements must satisfy. If set to <see langword="null"/>, all elements satisfy.</param>
        /// <returns>Whether there are at least <paramref name="minCount"/> elements in <paramref name="this"/> that satisfy <paramref name="predicate"/>.</returns>
        /// <remarks>After confirming <paramref name="this"/> at least <paramref name="minCount"/> members, enumeration will halt.</remarks>
        public static bool CountAtLeast<T>(this IEnumerable<T> @this, int minCount, Func<T,bool> predicate = null)
        {
            @this.ThrowIfNull(nameof(@this));
            if (minCount <= 0)
                return true;
            if (predicate != null)
                @this = @this.Where(predicate);
            else
            {
                var rec = @this.RecommendCount();
                if (rec.HasValue)
                    return rec.Value >= minCount;
            }
            return @this.Skip(minCount - 1).Any();
        }
    }
}
