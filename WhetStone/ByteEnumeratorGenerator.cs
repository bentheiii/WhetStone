using System.Linq;
using WhetStone.Looping;

namespace WhetStone.Random
{
    /// <summary>
    /// A random generator that utilizes the <see cref="RandomGenerator.Bytes()"/> to generate an arbitrary length array.
    /// </summary>
    public abstract class ByteEnumeratorGenerator : RandomGenerator
    {
        /// <inheritdoc />
        public override byte[] Bytes(int length)
        {
            return Bytes().Take(length).ToArray(length);
        }
        /// <inheritdoc />
        public override byte Byte()
        {
            return Bytes().First();
        }
    }
}
