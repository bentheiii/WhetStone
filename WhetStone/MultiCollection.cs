﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    public class MultiCollection<T> : ICollection<T>
    {
        private readonly IDictionary<T, int> _occurance;
        public MultiCollection(IEqualityComparer<T> comp = null)
        {
            comp = comp ?? EqualityComparer<T>.Default;
            _occurance = new Dictionary<T, int>(comp);
        }
        public IEnumerator<T> GetEnumerator()
        {
            return _occurance.SelectMany(a => a.Key.Enumerate(a.Value)).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void Add(T item, int amount)
        {
            _occurance.EnsureValue(item);
            _occurance[item]+=amount;
            Count += amount;
        }
        public void Add(T item)
        {
            Add(item,1);
        }
        public void Clear()
        {
            _occurance.Clear();
            Count = 0;
        }
        public bool Contains(T item)
        {
            return _occurance.ContainsKey(item);
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var t in this)
            {
                array[arrayIndex++] = t;
            }
        }
        public bool Remove(T item)
        {
            return Remove(item, 1);
        }
        public bool Remove(T item, int amount)
        {
            int oldval;
            if (!_occurance.TryGetValue(item, out oldval))
                return false;
            if (oldval <= amount)
                _occurance.Remove(item);
            else
                _occurance[item]-=amount;
            Count -= amount;
            return true;
        }
        public int Count { get; private set; } = 0;
        public bool IsReadOnly => false;
        public T this[int ind]
        {
            get
            {
                foreach (KeyValuePair<T, int> i in _occurance)
                {
                    if (i.Value > ind)
                        return i.Key;
                    ind -= i.Value;
                }
                throw new IndexOutOfRangeException();
            }
        }
    }
}
