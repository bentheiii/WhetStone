using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class getSize
    {
        /// <summary>
        /// Get the individual lengths of all of an <see cref="Array"/>'s dimensions.
        /// </summary>
        /// <param name="mat">The <see cref="Array"/> to measure.</param>
        /// <returns>A read-only list, representing <paramref name="mat"/>'s lengths on all dimensions.</returns>
        public static IList<int> GetSize(this Array mat)
        {
            return range.Range(mat.Rank).Select(mat.GetLength);
        }
    }
}
