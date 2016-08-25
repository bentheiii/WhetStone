using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Fielding;
using WhetStone.Looping;

namespace WhetStone
{
    public static class partialProducts
    {
        public static IEnumerable<T> PartialProducts<T>(this IEnumerable<T> @this)
        {
            var f = Fields.getField<T>();
            return @this.YieldAggregate(f.multiply, f.one);
        }
    }
}
