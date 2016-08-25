using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Arrays;
using WhetStone.Looping;
using WhetStone.SystemExtensions;

namespace WhetStone.NumbersMagic
{
    public static class factors
    {
        public static IEnumerable<int> Factors(this int x)
        {
            if (x <= 0)
                throw new ArithmeticException("cannot find factorization of a non-positive number");
            var primes = x.Primefactors().ToLookup(a => a).Select(a => Tuple.Create(a.Key, a.Count())).ToArray();
            foreach (var primesubsets in Enumerable.Select(primes, a => range.IRange(a.Item2).Select(z => a.Item1.pow(z)).ToArray()).ToArray().Join())
            {
                yield return primesubsets.GetProduct((a, b) => a * b);
            }
        }
    }
}
