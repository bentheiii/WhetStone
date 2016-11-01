using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;
using WhetStone.Looping;

namespace NumberStone
{
    public static class factors
    {
        public static LockedList<int> Factors(this int x)
        {
            if (x <= 0)
                throw new ArithmeticException("cannot find factorization of a non-positive number");
            if (x == 1)
                return new[] {1}.ToLockedList();
            var primes = x.Primefactors().ToOccurancesSorted().ToArray();
            IList<IList<int>> rangelist = primes.Select(a => (IList<int>)yieldAggregate.YieldAggregate(t=>t*a.Item1,1).Take(a.Item2+1).ToArray());
            var j = rangelist.Join();
            return j.Select(a => a.GetProduct((t, y) => t*y));
        }
    }
}
