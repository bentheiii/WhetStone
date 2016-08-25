using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    public static class positionBind
    {
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
    }
}
