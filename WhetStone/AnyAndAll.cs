using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    public static class anyAndAll
    {
        public static bool AnyAndAll<T>(this IEnumerable<T> @this, Func<T, bool> cond)
        {
            using (var tor = @this.GetEnumerator())
            {
                if (!tor.MoveNext() || !cond(tor.Current))
                    return false;
                while (tor.MoveNext())
                {
                    if (!cond(tor.Current))
                        return false;
                }
            }
            return true;
        }
    }
}
