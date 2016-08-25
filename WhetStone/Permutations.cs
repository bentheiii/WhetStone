using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Looping
{
    public static class permutations
    {
        public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> @this)
        {
            return @this.Join(@this.Count(), @join.CartesianType.NoReflexive);
        }
    }
}
