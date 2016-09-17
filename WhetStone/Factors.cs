using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;
using WhetStone.Looping;
using WhetStone.SystemExtensions;

namespace NumberStone
{
    public static class factors
    {
        public static IEnumerable<int> Factors(this int x)
        {
            if (x <= 0)
                throw new ArithmeticException("cannot find factorization of a non-positive number");
            var primes = x.Primefactors().ToLookup(a => a).Select(a => Tuple.Create(a.Key, a.Count())).ToArray();
            IList<IList<int>> rangelist = primes.Select(a => (IList<int>)range.IRange(a.Item2).Select(z => a.Item1.pow(z)).ToArray());
            foreach (var primesubsets in rangelist.Join())
            {
                yield return primesubsets.GetProduct((a, b) => a*b);
            }
        }
    }
}
