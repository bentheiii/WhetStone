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
            while (true)
            {
                switch (val.Length)
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
                val = val.SplitAt(val.Length / 2).Select(GreatestCommonDivisor).ToArray();
            }
        }
        public static long GreatestCommonDivisor(params long[] val)
        {
            while (true)
            {
                if (val.Length == 0)
                    return 1;
                if (val.Length == 1)
                    return val[0];
                if (val.Length == 2)
                {
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
                val = val.SplitAt(val.Length / 2).Select(GreatestCommonDivisor).ToArray();
            }
        }
        public static int GreatestCommonDivisor(params int[] val)
        {
            return (int)GreatestCommonDivisor(val.Select(a => (long)a).ToArray());
        }
    }
}
