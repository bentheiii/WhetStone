using System.Collections.Generic;
using WhetStone.Random;

namespace WhetStone.Looping
{
    public static class shuffle
    {
        public static T[] Shuffle<T>(this IList<T> arr, RandomGenerator gen = null, int? shufflecount = null)
        {
            int length = shufflecount ?? arr.Count;
            gen = gen ?? new GlobalRandomGenerator();
            T[] x = arr.ToArray(arr.Count);
            foreach (int i in range.Range(length))
            {
                int j = gen.Int(i, x.Length);
                x.Swap(i, j);
            }
            return x;
        }
    }
}
