using System;
using System.Collections.Generic;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    public static class mutateOnEnumeration
    {
        public static IEnumerable<T> MutateOnEnumerations<T>(this IEnumerable<T> @this, Action<T> mutation)
        {
            foreach (T t in @this)
            {
                t.Mutate(mutation);
                yield return t;
            }
        }
    }
}
