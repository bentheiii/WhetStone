using System;
using WhetStone.Arrays;

namespace WhetStone.Random
{
    public class ConstantRandomGenerator : RandomGenerator
    {
        public byte val { get; }
        public ConstantRandomGenerator(byte val)
        {
            this.val = val;
        }
        public override byte[] Bytes(int length)
        {
            return fill.Fill(length, () => val);
        }
        public override double Double(double min, double max)
        {
            var @base = val;
            return (Math.Abs(@base % (max - min)) + min);
        }
        public override int Int(int min, int max)
        {
            var @base = val;
            return (Math.Abs(@base % (max - min)) + min);
        }
        public override ulong ULong(ulong min, ulong max)
        {
            var @base = val;
            return (@base % (max - min) + min);
        }
        public override long Long(long min, long max)
        {
            var @base = val;
            return (Math.Abs(@base % (max - min)) + min);
        }
    }
}
