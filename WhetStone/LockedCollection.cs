using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WhetStone.LockedStructures
{
    public abstract class LockedCollection<T> : ICollection<T>, IReadOnlyCollection<T>
    {
        public abstract IEnumerator<T> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void Add(T item)
        {
            throw new NotSupportedException();
        }
        public void Clear()
        {
            throw new NotSupportedException();
        }
        public virtual bool Contains(T item)
        {
            return this.Any(a => a.Equals(item));
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
            throw new NotSupportedException();
        }
        public abstract int Count { get; }
        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }
    }
}