using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WhetStone.LockedStructures
{
    internal class ListBitArrayAdaptor : IList<bool>
    {
        private readonly BitArray _int;
        public ListBitArrayAdaptor(BitArray i)
        {
            _int = i;
        }
        public IEnumerator<bool> GetEnumerator()
        {
            return _int.Cast<bool>().GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void Add(bool item)
        {
            _int.Length++;
            _int[_int.Length-1] = item;
        }
        public void Clear()
        {
            _int.Length = 0;
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
        public int Count => _int.Count;
        public bool IsReadOnly => false;
        public int IndexOf(bool item)
        {
            int ret = 0;
            foreach (bool b in _int)
            {
                if (b == item)
                    return ret;
                ret++;
            }
            return -1;
        }
        public void Insert(int index, bool item)
        {
            if (index == Count)
                Add(item);
            else
                throw new NotSupportedException();
        }
        public void RemoveAt(int index)
        {
            if (index == Count - 1)
                _int.Length--;
            else
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
