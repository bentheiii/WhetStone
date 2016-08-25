using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Looping
{
    public static class enumerate
    {
        public static IEnumerable<T> Enumerate<T>(this T b)
        {
            yield return b;
        }
    }
}
