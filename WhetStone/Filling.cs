using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    public static class fill
    {
        public static void Fill<T>(this IList<T> tofill, params T[] v)
        {
            tofill.Fill(v, 0);
        }
        public static void Fill<T>(this IList<T> tofill, T[] v, int start, int count = int.MaxValue)
        {
            if (v.Length == 0)
                throw new ArgumentException("cannot be empty", nameof(v));
            Fill(tofill, i => (v[i % v.Length]), start, count);
        }
        public static void Fill<T>(this IList<T> tofill, Func<int, T> filler, int start = 0, int count = int.MaxValue)
        {
            for (int i = 0; i < tofill.Count - start && i < count; i++)
            {
                if (i + start < 0)
                    continue;
                tofill[i + start] = filler(i + start);
            }
        }
        public static void Fill<T>(this IList<T> tofill, Func<T> filler, int start = 0, int count = int.MaxValue)
        {
            tofill.Fill(a => filler(), start, count);
        }
        public static T[] Fill<T>(int length, T[] filler, int start, int count = int.MaxValue)
        {
            T[] ret = new T[length];
            ret.Fill(filler, start, count);
            return ret;
        }
        public static T[] Fill<T>(int length, params T[] filler)
        {
            T[] ret = new T[length];
            ret.Fill(filler);
            return ret;
        }
        public static T[] Fill<T>(int length, Func<int, T> filler, int start = 0, int count = int.MaxValue)
        {
            T[] ret = new T[length];
            ret.Fill(filler, start, count);
            return ret;
        }
        public static T[] Fill<T>(int length, Func<T> filler, int start = 0, int count = int.MaxValue)
        {
            T[] ret = new T[length];
            ret.Fill(a => filler(), start, count);
            return ret;
        }
    }
}
