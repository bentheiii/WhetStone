using System;
using System.Collections;
using System.Collections.Generic;
using WhetStone.Arrays;

namespace WhetStone.Tuples
{
    public static class Tuples
    {
        public static Tuple<T1, T2> Merge<T1, T2>(this Tuple<T1> t1, Tuple<T2> t2)
        {
            return Tuple.Create(t1.Item1, t2.Item1);
        }
        public static Tuple<T1, T2, T3> Merge<T1, T2, T3>(this Tuple<T1, T2> t1, Tuple<T3> t2)
        {
            return Tuple.Create(t1.Item1, t1.Item2, t2.Item1);
        }
        public static Tuple<T1, T2, T3> Merge<T1, T2, T3>(this Tuple<T1> t1, Tuple<T2, T3> t2)
        {
            return Tuple.Create(t1.Item1, t2.Item1, t2.Item2);
        }
        public static Tuple<T1, T2, T3> Merge<T1, T2, T3>(this Tuple<T1> t1, Tuple<T2> t2, Tuple<T3> t3)
        {
            return Tuple.Create(t1.Item1, t2.Item1, t3.Item1);
        }
        public static Tuple<T1, T2, T3, T4> Merge<T1, T2, T3, T4>(this Tuple<T1, T2, T3> t1, Tuple<T4> t2)
        {
            return Tuple.Create(t1.Item1, t1.Item2, t1.Item3, t2.Item1);
        }
        public static Tuple<T1, T2, T3, T4> Merge<T1, T2, T3, T4>(this Tuple<T1, T2> t1, Tuple<T3, T4> t2)
        {
            return Tuple.Create(t1.Item1, t1.Item2, t2.Item1, t2.Item2);
        }
        public static Tuple<T1, T2, T3, T4> Merge<T1, T2, T3, T4>(this Tuple<T1, T2> t1, Tuple<T3> t2, Tuple<T4> t3)
        {
            return Tuple.Create(t1.Item1, t1.Item2, t2.Item1, t3.Item1);
        }
        public static Tuple<T1, T2, T3, T4> Merge<T1, T2, T3, T4>(this Tuple<T1> t1, Tuple<T2, T3, T4> t2)
        {
            return Tuple.Create(t1.Item1, t2.Item1, t2.Item2, t2.Item3);
        }
        public static Tuple<T1, T2, T3, T4> Merge<T1, T2, T3, T4>(this Tuple<T1> t1, Tuple<T2, T3> t2, Tuple<T4> t3)
        {
            return Tuple.Create(t1.Item1, t2.Item1, t2.Item2, t3.Item1);
        }
        public static Tuple<T1, T2, T3, T4> Merge<T1, T2, T3, T4>(this Tuple<T1> t1, Tuple<T2> t2, Tuple<T3, T4> t3)
        {
            return Tuple.Create(t1.Item1, t2.Item1, t3.Item1, t3.Item2);
        }
        public static Tuple<T1, T2, T3, T4> Merge<T1, T2, T3, T4>(this Tuple<T1> t1, Tuple<T2> t2, Tuple<T3> t3, Tuple<T4> t4)
        {
            return Tuple.Create(t1.Item1, t2.Item1, t3.Item1, t4.Item1);
        }
        public static Tuple<T2, T1> FlipTuple<T1, T2>(this Tuple<T1, T2> @this)
        {
            return Tuple.Create(@this.Item2, @this.Item1);
        }
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
        public static T[] ToArray<T>(this Tuple<T> @this)
        {
            return new[] {@this.Item1};
        }
        public static T[] ToArray<T>(this Tuple<T,T> @this)
        {
            return new[] { @this.Item1, @this.Item2 };
        }
        public static T[] ToArray<T>(this Tuple<T, T, T> @this)
        {
            return new[] { @this.Item1, @this.Item2, @this.Item3 };
        }
        public static T[] ToArray<T>(this Tuple<T, T, T, T> @this)
        {
            return new[] { @this.Item1, @this.Item2, @this.Item3, @this.Item4 };
        }
        public static T[] ToArray<T>(this Tuple<T, T, T, T, T> @this)
        {
            return new[] { @this.Item1, @this.Item2, @this.Item3, @this.Item4, @this.Item5};
        }
        public static IEnumerable<T> ToEnumerable<T>(this Tuple<T> @this)
        {
            yield return @this.Item1;
        }
        public static IEnumerable<T> ToEnumerable<T>(this Tuple<T,T> @this)
        {
            yield return @this.Item1;
            yield return @this.Item2;
        }
        public static IEnumerable<T> ToEnumerable<T>(this Tuple<T, T, T> @this)
        {
            yield return @this.Item1;
            yield return @this.Item2;
            yield return @this.Item3;
        }
        public static IEnumerable<T> ToEnumerable<T>(this Tuple<T, T, T, T> @this)
        {
            yield return @this.Item1;
            yield return @this.Item2;
            yield return @this.Item3;
            yield return @this.Item4;
        }
        public static IEnumerable<T> ToEnumerable<T>(this Tuple<T, T, T, T, T> @this)
        {
            yield return @this.Item1;
            yield return @this.Item2;
            yield return @this.Item3;
            yield return @this.Item4;
            yield return @this.Item5;
        }
        public static Tuple<T1> ToTuple<T1>(this IEnumerable @this)
        {
            return @this.toObjArray().ToTuple<T1>();
        }
        public static Tuple<T1, T2> ToTuple<T1, T2>(this IEnumerable @this)
        {
            return @this.toObjArray().ToTuple<T1, T2>();
        }
        public static Tuple<T1, T2, T3> ToTuple<T1, T2, T3>(this IEnumerable @this)
        {
            return @this.toObjArray().ToTuple<T1, T2, T3>();
        }
        public static Tuple<T1, T2, T3, T4> ToTuple<T1, T2, T3, T4>(this IEnumerable @this)
        {
            return @this.toObjArray().ToTuple<T1, T2, T3, T4>();
        }
        public static Tuple<T1, T2, T3, T4, T5> ToTuple<T1, T2, T3, T4, T5>(this IEnumerable @this)
        {
            return @this.toObjArray().ToTuple<T1, T2, T3, T4, T5>();
        }
    }
}
