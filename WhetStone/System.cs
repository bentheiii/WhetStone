using System;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text;
using Numerics;

namespace WhetStone.SystemExtensions
{
    public static class SystemExtension
    {
        public static int Indicator(this bool @this) => @this ? 1 : 0;
        public static T Indicator<T>(this bool @this, T then, T @else) => @this ? then : @else;
        public static decimal pow(this decimal powbase, decimal powpower)
        {
            return (decimal)Math.Pow((double)powbase, (double)powpower);
        }
        public static decimal pow(this decimal powbase, int powpower)
        {
            return (powbase).pow((decimal)powpower);
        }
        public static double pow(this double powbase, double powpower)
        {
            return Math.Pow(powbase, powpower);
        }
        public static double pow(this double powbase, int powpower)
        {
            return (powbase).pow((double)powpower);
        }
        public static float pow(this float powbase, float powpower)
        {
            return (float)Math.Pow(powbase, powpower);
        }
        public static float pow(this float powbase, int powpower)
        {
            return (powbase).pow((float)powpower);
        }
        public static double pow(this int powbase, double powpower)
        {
            return ((double)powbase).pow(powpower);
        }
        public static int pow(this int powbase, int powpower)
        {
            return (int)((double)powbase).pow((double)powpower);
        }
        public static byte pow(this byte powbase, byte powpower)
        {
            return (byte)((double)powbase).pow((double)powpower);
        }
        public static sbyte pow(this sbyte powbase, sbyte powpower)
        {
            return (sbyte)((double)powbase).pow((double)powpower);
        }
        public static short pow(this short powbase, short powpower)
        {
            return (short)((double)powbase).pow((double)powpower);
        }
        public static ushort pow(this ushort powbase, ushort powpower)
        {
            return (ushort)((double)powbase).pow((double)powpower);
        }
        public static uint pow(this uint powbase, uint powpower)
        {
            return (uint)((double)powbase).pow(powpower);
        }
        public static long pow(this long powbase, long powpower)
        {
            switch (powbase)
            {
                case 1:
                    return 1;
                case -1:
                    return powpower % 2 == 0 ? 1 : -1;
            }
            if (powpower < 0)
                return 0;
            switch (powpower)
            {
                case 0:
                    return 1;
                case 1:
                    return powbase;
            }
            var half = powbase.pow(powpower / 2);
            half *= half;
            if (powpower % 2 == 1)
                half *= powbase;
            return half;
        }
        public static ulong pow(this ulong powbase, ulong powpower)
        {
            switch (powbase)
            {
                case 1:
                    return 1;
            }
            switch (powpower)
            {
                case 0:
                    return 1;
                case 1:
                    return powbase;
            }
            var half = powbase.pow(powpower / 2);
            half *= half;
            if (powpower % 2 == 1)
                half *= powbase;
            return half;
        }
        public static int powmod(this int powbase, int power, int modulo)
        {
            if (power < 0)
                throw new ArithmeticException("cannot modpow to a negative power");
            if (power == 0)
                return 1;
            powbase %= modulo;
            int halfpow = powmod(powbase, power / 2, modulo);
            halfpow %= modulo;
            int ret = (halfpow * halfpow) % modulo;
            if (power % 2 == 1)
            {
                ret *= powbase;
                ret %= modulo;
            }
            return ret;
        }
        public static long powmod(this long powbase, long power, long modulo)
        {
            if (power < 0)
                throw new ArithmeticException("cannot modpow to a negative power");
            if (power == 0)
                return 1;
            powbase %= modulo;
            long halfpow = powmod(powbase, power / 2, modulo);
            halfpow %= modulo;
            long ret = (halfpow * halfpow) % modulo;
            if (power % 2 == 1)
            {
                ret *= powbase;
                ret %= modulo;
            }
            return ret;
        }
        public static double log(this double logpow, double logbase = Math.E)
        {
            return Math.Log(logpow, logbase);
        }
        public static double log(this double logpow, int logbase)
        {
            return logpow.log((double)logbase);
        }
        public static double log(this int logpow, double logbase = Math.E)
        {
            return ((double)logpow).log(logbase);
        }
        public static double log(this int logpow, int logbase)
        {
            return ((double)logpow).log((double)logbase);
        }
        public static double log(this long logpow, double logbase = Math.E)
        {
            return ((double)logpow).log(logbase);
        }
        public static double log(this ulong logpow, double logbase = Math.E)
        {
            return ((double)logpow).log(logbase);
        }
        public static double log(this uint logpow, double logbase = Math.E)
        {
            return ((double)logpow).log(logbase);
        }
        public static double abs(this double a)
        {
            return Math.Abs(a);
        }
        public static int abs(this int a)
        {
            return Math.Abs(a);
        }
        public static long abs(this long a)
        {
            return a < 0 ? -a : a;
        }
        public static sbyte abs(this sbyte a)
        {
            return (sbyte)(a < 0 ? -a : a);
        }
        public static short abs(this short a)
        {
            return (short)(a < 0 ? -a : a);
        }
        public static float abs(this float a)
        {
            return Math.Abs(a);
        }
        public static decimal abs(this decimal a)
        {
            return a < 0 ? -a : a;
        }
        /// <summary>
        /// in radians
        /// </summary>
        public static double sin(this double a)
        {
            return Math.Sin(a);
        }
        /// <summary>
        /// in radians
        /// </summary>
        public static double cos(this double a)
        {
            return Math.Cos(a);
        }
        /// <summary>
        /// in radians
        /// </summary>
        public static double tan(this double a)
        {
            return Math.Tan(a);
        }
        /// <summary>
        /// in radians
        /// </summary>
        public static double asin(this double a)
        {
            return Math.Asin(a);
        }
        /// <summary>
        /// in radians
        /// </summary>
        public static double acos(this double a)
        {
            return Math.Acos(a);
        }
        /// <summary>
        /// in radians
        /// </summary>
        public static double atan(this double a)
        {
            return Math.Atan(a);
        }
        public static int ceil(this double a)
        {
            return (int)Math.Ceiling(a);
        }
        public static int floor(this double a)
        {
            return (int)Math.Floor(a);
        }
        public static double sinh(this double a)
        {
            return Math.Sinh(a);
        }
        public static double cosh(this double a)
        {
            return Math.Cosh(a);
        }
        public static double tanh(this double a)
        {
            return Math.Tanh(a);
        }
        public static double log10(this double a)
        {
            return Math.Log10(a);
        }
        public static double log10(this int a)
        {
            return Math.Log10(a);
        }
        public static int sign(this int x)
        {
            return Math.Sign(x);
        }
        public static int sign(this double x)
        {
            return Math.Sign(x);
        }
        public static int sign(this long x)
        {
            return Math.Sign(x);
        }
        public static double sqrt(this double x)
        {
            return Math.Sqrt(x);
        }
        public static double sqrt(this int x)
        {
            return ((double)x).sqrt();
        }
        public static BigRational abs(this BigRational a)
        {
            return BigRational.Abs(a);
        }
        public static BigRational pow(this BigRational a, BigRational pow, int iterations)
        {
            return a.pow(pow.Numerator).root(pow.Denominator, iterations);
        }
        public static BigRational pow(this BigRational a, BigRational pow, BigRational delta)
        {
            return a.pow(pow.Numerator).root(a.Denominator, delta);
        }
        public static BigRational pow(this BigRational a, BigInteger exponent)
        {
            return BigRational.Pow(a, exponent);
        }
        public static BigRational root(this BigRational @this, BigInteger n, int iterations)
        {
            return @this.Numerator.root(n, iterations) / @this.Denominator.root(n, iterations);
        }
        public static BigRational root(this BigRational @this, BigInteger n, BigRational delta)
        {
            return @this.Numerator.root(n, delta) / @this.Denominator.root(n, delta);
        }
        public static string ToDecimalString(this BigRational r, int precision)
        {
            var fraction = r.GetFractionPart();

            // Case where the rational number is a whole number
            if (fraction.Numerator == 0 && fraction.Denominator == 1)
            {
                return r.GetWholePart() + ".0";
            }

            var adjustedNumerator = (fraction.Numerator
                                     * BigInteger.Pow(10, precision));
            var decimalPlaces = adjustedNumerator / fraction.Denominator;

            // Case where precision wasn't large enough.
            if (decimalPlaces == 0)
            {
                return "0.0";
            }

            // Give it the capacity for around what we should need for 
            // the whole part and total precision
            // (this is kinda sloppy, but does the trick)
            var sb = new StringBuilder(precision + r.ToString().Length);

            bool noMoreTrailingZeros = false;
            for (int i = precision; i > 0; i--)
            {
                if (!noMoreTrailingZeros)
                {
                    if ((decimalPlaces % 10) == 0)
                    {
                        decimalPlaces = decimalPlaces / 10;
                        continue;
                    }

                    noMoreTrailingZeros = true;
                }

                // Add the right most decimal to the string
                sb.Insert(0, BigInteger.Abs(decimalPlaces % 10));
                decimalPlaces = decimalPlaces / 10;
            }

            // Insert the whole part and decimal
            sb.Insert(0, ".");
            sb.Insert(0, r.GetWholePart());

            return sb.ToString();
        }
        public static BigRational toRational(this BigInteger @this)
        {
            return new BigRational(@this);
        }
        public static int magnitude(this BigInteger x, int newbase = 10)
        {
            if (x <= 0)
                return 0;
            return (int)Math.Floor(BigInteger.Log(x, newbase));
        }
        public static bool wholetomagnitude(this BigInteger x, int magnitudeadjust = 0, int originalbase = 10)
        {

            BigInteger powerof = BigInteger.Pow(originalbase, x.magnitude(originalbase) - magnitudeadjust);
            if (powerof.IsZero)
                return true;
            return (x % powerof == 0);
        }
        public static BigInteger pow(this BigInteger a, BigInteger b)
        {
            BigInteger rem;
            var div = BigInteger.DivRem(b, new BigInteger(int.MaxValue), out rem);
            BigInteger divpow = BigInteger.One;
            if (!div.IsZero)
            {
                divpow = BigInteger.Pow(a.pow(div), int.MaxValue);
            }
            BigInteger modPow = BigInteger.Pow(a, (int)rem);
            return divpow * modPow;
        }
        public static BigRational root(this BigInteger @this, BigInteger n, BigRational delta)
        {
            BigRational prev = -@this, ret = @this;
            while ((prev - @this).abs() >= delta)
            {
                prev = ret;
                ret = new BigRational(BigInteger.One, n) * ((n - 1) * ret + @this / ret.pow(n - 1));
            }
            return ret;
        }
        public static BigRational root(this BigInteger @this, BigInteger n, int iterations)
        {
            BigRational ret = @this;
            for(int i = 0; i < iterations; i++)
            {
                ret = new BigRational(BigInteger.One, n) * ((n - 1) * ret + @this / ret.pow(n - 1));
            }
            return ret;
        }
        public static BigInteger BigFactorial(this int @this)
        {
            BigInteger a = BigInteger.One;
            for (int i = 2; i <= @this; i++)
            {
                a *= i;
            }
            return a;
        }
        public static BigInteger BigFactorial(this BigInteger @this)
        {
            BigInteger a = BigInteger.One;
            for (int i = 2; i <= @this; i++)
            {
                a *= i;
            }
            return a;
        }
        public static T Mutate<T>(this T @this, Action<T> mutation)
        {
            mutation(@this);
            return @this;
        }
    }
    public static class Cloning
    {
        public static T Copy<T>(this T @this) where T : ICloneable
        {
            return (T)@this.Clone();
        }
        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
    }
    public static class ObjIdExtentensions
    {
        public static long GetId(this ObjectIDGenerator @this, object o)
        {
            bool proxy;
            return @this.GetId(o, out proxy);
        }
    }
    public static class IdDistributer
    {
        private static readonly ObjectIDGenerator _g = new ObjectIDGenerator();
        public static long getId<T>(T o)
        {
            return _g.GetId(o);
        }
    }
}
