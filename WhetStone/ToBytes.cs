using System.Linq;

namespace WhetStone.WordPlay
{
    public static class toBytes
    {
        public static byte[] ToBytes(this string @this)
        {
            return @this.Select(a => (byte)a).ToArray();
        }
    }
}
