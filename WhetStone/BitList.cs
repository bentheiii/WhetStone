//#define BYTEWORD

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Looping;
using word =
#if BYTEWORD
System.Byte
#elif WIN64
System.UInt64
#elif WIN32
System.UIint32
#else
#error word not defined
#endif
;

namespace WhetStone.SystemExtensions
{
    public class BitList : IList<bool>
    {
        private readonly List<word> _int;
        private const int BITS_IN_CELL = 8*sizeof(word);
        public BitList(int size=0)
        {
            _int = new List<word>(range.Range((size/(double)BITS_IN_CELL).ceil()).Select(a=>(word)0));
            Count = size;
        }
        public IEnumerator<bool> GetEnumerator()
        {
            int countdown = Count;
            foreach (var byt in _int)
            {
                word k = byt;
                foreach (var _ in range.Range(BITS_IN_CELL))
                {
                    if (countdown == 0)
                        yield break;
                    yield return (k&1) == 1;
                    countdown--;
                    k >>= 1;
                }
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void Add(bool item)
        {
            var lmod = Count%BITS_IN_CELL;
            if (lmod == 0)
            {
                _int.Add(item.Indicator((word)1,(word)0));
            }
            else
            {
                _int[_int.Count - 1] |= (word)1 << lmod;
            }
            Count++;
        }
        public void Clear()
        {
            Count = 0;
            _int.Clear();
        }
        public bool Contains(bool item)
        {
            return this.Any(a=>a.Equals(item));
        }
        public void CopyTo(bool[] array, int arrayIndex)
        {
            foreach (var v in this)
            {
                array[arrayIndex++] = v;
            }
        }
        public bool Remove(bool item)
        {
            var ind = this.IndexOf(item);
            if (ind == -1)
                return false;
            RemoveAt(ind);
            return true;
        }
        public int Count { get; private set; }
        public bool IsReadOnly => false;
        public int IndexOf(bool item)
        {
            int ret = 0;
            foreach (var v in this)
            {
                if (v == item)
                    return ret;
                ret++;
            }
            return -1;
        }
        public void Insert(int index, bool item)
        {
            if (index == Count)
            {
                Add(item);
                return;
            }

            int shiftMemberInd = index/BITS_IN_CELL;
            int newmemberind = index%BITS_IN_CELL;
            const word carrymask = (word)1 << (BITS_IN_CELL - 1);
            bool carry = (_int[shiftMemberInd] & carrymask) != 0;

            ulong unshifted;
            ulong shifted;
            if (newmemberind == 63)
            {
                shifted = 0;
            }
            else
            {
                shifted = (_int[shiftMemberInd] >> newmemberind) << newmemberind;
            }
            if (newmemberind == 0)
            {
                unshifted = 0;
            }
            else
            {
                unshifted = _int[shiftMemberInd] & ~shifted;
            }
            var newval = (shifted << 1) | unshifted;
            if (item)
                newval |= ((word)1 << newmemberind);
            else
                newval &= ~((word)1 << newmemberind);
            _int[shiftMemberInd] = newval;
            foreach (var ind in range.Range(shiftMemberInd+1,_int.Count,1))
            {
                var newcarry = (_int[ind] & carrymask) != 0;
                _int[ind] <<= 1;
                if (carry)
                    _int[ind] |= 1;
                carry = newcarry;
            }
            if (carry)
                _int.Add(1);
            Count++;
        }
        public void RemoveAt(int index)
        {
            if (index == Count-1)
            {
                Count--;
                _int[_int.Count - 1] &= ~((word)1 << (index));
                return;
            }
            int shiftMemberInd = index / BITS_IN_CELL;
            int removedmemberind = index % BITS_IN_CELL;
            bool carry = false;
            const word carrymask = (word)1 << (BITS_IN_CELL - 1);
            foreach (var ind in range.Range(_int.Count-1, shiftMemberInd, -1))
            {
                var newcarry = (_int[ind] & 1) == 1;
                _int[ind] >>= 1;
                if (carry)
                    _int[ind] |= carrymask;
                carry = newcarry;
            }

            ulong unshifted;
            ulong shifted;
            if (removedmemberind == 63)
            {
                shifted = 0;
            }
            else
            {
                shifted = (_int[shiftMemberInd] >> removedmemberind) << removedmemberind;
            }
            if (removedmemberind == 0)
            {
                unshifted = 0;
            }
            else
            {
                unshifted = _int[shiftMemberInd] & ~shifted;
            }
            var newval = (shifted >> 1) | unshifted;
            if (carry)
                newval |= carrymask;
            else
                newval &= ~carrymask;
            _int[shiftMemberInd] = newval;
            Count--;
            
            if ((Count/(double)BITS_IN_CELL).ceil() < _int.Count)
                _int.RemoveAt(_int.Count-1);
        }
        public bool this[int index]
        {
            get
            {
                return (_int[index/BITS_IN_CELL]&((word)1 << (index%BITS_IN_CELL))) != 0;
            }
            set
            {
                var mask = (word)1 << (index%BITS_IN_CELL);
                if (value)
                    _int[index/BITS_IN_CELL] |= mask;
                else
                    _int[index/BITS_IN_CELL] &= ~mask;
            }
        }
        public void SetRange(int start, int length, bool value)
        {
            var fill = value ? ~(word)0 : (word)0;
            int fillStart = (start/(double)BITS_IN_CELL).ceil();
            int fillEnd = ((start+length) / (double)BITS_IN_CELL).floor();

            _int.Fill(new[] {fill}, fillStart,fillEnd-fillStart);

            var smod = start%BITS_IN_CELL;
            if (smod != 0)
            {
                int ind = fillStart - 1;
                word mask = (~(word)0) << smod;
                if (value)
                    _int[ind] |= mask;
                else
                    _int[ind] &= ~mask;
            }

            var emod = (start+length) % BITS_IN_CELL;
            if (emod != 0)
            {
                int ind = fillEnd;
                word mask = (~(word)0) >> (BITS_IN_CELL-emod);
                if (value)
                    _int[ind] |= mask;
                else
                    _int[ind] &= ~mask;
            }
        }
    }
}
