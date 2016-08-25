using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Random
{
    public class LocalRandomGenerator : RandomGenerator
    {
        private readonly System.Random _int;
        public LocalRandomGenerator()
        {
            _int = new System.Random();
        }
        public LocalRandomGenerator(int seed)
        {
            _int = new System.Random(seed);
        }
        public override byte[] Bytes(int length)
        {
            byte[] ret = new byte[length];
            _int.NextBytes(ret);
            return ret;
        }
        public override double Double()
        {
            return _int.NextDouble();
        }
        public override double Double(double min, double max)
        {
            return (max - min) * _int.NextDouble() + min;
        }
        public override int Int(int min, int max)
        {
            return _int.Next(min, max);
        }
    }
}
