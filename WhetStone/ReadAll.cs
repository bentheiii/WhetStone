using System;
using System.IO;
using WhetStone.SystemExtensions;

namespace WhetStone.Streams
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class readAll
    {
        /// <summary>
        /// Get all bytes left in a <see cref="Stream"/>.
        /// </summary>
        /// <param name="this">The <see cref="Stream"/> to read.</param>
        /// <param name="initialchunksize">The initial buffer size, it will grow exponentially as the stream continues.</param>
        /// <param name="bufferGrowthCoefficient">The rate at wich to expand the buffer.</param>
        /// <returns>An <see cref="Array"/> of all the bytes in <paramref name="this"/>.</returns>
        /// <exception cref="ArgumentException">The stream is unreadable.</exception>
        public static byte[] ReadAll(this Stream @this, int initialchunksize = 256, double bufferGrowthCoefficient = 2.0)
        {
            int NextSize(int prevsize)
            {
                var ret = (int)(prevsize * bufferGrowthCoefficient);
                if (ret <= prevsize)
                    return prevsize + 1;
                return ret;
            }
            @this.ThrowIfNull(nameof(@this));
            initialchunksize.ThrowIfAbsurd(nameof(initialchunksize),false);
            bufferGrowthCoefficient.ThrowIfAbsurd(nameof(bufferGrowthCoefficient), allowNegative:false, allowZero:false);
            if (!@this.CanRead)
                throw new ArgumentException("stream is unreadable");
            byte[] buffer = new byte[initialchunksize];
            int written = 0;
            int r = int.MaxValue;
            while (r > 0)
            {
                r = @this.Read(buffer, written, buffer.Length - written);
                if (r > 0 && r == buffer.Length - written)
                    Array.Resize(ref buffer, NextSize(buffer.Length));
                written += r;
            }
            Array.Resize(ref buffer, written);
            return buffer;
        }
    }
}
