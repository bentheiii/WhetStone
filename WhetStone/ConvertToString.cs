using System.Collections.Generic;
using System.Linq;

namespace WhetStone.WordPlay
{
    public static class convertToString
    {
        public static string ConvertToString(this IEnumerable<char> x)
        {
            return new string(x.ToArray());
        }
    }
}
