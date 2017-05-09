using System;
using System.Collections.Generic;
using System.Threading;
using WhetStone.SystemExtensions;

namespace WhetStone.Random
{
    /// <summary>
    /// A <see cref="RandomGenerator"/> that wraps a static instance of a <see cref="RandomGenerator"/>.
    /// </summary>
    /// <remarks>
    /// <para>To ensure thread safety, usage of this <see cref="RandomGenerator"/> is slightly slower than using <see cref="LocalRandomGenerator"/>. Consider creating an instance upon rapid usage.</para>
    /// <para>Every thread has an independent static <see cref="RandomGenerator"/>, this means that advancing one will not advance others.</para>
    /// </remarks>
    public class GlobalRandomGenerator : RandomGenerator
    {
        private static readonly IDictionary<Thread, RandomGenerator> _dic = new Dictionary<Thread, RandomGenerator>(1);
        private static void EnsureGeneratorExists(Thread t)
        {
            var ret = _dic.ContainsKey(t);
            if (!ret)
                Reset(t);
        }
        /// <inheritdoc />
        public override byte[] Bytes(int length)
        {
            length.ThrowIfAbsurd(nameof(length));
            return ThreadLocal().Bytes(length);
        }
        /// <inheritdoc />
        public override byte Byte()
        {
            return ThreadLocal().Byte();
        }
        /// <inheritdoc />
        public override double Double()
        {
            return ThreadLocal().Double();
        }
        /// <inheritdoc />
        public override double Double(double min, double max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException(nameof(min)+" must be lower than "+nameof(max));
            return ThreadLocal().Double(min, max);
        }
        /// <inheritdoc />
        public override int Int(int min, int max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException(nameof(min) + " must be lower than " + nameof(max));
            return ThreadLocal().Int(min, max);
        }
        /// <summary>
        /// Resets an instance of the static generator.
        /// </summary>
        /// <param name="thread">The thread instance for which to reset the generator.</param>
        /// <param name="seed">The new seed for which to reset the generator. <see langword="null"/> for a pseudo-random seed.</param>
        public static void Reset(Thread thread = null, int? seed = null)
        {
            _dic[thread ?? Thread.CurrentThread] =
                new LocalRandomGenerator(seed);
        }
        /// <summary>
        /// Get the thread-unsafe <see cref="RandomGenerator"/> reserved for this thread only.
        /// </summary>
        /// <returns>The specific <see cref="RandomGenerator"/> for the calling thread.</returns>
        public static RandomGenerator ThreadLocal()
        {
            EnsureGeneratorExists(Thread.CurrentThread);
            return _dic[Thread.CurrentThread];
        }
    }
}
