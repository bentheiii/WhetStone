using System;
using System.Collections.Generic;
using WhetStone.Looping;

namespace WhetStone.LockedStructures
{
    internal class LockedListReadOnlyAdaptor<T> : LockedList<T>
    {
        private readonly IReadOnlyList<T> _inner;
        public LockedListReadOnlyAdaptor(IReadOnlyList<T> inner)
        {
            _inner = inner;
        }
        public override IEnumerator<T> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }
        public override int Count
        {
            get
            {
                return _inner.Count;
            }
        }
        public override int IndexOf(T item)
        {
            return _inner.CountBind().FirstOrDefault(a=>a.element.Equals(item),(element: default(T), index: -1)).index;
        }
        public override T this[int index]
        {
            get
            {
                return _inner[index];
            }
        }
    }
}