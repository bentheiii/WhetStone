using System;
using System.Linq;
using System.Numerics;
using WhetStone.Guard;
using WhetStone.Looping;

namespace NumberStone
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class choose
    {
        /// <summary>
        /// Gets the multinomial coefficient of <paramref name="n"/> and one or more numbers in <paramref name="k"/>.
        /// </summary>
        /// <param name="n">The super of the multinomial coefficient</param>
        /// <param name="k">The subs of the multinomial coefficient</param>
        /// <returns>The result of the multinomial coefficient.</returns>
        /// <exception cref="ArgumentException">If the sum of <paramref name="k"/> is higher than <paramref name="n"/>, or if any of <paramref name="k"/> is negative.</exception>
        public static BigInteger Choose(int n, params int[] k)
        {
            if (k.Length == 0)
            {
                return 1;
            }
            IGuard<int> anyNeg = new Guard<int>(1);
            int sum = k.HookFirst(anyNeg,a=>a<0).Sum();
            if (sum > n)
            {
                throw new ArgumentException("cannot choose a subgroup larger than the supergroup.");
            }
            if (anyNeg.value < 0)
            {
                throw new ArgumentException("cannot compute multinomial of negative arguments.");
            }
            if (sum != n)
            {
                append.Append(ref k, n - sum);
            }
            BigProduct ret = new BigProduct();
            ret.MultiplyFactorial(n);
            foreach (int i in k)
            {
                ret.DivideFactorial(i);
            }
            return ret.toFraction().Numerator;
        }
    }
}