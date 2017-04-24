using System;
using System.Numerics;
using WhetStone.Looping;
using WhetStone.SystemExtensions;

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
            k.ThrowIfNull(nameof(k));
            n.ThrowIfAbsurd(nameof(n));
            if (k.Length == 0)
            {
                return 1;
            }
            var tal = k.Tally().TallyAggregate((a, b) => a + b, 0, a => a > n)
                                .TallyAny(a => a < 0, true).Do();
            if (tal.Item1 > n)
            {
                throw new ArgumentException("cannot choose a subgroup larger than the supergroup.");
            }
            if (tal.Item2)
            {
                throw new ArgumentException("cannot compute multinomial of negative arguments.");
            }
            if (tal.Item1 != n)
            {
                append.Append(ref k, n - tal.Item1);
            }
            BigProduct ret = new BigProduct();
            ret.MultiplyFactorial(n);
            foreach (int i in k)
            {
                ret.DivideFactorial(i);
            }
            return ret.toFraction().Item1;
        }
    }
}