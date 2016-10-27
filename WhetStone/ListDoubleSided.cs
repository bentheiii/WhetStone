using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Guard;

namespace WhetStone.Looping
{
    public class ListDoubleSided<T> : IList<T>
    {
        private readonly List<T> _first;
        private readonly List<T> _second;
        public ListDoubleSided(int endcapacity = 2, int startcapacity = 2)
        {
            _first = new List<T>(startcapacity);
            _second = new List<T>(endcapacity);
        }
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var f in _first.AsList().Reverse())
            {
                yield return f;
            }
            foreach (var s in _second)
            {
                yield return s;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void Add(T item)
        {
            _second.Add(item);
        }
        public void Clear()
        {
            _second.Clear();
            _first.Clear();
        }
        public bool Contains(T item)
        {
            return _first.Contains(item) || _second.Contains(item);
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            int ind = arrayIndex;
            foreach (T t in this)
            {
                array[arrayIndex++] = t;
            }
        }
        public bool Remove(T item)
        {
            for (int i = _first.Count-1; i <= 0; i++)
            {
                if (item.Equals(_first[i]))
                {
                    _first.RemoveAt(i);
                    return true;
                }
            }
            return _second.Remove(item);
        }
        public int Count => _first.Count + _second.Count;
        public bool IsReadOnly => false;
        public int IndexOf(T item)
        {
            Guard<int> ind = new Guard<int>();
            if (this.CountBind().Detach(ind).Contains(item))
            {
                return ind.value;
            }
            return -1;
        }
        public void Insert(int index, T item)
        {
            if (index == _first.Count)
            {
                if (_first.Count > _second.Count)
                    insertToSecond(index, item);
                else
                    insertToFirst(index, item);
            }
            else
            {
                if (index > _first.Count)
                    insertToSecond(index, item);
                else
                    insertToFirst(index, item);
            }
        }
        private void insertToFirst(int index, T item)
        {
            _first.Insert(_first.Count-index, item);
        }
        private void insertToSecond(int index, T item)
        {
            _second.Insert(index - _first.Count, item);
        }
        public void InsertRange(int index, IEnumerable<T> items)
        {
            if (index < _first.Count)
            {
                _first.InsertRange(_first.Count - 1 - index, items.Reverse());
            }
            else
            {
                _second.InsertRange(index - _first.Count, items);
            }
        }
        public void TrimExcess()
        {
            _first.TrimExcess();
            _second.TrimExcess();
        }
        public void RemoveAt(int index)
        {
            if (index >= _first.Count)
                _second.RemoveAt(index - _first.Count);
            else
                _first.RemoveAt(_first.Count -1 - index);
        }
        public void AddRange(IEnumerable<T> range)
        {
            _second.AddRange(range);
        }
        public int RemoveAll(Predicate<T> pred)
        {
            return _first.RemoveAll(pred) + _second.RemoveAll(pred);
        }
        public void RemoveRange(int index, int count)
        {
            if (index < _first.Count)
            {
                if (index + count < _first.Count)
                {
                    _first.RemoveRange(index,count);
                    return;
                }
                int part = _first.Count - index;
                _first.RemoveRange(_first.Count - 1 -index,part);
                _second.RemoveRange(0,count-part);
                return;
            }
            _second.RemoveRange(index - _first.Count,count);
        }
        public T this[int index]
        {
            get
            {
                if (index >= _first.Count)
                    return _second[index - _first.Count];
                return _first[_first.Count - 1 - index];
            }
            set
            {
                if (index >= _first.Count)
                    _second[index - _first.Count] = value;
                _first[_first.Count - 1 - index] = value;
            }
        }
    }
}
