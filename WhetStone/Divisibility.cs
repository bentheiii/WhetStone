using System;
using System.Numerics;
using WhetStone.Looping;
using WhetStone.SystemExtensions;

namespace NumberStone
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class divisibility
    {
        /// <overloads>Returns the divisibility of a number by another</overloads>
        /// <summary>
        /// Get the number of times <paramref name="n"/> can be evenly divided by <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The dividend.</param>
        /// <param name="b">The divisor.</param>
        /// <returns>The maximum power by which you can raise <paramref name="b"/> and still have it divide <paramref name="n"/>.</returns>
        /// <remarks>Running time: O( log( log(n)/log(b) ) )</remarks>
        public static int Divisibility(this int n, int b)
        {
            n.ThrowIfAbsurd(nameof(n),false);
            if (b <= 1)
                throw new ArgumentOutOfRangeException(nameof(b));
            //The recursion idea is like this:
            //div(n,b):
            //k <- 2*div(n,b^2)
            //if b^(k+1) divides n, increment k by 1
            //return k 
            var d = Math.DivRem(n, b, out int rem);
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
            if (n <= 0)
                throw new ArgumentOutOfRangeException(nameof(n));
            if (b <= 1)
                throw new ArgumentOutOfRangeException(nameof(b));
            var d = BigInteger.DivRem(n, b, out BigInteger rem);
            if (n == 1 || b > n || rem != 0)
                return 0;
            n = d;
            var l = n.ToByteArray().Length*8;
            if (l > 64)
            {
                // ReSharper disable once CollectionNeverUpdated.Local
                var q = new LazyList<BigInteger>((i, laz) =>
                {
                    if (i == 0)
                        return 1;
                    if (i % 2 == 1 || laz.Initialized(i - 1))
                        return laz[i - 1] * b;
                    var x = laz[i / 2];
                    return x * x;
                });
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
