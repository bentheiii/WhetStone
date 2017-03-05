using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using WhetStone.Looping;

namespace WhetStone.Streams
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class loop
    {
        /// <summary>
        /// Get a <see cref="TextReader"/>'s contents as an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="this">The <see cref="TextReader"/> to read from.</param>
        /// <param name="length">The length of each element in the returned <see cref="IEnumerable{T}"/>. Setting to 0 will chunk the elements by lines.</param>
        /// <param name="cache">The maximum cache size (only storing the first-most members). Setting to 0 is an infinite cache, setting to less is no cache.</param>
        /// <returns>A (possibly cached) <see cref="IEnumerable{T}"/> of parts of <paramref name="this"/>.</returns>
        public static IEnumerable<string> Loop(this TextReader @this, int length = 0, int cache = 0)
        {
            IEnumerable<string> ret;
            if (length <= 0)
                ret = generate.Generate(@this.ReadLine);
            else
            {
                var buffer = new char[length];
                ret = generate.Generate(() =>
                {
                    int read = @this.Read(buffer, 0, length);
                    return read == 0 ? null : new string(buffer,0,read);
                });
            }
            ret = ret.TakeWhile(a => a != null);
            if (cache >= 0)
            {
                ret = cache==0 ? ret.Cache() : ret.Cache(cache);
            }
            return ret;
        }
        /// <summary>
        /// Get a <see cref="Stream"/>'s contents as an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="this">The <see cref="Stream"/> to read from</param>
        /// <param name="cache">The maximum cache size (only storing the first-most members). Setting to 0 is an infinite cache, setting to less is no cache. setting to <see langword="null"/> means decide by whether <see cref="Stream.CanSeek"/>.</param>
        /// <returns>A (possibly cached) <see cref="IEnumerable{T}"/> of bytes from <paramref name="this"/>.</returns>
        public static IEnumerable<byte> Loop(this Stream @this, int? cache = null)
        {
            if (!@this.CanRead)
                throw new ArgumentException("stream is unreadable");
            if (cache == null)
                cache = @this.CanSeek ? -1 : 0;
            var ret = generate.Generate(@this.ReadByte).TakeWhile(a => a > 0).Select(a => (byte)a);
            if (@this.CanSeek)
                ret.EnumerationHook(begin: () => @this.Seek(0,SeekOrigin.Begin));
            if (cache >= 0)
            {
                ret = cache == 0 ? ret.Cache() : ret.Cache(cache);
            }
            return ret;
        }
    }
}
