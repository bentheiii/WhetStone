using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace WhetStone.Random
{
    public class GlobalRandomGenerator : RandomGenerator
    {
        private static readonly IDictionary<Thread, RandomGenerator> _dic = new Dictionary<Thread, RandomGenerator>(1);
        private static void EnsureGeneratorExists(Thread t)
        {
            var ret = _dic.ContainsKey(t);
            if (!ret)
                reset(t);
        }
        public override byte[] Bytes(int length)
        {
            return ThreadLocal().Bytes(length);
        }
        public override byte Byte()
        {
            return ThreadLocal().Byte();
        }
        public override double Double()
        {
            return ThreadLocal().Double();
        }
        public override double Double(double min, double max)
        {
            return ThreadLocal().Double(min, max);
        }
        public override int Int(int min, int max)
        {
            return ThreadLocal().Int(min, max);
        }
        public static void reset(Thread thread = null, int? seed = null)
        {
            _dic[thread ?? Thread.CurrentThread] =
                new LocalRandomGenerator(seed);
        }
        public static RandomGenerator ThreadLocal()
        {
            EnsureGeneratorExists(Thread.CurrentThread);
            return _dic[Thread.CurrentThread];
        }
    }
}
