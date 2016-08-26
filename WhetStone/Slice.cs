using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    //todo steps
    //todo mutability?
    public static class slice
    {
        public static LockedList<T> Slice<T>(this IList<T> @this, int start, int length)
        {
            var s = @this as ListSlice<T>;
            if (s != null)
                return s.ReSlice(start, length);
            return new ListSlice<T>(@this, start, length);
        }
        public static LockedList<T> Slice<T>(this IList<T> @this, int start)
        {
            return Slice(@this, start, @this.Count - start);
        }
        private class ListSlice<T> : LockedList<T>
        {
            private readonly IList<T> _inner;
            private readonly int _start;
            public ListSlice(IList<T> inner, int start, int length)
            {
                _inner = inner;
                _start = start;
                Count = length;
            }
            private IEnumerable<int> IndexRange()
            {
                return range.Range(Count).Select(a => a + _start);
            }
            public override IEnumerator<T> GetEnumerator()
            {
                return IndexRange().Select(i => _inner[i]).GetEnumerator();
            }
            public override int Count { get; }
            public override T this[int index]
            {
                get
                {
                    if (index > Count || index < 0)
                        throw new ArgumentOutOfRangeException();
                    return _inner[index + _start];
                }
            }
            public ListSlice<T> ReSlice(int start, int length)
            {
                return new ListSlice<T>(this._inner, this._start + start, length);
            }
        }
    }
}
