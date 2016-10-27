using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    public static class toArray
    {
        public static T[] ToArray<T>(this IEnumerable<T> @this, int capacity, bool limitToCapacity = false)
        {
            T[] ret = new T[capacity <= 0 ? 1 : capacity];
            int i = 0;
            foreach (T t in @this)
            {
                if (ret.Length <= i)
                {
                    if (limitToCapacity)
                        return ret;
                    Array.Resize(ref ret, ret.Length * 2);
                }
                ret[i] = t;
                i++;
            }
            Array.Resize(ref ret, i);
            return ret;
        }
    }
}
