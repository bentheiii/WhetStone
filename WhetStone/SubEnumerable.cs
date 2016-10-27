using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class subEnumerable
    {
        public static IEnumerable<T> SubEnumerable<T>(this IEnumerable<T> @this, int start = 0, int count = -1, int step = 1)
        {
            var ts = @this.AsList(false);
            if (ts != null)
                return count > 0 ? ts.Slice(start, count+start, step) : ts.Slice(start, steps: step);
            var temp = @this.Skip(start).Step(step);
            return count >= 0 ? temp.Take(count) : temp;
        }
    }
}
