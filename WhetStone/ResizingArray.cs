using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public class ResizingArray<T> : IList<T>, IReadOnlyList<T>
    {
        public ResizingArray(int capacity = 0)
        {
            _arr = new T[capacity];
        }
        public T[] arr
        {
            get
            {
                minimize();
                return _arr;
            }
        }
        public bool Remove(T item)
        {
            var ind = IndexOf(item);
            if (ind == -1)
                return false;
            RemoveAt(ind);
            return true;
        }
        public int Count { get; private set; } = 0;
        public bool IsReadOnly
        {
            get
            {
                return _arr.IsReadOnly;
            }
        }
        private T[] _arr;
        public void minimize()
        {
            Array.Resize(ref _arr, Count);
        }
        public void ResizeTo(int lastindex)
        {
            if (lastindex == 0)
            {
                _arr = new T[0];
                return;
            }
            while (!_arr.IsWithinBounds(lastindex))
                Array.Resize(ref _arr, arr.Length == 0 ? lastindex + 1 : Math.Max(arr.Length * 2, lastindex + 1));
        }
        public void Add(T x)
        {
            ResizeTo(Count + 1);
            _arr[Count++] = x;
        }
        public void AddRange(IEnumerable<T> x)
        {
            int c = x.Count();
            ResizeTo(Count + c);
            foreach (var i in countUp.CountUp(Count).Zip(x))
            {
                _arr[i.Item1] = i.Item2;
            }
            Count += c;
        }
        public void Clear()
        {
            _arr = new T[0];
        }
        public bool Contains(T item)
        {
            return _arr.Contains(item);
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            _arr.CopyTo(array, arrayIndex);
        }
        public IEnumerator<T> GetEnumerator()
        {
            return _arr.Take(Count).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        public int IndexOf(T item)
        {
            return arr.CountBind().FirstOrDefault(a => a.Item1.Equals(item),Tuple.Create(default(T),-1)).Item2;
        }
        public void Insert(int index, T item)
        {
            if (index == Count)
            {
                Add(item);
                return;
            }
            Add(default(T));
            foreach (int i in range.Range(Count - 1, index))
            {
                _arr[i] = _arr[i - 1];
            }
            _arr[index] = item;
        }
        public void RemoveAt(int index)
        {
            foreach (int i in range.Range(index,Count-1))
            {
                _arr[i] = _arr[i + 1];
            }
            Count--;
        }
        public T this[int index]
        {
            get
            {
                if (index >= Count)
                    throw new ArgumentOutOfRangeException();
                return _arr[index];
            }
            set
            {
                if (index >= Count)
                    throw new ArgumentOutOfRangeException();
                _arr[index] = value;
            }
        }
        public static implicit operator T[] (ResizingArray<T> @this)
        {
            Array.Resize(ref @this._arr, @this.Count);
            return @this.arr;
        }
    }
}
