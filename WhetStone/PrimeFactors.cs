using System.Collections.Generic;

namespace NumberStone
{
    public static class primeFactors
    {
        public static IEnumerable<int> Primefactors(this int x)
        {
            int? last = null;
            while (x != 1)
            {
                var f = x.SmallestFactor(last);
                yield return f;
                last = f;
                x = x / f;
            }
        }
        public static IEnumerable<long> Primefactors(this long x)
        {
            long? last = null;
            while (x != 1)
            {
                var f = x.SmallestFactor(last);
                yield return f;
                last = f;
                x = x / f;
            }
        }
    }
}
