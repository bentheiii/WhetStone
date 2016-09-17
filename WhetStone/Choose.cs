using System;
using System.Linq;
using System.Numerics;
using WhetStone.Looping;

namespace NumberStone
{
    public static class choose
    {
        public static BigInteger Choose(int n, params int[] k)
        {
            if (k.Length == 0)
            {
                return 1;
            }
            int sum = k.Sum();
            if (sum > n)
            {
                throw new ArithmeticException("cannot choose a subgroup larger than the supergroup");
            }
            if (sum != n)
            {
                k = k.Concat((n - sum).Enumerate()).ToArray();
            }
            BigProduct ret = new BigProduct();
            ret.MultiplyFactorial(n);
            foreach (int i in k)
            {
                ret.DivideFactorial(i);
            }
            return ret.toNum();
        }
    }
}