using System.Collections.Generic;
using System.Linq;
using WhetStone.SystemExtensions;

namespace WhetStone.WordPlay
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class convertToString
    {
        /// <summary>
        /// Converts an <see cref="IEnumerable{T}"/> of <see cref="char"/>s into a <see cref="string"/>.
        /// </summary>
        /// <param name="x">The <see cref="IEnumerable{T}"/> to convert.</param>
        /// <returns>A string formed of <paramref name="x"/>'s characters.</returns>
        public static string ConvertToString(this IEnumerable<char> x)
        {
            x.ThrowIfNull(nameof(x));
            return new string(x as char[] ?? x.ToArray());
        }
    }
}
