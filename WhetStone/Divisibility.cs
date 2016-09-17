using System;
using System.Numerics;
using WhetStone.Looping;
using WhetStone.RecursiveQuerier;

namespace NumberStone
{
    public static class divisibility
    {
        public static int Divisibility(this int n, int b)
        {
            if (n == 1 || b > n || n % b != 0)
                return 0;
            var sq = b * b;
            var th = sq * b;
            var k = Divisibility(th, n);
            var p = n / (int)Math.Pow(th, k);
            return 3 * k + (p % sq == 0 ? 2 : (p % b == 0 ? 1 : 0));
        }
        public static int Divisibility(this BigInteger n, BigInteger b)
        {
            var l = BigInteger.Log(n, 2);
            if (l > 32)
            {
                var q = new HalvingQuerier<BigInteger>(b, (x, y) => x * y, 1);
                return binarySearch.BinarySearch(a => (n % q[a]).IsZero, 0, (int)(l / BigInteger.Log(b, 2)) + 2);
            }
            if (n == 1 || b > n || n % b != 0)
                return 0;
            var sq = b * b;
            var th = sq * b;
            var fo = th * b;
            var k = Divisibility(fo, n);
            var p = n / BigInteger.Pow(fo, k);
            return 4 * k + (p % th == 0 ? 3 : (p % sq == 0 ? 2 : (p % b == 0 ? 1 : 0)));
        }
    }
}
