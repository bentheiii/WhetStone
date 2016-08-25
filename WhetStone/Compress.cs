using System.IO;
using System.IO.Compression;
using WhetStone.Streams;

namespace WhetStone.Serializations
{
    public static class compress
    {
        public static byte[] Compress(this byte[] raw)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(memory, CompressionMode.Compress, true))
                {
                    gzip.Write(raw, 0, raw.Length);
                }
                return memory.ToArray();
            }
        }
        public static byte[] Decompress(this byte[] gzip)
        {
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                return stream.ReadAll();
            }
        }
    }
}