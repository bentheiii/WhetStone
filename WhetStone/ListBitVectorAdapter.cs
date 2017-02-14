using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Looping;

namespace WhetStone.LockedStructures
{
    internal class ListBitVectorAdapter : IList<bool>
    {
        private BitVector32 _int;
        public ListBitVectorAdapter(BitVector32 i)
        {
            _int = i;
        }
        public IEnumerator<bool> GetEnumerator()
        {
            return range.Range(32).Select(i => _int[i]).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void Add(bool item)
        {
            throw new NotSupportedException();
        }
        public void Clear()
        {
            throw new NotSupportedException();
        }
        public bool Contains(bool item)
        {
            return IndexOf(item) >= 0;
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
            throw new NotSupportedException();
        }
        public int Count => 32;
        public bool IsReadOnly => false;
        public int IndexOf(bool item)
        {
            int ret = 0;
            foreach (bool b in this)
            {
                if (b == item)
                    return ret;
                ret++;
            }
            return -1;
        }
        public void Insert(int index, bool item)
        {
            throw new NotSupportedException();
        }
        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }
        public bool this[int index]
        {
            get
            {
                return _int[index];
            }
            set
            {
                _int[index] = value;
            }
        }
    }
}
