using System;
using System.Globalization;

namespace WhetStone.WordPlay
{
    public static class toString
    {
        public static string ToString(this IFormattable @this, string format)
        {
            return @this.ToString(format, CultureInfo.CurrentCulture);
        }
    }
}
