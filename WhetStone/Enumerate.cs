using System;
using System.Collections.Generic;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class enumerate
    {
        /// <summary>
        /// Creates a new <see cref="IList{T}"/>, including only <paramref name="b"/> as an element.
        /// </summary>
        /// <typeparam name="T">The type of the list to return</typeparam>
        /// <param name="b">The element to make the <see cref="IList{T}"/> out of.</param>
        /// <param name="count">How many elements the <see cref="IList{T}"/> should contain.</param>
        /// <returns>An <see cref="IList{T}"/> that includes only <paramref name="b"/>, <paramref name="count"/> times.</returns>
        public static IList<T> Enumerate<T>(this T b, int count = 1)
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
                return range.Range(_count).Select(i => _member).GetEnumerator();
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
