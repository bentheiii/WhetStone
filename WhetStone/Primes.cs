using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;
using WhetStone.Looping;
using WhetStone.Random;
using WhetStone.SystemExtensions;

namespace NumberStone
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class primes
    {
        /// <summary>
        /// Get an <see cref="IEnumerable{T}"/> of all prime numbers. 
        /// </summary>
        /// <returns>An infinite <see cref="IEnumerable{T}"/> of primes.</returns>
        /// <remarks>The first primes are pre-compiled and will be returned i constant time. After that, the <see cref="IEnumerable{T}"/> will require O(n/log(n)) space.</remarks>
        public static IEnumerable<int> Primes()
        {
            foreach (int p in isPrime.PrimeList)
            {
                yield return p;
            }
            List<int> ret = new List<int>(isPrime.PrimeList);
            int i = ret.Last();
            var log = new LogarithmicProgresser(2,i);
            var gen = new LocalRandomGenerator();
            using (var steps = new[] {2, 6, 4, 2, 4, 2, 4, 6}.Cycle().GetEnumerator())
            {
                while (true)
                {
                    steps.MoveNext();
                    i += steps.Current;
                    log.Increment(steps.Current);
                    if (!i.isProbablyPrime(log.log,gen))
                        continue;
                    int sqrt = Math.Sqrt(i).ceil() + 1;
                    if (ret.TakeWhile(a => a < sqrt + 1).All(a => i%a != 0))
                    {
                        ret.Add(i);
                        yield return i;
                    }
                }
            }
        }
        private class CeiledPrimeList : LockedList<int>
        {
            private readonly int _ceiling;
            private readonly Lazy<int> _count;
            public CeiledPrimeList(int ceiling)
            {
                _ceiling = ceiling;
                _count = new Lazy<int>(()=>isPrime.PrimeList.BinarySearchStartBias(a => a < _ceiling) + 1);
            }
            public override int Count => _count.Value;
            public override IEnumerator<int> GetEnumerator()
            {
                return isPrime.PrimeList.TakeWhile(a => a < _ceiling).GetEnumerator();
            }
            public override int this[int index]
            {
                get
                {
                    var ret = isPrime.PrimeList[index];
                    if (ret >= _ceiling)
                        throw new IndexOutOfRangeException();
                    return ret;
                }
            }
            public override int IndexOf(int item)
            {
                return this.BinarySearchStartBias(item);
            }
            public override bool Contains(int item)
            {
                return IndexOf(item) != -1;
            }
        }
        /// <summary>
        /// Get all prime numbers under a maximum bound.
        /// </summary>
        /// <param name="max">The maximum bound of the returned primes (exclusive).</param>
        /// <returns>All primes under <paramref name="max"/>.</returns>
        /// <remarks>If <paramref name="max"/> is small enough (under <see cref="isPrime.PrimeList"/>'s last value), the returned value will be an <see cref="IList{T}"/>.</remarks>
        public static IEnumerable<int> Primes(int max)
        {
            if (max <= isPrime.PrimeList.Last())
            {
                return new CeiledPrimeList(max);
            }
            return EuclidPrimes(max);
        }
        private static IEnumerable<int> EuclidPrimes(int max)
        {
            var preload = new[] {2, 3, 5, 7, 11, 13, 17, 19, 23, 29};
            foreach (int i in preload)
            {
                yield return i;
            }
            var nums = new SortedSet<int>(new[] { 2, 6, 4, 2, 4, 2, 4, 6 }.Cycle().YieldAggregate((a,b)=>a+b,29).TakeWhile(a=>a<max));
            foreach (int y in preload)
            {
                foreach (int i in range.Range(y, max, y))
                {
                    nums.Remove(i);
                }
            }

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
