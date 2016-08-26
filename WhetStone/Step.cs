using System.Collections.Generic;

namespace WhetStone.Looping
{
    public static class step
    {
        //todo slice integration
        public static IEnumerable<T> Step<T>(this IEnumerable<T> @this, int step = 2, int start = 0)
        {
            int c = start;
            foreach (var t in @this)
            {
                if (c == 0)
                    yield return t;
                c++;
                if (c == step)
                    c = 0;
            }
        }
    }
}
