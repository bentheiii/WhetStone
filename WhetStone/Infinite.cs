using System;
using System.Collections.Generic;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class infinite
    {
        /// <summary>
        /// Get an infinite <see cref="IList{T}"/>
        /// </summary>
        /// <returns>A read-only <see cref="IList{T}"/> of infinite size.</returns>
        public static IList<positionBind.Position> Infinite()
        {
            return new InfiniteLockedList();
        }
        private class InfiniteLockedList : LockedList<positionBind.Position>
        {
            public override IEnumerator<positionBind.Position> GetEnumerator()
            {
                yield return positionBind.Position.First;
                while (true)
                {
                    yield return positionBind.Position.None;
                }
            }
            public override int Count => int.MaxValue;
            public override positionBind.Position this[int index]
            {
                get
                {
                    if (index < 0)
                        throw new IndexOutOfRangeException();
                    if (index == 0)
                        return positionBind.Position.First;
                    return positionBind.Position.None;
                }
            }
            public override bool Contains(positionBind.Position item)
            {
                return item.HasFlag(positionBind.Position.None) && !item.HasFlag(positionBind.Position.Last);
            }
            public override int IndexOf(positionBind.Position item)
            {
                switch (item)
                {
                    case positionBind.Position.First:
                        return 0;
                    case positionBind.Position.None:
                        return 1;
                    default:
                        return -1;
                }
            }
            public override bool Equals(object obj)
            {
                return obj is InfiniteLockedList;
            }
            public override int GetHashCode()
            {
                return typeof(InfiniteLockedList).GetHashCode();
            }
        }
    }
}
