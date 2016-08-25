using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Arrays;
using WhetStone.Random;

namespace WhetStone.Random
{
    public abstract class ByteEnumeratorGenerator : RandomGenerator
    {
        public override byte[] Bytes(int length)
        {
            return Bytes().Take(length).ToArray(length);
        }
    }
}
