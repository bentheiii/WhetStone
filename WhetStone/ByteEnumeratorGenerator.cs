using System.Linq;
using WhetStone.Looping;

namespace WhetStone.Random
{
    public abstract class ByteEnumeratorGenerator : RandomGenerator
    {
        public override byte[] Bytes(int length)
        {
            return Bytes().Take(length).ToArray(length);
        }
        public override byte Byte()
        {
            return Bytes().First();
        }
    }
}
