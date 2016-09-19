using System.Collections;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    public class ExpandingArray<T> : IList<T>
    {
        private readonly List<T> _data;
        public T defaultValue { get; }
        public ExpandingArray(T defaultValue = default(T), int capacity = 4)
        {
            this.defaultValue = defaultValue;
            _data = new List<T>(capacity);
        }
        public void ExpandTo(int newsize)
        {
            if (_data.Count < newsize)
            {
                _data.Capacity = newsize;
                _data.AddRange(defaultValue.Enumerate().Repeat(newsize - _data.Count));
            }
        }
        public int IndexOf(T item)
        {
            return _data.IndexOf(item);
        }
        public void Insert(int index, T item)
        {
            _data.Insert(index, item);
        }
        public void RemoveAt(int index)
        {
            _data.RemoveAt(index);
        }
        public T this[int ind]
        {
            get
            {
                return _data.Count <= ind ? defaultValue : _data[ind];
            }
            set
            {
                ExpandTo(ind + 1);
                _data[ind] = value;
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            return _data.Concat(defaultValue.Enumerate().Cycle()).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_data).GetEnumerator();
        }
        public void Add(T item)
        {
            if (item.Equals(defaultValue))
                return;
            this[_data.Count] = item;
        }
        public void Clear()
        {
            _data.Clear();
        }
        public bool Contains(T item)
        {
            return item.Equals(defaultValue) || _data.Contains(item);
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            _data.CopyTo(array, arrayIndex);
        }
        public bool Remove(T item)
        {
            return _data.Remove(item) || item.Equals(defaultValue);
        }
        public int Count => int.MaxValue;
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
    }
}
