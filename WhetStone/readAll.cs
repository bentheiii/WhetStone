using System;
using System.IO;

namespace WhetStone.Streams
{
    public static class readAll
    {
        public static byte[] ReadAll(this Stream @this, int initialchunksize = 256)
        {
            if (!@this.CanRead)
                throw new ArgumentException("stream is unreadable");
            byte[] buffer = new byte[initialchunksize / 2];
            int written = 0;
            int r = int.MaxValue;
            while (r > 0)
            {
                Array.Resize(ref buffer, buffer.Length * 2);
                r = @this.Read(buffer, written, buffer.Length - written);
                written += r;
            }
            Array.Resize(ref buffer, written);
            return buffer;
        }
    }
}
