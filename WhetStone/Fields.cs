using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using WhetStone.Looping;
using WhetStone.SystemExtensions;
using WhetStone.WordPlay;
using Microsoft.CSharp.RuntimeBinder;
using Numerics;
namespace WhetStone.Fielding {
    /// <summary>
    /// A non-generic interface for all <see cref="Field{T}"/>s to use.
    /// </summary>
    public interface Field
    {
        /// <summary>
        /// Get the type the <see cref="Field"/> affects.
        /// </summary>
        Type SubjectType { get; }
    }
    /// <summary>
    /// The arguments needed to generate an arbitrary of a certain type.
    /// </summary>
    public enum GenerationType
    {
        /// <summary>
        /// An arbitrary value cannot be created.
        /// </summary>
        Never,
        /// <summary>
        /// An arbitrary value can be created only with an enumeration of bytes.
        /// </summary>
        FromBytes,
        /// <summary>
        /// An  arbitrary value can be created with an enumeration of bytes, but can also be created with a range bounding the generated value.
        /// </summary>
        FromRange,
        /// <summary>
        /// An arbitrary value can be created, but special constraints or arguments are necessary.
        /// </summary>
        Special
    }
    //todo max/min
    /// <summary>
    /// A universal class storing arithmetic operations for a certain type.
    /// </summary>
    /// <typeparam name="T">The type to affect.</typeparam>
    public abstract class Field<T> : IComparer<T>, IEqualityComparer<T>, Field
    {
        /// <summary>
        /// The additive identity of the type.
        /// </summary>
        /// <remarks>
        /// If addition and multiplication are defined, the following should be true for any a of type <typeparamref name="T"/>:
        /// a+<c>zero</c> == a
        /// a*<c>zero</c> == <c>zero</c>
        /// </remarks>
        public abstract T zero { get; }
        /// <summary>
        /// The multiplicative identity of the type.
        /// </summary>
        /// <remarks>
        /// If multiplication is defined, the following should be true for any a of type <typeparamref name="T"/>:
        /// a*<c>one</c> == <c>one</c>
        /// </remarks>
        public abstract T one { get; }
        /// <summary>
        /// The negative identity of the type.
        /// </summary>
        /// <remarks>
        /// If addition and multiplication are defined, the following should be true for any a of type <typeparamref name="T"/>:
        /// a*<c>negative one</c> + a == <c>zero</c>
        /// </remarks>
        public virtual T negativeone => Negate(one);
        /// <summary>
        /// Get the negative of an object.
        /// </summary>
        /// <param name="x">The number to negate.</param>
        /// <returns>The negative value of <paramref name="x"/>.</returns>
        public virtual T Negate(T x)
        {
            return subtract(zero,x);
        }
        /// <summary>
        /// Get the inverse of an object.
        /// </summary>
        /// <param name="x">The number to invert.</param>
        /// <returns>The inverse value of <paramref name="x"/>.</returns>
        public virtual T Invert(T x)
        {
            return divide(one,x);
        }
        /// <summary>
        /// Get the product of two elements.
        /// </summary>
        /// <param name="a">The first multiplicand.</param>
        /// <param name="b">The second multiplicand.</param>
        /// <returns>The product of <paramref name="a"/> and <paramref name="b"/>.</returns>
        public abstract T multiply(T a, T b);
        /// <summary>
        /// Get the quotient of two elements.
        /// </summary>
        /// <param name="a">The dividend.</param>
        /// <param name="b">The divisor.</param>
        /// <returns>The quotient <paramref name="a"/> and <paramref name="b"/>.</returns>
        public abstract T divide(T a, T b);
        /// <summary>
        /// Get the sum of two elements.
        /// </summary>
        /// <param name="a">The first addend.</param>
        /// <param name="b">The second addend.</param>
        /// <returns>The sum of <paramref name="a"/> and <paramref name="b"/></returns>
        public abstract T add(T a, T b);
        /// <summary>
        /// Get the difference between two elements.
        /// </summary>
        /// <param name="a">The minuend.</param>
        /// <param name="b">The subtrahend.</param>
        /// <returns>The difference between <paramref name="a"/> and <paramref name="b"/>.</returns>
        public abstract T subtract(T a, T b);
        /// <summary>
        /// Get the power of two elements.
        /// </summary>
        /// <param name="a">The base.</param>
        /// <param name="b">The exponential.</param>
        /// <returns><paramref name="a"/> to the power of <paramref name="b"/>.</returns>
        public abstract T pow(T a, T b);
        /// <summary>
        /// Get the logarithm of two elements.
        /// </summary>
        /// <param name="a">The antilogarithm.</param>
        /// <param name="b">The base.</param>
        /// <returns>The logarithm of <paramref name="a"/> in base <paramref name="b"/>.</returns>
        public abstract T log(T a, T b);
        /// <summary>
        /// Get the modulo of two elements.
        /// </summary>
        /// <param name="a">The dividend.</param>
        /// <param name="b">The divisor.</param>
        /// <returns>The modulo of <paramref name="a"/> and <paramref name="b"/>.</returns>
        public abstract T mod(T a, T b);
        /// <summary>
        /// Attempts to convert an element to <see cref="double"/>.
        /// </summary>
        /// <param name="a">The element to convert to double.</param>
        /// <returns><paramref name="a"/> converted to a double,  or <see langword="null"/> if conversion is impossible.</returns>
        public abstract double? toDouble(T a);
        /// <summary>
        /// Get whether every (non-<see cref="zero"/>) element in the field can be inverted.
        /// </summary>
        public virtual bool Invertible => true;
        /// <summary>
        /// Get whether every element in the field can be negated.
        /// </summary>
        public virtual bool Negatable => true;
        /// <summary>
        /// Get whether an element is negative.
        /// </summary>
        /// <param name="x">The element to check.</param>
        /// <returns>Whether <paramref name="x"/> is strictly negative (<see cref="zero"/> is not negative).</returns>
        public virtual bool isNegative(T x)
        {
            return this.Compare(zero, x) > 0;
        }
        /// <summary>
        /// Get whether an element is positive.
        /// </summary>
        /// <param name="x">The element to check.</param>
        /// <returns>Whether <paramref name="x"/> is strictly positive (<see cref="zero"/> is not negative).</returns>
        public virtual bool isPositive(T x)
        {
            return !Equals(x, zero) && !isNegative(x);
        }
        /// <summary>
        /// The generation type for <typeparamref name="T"/>.
        /// </summary>
        public virtual GenerationType GenType => GenerationType.Never;
        /// <summary>
        /// Generate an element of the type.
        /// </summary>
        /// <param name="bytes">A {potentially infinite) <see cref="IEnumerable{T}"/> of bytes to serve as the generation seed of the element.</param>
        /// <param name="bounds">The bounds in which to constrain the created element. The first element is inclusive, the second is exclusive.</param>
        /// <param name="special">Special constraints for the created element.</param>
        /// <returns>An element created from the bytes and the constraints.</returns>
        public virtual T Generate(IEnumerable<byte> bytes, Tuple<T, T> bounds = null, object special = null)
        {
            throw new NotSupportedException();
        }
        /// <inheritdoc />
        public virtual int Compare(T x, T y)
        {
            return Comparer<T>.Default.Compare(x, y);
        }
        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (!(obj is Field<T>))
            {
                return false;
            }
            return IdDistributer.getId(this) == IdDistributer.getId(obj);
        }
        /// <inheritdoc />
        public override int GetHashCode()
        {
            return IdDistributer.getId(this).GetHashCode();
        }
        /// <inheritdoc />
        public Type SubjectType=> typeof (T);
        /// <summary>
        /// Returns the absolute value of an element.
        /// </summary>
        /// <param name="x">The element to turn to absolute value.</param>
        /// <returns><paramref name="x"/>'s absolute value.</returns>
        public virtual T abs(T x)
        {
            return isNegative(x) && this.Negatable ? Negate(x) : x;
        }
        /// <summary>
        /// Returns an element from an integer.
        /// </summary>
        /// <param name="x">The integer to generate the element from.</param>
        /// <returns>An element corresponding to an integer <paramref name="x"/>.</returns>
        public virtual T fromInt(ulong x)
        {
            switch (x)
            {
                case 0:
                    return this.zero;
                case 1:
                    return this.one;
            }
            T h = fromInt(x / 2);
            T sum = add(h, h);
            if (x % 2 == 1)
                sum = add(sum, one);
            return sum;
        }
        /// <summary>
        /// Returns an element from an integer.
        /// </summary>
        /// <param name="x">The integer to generate the element from.</param>
        /// <returns>An element corresponding to an integer <paramref name="x"/>.</returns>
        public virtual T fromInt(int x)
        {
            return x < 0 ? this.Negate(this.fromInt((ulong)-x)) : this.fromInt((ulong) x);
        }
        /// <summary>
        /// Returns an element from a double.
        /// </summary>
        /// <param name="a">The double to generate the element from.</param>
        /// <returns>An element corresponding to a double <paramref name="a"/>.</returns>
        public virtual T fromFraction(double a)
        {
            var f = new BigRational(a);
            return fromFraction((int)f.Numerator, (int)f.Denominator);
        }
        /// <summary>
        /// Returns an element from a fraction.
        /// </summary>
        /// <param name="numerator">The numerator of the fraction.</param>
        /// <param name="denominator">The denominator of the fraction.</param>
        /// <returns>An element corresponding to the fraction of <paramref name="numerator"/> and <paramref name="denominator"/>.</returns>
        public virtual T fromFraction(int numerator, int denominator)
        {
            return divide(fromInt(numerator), fromInt(denominator));
        }
        /// <summary>
        /// Returns an element raised to an integer power.
        /// </summary>
        /// <param name="base">The element to raise.</param>
        /// <param name="x">The integer exponent.</param>
        /// <returns><paramref name="base"/> raised to <paramref name="x"/>.</returns>
        public virtual T Pow(T @base, int x)
        {
            return pow(@base, fromInt(x));
        }
        /// <inheritdoc />
        public bool Equals(T x, T y)
        {
            return Compare(x,y) == 0;
        }
        /// <inheritdoc />
        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
        //todo to tryparse
        /// <summary>
        /// Parse a string into an element.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>An element parsed from <paramref name="s"/>.</returns>
        public virtual T Parse(string s)
        {
            throw new NotSupportedException();
        }
    }
    /// <summary>
    /// A central storage of all fields, allowing for easy field lookup.
    /// </summary>
    public static class Fields
    {
        #region stock
        private class DynamicField<T> : Field<T>
        {
            public override T zero => (T)(dynamic)0;
            public override T one => (T)(dynamic)1;
            public override T multiply(T a, T b)
            {
                return a * (dynamic)b;
            }
            public override T divide(T a, T b)
            {
                return a / (dynamic)b;
            }
            public override T add(T a, T b)
            {
                return a + (dynamic)b;
            }
            public override T subtract(T a, T b)
            {
                return a - (dynamic)b;
            }
            public override T pow(T a, T b)
            {
                try
                {
                    return ((dynamic)a).pow(b);
                }
                catch (RuntimeBinderException)
                {
                    return (dynamic)a^b;
                }
            }
            public override T mod(T a, T b)
            {
                return a % (dynamic)b;
            }
            public override double? toDouble(T a)
            {
                try
                {
                    return (double)(dynamic)a;
                }
                catch (RuntimeBinderException)
                {
                    return null;
                }
            }
            public override T log(T a, T b)
            {
                return ((dynamic)a).log(b);
            }
        }
        private class DoubleField : Field<double>
        {
            public override double one { get; } = 1;
            public override double zero { get; } = 0;
            public override double add(double a, double b) => a+b;
            public override double pow(double a, double b) => Math.Pow(a,b);
            public override int Compare(double x, double y) => x.CompareTo(y);
            public override double fromInt(int x) => x;
            public override double fromInt(ulong x) => x;
            public override double abs(double x) => Math.Abs(x);
            public override double divide(double a, double b) => a/b;
            public override double mod(double a, double b) => a%b;
            public override double fromFraction(int numerator, int denumerator) => numerator/(double)denumerator;
            public override double Invert(double x) => 1/x;
            public override bool isNegative(double x) => x < 0;
            public override double log(double a, double b) => Math.Log(a, b);
            public override double multiply(double a, double b) => a*b;
            public override double Negate(double x) => -x;
            public override double subtract(double a, double b) => a-b;
            public override double? toDouble(double a) => a;
            public override double Parse(string s)
            {
                return double.Parse(s);
            }
            public override GenerationType GenType => GenerationType.FromRange;
            public override double Generate(IEnumerable<byte> bytes, Tuple<double, double> bounds = null, object special = null)
            {
                bounds = bounds ?? new Tuple<double, double>(0, 1);
                if (bounds.Item1 == 0 && bounds.Item2 == 1)
                {
                    var seedn = bytes.Take(sizeof(ulong));
                    var seedd = bytes.Skip(sizeof(ulong)).Take(sizeof(ulong));
                    ulong d = BitConverter.ToUInt64(seedd.ToArray(),0)%(int.MaxValue)+1;
                    ulong n = BitConverter.ToUInt64(seedn.ToArray(), 0)%d;
                    return n / (double)d;
                }
                var max = bounds.Item2;
                var min = bounds.Item1;
                var @base = Generate(bytes);
                @base *= (max-min);
                return @base + min;
            }
            public override double fromFraction(double a)
            {
                return a;
            }
            public override bool isPositive(double x)
            {
                return x > 0;
            }
        }
        private class FloatField : Field<float>
        {
            public override float one { get; } = 1;
            public override float zero { get; } = 0;
            public override float add(float a, float b) => a + b;
            public override float pow(float a, float b) => a.pow(b);
            public override int Compare(float x, float y) => x.CompareTo(y);
            public override float fromInt(int x) => x;
            public override float fromInt(ulong x) => x;
            public override float abs(float x) => Math.Abs(x);
            public override float divide(float a, float b) => a / b;
            public override float mod(float a, float b) => a % b;
            public override float fromFraction(int numerator, int denumerator) => numerator / (float)denumerator;
            public override float Invert(float x) => 1 / x;
            public override bool isNegative(float x) => x < 0;
            public override float log(float a, float b) => (float)Math.Log(a, b);
            public override float multiply(float a, float b) => a * b;
            public override float Negate(float x) => -x;
            public override float subtract(float a, float b) => a - b;
            public override double? toDouble(float a) => a;
            public override float Parse(string s)
            {
                return float.Parse(s);
            }
            public override GenerationType GenType => GenerationType.FromRange;
            public override float Generate(IEnumerable<byte> bytes, Tuple<float, float> bounds = null, object special = null)
            {
                bounds = bounds ?? new Tuple<float, float>(0, 1);
                if (bounds.Item1 == 0 && bounds.Item2 == 1)
                {
                    var seedn = bytes.Take(sizeof(ulong));
                    var seedd = bytes.Skip(sizeof(ulong)).Take(sizeof(ulong));
                    ulong d = BitConverter.ToUInt64(seedd.ToArray(), 0) % (int.MaxValue) + 1;
                    ulong n = BitConverter.ToUInt64(seedn.ToArray(), 0) % d;
                    return n / (float)d;
                }
                var max = bounds.Item2;
                var min = bounds.Item1;
                var @base = Generate(bytes);
                @base *= (max - min);
                return @base + min;
            }
            public override float fromFraction(double a)
            {
                return (float)a;
            }
            public override bool isPositive(float x)
            {
                return x > 0;
            }
        }
        private class DecimalField : Field<decimal>
        {
            public override decimal one { get; } = 1;
            public override decimal zero { get; } = 0;
            public override decimal add(decimal a, decimal b) => a + b;
            public override decimal pow(decimal a, decimal b) => a.pow(b);
            public override int Compare(decimal x, decimal y) => x.CompareTo(y);
            public override decimal fromInt(int x) => x;
            public override decimal fromInt(ulong x) => x;
            public override decimal abs(decimal x) => Math.Abs(x);
            public override decimal divide(decimal a, decimal b) => a / b;
            public override decimal mod(decimal a, decimal b) => a % b;
            public override decimal fromFraction(int numerator, int denumerator) => numerator / (decimal)denumerator;
            public override decimal Invert(decimal x) => 1 / x;
            public override bool isNegative(decimal x) => x < 0;
            public override decimal log(decimal a, decimal b) => (decimal)Math.Log((double)a, (double)b);
            public override decimal multiply(decimal a, decimal b) => a * b;
            public override decimal Negate(decimal x) => -x;
            public override decimal subtract(decimal a, decimal b) => a - b;
            public override double? toDouble(decimal a) => (double) a;
            public override decimal Parse(string s)
            {
                return decimal.Parse(s);
            }
            public override GenerationType GenType => GenerationType.FromRange;
            public override decimal Generate(IEnumerable<byte> bytes, Tuple<decimal, decimal> bounds = null, object special = null)
            {
                bounds = bounds ?? new Tuple<decimal, decimal>(0, 1);
                if (bounds.Item1 == 0 && bounds.Item2 == 1)
                {
                    var seedn = bytes.Take(sizeof(ulong));
                    var seedd = bytes.Skip(sizeof(ulong)).Take(sizeof(ulong));
                    ulong d = BitConverter.ToUInt64(seedd.ToArray(), 0) % (int.MaxValue) + 1;
                    ulong n = BitConverter.ToUInt64(seedn.ToArray(), 0) % d;
                    return n / (decimal)d;
                }
                var max = bounds.Item2;
                var min = bounds.Item1;
                var @base = Generate(bytes);
                @base *= (max - min);
                return @base + min;
            }
            public override decimal fromFraction(double a)
            {
                return (decimal)a;
            }
            public override bool isPositive(decimal x)
            {
                return x > 0;
            }
        }
        private class IntField : Field<int>
        {
            public override int one { get; } = 1;
            public override int zero { get; } = 0;
            public override int add(int a, int b) => a + b;
            public override int pow(int a, int b) => a.pow(b);
            public override int Compare(int x, int y) => x.CompareTo(y);
            public override int fromInt(int x) => x;
            public override int fromInt(ulong x) => (int)x;
            public override int abs(int x) => Math.Abs(x);
            public override int divide(int a, int b) => a / b;
            public override int fromFraction(int numerator, int denumerator) => numerator / denumerator;
            public override int Invert(int x) => 1 / x;
            public override bool isNegative(int x) => x < 0;
            public override int multiply(int a, int b) => a * b;
            public override int mod(int a, int b) => a % b;
            public override int Negate(int x) => -x;
            public override int subtract(int a, int b) => a - b;
            public override double? toDouble(int a) => a;
            public override bool Invertible => false;
            public override int log(int a, int b)
            {
                return (int)Math.Log(a, b);
            }
            public override int Parse(string s)
            {
                return int.Parse(s);
            }
            public override GenerationType GenType => GenerationType.FromRange;
            public override int Generate(IEnumerable<byte> bytes, Tuple<int, int> bounds = null, object special = null)
            {
                var @base = BitConverter.ToInt32(bytes.Take(sizeof(int)).ToArray(),0);
                if (@base < 0)
                    @base = -@base;
                var max = bounds?.Item2 ?? 0;
                var min = bounds?.Item1 ?? 0;
                if (bounds != null)
                {
                    @base %= (max - min);
                }
                return @base + min;
            }
            public override int fromFraction(double a)
            {
                return (int)Math.Round(a,0,MidpointRounding.AwayFromZero);
            }
            public override bool isPositive(int x)
            {
                return x > 0;
            }
        }
        private class LongField : Field<long>
        {
            public override long one { get; } = 1;
            public override long zero { get; } = 0;
            public override long add(long a, long b) => a + b;
            public override long pow(long a, long b) => a.pow(b);
            public override int Compare(long x, long y) => x.CompareTo(y);
            public override long fromInt(int x) => x;
            public override long fromInt(ulong x) => (long) x;
            public override long abs(long x) => Math.Abs(x);
            public override long divide(long a, long b) => a / b;
            public override long mod(long a, long b) => a % b;
            public override long fromFraction(int numerator, int denumerator) => numerator / (long)denumerator;
            public override long Invert(long x) => 1 / x;
            public override bool isNegative(long x) => x < 0;
            public override long multiply(long a, long b) => a * b;
            public override long Negate(long x) => -x;
            public override long subtract(long a, long b) => a - b;
            public override double? toDouble(long a) => a;
            public override bool Invertible => false;
            public override long Parse(string s)
            {
                return long.Parse(s);
            }
            public override GenerationType GenType => GenerationType.FromRange;
            public override long Generate(IEnumerable<byte> bytes, Tuple<long, long> bounds = null, object special = null)
            {
                var @base = BitConverter.ToInt64(bytes.Take(sizeof(long)).ToArray(), 0);
                if (@base < 0)
                    @base = -@base;
                var max = bounds?.Item2 ?? 0;
                var min = bounds?.Item1 ?? 0;
                if (bounds != null)
                {
                    @base %= (max - min);
                }
                return @base + min;
            }
            public override long fromFraction(double a)
            {
                return (long)Math.Round(a, 0, MidpointRounding.AwayFromZero);
            }
            public override long log(long a, long b)
            {
                return (long)Math.Log(a, b);
            }
            public override bool isPositive(long x)
            {
                return x > 0;
            }
        }
        private class UIntField : Field<uint>
        {
            public override uint one { get; } = 1;
            public override uint zero { get; } = 0;
            public override uint add(uint a, uint b) => a + b;
            public override uint pow(uint a, uint b) => a.pow(b);
            public override int Compare(uint x, uint y) => x.CompareTo(y);
            public override uint fromInt(int x)
            {
                if (x < 0)
                    throw new Exception("cannot get unsigned from negative int");
                return (uint) x;
            }
            public override uint fromInt(ulong x) => (uint)x;
            public override uint abs(uint x) => x;
            public override uint divide(uint a, uint b) => a / b;
            public override uint mod(uint a, uint b) => a % b;
            public override uint fromFraction(int numerator, int denumerator) => (uint) (numerator / denumerator);
            public override uint Invert(uint x) => 1 / x;
            public override bool isNegative(uint x) => false;
            public override uint multiply(uint a, uint b) => a * b;
            public override uint Negate(uint x)
            {
                if (x == 0)
                    return 0;
                throw new Exception("cannot negate unsigned");
            }
            public override uint subtract(uint a, uint b) => a - b;
            public override double? toDouble(uint a) => a;
            public override bool Invertible => false;
            public override bool Negatable => false;
            public override uint Parse(string s)
            {
                return uint.Parse(s);
            }
            public override GenerationType GenType => GenerationType.FromRange;
            public override uint Generate(IEnumerable<byte> bytes, Tuple<uint, uint> bounds = null, object special = null)
            {
                var @base = BitConverter.ToUInt32(bytes.Take(sizeof(uint)).ToArray(), 0);
                var max = bounds?.Item2 ?? 0;
                var min = bounds?.Item1 ?? 0;
                if (bounds != null)
                {
                    @base %= (max - min);
                }
                return @base + min;
            }
            public override uint fromFraction(double a)
            {
                return (uint)Math.Round(a, 0, MidpointRounding.AwayFromZero);
            }
            public override uint log(uint a, uint b)
            {
                return (uint)Math.Log(a, b);
            }
            public override bool isPositive(uint x)
            {
                return x > 0;
            }
        }
        private class ULongField : Field<ulong>
        {
            public override ulong one { get; } = 1;
            public override ulong zero { get; } = 0;
            public override ulong add(ulong a, ulong b) => a + b;
            public override ulong pow(ulong a, ulong b) => a.pow(b);
            public override int Compare(ulong x, ulong y) => x.CompareTo(y);
            public override ulong fromInt(int x)
            {
                if (x < 0)
                    throw new Exception("cannot get unsigned from negative int");
                return (ulong)x;
            }
            public override ulong fromInt(ulong x) => x;
            public override ulong abs(ulong x) => x;
            public override ulong divide(ulong a, ulong b) => a / b;
            public override ulong mod(ulong a, ulong b) => a % b;
            public override ulong fromFraction(int numerator, int denumerator) => (ulong)(numerator / denumerator);
            public override ulong Invert(ulong x) => 1 / x;
            public override bool isNegative(ulong x) => false;
            public override ulong multiply(ulong a, ulong b) => a * b;
            public override ulong Negate(ulong x)
            {
                if (x == 0)
                    return 0;
                throw new Exception("cannot negate unsigned");
            }
            public override ulong subtract(ulong a, ulong b) => a - b;
            public override double? toDouble(ulong a) => a;
            public override bool Invertible => false;
            public override bool Negatable => false;
            public override ulong Parse(string s)
            {
                return ulong.Parse(s);
            }
            public override GenerationType GenType => GenerationType.FromRange;
            public override ulong Generate(IEnumerable<byte> bytes, Tuple<ulong, ulong> bounds = null, object special = null)
            {
                var @base = BitConverter.ToUInt64(bytes.Take(sizeof(ulong)).ToArray(), 0);
                var max = bounds?.Item2 ?? 0;
                var min = bounds?.Item1 ?? 0;
                if (bounds != null)
                {
                    @base %= (max - min);
                }
                return @base + min;
            }
            public override ulong fromFraction(double a)
            {
                return (ulong)Math.Round(a, 0, MidpointRounding.AwayFromZero);
            }
            public override ulong log(ulong a, ulong b)
            {
                return (ulong)Math.Log(a, b);
            }
            public override bool isPositive(ulong x)
            {
                return x > 0;
            }
        }
        private class ByteField : Field<byte>
        {
            public override byte one { get; } = 1;
            public override byte zero { get; } = 0;
            public override byte add(byte a, byte b) => (byte)(a + b);
            public override byte pow(byte a, byte b) => a.pow(b);
            public override int Compare(byte x, byte y) => x.CompareTo(y);
            public override byte fromInt(int x)
            {
                if (x < 0)
                    throw new Exception("cannot get unsigned from negative int");
                return (byte)x;
            }
            public override byte fromInt(ulong x) => (byte)x;
            public override byte abs(byte x) => x;
            public override byte divide(byte a, byte b) => (byte)(a / b);
            public override byte mod(byte a, byte b) => (byte)(a % b);
            public override byte fromFraction(int numerator, int denumerator) => (byte)(numerator / denumerator);
            public override byte Invert(byte x) => (byte)(1 / x);
            public override bool isNegative(byte x) => false;
            public override byte multiply(byte a, byte b) => (byte)(a * b);
            public override byte Negate(byte x)
            {
                if (x == 0)
                    return 0;
                throw new Exception("cannot negate unsigned");
            }
            public override byte subtract(byte a, byte b) => (byte)(a - b);
            public override double? toDouble(byte a) => a;
            public override bool Invertible => false;
            public override bool Negatable => false;
            public override byte Parse(string s)
            {
                return byte.Parse(s);
            }
            public override GenerationType GenType => GenerationType.FromRange;
            public override byte Generate(IEnumerable<byte> bytes, Tuple<byte, byte> bounds = null, object special = null)
            {
                var @base = bytes.First();
                var max = bounds?.Item2 ?? 0;
                var min = bounds?.Item1 ?? 0;
                if (bounds != null)
                {
                    @base %= (byte)(max - min);
                }
                return (byte)(@base + min);
            }
            public override byte fromFraction(double a)
            {
                return (byte)Math.Round(a, 0, MidpointRounding.AwayFromZero);
            }
            public override byte log(byte a, byte b)
            {
                return (byte)Math.Log(a, b);
            }
            public override bool isPositive(byte x)
            {
                return x > 0;
            }
        }
        private class SbyteField : Field<sbyte>
        {
            public override sbyte one { get; } = 1;
            public override sbyte zero { get; } = 0;
            public override sbyte add(sbyte a, sbyte b) => (sbyte)(a + b);
            public override sbyte pow(sbyte a, sbyte b) => a.pow(b);
            public override int Compare(sbyte x, sbyte y) => x.CompareTo(y);
            public override sbyte fromInt(int x)
            {
                return (sbyte)x;
            }
            public override sbyte fromInt(ulong x) => (sbyte)x;
            public override sbyte abs(sbyte x) => Math.Abs(x);
            public override sbyte divide(sbyte a, sbyte b) => (sbyte)(a / b);
            public override sbyte mod(sbyte a, sbyte b) => (sbyte)(a % b);
            public override sbyte fromFraction(int numerator, int denumerator) => (sbyte)(numerator / denumerator);
            public override sbyte Invert(sbyte x) => (sbyte)(1 / x);
            public override bool isNegative(sbyte x) => x < 0;
            public override sbyte multiply(sbyte a, sbyte b) => (sbyte)(a * b);
            public override sbyte Negate(sbyte x)
            {
                return (sbyte)-x;
            }
            public override sbyte subtract(sbyte a, sbyte b) => (sbyte)(a - b);
            public override double? toDouble(sbyte a) => a;
            public override bool Invertible => false;
            public override bool Negatable => true;
            public override sbyte Parse(string s)
            {
                return sbyte.Parse(s);
            }
            public override GenerationType GenType => GenerationType.FromRange;
            public override sbyte Generate(IEnumerable<byte> bytes, Tuple<sbyte, sbyte> bounds = null, object special = null)
            {
                var @base = (sbyte)bytes.First();
                if (@base < 0)
                    @base = (sbyte)-@base;
                var max = bounds?.Item2 ?? 0;
                var min = bounds?.Item1 ?? 0;
                if (bounds != null)
                {
                    @base %= (sbyte)(max - min);
                }
                return (sbyte)(@base + min);
            }
            public override sbyte fromFraction(double a)
            {
                return (sbyte)Math.Round(a, 0, MidpointRounding.AwayFromZero);
            }
            public override sbyte log(sbyte a, sbyte b)
            {
                return (sbyte)Math.Log(a, b);
            }
            public override bool isPositive(sbyte x)
            {
                return x > 0;
            }
        }
        private class ShortField : Field<short>
        {
            public override short one { get; } = 1;
            public override short zero { get; } = 0;
            public override short add(short a, short b) => (short)(a + b);
            public override short pow(short a, short b) => a.pow(b);
            public override int Compare(short x, short y) => x.CompareTo(y);
            public override short fromInt(int x)
            {
                return (short)x;
            }
            public override short fromInt(ulong x) => (short)x;
            public override short abs(short x) => x;
            public override short divide(short a, short b) => (short)(a / b);
            public override short mod(short a, short b) => (short)(a % b);
            public override short fromFraction(int numerator, int denumerator) => (short)(numerator / denumerator);
            public override short Invert(short x) => (short)(1 / x);
            public override bool isNegative(short x) => x < 0;
            public override short multiply(short a, short b) => (short)(a * b);
            public override short Negate(short x)
            {
                return (short)-x;
            }
            public override short subtract(short a, short b) => (short)(a - b);
            public override double? toDouble(short a) => a;
            public override bool Invertible => false;
            public override bool Negatable => true;
            public override short Parse(string s)
            {
                return short.Parse(s);
            }
            public override GenerationType GenType => GenerationType.FromRange;
            public override short Generate(IEnumerable<byte> bytes, Tuple<short, short> bounds = null, object special = null)
            {
                var @base = BitConverter.ToInt16(bytes.Take(sizeof(short)).ToArray(),0);
                if (@base < 0)
                    @base = (short)-@base;
                var max = bounds?.Item2 ?? 0;
                var min = bounds?.Item1 ?? 0;
                if (bounds != null)
                {
                    @base %= (short)(max - min);
                }
                return (short)(@base + min);
            }
            public override short fromFraction(double a)
            {
                return (short)Math.Round(a, 0, MidpointRounding.AwayFromZero);
            }
            public override short log(short a, short b)
            {
                return (short)Math.Log(a, b);
            }
        }
        private class UshortField : Field<ushort>
        {
            public override ushort one { get; } = 1;
            public override ushort zero { get; } = 0;
            public override ushort add(ushort a, ushort b) => (ushort)(a + b);
            public override ushort pow(ushort a, ushort b) => a.pow(b);
            public override int Compare(ushort x, ushort y) => x.CompareTo(y);
            public override ushort fromInt(int x)
            {
                if (x < 0)
                    throw new Exception("cannot get unsigned from negative int");
                return (ushort)x;
            }
            public override ushort fromInt(ulong x) => (ushort)x;
            public override ushort abs(ushort x) => x;
            public override ushort divide(ushort a, ushort b) => (ushort)(a / b);
            public override ushort mod(ushort a, ushort b) => (ushort)(a % b);
            public override ushort fromFraction(int numerator, int denumerator) => (ushort)(numerator / denumerator);
            public override ushort Invert(ushort x) => (ushort)(1 / x);
            public override bool isNegative(ushort x) => false;
            public override ushort multiply(ushort a, ushort b) => (ushort)(a * b);
            public override ushort Negate(ushort x)
            {
                if (x == 0)
                    return 0;
                throw new Exception("cannot negate unsigned");
            }
            public override ushort subtract(ushort a, ushort b) => (ushort)(a - b);
            public override double? toDouble(ushort a) => a;
            public override bool Invertible => false;
            public override bool Negatable => false;
            public override ushort Parse(string s)
            {
                return ushort.Parse(s);
            }
            public override GenerationType GenType => GenerationType.FromRange;
            public override ushort Generate(IEnumerable<byte> bytes, Tuple<ushort, ushort> bounds = null, object special = null)
            {
                var @base = BitConverter.ToUInt16(bytes.Take(sizeof(short)).ToArray(), 0);
                var max = bounds?.Item2 ?? 0;
                var min = bounds?.Item1 ?? 0;
                if (bounds != null)
                {
                    @base %= (ushort)(max - min);
                }
                return (ushort)(@base + min);
            }
            public override ushort fromFraction(double a)
            {
                return (ushort)Math.Round(a, 0, MidpointRounding.AwayFromZero);
            }
            public override ushort log(ushort a, ushort b)
            {
                return (ushort)Math.Log(a, b);
            }
            public override bool isPositive(ushort x)
            {
                return x > 0;
            }
        }
        private class StringField : Field<string>
        {
            public override string one { get; } = "";
            public override string zero { get; } = "";
            public override string add(string a, string b) => a + b;
            public override string pow(string a, string b)
            {
                throw new NotSupportedException();
            }
            public override int Compare(string x, string y) => x.CompareTo(y);
            public override string fromInt(int x)
            {
                return new string(' ',x);
            }
            public override string fromInt(ulong x)
            {
                return new string(' ', (int)x);
            }
            public override string abs(string x) => x;
            public override string divide(string a, string b)
            {
                throw new NotSupportedException();
            }
            public override string fromFraction(int numerator, int denumerator)
            {
                throw new NotSupportedException();
            }
            public override string Invert(string x)
            {
                throw new NotSupportedException();
            }
            public override bool isNegative(string x) => false;
            public override string multiply(string a, string b)
            {
                throw new NotSupportedException();
            }
            public override string mod(string a, string b)
            {
                throw new NotSupportedException();
            }
            public override string Negate(string x)
            {
                throw new NotSupportedException();
            }
            public override string subtract(string a, string b)
            {
                throw new NotSupportedException();
            }
            public override double? toDouble(string a)
            {
                double o;
                bool suc = double.TryParse(a,out o);
                return suc ? (double?)o : null;
            }
            public override bool Invertible => false;
            public override bool Negatable => false;
            public override string Parse(string s)
            {
                return s;
            }
            public override GenerationType GenType => GenerationType.Special;
            public override string Generate(IEnumerable<byte> bytes, Tuple<string, string> bounds = null, object special = null)
            {
                int length = special as int? ?? 0;
                if (length == 0)
                    return "";
                Tuple<char, char> charbounds = null;
                if (!string.IsNullOrEmpty(bounds?.Item1) && !string.IsNullOrEmpty(bounds.Item2))
                    charbounds = Tuple.Create(bounds.Item1[0], bounds.Item2[0]);
                var cf = getField<char>();
                return bytes.Chunk(sizeof(char)).Select(a => cf.Generate(a,charbounds)).Take(length).ConvertToString();
            }
            public override string fromFraction(double a)
            {
                return a.ToString();
            }
            public override string log(string a, string b)
            {
                throw new NotSupportedException();
            }
        }
        private class CharField : Field<char>
        {
            public override char one { get; } = '\u0000';
            public override char zero { get; } = '\u0001';
            public override char add(char a, char b) => (char)(a + b);
            public override char pow(char a, char b)
            {
                throw new NotSupportedException();
            }
            public override int Compare(char x, char y) => x.CompareTo(y);
            public override char fromInt(int x)
            {
                return (char)x;
            }
            public override char fromInt(ulong x)
            {
                return (char)x;
            }
            public override char abs(char x) => x;
            public override char divide(char a, char b) => (char)(a / b);
            public override char fromFraction(int numerator, int denumerator)
            {
                throw new NotSupportedException();
            }
            public override char Invert(char x)
            {
                throw new NotSupportedException();
            }
            public override bool isNegative(char x) => false;
            public override char multiply(char a, char b) => (char)(a * b);
            public override char mod(char a, char b) => (char)(a % b);
            public override char Negate(char x) => (char)(char.MaxValue - x);
            public override char subtract(char a, char b) => (char)(a - b);
            public override double? toDouble(char a)
            {
                return a;
            }
            public override bool Invertible => false;
            public override bool Negatable => true;
            public override char Parse(string s)
            {
                return s[0];
            }
            public override GenerationType GenType => GenerationType.FromRange;
            public override char Generate(IEnumerable<byte> bytes, Tuple<char, char> bounds = null, object special = null)
            {
                var @base = BitConverter.ToChar(bytes.Take(sizeof(char)).ToArray(), 0);
                var max = bounds?.Item2 ?? 0;
                var min = bounds?.Item1 ?? 0;
                if (bounds != null)
                {
                    @base %= (char)(max - min);
                }
                return (char)(@base + min);
            }
            public override char fromFraction(double a)
            {
                return (char)Math.Round(a, 0, MidpointRounding.AwayFromZero);
            }
            public override char log(char a, char b)
            {
                throw new NotSupportedException();
            }
        }
        private class BigRationalField : Field<BigRational>
        {
            public override BigRational one { get; } = 1;
            public override BigRational zero { get; } = 0;
            public override BigRational add(BigRational a, BigRational b) => a + b;
            public override BigRational pow(BigRational a, BigRational b) => a.pow(b,new BigRational(1,a.Denominator));
            public override int Compare(BigRational x, BigRational y) => x.CompareTo(y);
            public override BigRational fromInt(int x) => x;
            public override BigRational fromInt(ulong x) => x;
            public override BigRational abs(BigRational x) => BigRational.Abs(x);
            public override BigRational divide(BigRational a, BigRational b) => a / b;
            public override BigRational mod(BigRational a, BigRational b) => a % b;
            public override BigRational fromFraction(int numerator, int denumerator) => numerator / (BigRational)denumerator;
            public override BigRational Invert(BigRational x) => 1 / x;
            public override bool isNegative(BigRational x) => x.Sign < 0;
            public override BigRational multiply(BigRational a, BigRational b) => a * b;
            public override BigRational Negate(BigRational x) => -x;
            public override BigRational subtract(BigRational a, BigRational b) => a - b;
            public override double? toDouble(BigRational a) => (double)a;
            public override BigRational Parse(string s)
            {
                return new BigRational(double.Parse(s));
            }
            public override GenerationType GenType => GenerationType.FromRange;
            public override BigRational Generate(IEnumerable<byte> bytes, Tuple<BigRational, BigRational> bounds = null, object special = null)
            {
                bounds = bounds ?? Tuple.Create(BigRational.Zero, BigRational.One);
                if (bounds.Item1.Numerator.IsZero && bounds.Item2 == 1)
                {
                    return getField<decimal>().Generate(bytes);
                }
                var max = bounds.Item2;
                var min = bounds.Item1;
                var @base = Generate(bytes);
                @base *= (max - min);
                return @base + min;
            }
            public override BigRational fromFraction(double a)
            {
                return a;
            }
            public override BigRational log(BigRational a, BigRational b)
            {
                return Math.Log((double)a, (double)b);
            }
        }
        private class BigIntegerField : Field<BigInteger>
        {
            public override BigInteger one { get; } = 1;
            public override BigInteger zero { get; } = 0;
            public override BigInteger add(BigInteger a, BigInteger b) => a + b;
            public override BigInteger pow(BigInteger a, BigInteger b) => a.pow(b);
            public override int Compare(BigInteger x, BigInteger y) => x.CompareTo(y);
            public override BigInteger fromInt(int x) => x;
            public override BigInteger fromInt(ulong x) => x;
            public override BigInteger abs(BigInteger x) => BigInteger.Abs(x);
            public override BigInteger divide(BigInteger a, BigInteger b) => a / b;
            public override BigInteger mod(BigInteger a, BigInteger b) => a % b;
            public override BigInteger fromFraction(int numerator, int denumerator) => numerator / (BigInteger)denumerator;
            public override BigInteger Invert(BigInteger x) => 1 / x;
            public override bool isNegative(BigInteger x) => x < 0;
            public override BigInteger multiply(BigInteger a, BigInteger b) => a * b;
            public override BigInteger Negate(BigInteger x) => -x;
            public override BigInteger subtract(BigInteger a, BigInteger b) => a - b;
            public override double? toDouble(BigInteger a) => (double) a;
            public override bool Invertible => false;
            public override BigInteger Parse(string s)
            {
                return BigInteger.Parse(s);
            }
            public override GenerationType GenType => GenerationType.FromRange;
            public override BigInteger Generate(IEnumerable<byte> bytes, Tuple<BigInteger, BigInteger> bounds = null, object special = null)
            {
                bounds = bounds ?? Tuple.Create(BigInteger.Zero, (BigInteger)2);
                int bytecount = Math.Max(bounds.Item1.ToByteArray().Length, bounds.Item2.ToByteArray().Length);
                var max = bounds.Item2;
                var min = bounds.Item1;
                var @base = new BigInteger(bytes.Take(bytecount).ToArray());
                if (@base < 0)
                    @base = -@base;
                @base %= (max - min);
                return @base + min;
            }
            public override BigInteger fromFraction(double a)
            {
                return (BigInteger)Math.Round(a, 0, MidpointRounding.AwayFromZero);
            }
            public override BigInteger log(BigInteger a, BigInteger b)
            {
                return (BigInteger)Math.Log((double)a, (double)b);
            }
        }
        private class BoolField : Field<bool>
        {
            public override bool one { get; } = true;
            public override bool zero { get; } = false;
            public override bool pow(bool a, bool b)
            {
                if (!a && !b)
                    throw new ArithmeticException("can't raise 0 by 0");
                return !b || a;
            }
            public override bool add(bool a, bool b) => a ^ b;
            public override int Compare(bool x, bool y)
            {
                return x == y ? 0 : (x ? 1 : -1);
            }
            public override bool Pow(bool @base, int x) => @base && x%2 == 1;
            public override bool abs(bool x) => x;
            public override bool divide(bool a, bool b)
            {
                if (!b)
                    throw new ArithmeticException("division by 0");
                return a;
            }
            public override bool mod(bool a, bool b)
            {
                if (!b)
                    throw new ArithmeticException("division by 0");
                return false;
            }
            public override bool fromInt(int x) => x%2==1;
            public override bool Invert(bool x)
            {
                if (!x)
                    throw new ArithmeticException("division by 0");
                return true;
            }
            public override bool fromInt(ulong x) => x % 2 == 1;
            public override bool isNegative(bool x) => false;
            public override bool multiply(bool a, bool b) => a & b;
            public override bool Negate(bool x) => x;
            public override bool subtract(bool a, bool b) => a ^ !b;
            public override double? toDouble(bool a)
            {
                return a ? 1 : 0;
            }
            public override bool Parse(string s)
            {
                return bool.Parse(s);
            }
            public override GenerationType GenType => GenerationType.FromBytes;
            public override bool Generate(IEnumerable<byte> bytes, Tuple<bool, bool> bounds = null, object special = null)
            {
                return (bytes.First()&1) == 1;
            }
            public override bool fromFraction(double a)
            {
                return ((int)Math.Round(a, 0, MidpointRounding.AwayFromZero))%2 == 1;
            }
            public override bool log(bool a, bool b)
            {
                throw new NotSupportedException();
            }
        }
        
    #endregion
        private static readonly IDictionary<Type,Field> _quickFieldDictionary = new Dictionary<Type, Field>();
        static Fields()
        {
            Init();
        }
        private static void Init()
        {
            setField(new DoubleField());
            setField(new FloatField());
            setField(new DecimalField());
            setField(new IntField());
            setField(new LongField());
            setField(new UIntField());
            setField(new ULongField());
            setField(new ByteField());
            setField(new SbyteField());
            setField(new ShortField());
            setField(new UshortField());
            setField(new StringField());
            setField(new BigRationalField());
            setField(new BigIntegerField());
            setField(new BoolField());
            setField(new CharField());
        }
        /// <summary>
        /// Returns a <see cref="Field{T}"/> relevant to type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type return a <see cref="Field{T}"/> for.</typeparam>
        /// <returns>A new <see cref="Field{T}"/> relevant for type |<typeparamref name="T"/></returns>
        /// <remarks>
        /// <para>Will first look up for a field in the central dictionary. If it doesn't exist, it calls <typeparamref name="T"/>'s static constructor and looks up again. If it still doesn't exist, it returns a dynamic <see cref="Field{T}"/>.</para>
        /// </remarks>
        public static Field<T> getField<T>()
        {
            Type t = typeof(T);
            bool constructorcalled = false;
            while (true)
            {
                if (_quickFieldDictionary.ContainsKey(t))
                {
                    return (Field<T>)_quickFieldDictionary[t];
                }
                if (!constructorcalled)
                {
                    try
                    {
                        constructorcalled = true;
                        RuntimeHelpers.RunClassConstructor(typeof(T).TypeHandle);
                        continue;
                    }
                    catch
                    {
                    }
                }
                return new DynamicField<T>();
            }
        }
        /// <summary>
        /// Sets the <see cref="Field{T}"/> for type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Field{T}"/>'s subject type.</typeparam>
        /// <param name="f">The <see cref="Field{T}"/> to set to the type.</param>
        /// <returns>Whether or not a <see cref="Field{T}"/> was set for this type already.</returns>
        /// <remarks>If a <see cref="Field{T}"/> was already set for the type, it is overridden.</remarks>
        public static bool setField<T>(Field<T> f)
        {
            var ret = _quickFieldDictionary.ContainsKey(f.SubjectType);
            _quickFieldDictionary[f.SubjectType] = f;
            return ret;
        }
        /// <summary>
        /// Converts an element to its respective <see cref="FieldWrapper{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the element.</typeparam>
        /// <param name="this">The element to wrap in a <see cref="FieldWrapper{T}"/> object.</param>
        /// <returns>A <see cref="FieldWrapper{T}"/> that encapsulates <paramref name="this"/>.</returns>
        public static FieldWrapper<T> ToFieldWrapper<T>(this T @this) => @this;
    }
}
