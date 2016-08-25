using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Looping;

namespace WhetStone.NumbersMagic
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
