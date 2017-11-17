using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using WhetStone.Comparison;
using WhetStone.Looping;
using WhetStone.SystemExtensions;

namespace NumberStone
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class greatestCommonDivisor
    {
        /// <summary>
        /// Get the greatest common divisor of an array on <see cref="BigInteger"/>s.
        /// </summary>
        /// <param name="val">An <see cref="Array"/> of numbers.</param>
        /// <returns>The greatest common divisor of <paramref name="val"/>.</returns>
        public static BigInteger GreatestCommonDivisor(params BigInteger[] val)
        {
            val.ThrowIfNull(nameof(val));
            return GreatestCommonDivisor(val.AsList());
        }
        /// <summary>
        /// Get the greatest common divisor of an array on <see cref="BigInteger"/>s.
        /// </summary>
        /// <param name="val">An <see cref="IList{T}"/> of numbers.</param>
        /// <returns>The greatest common divisor of <paramref name="val"/>.</returns>
        public static BigInteger GreatestCommonDivisor(IList<BigInteger> val)
        {
            val.ThrowIfNull(nameof(val));
            while (true)
            {
                switch (val.Count)
                {
                    case 0:
                        throw new ArgumentException("val is empty!");
                    case 1:
                        return val[0];
                    case 2:
                        var a = val[0];
                        var b = val[1];
                        if (a < 0)
                        {
                            a *= -1;
                        }
                        if (b < 0)
                        {
                            b *= -1;
                        }
                        minmax.MinMax(ref b, ref a);
                        while (b != 0)
                        {
                            BigInteger temp = a % b;
                            a = b;
                            b = temp;
                        }
                        return a;
                }
                val = val.SplitAt(val.Count / 2).Select(a => GreatestCommonDivisor(a.AsList()));
            }
        }
        /// <summary>
        /// Get the greatest common divisor of an array on <see cref="long"/>s.
        /// </summary>
        /// <param name="val">An <see cref="Array"/> of numbers.</param>
        /// <returns>The greatest common divisor of <paramref name="val"/>.</returns>
        public static BigInteger GreatestCommonDivisor(params long[] val)
        {
            return GreatestCommonDivisor(val.AsList());
        }
        /// <summary>
        /// Get the greatest common divisor of an array on <see cref="long"/>s.
        /// </summary>
        /// <param name="val">An <see cref="IList{T}"/> of numbers.</param>
        /// <returns>The greatest common divisor of <paramref name="val"/>.</returns>
        public static long GreatestCommonDivisor(IList<long> val)
        {
            val = val.Select(Math.Abs);
            while (true)
            {
                switch (val.Count)
                {
                    case 0:
                        return 1;
                    case 1:
                        return val[0];
                    case 2:
                        var a = val[0];
                        var b = val[1];
                        minmax.MinMax(ref b, ref a);
                        while (b != 0)
                        {
                            long temp = a % b;
                            a = b;
                            b = temp;
                        }
                        return a;
                }
                val = val.SplitAt(val.Count / 2).Select(a => GreatestCommonDivisor(a.AsList()));
            }
        }
        /// <summary>
        /// Get the greatest common divisor of an array on <see cref="int"/>s.
        /// </summary>
        /// <param name="val">An <see cref="Array"/> of numbers.</param>
        /// <returns>The greatest common divisor of <paramref name="val"/>.</returns>
        public static int GreatestCommonDivisor(params int[] val)
        {
            return (int)GreatestCommonDivisor(val.Select(a => (long)a).ToArray());
        }
    }
}
