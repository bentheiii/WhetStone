﻿using System;
using System.Numerics;
using WhetStone.Fielding;

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
        /// Get 0 or 1 depending on a <see cref="bool"/>.
        /// </summary>
        /// <param name="this">The <see cref="bool"/> to use.</param>
        /// <returns>1 if <paramref name="this"/> is <see langword="true"/>, or 0 otherwise.</returns>
        public static T Indicator<T>(this bool @this)
        {
            var f = Fields.getField<T>();
            return @this ? f.one : f.zero;
        }
        /// <summary>
        /// Get one <see cref="decimal"/> to the power of another.
        /// </summary>
        /// <param name="powbase">The base of the power.</param>
        /// <param name="powpower">The exponent of the power.</param>
        /// <returns><paramref name="powbase"/> raised to <paramref name="powpower"/>.</returns>
        public static decimal Pow(this decimal powbase, decimal powpower)
        {
            return (decimal)Math.Pow((double)powbase, (double)powpower);
        }
        /// <summary>
        /// Get one <see cref="double"/> to the power of another.
        /// </summary>
        /// <param name="powbase">The base of the power.</param>
        /// <param name="powpower">The exponent of the power.</param>
        /// <returns><paramref name="powbase"/> raised to <paramref name="powpower"/>.</returns>
        public static double Pow(this double powbase, double powpower)
        {
            return Math.Pow(powbase, powpower);
        }
        /// <summary>
        /// Get one <see cref="float"/> to the power of another.
        /// </summary>
        /// <param name="powbase">The base of the power.</param>
        /// <param name="powpower">The exponent of the power.</param>
        /// <returns><paramref name="powbase"/> raised to <paramref name="powpower"/>.</returns>
        public static float Pow(this float powbase, float powpower)
        {
            return (float)Math.Pow(powbase, powpower);
        }
        /// <summary>
        /// Get one <see cref="int"/> to the power of another.
        /// </summary>
        /// <param name="powbase">The base of the power.</param>
        /// <param name="powpower">The exponent of the power.</param>
        /// <returns><paramref name="powbase"/> raised to <paramref name="powpower"/>.</returns>
        public static int Pow(this int powbase, int powpower)
        {
            return (int)((double)powbase).Pow(powpower);
        }
        /// <summary>
        /// Get one <see cref="byte"/> to the power of another.
        /// </summary>
        /// <param name="powbase">The base of the power.</param>
        /// <param name="powpower">The exponent of the power.</param>
        /// <returns><paramref name="powbase"/> raised to <paramref name="powpower"/>.</returns>
        public static byte Pow(this byte powbase, byte powpower)
        {
            return (byte)((double)powbase).Pow(powpower);
        }
        /// <summary>
        /// Get one <see cref="sbyte"/> to the power of another.
        /// </summary>
        /// <param name="powbase">The base of the power.</param>
        /// <param name="powpower">The exponent of the power.</param>
        /// <returns><paramref name="powbase"/> raised to <paramref name="powpower"/>.</returns>
        public static sbyte Pow(this sbyte powbase, sbyte powpower)
        {
            return (sbyte)((double)powbase).Pow(powpower);
        }
        /// <summary>
        /// Get one <see cref="short"/> to the power of another.
        /// </summary>
        /// <param name="powbase">The base of the power.</param>
        /// <param name="powpower">The exponent of the power.</param>
        /// <returns><paramref name="powbase"/> raised to <paramref name="powpower"/>.</returns>
        public static short Pow(this short powbase, short powpower)
        {
            return (short)((double)powbase).Pow(powpower);
        }
        /// <summary>
        /// Get one <see cref="ushort"/> to the power of another.
        /// </summary>
        /// <param name="powbase">The base of the power.</param>
        /// <param name="powpower">The exponent of the power.</param>
        /// <returns><paramref name="powbase"/> raised to <paramref name="powpower"/>.</returns>
        public static ushort Pow(this ushort powbase, ushort powpower)
        {
            return (ushort)((double)powbase).Pow(powpower);
        }
        /// <summary>
        /// Get one <see cref="uint"/> to the power of another.
        /// </summary>
        /// <param name="powbase">The base of the power.</param>
        /// <param name="powpower">The exponent of the power.</param>
        /// <returns><paramref name="powbase"/> raised to <paramref name="powpower"/>.</returns>
        public static uint Pow(this uint powbase, uint powpower)
        {
            return (uint)((double)powbase).Pow(powpower);
        }
        /// <summary>
        /// Get one <see cref="long"/> to the power of another.
        /// </summary>
        /// <param name="powbase">The base of the power.</param>
        /// <param name="powpower">The exponent of the power.</param>
        /// <returns><paramref name="powbase"/> raised to <paramref name="powpower"/>.</returns>
        public static long Pow(this long powbase, long powpower)
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
            var half = powbase.Pow(powpower / 2);
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
        public static ulong Pow(this ulong powbase, ulong powpower)
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
            var half = powbase.Pow(powpower / 2);
            half *= half;
            if (powpower % 2 == 1)
                half *= powbase;
            return half;
        }
        /// <summary>
        /// Get one <see cref="BigInteger"/> to the power of another.
        /// </summary>
        /// <param name="a">The base of the power.</param>
        /// <param name="b">The exponent of the power.</param>
        /// <returns><paramref name="a"/> raised to <paramref name="b"/>.</returns>
        public static BigInteger Pow(this BigInteger a, BigInteger b)
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
                divpow = BigInteger.Pow(a.Pow(div), int.MaxValue);
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
        public static int PowMod(this int powbase, int power, int modulo)
        {
            if (power < 0)
                throw new ArithmeticException("cannot modpow to a negative power");
            if (power == 0)
                return 1;
            powbase %= modulo;
            int halfpow = PowMod(powbase, power / 2, modulo);
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
        public static long PowMod(this long powbase, long power, long modulo)
        {
            if (power < 0)
                throw new ArithmeticException("cannot modpow to a negative power");
            if (power == 0)
                return 1;
            powbase %= modulo;
            long halfpow = PowMod(powbase, power / 2, modulo);
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
        /// <remarks></remarks>
        public static int Ceil(this double a)
        {
            a.ThrowIfAbsurd(nameof(a));
            return (int)Math.Ceiling(a);
        }
        /// <summary>
        /// Rounds a <see cref="double"/> down to the nearest integer.
        /// </summary>
        /// <param name="a">The <see cref="double"/> to round.</param>
        /// <returns><paramref name="a"/> rounded down.</returns>
        public static int Floor(this double a)
        {
            a.ThrowIfAbsurd(nameof(a));
            return (int)Math.Floor(a);
        }
    }
}
