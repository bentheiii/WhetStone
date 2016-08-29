using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    public static class anyAndAll
    {
        public static bool AnyAndAll<T>(this IEnumerable<T> @this, Func<T, bool> cond)
        {
            bool any = false;
            foreach (T t in @this)
            {
                if (cond(t) == false)
                    return false;
                any = true;
            }
            return any;
        }
    }
}
