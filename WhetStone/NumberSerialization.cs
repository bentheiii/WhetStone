using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using WhetStone.Arrays;
using WhetStone.Looping;
using WhetStone.WordPlay;

namespace WhetStone.Serializations
{
    public static class NumberSerialization
    {
        public interface INumberSerializer
        {
            BigInteger FromBytes(IEnumerable<byte> bytes);
            IEnumerable<byte> ToBytes(BigInteger num);
        }
        private class FullcodeNumberSerializer : INumberSerializer
        {
            public IEnumerable<byte> ToBytes(BigInteger num)
            {
                while (num != 0)
                {
                    yield return (byte)(num % 256);
                    num /= 256;
                }
            }
            public BigInteger FromBytes(IEnumerable<byte> bytes)
            {
                ulong ret = 0;
                ulong pow = 1;
                foreach (byte b in bytes)
                {
                    ret += (b * pow);
                    pow <<= 8;
                }
                return ret;
            }
        }
        public class ClosedListNumberSerializer : INumberSerializer
        {
            private readonly byte[] _closed;
            public ClosedListNumberSerializer(IEnumerable<char> closedlist) : this(closedlist.Select(a=>(byte)a).ToArray()) { }
            public ClosedListNumberSerializer(byte[] closedList)
            {
                if (closedList.Duplicates().Any() || !closedList.Any())
                    throw new ArgumentException("closed list must be unique and non-empty");
                _closed = closedList.sort();
            }
            public BigInteger FromBytes(IEnumerable<byte> bytes)
            {
                ulong ret = 0;
                ulong pow = 1;
                foreach (byte b in bytes)
                {
                    ret += ((uint)_closed.BinarySearch(b) * pow);
                    pow *= (uint)_closed.Length;
                }
                return ret;
            }
            public IEnumerable<byte> ToBytes(BigInteger num)
            {
                while (num != 0)
                {
                    yield return _closed[(uint)(num % (uint)_closed.Length)];
                    num /= (uint)_closed.Length;
                }
            }
        }
        public static readonly INumberSerializer FullCodeSerializer = new FullcodeNumberSerializer();
        public static readonly INumberSerializer AlphaNumericSerializer = new ClosedListNumberSerializer(range.IRange('a','z').Concat(range.IRange('0','9')));
        public static BigInteger FromString(this INumberSerializer @this, string s)
        {
            return @this.FromBytes(s.Select(a => (byte)a));
        }
        public static string ToString(this INumberSerializer @this, ulong s)
        {
            return @this.ToBytes(s).Select(a => (char)a).ConvertToString();
        }
        public static string EncodeSpecificLength(this INumberSerializer @this, string s, int maxlengthlengthlength = 1)
        {
            int length = s.Length;
            var lenstring = @this.ToString((ulong)length);
            int lengthlength = lenstring.Length;
            var lenlenstring = @this.ToString((ulong)lengthlength);
            int lengthlengthlength = lenlenstring.Length;
            if (lengthlengthlength > maxlengthlengthlength)
                throw new ArgumentException("message too large");
            if (lengthlengthlength < maxlengthlengthlength)
                lenlenstring = lenlenstring + new string((char)0, maxlengthlengthlength - lengthlengthlength);
            return lenlenstring + lenstring + s;
        }
        public static string DecodeSpecifiedLength(this INumberSerializer @this, string s, int lengthlengthlength = 1)
        {
            string remainder;
            return DecodeSpecifiedLength(@this, s, out remainder, lengthlengthlength);
        }
        public static string DecodeSpecifiedLength(this INumberSerializer @this, string s, out string remainder, int lengthlengthlength = 1)
        {
            int lengthlength = (int)@this.FromString(s.Substring(0, lengthlengthlength));
            s = s.Remove(0, lengthlengthlength);
            int length = (int)@this.FromString(s.Substring(0, lengthlength));
            s = s.Remove(0, lengthlength);
            remainder = s.Substring(length);
            return s.Substring(0, length);
        }
    }
}