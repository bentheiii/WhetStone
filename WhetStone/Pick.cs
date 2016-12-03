using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Looping;
using WhetStone.Random;

namespace WhetStone
{
    public static class pick
    {
        public static T Pick<T>(this IEnumerable<T> @this, RandomGenerator gen = null)
        {
            return @this.Pick(1, gen).First();
        }
        public static IEnumerable<T> Pick<T>(this IEnumerable<T> @this, int count, RandomGenerator gen = null)
        {
            gen = gen ?? new GlobalRandomGenerator();
            int nom = count;
            int denom = @this.Count();
            if (nom > denom || nom < 0)
                throw new ArgumentException();
            foreach (var t in @this)
            {
                if (nom == 0)
                    yield break;
                if (gen.success(nom / (double)denom))
                {
                    yield return t;
                    nom--;
                }
                denom--;
            }
        }
        public static T Pick<T>(this IList<T> @this, RandomGenerator gen = null)
        {
            gen = gen ?? new GlobalRandomGenerator();
            return @this[gen.Int(@this.Count)];
        }
        public static IList<T> Pick<T>(this IList<T> @this, int count, RandomGenerator gen = null)
        {
            return range.Range(@this.Count).Join(count, join.CartesianType.NoReflexive | join.CartesianType.NoSymmatry).Pick(gen).Select(a => @this[a]).Reverse();
        }
    }
}
