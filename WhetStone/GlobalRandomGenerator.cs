using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            EnsureGeneratorExists(Thread.CurrentThread);
            return _dic[Thread.CurrentThread].Bytes(length);
        }
        public override double Double()
        {
            EnsureGeneratorExists(Thread.CurrentThread);
            return _dic[Thread.CurrentThread].Double();
        }
        public override double Double(double min, double max)
        {
            EnsureGeneratorExists(Thread.CurrentThread);
            return _dic[Thread.CurrentThread].Double(min, max);
        }
        public override int Int(int min, int max)
        {
            EnsureGeneratorExists(Thread.CurrentThread);
            return _dic[Thread.CurrentThread].Int(min, max);
        }
        public static void reset()
        {
            reset(Thread.CurrentThread);
        }
        public static void reset(Thread t)
        {
            reset(DateTime.Now.GetHashCode() ^ Process.GetCurrentProcess().GetHashCode() ^ Thread.CurrentThread.GetHashCode(), t);
        }
        public static void reset(int seed)
        {
            reset(seed, Thread.CurrentThread);
        }
        public static void reset(int seed, Thread t)
        {
            _dic[t] = new LocalRandomGenerator(seed);
        }
    }
}
