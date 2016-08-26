using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public static class subEnumerable
    {
        public static IEnumerable<T> SubEnumerable<T>(this IEnumerable<T> @this, int start = 0, int count = -1, int step = 1)
        {
            if (step == 1)
            {
                var ts = @this as IList<T>;
                ts = ts ?? (@this as IReadOnlyList<T>)?.ToLockedList();
                if (ts != null)
                    return count > 0 ? ts.Slice(start, count) : ts.Slice(start);
            }
            var temp = @this.Skip(start).Step(step);
            return count >= 0 ? temp.Take(count) : temp;
        }
    }
}
