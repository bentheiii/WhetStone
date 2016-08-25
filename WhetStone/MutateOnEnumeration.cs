using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
