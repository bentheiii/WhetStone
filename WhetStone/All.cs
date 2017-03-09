using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class all
    {
        /// <summary>
        /// Checks whether all the elements of a <see cref="IEnumerable{T}"/> of type <see cref="bool"/> are true.
        /// </summary>
        /// <param name="this">The <see cref="IEnumerable{T}"/> of type <see cref="bool"/> to check.</param>
        /// <returns>Whether all elements of <paramref name="this"/> are true.</returns>
        public static bool All(this IEnumerable<bool> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.All(t => t);
        }
    }
}
