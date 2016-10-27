using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public static class positionBind
    {
        private class PositionBoundList<T> : LockedList<Tuple<T,Position>>
        {
            private readonly IList<T> _source;
            public PositionBoundList(IList<T> source)
            {
                _source = source;
            }
            public override IEnumerator<Tuple<T, Position>> GetEnumerator()
            {
                return _source.AsEnumerable().PositionBind().GetEnumerator();
            }
            public override int Count => _source.Count;
            public override Tuple<T, Position> this[int index]
            {
                get
                {
                    Position ret = Position.Middle;
                    if (index == 0)
                        ret |= Position.First;
                    if (index == Count-1)
                        ret |= Position.Last;
                    return Tuple.Create(_source[index], ret);
                }
            }
        }
        [Flags]
        public enum Position { First = 1, Middle = 2, Last = 4, None = 0, Only = First | Middle | Last }
        public static IEnumerable<Tuple<T, Position>> PositionBind<T>(this IEnumerable<T> @this)
        {
            bool first = true;
            var num = @this.GetEnumerator();
            bool last = !num.MoveNext();
            while (!last)
            {
                var v = num.Current;
                last = !num.MoveNext();
                Position ret = Position.Middle;
                if (first)
                {
                    ret |= Position.First;
                    first = false;
                }
                if (last)
                {
                    ret |= Position.Last;
                }
                yield return Tuple.Create(v, ret);
            }
        }
        public static IList<Tuple<T, Position>> PositionBind<T>(this IList<T> @this)
        {
            return new PositionBoundList<T>(@this);
        }
    }
}
