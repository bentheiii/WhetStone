using System;
using System.Collections.Generic;

namespace WhetStone.Arrays
{
    public static class toArray
    {
        public static T[] ToArray<T>(this IEnumerable<T> @this, Action<int> reporter, bool limitToCapacity = false)
        {
            return ToArray(@this, @this.RecommendSize() ?? 0, (arg1, i) => reporter?.Invoke(i), limitToCapacity);
        }
        public static T[] ToArray<T>(this IEnumerable<T> @this, Action<T, int> reporter, bool limitToCapacity = false)
        {
            return ToArray(@this, @this.RecommendSize() ?? 0, reporter, limitToCapacity);
        }
        public static T[] ToArray<T>(this IEnumerable<T> @this, int capacity, bool limitToCapacity = false)
        {
            return ToArray(@this, capacity, (Action<T, int>)null, limitToCapacity);
        }
        public static T[] ToArray<T>(this IEnumerable<T> @this, int capacity, Action<int> reporter, bool limitToCapacity = false)
        {
            return ToArray(@this, capacity, (arg1, i) => reporter?.Invoke(i), limitToCapacity);
        }
        public static T[] ToArray<T>(this IEnumerable<T> @this, int capacity, Action<T, int> reporter, bool limitToCapacity = false)
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
                reporter?.Invoke(t, i);
                i++;
            }
            Array.Resize(ref ret, i);
            return ret;
        }
    }
}
