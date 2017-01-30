using System;
using System.Collections.Generic;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public static class infinite
    {
        public static LockedList<positionBind.Position> Infinite()
        {
            return new InfiniteLockedList();
        }
        private class InfiniteLockedList : LockedList<positionBind.Position>
        {
            public override IEnumerator<positionBind.Position> GetEnumerator()
            {
                yield return positionBind.Position.First | positionBind.Position.Middle;
                while (true)
                {
                    yield return positionBind.Position.Middle;
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
                        return positionBind.Position.First | positionBind.Position.Middle;
                    return positionBind.Position.Middle;
                }
            }
            public override bool Contains(positionBind.Position item)
            {
                return item.HasFlag(positionBind.Position.Middle) && !item.HasFlag(positionBind.Position.Last);
            }
            public override int IndexOf(positionBind.Position item)
            {
                switch (item)
                {
                    case positionBind.Position.First | positionBind.Position.Middle:
                        return 0;
                    case positionBind.Position.Middle:
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
