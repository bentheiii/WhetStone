using System;
using System.Collections.Generic;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class getBounds
    {
        /// <summary>
        /// Get the boundaries for all dimensions of an <see cref="Array"/>.
        /// </summary>
        /// <param name="this">The <see cref="Array"/> to check.</param>
        /// <returns>A read-only list, each element represent the boundaries in the appropriate dimension.</returns>
        public static IList<Tuple<int, int>> GetBounds(this Array @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return range.Range(@this.Rank).Select(a => Tuple.Create(@this.GetLowerBound(a), @this.GetUpperBound(a)+1));
        }
    }
}
