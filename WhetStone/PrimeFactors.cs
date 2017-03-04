using System.Collections.Generic;

namespace NumberStone
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class primeFactors
    {
        /// <overloads>Get the prime factors of a number.</overloads>
        /// <summary>
        /// Get the prime factors of a number. 
        /// </summary>
        /// <param name="x">The number to find factors of.</param>
        /// <returns>All the prime factors of <paramref name="x"/>.</returns>
        /// <remarks>
        /// <para>If a prime divides <paramref name="x"/> more than once, it will be returned multiple times.</para>
        /// <para>The returned primes will be sorted in ascending order.</para>
        /// </remarks>
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
        /// <summary>
        /// Get the prime factors of a number. 
        /// </summary>
        /// <param name="x">The number to find factors of.</param>
        /// <returns>All the prime factors of <paramref name="x"/>.</returns>
        /// <remarks>
        /// <para>If a prime divides <paramref name="x"/> more than once, it will be returned multiple times.</para>
        /// <para>The returned primes will be sorted in ascending order.</para>
        /// </remarks>
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
