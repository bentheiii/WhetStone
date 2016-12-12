using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Looping
{
    public static class attachAggregate
    {
        public static IEnumerable<Tuple<T, R>> AttachAggregate<T, R>(this IEnumerable<T> @this, Func<T, R, R> aggregator, R seed = default(R))
        {
            return @this.Zip(@this.YieldAggregate(aggregator, seed).Skip(1));
        }
    }
}
