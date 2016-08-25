using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Arrays;
using WhetStone.Looping;

namespace WhetStone.NumbersMagic
{
    public static class partition
    {
        public static IEnumerable<IEnumerable<int>> Partition(this int @this, int? largestpart = int.MaxValue)
        {
            if (@this < 0)
                yield break;
            if (@this == 0)
            {
                yield return Enumerable.Empty<int>();
                yield break;
            }
            foreach (int i in range.IRange(Math.Min(largestpart ?? @this, @this), 1, -1))
            {
                foreach (var p in (@this - i).Partition(largestpart.HasValue ? i : (int?)null))
                {
                    yield return i.Enumerate().Concat(p);
                }
            }
        }
        public static int PartitionCount(this int t)
        {
            var val = new[]
            {
                1, 1, 2, 3, 5, 7, 11, 15, 22, 30, 42, 56, 77, 101, 135, 176, 231, 297, 385, 490, 627, 792, 1002, 1255, 1575, 1958, 2436, 3010,
                3718, 4565, 5604, 6842, 8349, 10143, 12310, 14883, 17977, 21637, 26015, 31185, 37338, 44583, 53174, 63261, 75175, 89134, 105558,
                124754, 147273, 173525
            };
            return new LazyArray<int>((@this, cache) =>
            {
                if (val.IsWithinBounds(t))
                    return val[t];
                if (@this < 0)
                    return 0;
                var partindices = pentagonals.Pentagonals(1).Select(a => @this - a).TakeWhile(a => a >= 0).ToArray();
                var parts = Enumerable.Select(partindices, a => cache[a]);
                var sums = parts.Chunk(2).Select(a => a[0] + a[1]);
                return sums.Chunk(2).Sum(a => a.Sum());
            })[t];
        }
    }
}
