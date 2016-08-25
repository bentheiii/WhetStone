using System;
using System.Collections.Generic;
using WhetStone.Arrays;
using WhetStone.Looping;

namespace WhetStone.LockedStructures
{
    public class LockedListReadOnlyAdaptor<T> : LockedList<T>
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
            return _inner.CountBind().FirstOrDefault(a=>a.Item1.Equals(item),Tuple.Create(default(T),-1)).Item2;
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