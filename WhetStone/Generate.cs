using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    public static class generate
    {
        public static IEnumerable<T> Generate<T>(Func<T> gen)
        {
            while (true)
            {
                yield return gen();
            }
        }
    }
}
