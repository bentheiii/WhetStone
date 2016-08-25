using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Looping;

namespace WhetStone.Arrays
{
    public static class count
    {
        public static int Count<T>(this IEnumerable<T> @this, T query, IEqualityComparer<T> comp = null)
        {
            comp = comp ?? EqualityComparer<T>.Default;
            return @this.Count(a => comp.Equals(a, query));
        }
        public static int Count<T>(this IEnumerable<T> @this, IEnumerable<T> query, IEqualityComparer<T> comp = null)
        {
            comp = comp ?? EqualityComparer<T>.Default;
            return @this.Trail(query.Count()).Count(a => a.SequenceEqual(query, comp));
        }
    }
}
