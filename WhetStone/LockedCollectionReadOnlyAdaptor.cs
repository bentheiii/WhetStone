using System.Collections.Generic;
using System.Linq;

namespace WhetStone.LockedStructures
{
    public class LockedCollectionReadOnlyAdaptor<T> : LockedCollection<T>
    {
        private readonly IReadOnlyCollection<T> _inner;
        public LockedCollectionReadOnlyAdaptor(IReadOnlyCollection<T> inner)
        {
            _inner = inner;
        }
        public override IEnumerator<T> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }
        public override bool Contains(T item)
        {
            return _inner.Contains(item);
        }
        public override int Count
        {
            get
            {
                return _inner.Count;
            }
        }
    }
}