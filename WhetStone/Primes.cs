using System.Collections.Generic;
using System.Linq;
using WhetStone.Looping;
using WhetStone.SystemExtensions;

namespace WhetStone.NumbersMagic
{
    public static class primes
    {
        public static IEnumerable<int> Primes()
        {
            foreach (int p in isPrime.PrimeList)
            {
                yield return p;
            }
            List<int> ret = new List<int>(isPrime.PrimeList);
            int i = ret.Last();
            while (true)
            {
                i += 2;
                int i1 = i;
                if (!i.isProbablyPrime(3))
                    continue;
                int sqrt = i.sqrt().ceil()+1;
                if (ret.TakeWhile(a=>a<sqrt+1).All(a => i1 % a != 0))
                {
                    ret.Add(i);
                    yield return i;
                }
            }
        }
        public static IEnumerable<int> Primes(int max)
        {
            if (max <= isPrime.PrimeList.Last())
            {
                foreach (var t in isPrime.PrimeList.TakeWhile(a => a < max))
                {
                    yield return t;
                }
                yield break;
            }
            var nums = new SortedSet<int>(range.Range(2, max));
            while (nums.Count > 0)
            {
                var y = nums.Min;
                yield return y;
                foreach (int i in range.Range(y, max, y))
                {
                    nums.Remove(i);
                }
            }
        }
    }
}
