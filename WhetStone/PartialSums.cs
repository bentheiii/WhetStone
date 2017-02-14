using System.Collections.Generic;
using NumberStone;
using WhetStone.Fielding;

namespace WhetStone.Looping
{
    public static class partialSums
    {
        public static IEnumerable<T> PartialSums<T>(this IEnumerable<T> @this)
        {
            var f = Fields.getField<T>();
            return @this.YieldAggregate(f.add, f.zero);
        }
        public static IEnumerable<T> PartialCompensatingSums<T>(this IEnumerable<T> @this)
        {
            KahanSum<T> ret = new KahanSum<T>();
            yield return ret.Sum;
            foreach (T t in @this)
            {
                ret.Add(t);
                yield return ret.Sum;
            }
        }
        public static IEnumerable<double> PartialCompensatingSums(this IEnumerable<double> @this)
        {
            var ret = new KahanSum();
            yield return 0.0;
            foreach (var t in @this)
            {
                ret.Add(t);
                yield return ret.Sum;
            }
        }
    }
}
