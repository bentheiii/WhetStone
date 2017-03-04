using System;
using System.Globalization;

namespace WhetStone.WordPlay
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class toString
    {
        /// <summary>
        /// Convert an <see cref="IFormattable"/> to a <see cref="string"/> with a specific format but default <see cref="IFormatProvider"/>.
        /// </summary>
        /// <param name="this">The <see cref="IFormattable"/> to convert.</param>
        /// <param name="format">The format to use.</param>
        /// <returns><paramref name="this"/> converted to <see cref="string"/> under <paramref name="format"/> and <see cref="CultureInfo.CurrentCulture"/>.</returns>
        public static string ToString(this IFormattable @this, string format)
        {
            return @this.ToString(format, CultureInfo.CurrentCulture);
        }
    }
}
