using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WhetStone.Looping;

namespace WhetStone.Streams
{
    public static class loop
    {
        public static IEnumerable<string> Loop(this TextReader @this)
        {
            return generate.Generate(@this.ReadLine).TakeWhile(a => a != null);
        }
        public static IEnumerable<byte> Loop(this Stream @this)
        {
            if (!@this.CanRead)
                throw new ArgumentException("stream is unreadable");
            return generate.Generate(@this.ReadByte).TakeWhile(a => a > 0).Select(a => (byte)a);
        }
    }
}