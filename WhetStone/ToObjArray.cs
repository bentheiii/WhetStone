using System.Collections;
using System.Linq;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class toObjArray
    {
        /// <summary>
        /// Convert a non-generic <see cref="IEnumerator"/> to an <see cref="System.Array"/> of <see cref="object"/>s.
        /// </summary>
        /// <param name="this">The non-generic <see cref="IEnumerator"/> to convert.</param>
        /// <returns>An <see cref="object"/> <see cref="System.Array"/> with <paramref name="this"/>'s elements.</returns>
        public static object[] ToObjArray(this IEnumerable @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.Cast<object>().ToArray();
        }
    }
} 
