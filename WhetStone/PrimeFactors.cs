using System.Collections.Generic;

namespace NumberStone
{
    public static class primeFactors
    {
        public static IEnumerable<int> Primefactors(this int x)
        {
            while (x != 1)
            {
                var f = x.SmallestFactor();
                yield return f;
                x = x / f;
            }
        }
        public static IEnumerable<long> Primefactors(this long x)
        {
            while (x != 1)
            {
                var f = x.SmallestFactor();
                yield return f;
                x = x / f;
            }
        }
    }
}
