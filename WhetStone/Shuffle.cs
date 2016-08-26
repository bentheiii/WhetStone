using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Random;

namespace WhetStone.Looping
{
    public static class shuffle
    {
        public static T[] Shuffle<T>(this IList<T> arr, RandomGenerator gen = null)
        {
            gen = gen ?? new GlobalRandomGenerator();
            T[] x = arr.ToArray(arr.Count);
            for (int i = 0; i < x.Length; i++)
            {
                int j = gen.Int(i, x.Length);
                x.Swap(i, j);
            }
            return x;
        }
        public static T Pick<T>(this IEnumerable<T> @this, RandomGenerator gen = null)
        {
            gen = gen ?? new GlobalRandomGenerator();
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
    }
}
