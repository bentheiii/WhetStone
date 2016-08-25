using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Looping
{
    public static class subsets
    {
        public static IEnumerable<IEnumerable<T>> SubSets<T>(this IEnumerable<T> @this)
        {
            return countUp.CountUp().Select(@this.SubSets).TakeWhile(a => a.Any()).Concat();
        }
        public static IEnumerable<IEnumerable<T>> SubSets<T>(this IEnumerable<T> @this, int setSize)
        {
            return @this.Join(setSize, join.CartesianType.NoReflexive | join.CartesianType.NoSymmatry);
        }
    }
}
