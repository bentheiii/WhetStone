using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Looping
{
    public static class distinct
    {
        public static IEnumerable<T> DistinctSorted<T>(this IEnumerable<T> @this, IEqualityComparer<T> comp = null)
        {
            comp = comp ?? EqualityComparer<T>.Default;
            T last = default(T);
            bool any = false;
            foreach (T t in @this)
            {
                if (!any || !comp.Equals(last, t))
                {
                    yield return t;
                    last = t;
                    any = true;
                }
            }
        }
    }
}
