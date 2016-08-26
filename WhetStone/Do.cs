using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    public static class @do
    {
        public static void Do<T>(this IEnumerable<T> @this, Action<T> action = null)
        {
            foreach (T t in @this)
            {
                action?.Invoke(t);
            }
        }
    }
}
