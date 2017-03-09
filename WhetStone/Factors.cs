using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Looping;

namespace NumberStone
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class factors
    {
        /// <summary>
        /// Gets all the whole numbers that divide <paramref name="x"/>.
        /// </summary>
        /// <param name="x">The numbers to find factors of.</param>
        /// <returns>All the whole numbers that divide <paramref name="x"/></returns>
        public static IList<int> Factors(this int x)
        {
            if (x <= 0)
                throw new ArithmeticException("cannot find factorization of a non-positive number");
            if (x == 1)
                return new[] {1};
            var primes = x.Primefactors().ToOccurancesSorted().ToArray();
            IList<IList<int>> rangelist = primes.Select(a => (IList<int>)yieldAggregate.YieldAggregate(t=>t*a.Item1,1).Take(a.Item2+1).ToArray());
            var j = rangelist.Join();
            return j.Select(a => a.Aggregate(1,(t, y) => t*y));
        }
    }
}
