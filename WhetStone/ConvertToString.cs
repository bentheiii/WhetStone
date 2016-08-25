using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.WordPlay
{
    public static class convertToString
    {
        public static string ConvertToString(this IEnumerable<byte> x)
        {
            return new string(x.Select(a => (char)a).ToArray());
        }
        public static string ConvertToString(this IEnumerable<char> x)
        {
            return new string(x.ToArray());
        }
    }
}
