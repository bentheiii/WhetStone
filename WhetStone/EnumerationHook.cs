using System;
using System.Collections.Generic;

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
