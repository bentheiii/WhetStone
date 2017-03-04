using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace WhetStone.Random
{
    /// <summary>
    /// A random generator wrapping a <see cref="Random"/> object.
    /// </summary>
    public class LocalRandomGenerator : RandomGenerator
    {
        private readonly System.Random _int;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="seed">The initial seed for the inner <see cref="Random"/>. Setting to <see langword="null"/> will generate a seed.</param>
        public LocalRandomGenerator(int? seed = null)
        {
            _int = new System.Random(seed ??
                                  DateTime.Now.GetHashCode() ^ Process.GetCurrentProcess().GetHashCode() ^ Thread.CurrentThread.GetHashCode());
        }
        public LocalRandomGenerator(out int seed)
        {
            seed = DateTime.Now.GetHashCode() ^ Process.GetCurrentProcess().GetHashCode() ^ Thread.CurrentThread.GetHashCode();
            _int = new System.Random(seed);
        }
        /// <inheritdoc />
        public override byte[] Bytes(int length)
        {
            byte[] ret = new byte[length];
            _int.NextBytes(ret);
            return ret;
        }
        /// <inheritdoc />
        public override byte Byte()
        {
            return Bytes(1).Single();
        }
        /// <inheritdoc />
        public override double Double()
        {
            return _int.NextDouble();
        }
        /// <inheritdoc />
        public override double Double(double min, double max)
        {
            return (max - min) * _int.NextDouble() + min;
        }
        /// <inheritdoc />
        public override int Int(int min, int max)
        {
            if (min == 0 && max == 0)
                return _int.Next();
            return _int.Next(min, max);
        }
    }
}
