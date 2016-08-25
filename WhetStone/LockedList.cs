using System;
using System.Collections.Generic;

namespace WhetStone.LockedStructures
{
    public abstract class LockedList<T> : LockedCollection<T>, IList<T>, IReadOnlyList<T>
    {
        public override bool Contains(T item)
        {
            return IndexOf(item)!=-1;
        }
        public virtual int IndexOf(T item)
        {
            int ret = 0;
            foreach (var t in this)
            {
                if (t.Equals(item))
                    return ret;
                ret++;
            }
            return -1;
        }
        public void Insert(int index, T item)
        {
            throw new NotSupportedException();
        }
        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }
        public abstract T this[int index] { get; }
        T IList<T>.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                throw new NotSupportedException();
            }
        }
    }
}