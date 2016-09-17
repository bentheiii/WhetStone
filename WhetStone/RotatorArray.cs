using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WhetStone.SpecialNumerics;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    public sealed class RotatorArray<T> : IList<T>
    {
        private readonly T[] _items;
        private RollerNum<int> _mIndex;
        public int Index
        {
            get
            {
                return _mIndex.value;
            }
        }
        public int IndexOf(T item)
        {
            return ((IList<T>)_items).IndexOf(item);
        }
        void IList<T>.Insert(int index, T item)
        {
            ((IList<T>)_items).Insert(index, item);
        }
        void IList<T>.RemoveAt(int index)
        {
            ((IList<T>)_items).RemoveAt(index);
        }
        public T this[int i]
        {
            get
            {
                return _items[(_mIndex + i).value];
            }
            set
            {
                _items[(_mIndex + i).value] = value;
            }
        }
        public void Rotate(int val = 1)
        {
            _mIndex += val;
        }
        public static RotatorArray<T> operator ++(RotatorArray<T> a)
        {
            a.Rotate();
            return a;
        }
        public static RotatorArray<T> operator --(RotatorArray<T> a)
        {
            a.Rotate(-1);
            return a;
        }
        public RotatorArray(params T[] switchvalues)
        {
            if (switchvalues?.Length == 0)
                throw new ArgumentException("cannot be empty", nameof(switchvalues));
            this._items = switchvalues.Copy();
            this._mIndex = new RollerNum<int>(0, _items.Length, 0);
        }
        public RotatorArray(int length)
        {
            this._items = new T[length];
            this._mIndex = new RollerNum<int>(0, _items.Length, 0);
        }
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                yield return this[i];
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        void ICollection<T>.Add(T item)
        {
            ((IList<T>)_items).Add(item);
        }
        void ICollection<T>.Clear()
        {
            ((IList<T>)_items).Clear();
        }
        public bool Contains(T item)
        {
            return _items.Contains(item);
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }
        bool ICollection<T>.Remove(T item)
        {
            return ((IList<T>)_items).Remove(item);
        }
        public int Count
        {
            get
            {
                return _items.Length;
            }
        }
        public bool IsReadOnly
        {
            get
            {
                return _items.IsReadOnly;
            }
        }
    }
}
