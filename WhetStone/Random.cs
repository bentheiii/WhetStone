using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Security.Cryptography;
using WhetStone.Arrays;
using WhetStone.Fielding;
using WhetStone.Looping;
using WhetStone.WordsPlay;

namespace WhetStone.Random
{
    public abstract class RandomGenerator
    {
        public virtual byte[] Bytes(int length)
        {
            return ArrayExtensions.Fill(length, () => (byte)Int(0, byte.MaxValue));
        }
        public virtual IEnumerable<byte> Bytes()
        {
            return Loops.Generate(() => Bytes(sizeof(int))).Concat();
        }
        public int Int(int max)
        {
            return Int(0, max);
        }
        public virtual int Int(int min, int max)
        {
            return Fields.getField<int>().Generate(Bytes(sizeof(int)),Tuple.Create(min,max));
        }
        public int Int(int min, int max, bool inclusive)
        {
            return Int(min, max + (inclusive ? 1 : 0));
        }
        public long Long(long max)
        {
            return Long(0, max);
        }
        public virtual long Long(long min, long max)
        {
            return Fields.getField<long>().Generate(Bytes(sizeof(long)), Tuple.Create(min, max));
        }
        public long Long(long min, long max, bool inclusive)
        {
            return Long(min, max + (inclusive ? 1 : 0));
        }
        public ulong ULong(ulong max)
        {
            return ULong(0, max);
        }
        public virtual ulong ULong(ulong min, ulong max)
        {
            return Fields.getField<ulong>().Generate(Bytes(sizeof(ulong)), Tuple.Create(min, max));
        }
        public ulong ULong(ulong min, ulong max, bool inclusive)
        {
            return ULong(min, max + (inclusive ? 1U : 0U));
        }
        public virtual double Double()
        {
            return Double(1);
        }
        public double Double(double max)
        {
            return Double(0, max);
        }
        public virtual double Double(double min, double max)
        {
            return Fields.getField<double>().Generate(Bytes(sizeof(double)), Tuple.Create(min, max));
        }
        public bool success(double odds)
        {
            if (odds >= 1)
                return true;
            if (odds <= 0)
                return false;
            return Double() <= odds;
        }
        public bool randombool(int trueodds = 1, int falseodds = 1)
        {
            return success(trueodds / (double)(falseodds + trueodds));
        }
        public char randomchar(char min = 'a', char max = 'z')
        {
            return (char)(Int(min, max, true));
        }
        public char randomchar(string allowedchars)
        {
            return allowedchars[Int(0, allowedchars.Length)];
        }
        public object randomenum<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new Exception("type is not an enum");
            return Loops.Enum<T>().Pick(this);
        }
        public Color randomcolor()
        {
            return Color.FromArgb(Int(0, 256), Int(0, 256), Int(0, 256));
        }
        public string String(int length, char[] allowedChars)
        {
            return Loops.Range(length).Select(a => allowedChars[Int(allowedChars.Length)]).ConvertToString();
        }
        public T FromField<T>()
        {
            var f = Fields.getField<T>();
            if (f.GenType == GenerationType.None || f.GenType == GenerationType.Special)
                throw new NotSupportedException("Field does not support this generation");
            return f.Generate(Bytes());
        }
        public T FromField<T>(T min, T max)
        {
            var f = Fields.getField<T>();
            if (f.GenType != GenerationType.FromRange)
                throw new NotSupportedException("Field does not support this generation");
            return f.Generate(Bytes(),Tuple.Create(min,max));
        }
        public T FromField<T>(T min, T max, object special)
        {
            var f = Fields.getField<T>();
            if (f.GenType != GenerationType.Special)
                throw new NotSupportedException("Field does not support this generation");
            return f.Generate(Bytes(), Tuple.Create(min, max), special);
        }
    }
    public abstract class ByteEnumeratorGenerator : RandomGenerator
    {
        public override byte[] Bytes(int length)
        {
            return Bytes().Take(length).ToArray(length);
        }
    }
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
            return _int.Next(min,max);
        }
    }
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
            return _dic[Thread.CurrentThread].Double(min,max);
        }
        public override int Int(int min, int max)
        {
            EnsureGeneratorExists(Thread.CurrentThread);
            return _dic[Thread.CurrentThread].Int(min,max);
        }
        public static void reset()
        {
            reset(Thread.CurrentThread);
        }
        public static void reset(Thread t)
        {
            reset(DateTime.Now.GetHashCode() ^ Process.GetCurrentProcess().GetHashCode() ^ Thread.CurrentThread.GetHashCode(),t);
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
    public class ConstantRandomGenerator : RandomGenerator
    {
        public byte val { get; }
        public ConstantRandomGenerator(byte val)
        {
            this.val = val;
        }
        public override byte[] Bytes(int length)
        {
            return ArrayExtensions.Fill(length, () => val);
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
    public class ShaGenerator : ByteEnumeratorGenerator, IDisposable
    {
        private readonly SHA256 _sha;
        private readonly IList<byte> _seed;
        public ShaGenerator(IEnumerable<byte> seed)
        {
            _sha = SHA256.Create();
            _seed = new List<byte>(seed);
        }
        public override IEnumerable<byte> Bytes()
        {
            while (true)
            {
                _sha.ComputeHash(_seed.ToArray().Shuffle());
                _seed.Clear();
                foreach (byte b in _sha.Hash)
                {
                    yield return b;
                    _seed.Add(b);
                }
            }
        }
        public void Dispose()
        {
            ((IDisposable)_sha).Dispose();
        }
    }
    namespace ThreadEntropy
    {
        public class EntropyRandomGenerator : RandomGenerator, IDisposable
        {
            private volatile int _val = 0;
            private readonly Thread[] _runners;
            public EntropyRandomGenerator(int threadCount = 2, ThreadPriority priority = ThreadPriority.Lowest)
            {
                _runners = new Thread[threadCount];
                for (int i = 0; i < threadCount; i++)
                {
                    _runners[i] = new Thread(ThreadProcedure) {Priority = priority};
                    _runners[i].Start();
                }
            }
            private void ThreadProcedure()
            {
                while (true)
                    _val++;
            }
            protected virtual void Dispose(bool disposing)
            {
                if( disposing) foreach (Thread runner in _runners)
                {
                    runner.Abort();
                }
            }
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            public override int Int(int min, int max)
            {
                _val *= 258745143;
                return (Math.Abs(_val % (max - min)) + min);
            }
        }
    }
}