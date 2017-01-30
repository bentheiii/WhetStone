using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WhetStone.Looping;

namespace WhetStone.Streams
{
    public static class loop
    {
        public static IEnumerable<string> Loop(this TextReader @this, int? length = null, int cache = 0)
        {
            IEnumerable<string> ret;
            if (length == null)
                ret = generate.Generate(@this.ReadLine);
            else
            {
                var buffer = new char[length.Value];
                ret = generate.Generate(() =>
                {
                    int read = @this.Read(buffer, 0, length.Value);
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
        public static IEnumerable<byte> Loop(this Stream @this, bool cache = true)
        {
            if (!@this.CanRead)
                throw new ArgumentException("stream is unreadable");
            var ret = generate.Generate(@this.ReadByte).TakeWhile(a => a > 0).Select(a => (byte)a);
            if (cache)
                ret = ret.Cache();
            return ret;
        }
    }
}