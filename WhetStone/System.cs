using System;
using System.Numerics;
using Numerics;

namespace WhetStone.SystemExtensions
{
    /// <summary>
    /// A static class for common extension methods.
    /// </summary>
    public static class SystemExtension
    {
        /// <summary>
        /// Get 0 or 1 depending on a <see cref="bool"/>.
        /// </summary>
        /// <param name="this">The <see cref="bool"/> to use.</param>
        /// <returns>1 if <paramref name="this"/> is <see langword="true"/>, or 0 otherwise.</returns>
        public static int Indicator(this bool @this) => @this ? 1 : 0;
        /// <summary>
        /// Get a value depending on a <see cref="bool"/>.
        /// </summary>
        /// <typeparam name="T">The type of value to return.</typeparam>
        /// <param name="this">The <see cref="bool"/> to use.</param>
        /// <param name="then">The value to return if <see langword="true"/>.</param>
        /// <param name="else">The value to return if <see langword="false"/>.</param>
        /// <returns><paramref name="then"/> if <paramref name="this"/> is <see langword="true"/>, or <paramref name="else"/> otherwise.</returns>
        public static T Indicator<T>(this bool @this, T then, T @else) => @this ? then : @else;
        /// <summary>
        /// Get one <see cref="decimal"/> to the power of another.
        /// </summary>
        /// <param name="powbase">The base of the power.</param>
        /// <param name="powpower">The exponent of the power.</param>
        /// <returns><paramref name="powbase"/> raised to <paramref name="powpower"/>.</returns>
        public static decimal pow(this decimal powbase, decimal powpower)
        {
            return (decimal)Math.Pow((double)powbase, (double)powpower);
        }
        /// <summary>
        /// Get one <see cref="double"/> to the power of another.
        /// </summary>
        /// <param name="powbase">The base of the power.</param>
        /// <param name="powpower">The exponent of the power.</param>
        /// <returns><paramref name="powbase"/> raised to <paramref name="powpower"/>.</returns>
        public static double pow(this double powbase, double powpower)
        {
            return Math.Pow(powbase, powpower);
        }
        /// <summary>
        /// Get one <see cref="float"/> to the power of another.
        /// </summary>
        /// <param name="powbase">The base of the power.</param>
        /// <param name="powpower">The exponent of the power.</param>
        /// <returns><paramref name="powbase"/> raised to <paramref name="powpower"/>.</returns>
        public static float pow(this float powbase, float powpower)
        {
            return (float)Math.Pow(powbase, powpower);
        }
        /// <summary>
        /// Get one <see cref="int"/> to the power of another.
        /// </summary>
        /// <param name="powbase">The base of the power.</param>
        /// <param name="powpower">The exponent of the power.</param>
        /// <returns><paramref name="powbase"/> raised to <paramref name="powpower"/>.</returns>
        public static int pow(this int powbase, int powpower)
        {
            return (int)((double)powbase).pow(powpower);
        }
        /// <summary>
        /// Get one <see cref="byte"/> to the power of another.
        /// </summary>
        /// <param name="powbase">The base of the power.</param>
        /// <param name="powpower">The exponent of the power.</param>
        /// <returns><paramref name="powbase"/> raised to <paramref name="powpower"/>.</returns>
        public static byte pow(this byte powbase, byte powpower)
        {
            return (byte)((double)powbase).pow(powpower);
        }
        /// <summary>
        /// Get one <see cref="sbyte"/> to the power of another.
        /// </summary>
        /// <param name="powbase">The base of the power.</param>
        /// <param name="powpower">The exponent of the power.</param>
        /// <returns><paramref name="powbase"/> raised to <paramref name="powpower"/>.</returns>
        public static sbyte pow(this sbyte powbase, sbyte powpower)
        {
            return (sbyte)((double)powbase).pow(powpower);
        }
        /// <summary>
        /// Get one <see cref="short"/> to the power of another.
        /// </summary>
        /// <param name="powbase">The base of the power.</param>
        /// <param name="powpower">The exponent of the power.</param>
        /// <returns><paramref name="powbase"/> raised to <paramref name="powpower"/>.</returns>
        public static short pow(this short powbase, short powpower)
        {
            return (short)((double)powbase).pow(powpower);
        }
        /// <summary>
        /// Get one <see cref="ushort"/> to the power of another.
        /// </summary>
        /// <param name="powbase">The base of the power.</param>
        /// <param name="powpower">The exponent of the power.</param>
        /// <returns><paramref name="powbase"/> raised to <paramref name="powpower"/>.</returns>
        public static ushort pow(this ushort powbase, ushort powpower)
        {
            return (ushort)((double)powbase).pow(powpower);
        }
        /// <summary>
        /// Get one <see cref="uint"/> to the power of another.
        /// </summary>
        /// <param name="powbase">The base of the power.</param>
        /// <param name="powpower">The exponent of the power.</param>
        /// <returns><paramref name="powbase"/> raised to <paramref name="powpower"/>.</returns>
        public static uint pow(this uint powbase, uint powpower)
        {
            return (uint)((double)powbase).pow(powpower);
        }
        /// <summary>
        /// Get one <see cref="long"/> to the power of another.
        /// </summary>
        /// <param name="powbase">The base of the power.</param>
        /// <param name="powpower">The exponent of the power.</param>
        /// <returns><paramref name="powbase"/> raised to <paramref name="powpower"/>.</returns>
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
        /// <summary>
        /// Get one <see cref="ulong"/> to the power of another.
        /// </summary>
        /// <param name="powbase">The base of the power.</param>
        /// <param name="powpower">The exponent of the power.</param>
        /// <returns><paramref name="powbase"/> raised to <paramref name="powpower"/>.</returns>
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
        /// <summary>
        /// Approximate one <see cref="BigRational"/> to the power of another.
        /// </summary>
        /// <param name="a">The base of the power.</param>
        /// <param name="pow">The exponent of the power.</param>
        /// <param name="delta">The delta tolerance for root convergence.</param>
        /// <returns><paramref name="a"/> raised to <paramref name="pow"/> to within <paramref name="delta"/> tolerance.</returns>
        public static BigRational pow(this BigRational a, BigRational pow, BigRational delta)
        {
            return a.pow(pow.Numerator).root(a.Denominator, delta);
        }
        /// <summary>
        /// Get one <see cref="BigRational"/> to the power of an <see cref="BigInteger"/>.
        /// </summary>
        /// <param name="a">The base of the power.</param>
        /// <param name="exponent">The exponent of the power.</param>
        /// <returns><paramref name="a"/> raised to <paramref name="exponent"/>.</returns>
        public static BigRational pow(this BigRational a, BigInteger exponent)
        {
            return BigRational.Pow(a, exponent);
        }
        /// <summary>
        /// Get one <see cref="BigInteger"/> to the power of another.
        /// </summary>
        /// <param name="a">The base of the power.</param>
        /// <param name="b">The exponent of the power.</param>
        /// <returns><paramref name="a"/> raised to <paramref name="b"/>.</returns>
        public static BigInteger pow(this BigInteger a, BigInteger b)
        {
            /*
             * So BigInt only allows BigInt^int so what we do is this (m=int max value):
             * a^b = (a^[b/m])^m + a^(b%m)
             * note that a^[b/m] is recursive. Max stack size is log(b)/32
             */
            var div = BigInteger.DivRem(b, new BigInteger(int.MaxValue), out BigInteger rem);
            BigInteger divpow = BigInteger.One;
            if (!div.IsZero)
            {
                divpow = BigInteger.Pow(a.pow(div), int.MaxValue);
            }
            BigInteger modPow = BigInteger.Pow(a, (int)rem);
            return divpow * modPow;
        }
        /// <summary>
        /// Raises an <see cref="int"/> to the power of another with respect to a modulo.
        /// </summary>
        /// <param name="powbase">The base of the exponential.</param>
        /// <param name="power">The exponent of the exponential.</param>
        /// <param name="modulo">The modulo for the exponential.</param>
        /// <returns><paramref name="powbase"/> to the power of <paramref name="power"/> modulo <paramref name="modulo"/></returns>
        public static int powmod(this int powbase, int power, int modulo)
        {
            if (power < 0)
                throw new ArithmeticException("cannot modpow to a negative power");
            if (power == 0)
                return 1;
            powbase %= modulo;
            int halfpow = powmod(powbase, power / 2, modulo);
            int ret = (halfpow * halfpow) % modulo;
            if (power % 2 == 1)
            {
                ret *= powbase;
                ret %= modulo;
            }
            return ret;
        }
        /// <summary>
        /// Raises an <see cref="long"/> to the power of another with respect to a modulo.
        /// </summary>
        /// <param name="powbase">The base of the exponential.</param>
        /// <param name="power">The exponent of the exponential.</param>
        /// <param name="modulo">The modulo for the exponential.</param>
        /// <returns><paramref name="powbase"/> to the power of <paramref name="power"/> modulo <paramref name="modulo"/></returns>
        public static long powmod(this long powbase, long power, long modulo)
        {
            if (power < 0)
                throw new ArithmeticException("cannot modpow to a negative power");
            if (power == 0)
                return 1;
            powbase %= modulo;
            long halfpow = powmod(powbase, power / 2, modulo);
            long ret = (halfpow * halfpow) % modulo;
            if (power % 2 == 1)
            {
                ret *= powbase;
                ret %= modulo;
            }
            return ret;
        }
        /// <summary>
        /// Rounds a <see cref="double"/> up to the nearest integer.
        /// </summary>
        /// <param name="a">The <see cref="double"/> to round.</param>
        /// <returns><paramref name="a"/> rounded up.</returns>
        public static int ceil(this double a)
        {
            return (int)Math.Ceiling(a);
        }
        /// <summary>
        /// Rounds a <see cref="double"/> down to the nearest integer.
        /// </summary>
        /// <param name="a">The <see cref="double"/> to round.</param>
        /// <returns><paramref name="a"/> rounded down.</returns>
        public static int floor(this double a)
        {
            return (int)Math.Floor(a);
        }
        /// <summary>
        /// Approximate a root of an <see cref="BigRational"/>.
        /// </summary>
        /// <param name="this">The <see cref="BigRational"/> to root.</param>
        /// <param name="n">The degree of the root.</param>
        /// <param name="delta">The maximum tolerance for convergence.</param>
        /// <returns><paramref name="this"/>^(1/<paramref name="n"/>) to within <paramref name="delta"/> tolerance.</returns>
        public static BigRational root(this BigRational @this, BigInteger n, BigRational delta)
        {
            return @this.Numerator.root(n, delta) / @this.Denominator.root(n, delta);
        }
        /// <summary>
        /// Approximate a root of an <see cref="BigInteger"/>.
        /// </summary>
        /// <param name="this">The <see cref="BigInteger"/> to root.</param>
        /// <param name="n">The degree of the root.</param>
        /// <param name="delta">The maximum tolerance for convergence.</param>
        /// <returns><paramref name="this"/>^(1/<paramref name="n"/>) to within <paramref name="delta"/> tolerance.</returns>
        public static BigRational root(this BigInteger @this, BigInteger n, BigRational delta)
        {
            BigRational prev = -@this, ret = @this;
            while (BigRational.Abs(prev - @this) >= delta)
            {
                prev = ret;
                ret = new BigRational(BigInteger.One, n) * ((n - 1) * ret + @this / ret.pow(n - 1));
            }
            return ret;
        }
    }
}
