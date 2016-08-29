using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public static class enumerate
    {
        public static LockedList<T> Enumerate<T>(this T b, int count = 1)
        {
            return new EnumerateLockedList<T>(b, count);
        }
        private class EnumerateLockedList<T> : LockedList<T>
        {
            private readonly T _member;
            private readonly int _count;
            public EnumerateLockedList(T member, int count)
            {
                this._member = member;
                _count = count;
            }
            public override IEnumerator<T> GetEnumerator()
            {
                return Enumerable.Select(range.Range(_count), i => _member).GetEnumerator();
            }
            public override int Count => _count;
            public override T this[int index]
            {
                get
                {
                    if (index <0 || index >= _count)
                        throw new ArgumentOutOfRangeException();
                    return _member;
                }
            }
        }
    }
}
