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
        }
    }
}
