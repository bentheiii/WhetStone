using System.Collections.Generic;

namespace WhetStone.Looping
{
    public static class enumerate
    {
        public static IEnumerable<T> Enumerate<T>(this T b)
        {
            yield return b;
        }
    }
}
