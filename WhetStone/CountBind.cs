using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    public static class countBind
    {
        public static IEnumerable<Tuple<T, int>> CountBind<T>(this IEnumerable<T> a, int start = 0)
        {
            return a.Zip(countUp.CountUp(start));
        }
        public static IEnumerable<Tuple<T, C>> CountBind<T, C>(this IEnumerable<T> a, C start)
        {
            return a.Zip(countUp.CountUp(start));
        }
    }
}
