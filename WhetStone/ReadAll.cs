using System;
using System.IO;
using System.Linq;
using System.Text;
using WhetStone.Looping;
using WhetStone.SystemExtensions;

namespace WhetStone.Streams
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class readAll
    {
        private static int _nextSize(int prevsize, double growthCoff, int maxGrowth = int.MaxValue)
        {
            var ret = (int)(prevsize * growthCoff);
            if (ret <= prevsize)
                ret = prevsize + 1;
            if (ret - prevsize > maxGrowth)
                ret = prevsize + maxGrowth;
            return ret;
        }
        /// <summary>
        /// Get all bytes left in a <see cref="Stream"/>.
        /// </summary>
        /// <param name="this">The <see cref="Stream"/> to read.</param>
        /// <param name="initialchunksize">The initial buffer size, it will grow exponentially as the stream continues.</param>
        /// <param name="bufferGrowthCoefficient">The rate at wich to expand the buffer.</param>
        /// <param name="maxBufferGrowth">The maximum amount of bytes the buffer can grow at once</param>
        /// <returns>An <see cref="Array"/> of all the bytes in <paramref name="this"/>.</returns>
        /// <exception cref="ArgumentException">The stream is unreadable.</exception>
        public static byte[] ReadAll(this Stream @this, 
            int initialchunksize = 256, double bufferGrowthCoefficient = 2.0, int maxBufferGrowth = 4096)
        {
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
                    Array.Resize(ref buffer, _nextSize(buffer.Length, bufferGrowthCoefficient, maxGrowth: maxBufferGrowth));
                written += r;
            }
            Array.Resize(ref buffer, written);
            return buffer;
        }
        /// <summary>
        /// Reads a <see cref="Stream"/> until it ends or until a certain string is at the end.
        /// </summary>
        /// <param name="this">The <see cref="Stream"/> to read from.</param>
        /// <param name="end">The terminating string.</param>
        /// <param name="encoding">The encoding to decode <paramref name="this"/>'s bytes. Default is <see cref="Encoding.UTF8"/>.</param>
        /// <param name="keepEnd">Whether to remove <paramref name="end"/> from the return value or not. If the value is returned due to <paramref name="this"/> ending, nothing will be removed regardless.</param>
        /// <param name="initialchunksize">The initial size of the read buffer.</param>
        /// <param name="bufferGrowthCoefficient">The growth coefficient of the read buffer.</param>
        /// <param name="maxBufferGrowth">The maximum size of the read buffer.</param>
        /// <returns>The contents of <paramref name="this"/> up to <paramref name="end"/>, if it exists.</returns>
        /// <exception cref="ArgumentException">If <paramref name="this"/> cannot be read.</exception>
        /// <remarks>The encoding must be 1-to-1</remarks>
        public static string ReadAllTerminating(
            this Stream @this, string end, Encoding encoding = null, bool keepEnd = false,
            int initialchunksize = 256, double bufferGrowthCoefficient = 2.0, int maxBufferGrowth = 4096)
        {
            encoding = encoding ?? Encoding.UTF8;

            var endBytes = encoding.GetBytes(end);
            var bytes = @this.ReadAllTerminating(endBytes, keepEnd, initialchunksize, bufferGrowthCoefficient, maxBufferGrowth);
            return encoding.GetString(bytes);
        }
        /// <summary>
        /// Reads a <see cref="Stream"/> until it ends or until a certain array is at the end.
        /// </summary>
        /// <param name="this">The <see cref="Stream"/> to read from.</param>
        /// <param name="end">The terminating byte array.</param>
        /// <param name="keepEnd">Whether to remove <paramref name="end"/> from the return value or not. If the value is returned due to <paramref name="this"/> ending, nothing will be removed regardless.</param>
        /// <param name="initialchunksize">The initial size of the buffer.</param>
        /// <param name="bufferGrowthCoefficient">The growth coefficient of the buffer.</param>
        /// <param name="maxBufferGrowth">The maximum growth of the read buffer at once.</param>
        /// <returns>The contents of <paramref name="this"/> up to <paramref name="end"/>, if it exists.</returns>
        /// <exception cref="ArgumentException">If <paramref name="this"/> cannot be read.</exception>
        public static byte[] ReadAllTerminating(
            this Stream @this, byte[] end, bool keepEnd = false,
            int initialchunksize = 256, double bufferGrowthCoefficient = 2.0, int maxBufferGrowth = 4096)
        {
            @this.ThrowIfNull(nameof(@this));
            initialchunksize.ThrowIfAbsurd(nameof(initialchunksize), false);
            bufferGrowthCoefficient.ThrowIfAbsurd(nameof(bufferGrowthCoefficient), allowNegative: false, allowZero: false);
            if (!@this.CanRead)
                throw new ArgumentException("stream is unreadable");

            byte[] buffer = new byte[initialchunksize];
            int written = 0;
            while (true)
            {
                int r = @this.Read(buffer, written, buffer.Length - written);
                if (r == 0)
                {
                    keepEnd = true;
                    break;
                }
                written += r;
                if (written >= end.Length)
                {
                    var tail = buffer.Slice(written - end.Length, length: end.Length);
                    if (tail.SequenceEqual(end))
                    {
                        break;
                    }
                }
                if (written == buffer.Length)
                    Array.Resize(ref buffer, _nextSize(buffer.Length, bufferGrowthCoefficient, maxGrowth: maxBufferGrowth));
            }
            if (!keepEnd)
                written -= end.Length;
            Array.Resize(ref buffer, written);
            return buffer;
        }
    }
}
