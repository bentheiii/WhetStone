using System.Collections.Generic;
using System.Linq;

namespace WhetStone.LockedStructures
{
    public class EnumerableCountCache<T> : LockedCollection<T>
    {
        private readonly IEnumerable<T> _inner;
        private int? _count;
        public EnumerableCountCache(IEnumerable<T> inner)
        {
            _inner = inner;
            var col = inner as ICollection<T>;
            if (col != null)
                _count = col.Count;
            else
            {
                var rol = inner as IReadOnlyCollection<T>;
                if (rol != null)
                    _count = rol.Count;
                else
                {
                    _count = null;
                }
            }
        }
        public override IEnumerator<T> GetEnumerator()
        {
            int c = 0;
            foreach (var t in _inner)
            {
                yield return t;
                c++;
            }
            _count = c;

        }
        public override bool Contains(T item)
        {
            return _inner.Contains(item);
        }
        public override int Count
        {
            get
            {
                if (_count == null)
                    _count = _inner.Count();
                return _count.Value;
            }
        }
    }
}