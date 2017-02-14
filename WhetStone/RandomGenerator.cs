using System;
using System.Collections.Generic;
using System.Drawing;
using WhetStone.Looping;
using WhetStone.Fielding;
using WhetStone.WordPlay;

namespace WhetStone.Random
{
    public abstract class RandomGenerator
    {
        public virtual byte[] Bytes(int length)
        {
            return fill.Fill(length, Byte);
        }
        public virtual IEnumerable<byte> Bytes()
        {
            return generate.Generate(() => Bytes(sizeof(int))).Concat();
        }
        public virtual byte Byte()
        {
            return (byte)Int(0, byte.MaxValue, true);
        }
        public int Int()
        {
            return Int(0,int.MaxValue,true);
        }
        public int Int(int max)
        {
            return Int(0, max);
        }
        public virtual int Int(int min, int max)
        {
            return Fields.getField<int>().Generate(Bytes(sizeof(int)), Tuple.Create(min, max));
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
            return @enum.Enum<T>().Pick(this);
        }
        public Color randomcolor()
        {
            return Color.FromArgb(Int(0, 256), Int(0, 256), Int(0, 256));
        }
        public string String(int length, char[] allowedChars)
        {
            return range.Range(length).Select(a => allowedChars[Int(allowedChars.Length)]).ConvertToString();
        }
        public virtual T FromField<T>()
        {
            var f = Fields.getField<T>();
            if (f.GenType == GenerationType.Never || f.GenType == GenerationType.Special)
                throw new NotSupportedException("Field does not support this generation");
            return f.Generate(Bytes());
        }
        public virtual T FromField<T>(T min, T max)
        {
            var f = Fields.getField<T>();
            if (f.GenType != GenerationType.FromRange)
                throw new NotSupportedException("Field does not support this generation");
            return f.Generate(Bytes(), Tuple.Create(min, max));
        }
        public virtual T FromField<T>(T min, T max, object special)
        {
            var f = Fields.getField<T>();
            if (f.GenType != GenerationType.Special)
                throw new NotSupportedException("Field does not support this generation");
            return f.Generate(Bytes(), Tuple.Create(min, max), special);
        }
    }
}
