using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Arrays;

namespace WhetStone.Tuples
{
    public static class toTuple
    {
        public static Tuple<T1> ToTuple<T1>(this object[] @this)
        {
            return new Tuple<T1>((T1)@this[0]);
        }
        public static Tuple<T1, T2> ToTuple<T1, T2>(this object[] @this)
        {
            return new Tuple<T1, T2>((T1)@this[0], (T2)@this[1]);
        }
        public static Tuple<T1, T2, T3> ToTuple<T1, T2, T3>(this object[] @this)
        {
            return new Tuple<T1, T2, T3>((T1)@this[0], (T2)@this[1], (T3)@this[2]);
        }
        public static Tuple<T1, T2, T3, T4> ToTuple<T1, T2, T3, T4>(this object[] @this)
        {
            return new Tuple<T1, T2, T3, T4>((T1)@this[0], (T2)@this[1], (T3)@this[2], (T4)@this[3]);
        }
        public static Tuple<T1, T2, T3, T4, T5> ToTuple<T1, T2, T3, T4, T5>(this object[] @this)
        {
            return new Tuple<T1, T2, T3, T4, T5>((T1)@this[0], (T2)@this[1], (T3)@this[2], (T4)@this[3], (T5)@this[4]);
        }
        public static Tuple<T> ToTuple1<T>(this T[] @this)
        {
            return Tuple.Create(@this[0]);
        }
        public static Tuple<T, T> ToTuple2<T>(this T[] @this)
        {
            return Tuple.Create(@this[0], @this[1]);
        }
        public static Tuple<T, T, T> ToTuple3<T>(this T[] @this)
        {
            return Tuple.Create(@this[0], @this[1], @this[2]);
        }
        public static Tuple<T, T, T, T> ToTuple4<T>(this T[] @this)
        {
            return Tuple.Create(@this[0], @this[1], @this[2], @this[3]);
        }
        public static Tuple<T, T, T, T, T> ToTuple5<T>(this T[] @this)
        {
            return Tuple.Create(@this[0], @this[1], @this[2], @this[3], @this[4]);
        }
        public static Tuple<T1> ToTuple<T1>(this IEnumerable @this)
        {
            return @this.ToObjArray().ToTuple<T1>();
        }
        public static Tuple<T1, T2> ToTuple<T1, T2>(this IEnumerable @this)
        {
            return @this.ToObjArray().ToTuple<T1, T2>();
        }
        public static Tuple<T1, T2, T3> ToTuple<T1, T2, T3>(this IEnumerable @this)
        {
            return @this.ToObjArray().ToTuple<T1, T2, T3>();
        }
        public static Tuple<T1, T2, T3, T4> ToTuple<T1, T2, T3, T4>(this IEnumerable @this)
        {
            return @this.ToObjArray().ToTuple<T1, T2, T3, T4>();
        }
        public static Tuple<T1, T2, T3, T4, T5> ToTuple<T1, T2, T3, T4, T5>(this IEnumerable @this)
        {
            return @this.ToObjArray().ToTuple<T1, T2, T3, T4, T5>();
        }
    }
}
