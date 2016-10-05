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
        public static LockedList<int> Factors(this int x)
        {
            if (x <= 0)
                throw new ArithmeticException("cannot find factorization of a non-positive number");
            var primes = x.Primefactors().ToLookup(a => a).Select(a => Tuple.Create(a.Key, a.Count())).ToArray();
            IList<IList<int>> rangelist = primes.Select(a => (IList<int>)yieldAggregate.YieldAggregate((t)=>t*a.Item1,1).Take(a.Item2+1).ToArray());
            var j = rangelist.Join();
            return j.Select(a => a.GetProduct((t, y) => t*y));
        }
    }
}
