using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using WhetStone.Comparison;
using WhetStone.Looping;

namespace NumberStone
{
    public static class greatestCommonDivisor
    {
        public static BigInteger GreatestCommonDivisor(params BigInteger[] val)
        {
            return GreatestCommonDivisor(val.AsList());
        }
        public static BigInteger GreatestCommonDivisor(IList<BigInteger> val)
        {
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
                val = val.SplitAt(val.Count / 2).Select(a=>GreatestCommonDivisor(a.AsList()));
            }
        }
        public static BigInteger GreatestCommonDivisor(params long[] val)
        {
            return GreatestCommonDivisor(val.AsList());
        }
        public static long GreatestCommonDivisor(IList<long> val)
        {
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
                        if (a < 0)
                            a *= -1;
                        if (b < 0)
                            b *= -1;
                        minmax.MinMax(ref b, ref a);
                        while (b != 0)
                        {
                            long temp = a % b;
                            a = b;
                            b = temp;
                        }
                        return a;
                }
                val = val.SplitAt(val.Count / 2).Select(a=> GreatestCommonDivisor(a.AsList()));
            }
        }
        public static int GreatestCommonDivisor(params int[] val)
        {
            return (int)GreatestCommonDivisor(val.Select(a => (long)a).ToArray());
        }
    }
}
