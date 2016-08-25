using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Looping
{
    public static class enumerationHook
    {
        public static IEnumerable<T> EnumerationHook<T>(this IEnumerable<T> @this, Action<T> preYield = null, Action<T> postYield = null)
        {
            foreach (var t in @this)
            {
                preYield?.Invoke(t);
                yield return t;
                postYield?.Invoke(t);
            }
        }
    }
}
