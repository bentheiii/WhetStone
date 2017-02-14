﻿using System;
using System.Numerics;
using WhetStone.Looping;
using WhetStone.RecursiveQuerier;

namespace NumberStone
{
    //todo test this
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class divisibility
    {
        //todo overloads
        /// <summary>
        /// Get the number of times <paramref name="n"/> can be evenly divided by <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The dividend.</param>
        /// <param name="b">The divisor.</param>
        /// <returns>The maximum power by which you can raise <paramref name="b"/> and still have it divide <paramref name="n"/>.</returns>
        /// <remarks>Running time: O( log( log(n)/log(b) ) )</remarks>
        public static int Divisibility(this int n, int b)
        {
            //The recursion idea is like this:
            //div(n,b):
            //k <- 2*div(n,b^2)
            //if b^(k+1) divides n, increment k by 1
            //return k 
            int rem;
            var d = Math.DivRem(n, b, out rem);
            if (n == 1 || b > n || n % b != 0)
                return 0;
            n = d;
            var sq = b * b;
            var th = sq * b;
            var k = Divisibility(n, th);
            var p = n / (int)Math.Pow(th, k);
            return 3 * k + (p % sq == 0 ? 2 : (p % b == 0 ? 1 : 0)) +1;
        }
        /// <summary>
        /// Get the number of times <paramref name="n"/> can be evenly divided by <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The dividend.</param>
        /// <param name="b">The divisor.</param>
        /// <returns>The maximum power by which you can raise <paramref name="b"/> and still have it divide <paramref name="n"/>.</returns>
        /// <remarks>
        /// <para>Running time: O( log( log(n)/log(b) ) )</para>
        /// <para>If <paramref name="n"/> is larger than 64 bits, performs a binary search over the solution space (same computational complexity).</para>
        /// </remarks>
        public static int Divisibility(this BigInteger n, BigInteger b)
        {
            BigInteger rem;
            var d = BigInteger.DivRem(n, b, out rem);
            if (n == 1 || b > n || rem != 0)
                return 0;
            n = d;
            var l = n.ToByteArray().Length*8;
            if (l > 64)
            {
                // ReSharper disable once CollectionNeverUpdated.Local
                var q = new HalvingQuerier<BigInteger>(b, (x, y) => x * y, 1);
                return binarySearch.BinarySearch(a => (n % q[a]).IsZero, 0, (int)(l / BigInteger.Log(b, 2)) + 2)+1;
            }
            var sq = b * b;
            var th = sq * b;
            var fo = th * b;
            var k = Divisibility(n, fo);
            var p = n / BigInteger.Pow(fo, k);
            return 4 * k + (p % th == 0 ? 3 : (p % sq == 0 ? 2 : (p % b == 0 ? 1 : 0))) + 1;
        }
    }
}
