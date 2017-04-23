using System;
using System.Collections.Generic;
using WhetStone.Looping;
using WhetStone.Fielding;
using WhetStone.SystemExtensions;

namespace WhetStone.Random
{
    /// <summary>
    /// A class that generates random values.
    /// </summary>
    /// <remarks>Either <see cref="Bytes(int)"/>, <see cref="Byte"/>, or <see cref="Int(int,int)"/> must be implemented.</remarks>
    public abstract class RandomGenerator
    {
        /// <summary>
        /// Generate random <see cref="byte"/>s in an <see cref="Array"/>.
        /// </summary>
        /// <param name="length">The length of the resultant array.</param>
        /// <returns>A new <see cref="Array"/> full of random <see cref="byte"/>s.</returns>
        public virtual byte[] Bytes(int length)
        {
            return fill.Fill(length, Byte);
        }
        /// <summary>
        /// Generate random <see cref="byte"/>s in an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <returns>A new <see cref="IEnumerable{T}"/> full of random <see cref="byte"/>s.</returns>
        /// <remarks>The resultant <see cref="IEnumerable{T}"/> might return different members each time it is enumerated.</remarks>
        public virtual IEnumerable<byte> Bytes()
        {
            return generate.Generate(() => Bytes(sizeof(int))).Concat();
        }
        /// <summary>
        /// Get a random <see cref="byte"/>.
        /// </summary>
        /// <returns>A randomly generated <see cref="byte"/></returns>
        public virtual byte Byte()
        {
            return (byte)Int(0, byte.MaxValue, true);
        }
        /// <summary>
        /// Get a random <see cref="int"/>.
        /// </summary>
        /// <returns>A randomly generated <see cref="int"/></returns>
        public int Int()
        {
            // ReSharper disable once IntroduceOptionalParameters.Global
            return Int(int.MinValue,int.MaxValue);
        }
        /// <summary>
        /// Get a random <see cref="int"/>.
        /// </summary>
        /// <param name="max">The maximum value that can be returned. exclusive.</param>
        /// <returns>A randomly generated <see cref="int"/> between 0 and <paramref name="max"/>.</returns>
        public int Int(int max)
        {
            max.ThrowIfAbsurd(nameof(max),false);
            return Int(0, max);
        }
        /// <summary>
        /// Get a random <see cref="int"/>.
        /// </summary>
        /// <param name="min">The minimum value that can be returned. inclusive.</param>
        /// <param name="max">The maximum value that can be returned. exclusive.</param>
        /// <returns>A randomly generated <see cref="int"/> between <paramref name="min"/> and <paramref name="max"/>.</returns>
        public virtual int Int(int min, int max)
        {
            if (min >= max)
                throw new ArgumentException(nameof(min)+" must be less than "+nameof(max));
            return Fields.getField<int>().Generate(Bytes(sizeof(int)), Tuple.Create(min, max));
        }
        /// <summary>
        /// Get a random <see cref="int"/>.
        /// </summary>
        /// <param name="min">The minimum value that can be returned. inclusive.</param>
        /// <param name="max">The maximum value that can be returned.</param>
        /// <param name="inclusive">Whether <paramref name="max"/> is inclusive or not.</param>
        /// <returns>A randomly generated <see cref="int"/> between <paramref name="min"/> and <paramref name="max"/>.</returns>
        public int Int(int min, int max, bool inclusive)
        {
            if (min >= max + inclusive.Indicator())
                throw new ArgumentException(nameof(min) + " must be less than " + nameof(max));
            return Int(min, max + inclusive.Indicator());
        }
        /// <summary>
        /// Get a random <see cref="long"/>.
        /// </summary>
        /// <param name="max">The maximum value that can be returned. exclusive.</param>
        /// <returns>A randomly generated <see cref="long"/> between 0 and <paramref name="max"/>.</returns>
        public long Long(long max)
        {
            max.ThrowIfAbsurd(nameof(max), false);
            return Long(0, max);
        }
        /// <summary>
        /// Get a random <see cref="long"/>.
        /// </summary>
        /// <param name="min">The minimum value that can be returned. inclusive.</param>
        /// <param name="max">The maximum value that can be returned. exclusive.</param>
        /// <returns>A randomly generated <see cref="long"/> between <paramref name="min"/> and <paramref name="max"/>.</returns>
        public virtual long Long(long min, long max)
        {
            if (min >= max)
                throw new ArgumentException(nameof(min) + " must be less than " + nameof(max));
            return Fields.getField<long>().Generate(Bytes(sizeof(long)), Tuple.Create(min, max));
        }
        /// <summary>
        /// Get a random <see cref="long"/>.
        /// </summary>
        /// <param name="min">The minimum value that can be returned. inclusive.</param>
        /// <param name="max">The maximum value that can be returned.</param>
        /// <param name="inclusive">Whether <paramref name="max"/> is inclusive or not.</param>
        /// <returns>A randomly generated <see cref="long"/> between <paramref name="min"/> and <paramref name="max"/>.</returns>
        public long Long(long min, long max, bool inclusive)
        {
            if (min >= max + inclusive.Indicator())
                throw new ArgumentException(nameof(min) + " must be less than " + nameof(max));
            return Long(min, max + (inclusive ? 1 : 0));
        }
        /// <summary>
        /// Get a random <see cref="ulong"/>.
        /// </summary>
        /// <param name="max">The maximum value that can be returned. exclusive.</param>
        /// <returns>A randomly generated <see cref="ulong"/> between 0 and <paramref name="max"/>.</returns>
        public ulong ULong(ulong max)
        {
            if (max == 0)
                throw new ArgumentOutOfRangeException(nameof(max));
            return ULong(0, max);
        }
        /// <summary>
        /// Get a random <see cref="ulong"/>.
        /// </summary>
        /// <param name="min">The minimum value that can be returned. inclusive.</param>
        /// <param name="max">The maximum value that can be returned. exclusive.</param>
        /// <returns>A randomly generated <see cref="ulong"/> between <paramref name="min"/> and <paramref name="max"/>.</returns>
        public virtual ulong ULong(ulong min, ulong max)
        {
            if (min >= max)
                throw new ArgumentException(nameof(min) + " must be less than " + nameof(max));
            return Fields.getField<ulong>().Generate(Bytes(sizeof(ulong)), Tuple.Create(min, max));
        }
        /// <summary>
        /// Get a random <see cref="ulong"/>.
        /// </summary>
        /// <param name="min">The minimum value that can be returned. inclusive.</param>
        /// <param name="max">The maximum value that can be returned.</param>
        /// <param name="inclusive">Whether <paramref name="max"/> is inclusive or not.</param>
        /// <returns>A randomly generated <see cref="ulong"/> between <paramref name="min"/> and <paramref name="max"/>.</returns>
        public ulong ULong(ulong min, ulong max, bool inclusive)
        {
            if (min >= max + (inclusive ? 1U : 0U))
                throw new ArgumentException(nameof(min) + " must be less than " + nameof(max));
            return ULong(min, max + (inclusive ? 1U : 0U));
        }
        /// <summary>
        /// Get a random <see cref="double"/> between 0 and 1.
        /// </summary>
        /// <returns>A random <see cref="double"/> between 0 and 1</returns>
        public virtual double Double()
        {
            return Double(1);
        }
        /// <summary>
        /// Get a random <see cref="double"/>.
        /// </summary>
        /// <param name="max">The maximum value that can be returned. exclusive.</param>
        /// <returns>A randomly generated <see cref="double"/> between 0 and <paramref name="max"/>.</returns>
        public double Double(double max)
        {
            max.ThrowIfAbsurd(nameof(max));
            if (max <= 0)
                throw new ArgumentOutOfRangeException(nameof(max));
            return Double(0, max);
        }
        /// <summary>
        /// Get a random <see cref="double"/>.
        /// </summary>
        /// <param name="min">The minimum value that can be returned. inclusive.</param>
        /// <param name="max">The maximum value that can be returned. exclusive.</param>
        /// <returns>A randomly generated <see cref="double"/> between <paramref name="min"/> and <paramref name="max"/>.</returns>
        public virtual double Double(double min, double max)
        {
            if (min >= max)
                throw new ArgumentException(nameof(min) + " must be less than " + nameof(max));
            return Fields.getField<double>().Generate(Bytes(sizeof(double)), Tuple.Create(min, max));
        }
        /// <summary>
        /// get a random <see cref="bool"/>.
        /// </summary>
        /// <param name="odds">The likelihood <see langword="true"/> will be returned.</param>
        /// <returns><see langword="true"/> at likelihood <paramref name="odds"/></returns>
        public bool success(double odds)
        {
            odds.ThrowIfAbsurd(nameof(odds),true,true);
            if (odds >= 1)
                return true;
            if (odds <= 0)
                return false;
            return Double() <= odds;
        }
        /// <summary>
        /// get a random <see cref="bool"/>.
        /// </summary>
        /// <param name="trueodds">Odds <see langword="true"/>  will be returned.</param>
        /// <param name="falseodds">Odds <see langword="false"/> will be returned.</param>
        /// <returns>A random <see cref="bool"/> with likelihoods of <paramref name="trueodds"/>:<paramref name="falseodds"/></returns>
        public bool Bool(double trueodds = 1, double falseodds = 1)
        {
            if (trueodds <= 0)
                throw new ArgumentOutOfRangeException(nameof(trueodds));
            if (falseodds <= 0)
                throw new ArgumentOutOfRangeException(nameof(falseodds));
            return success(trueodds / (falseodds + trueodds));
        }
        /// <summary>
        /// Generate a random element of a generic type using fielding.
        /// </summary>
        /// <typeparam name="T">The type of element to generate</typeparam>
        /// <returns>A randomly generated element of type <typeparamref name="T"/>.</returns>
        /// <remarks>
        /// <para>This function uses fielding.</para>
        /// <para>The field for type <typeparamref name="T"/> must support <see cref="GenerationType.FromBytes"/> generation.</para>
        /// </remarks>
        public T FromField<T>()
        {
            var f = Fields.getField<T>();
            if (f.GenType == GenerationType.Never || f.GenType == GenerationType.Special)
                throw new NotSupportedException("Field does not support this generation");
            return f.Generate(Bytes());
        }
        /// <summary>
        /// Generate a random element of a generic type using fielding.
        /// </summary>
        /// <typeparam name="T">The type of element to generate</typeparam>
        /// <param name="min">The minimum value to be generates. Inclusive.</param>
        /// <param name="max">The maximum value to be generated. Exclusive.</param>
        /// <returns>A randomly generated element of type <typeparamref name="T"/>.</returns>
        /// <remarks>
        /// <para>This function uses fielding.</para>
        /// <para>The field for type <typeparamref name="T"/> must support <see cref="GenerationType.FromRange"/> generation.</para>
        /// </remarks>
        public T FromField<T>(T min, T max)
        {
            var f = Fields.getField<T>();
            if (f.GenType != GenerationType.FromRange)
                throw new NotSupportedException("Field does not support this generation");
            return f.Generate(Bytes(), Tuple.Create(min, max));
        }
        /// <summary>
        /// Generate a random element of a generic type using fielding.
        /// </summary>
        /// <typeparam name="T">The type of element to generate</typeparam>
        /// <param name="min">The minimum value to be generates. Inclusive.</param>
        /// <param name="max">The maximum value to be generated. Exclusive.</param>
        /// <param name="special">A special value to constrain the generated value.</param>
        /// <returns>A randomly generated element of type <typeparamref name="T"/>.</returns>
        /// <remarks>
        /// <para>This function uses fielding.</para>
        /// <para>The field for type <typeparamref name="T"/> must support <see cref="GenerationType.Special"/> generation.</para>
        /// </remarks>
        public T FromField<T>(T min, T max, object special)
        {
            var f = Fields.getField<T>();
            if (f.GenType != GenerationType.Special)
                throw new NotSupportedException("Field does not support this generation");
            return f.Generate(Bytes(), Tuple.Create(min, max), special);
        }
    }
}
