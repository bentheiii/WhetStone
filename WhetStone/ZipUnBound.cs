using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public static class zipUnBound
    {
        public static IEnumerable<IList<T>> ZipUnBound<T>(this IEnumerable<IEnumerable<T>> @this, T nilValue)
        {
            IEnumerator<T>[] tors = @this.Select(a => a.GetEnumerator()).ToArray();

            try
            {
                while (true)
                {
                    var ret = new T[tors.Length];
                    bool any = false;
                    foreach (int i in tors.Indices())
                    {
                        if (tors[i] == null)
                        {
                            ret[i] = nilValue;
                            continue;
                        }
                        if (!tors[i].MoveNext())
                        {
                            tors[i].Dispose();
                            tors[i] = null;
                            ret[i] = nilValue;
                        }
                        else
                        {
                            ret[i] = tors[i].Current;
                            any = true;
                        }
                    }
                    if (any)
                        yield return ret;
                    else
                        yield break;
                }
            }
            finally
            {
                tors.Where(a=>a!=null).Do(a=>a.Dispose());
            }

        }
        public static IEnumerable<IList<T?>> ZipUnBoundNullable<T>(this IEnumerable<IEnumerable<T>> @this) where T:struct
        {
            return @this.Select(a => a.Select(x => (T?)x)).ZipUnBound(null);
        }
        public static IEnumerable<IList<Tuple<T>>> ZipUnBoundTuple<T>(this IEnumerable<IEnumerable<T>> @this)
        {
            return @this.Select(a => a.Select(x => Tuple.Create(x))).ZipUnBound(null);
        }
        public static IEnumerable<IList<Tuple<object>>> ZipUnBound(this IEnumerable<IEnumerable<object>> @this)
        {
            return @this.Select(a => a.Select(x => Tuple.Create(x))).ZipUnBound(null);
        }
        public static IEnumerable<Tuple<T1, T2>> ZipUnBound<T1, T2>(this IEnumerable<T1> @this, IEnumerable<T2> other, T1 nilValue1, T2 nilValue2)
        {
            return new[] {@this.Select(a => (object)a), other.Select(a => (object)a) }.ZipUnBound().Select(a =>
            {
                T1 t1 = nilValue1;
                if (a[0] != null)
                    t1 = (T1)a[0].Item1;

                T2 t2 = nilValue2;
                if (a[1] != null)
                    t2 = (T2)a[1].Item1;

                return Tuple.Create(t1, t2);
            });
        }
        public static IEnumerable<Tuple<T1?, T2?>> ZipUnBoundNullable<T1, T2>(this IEnumerable<T1> @this, IEnumerable<T2> other) where T1 :struct where T2 : struct
        {
            return @this.Select(a => (T1?)a).ZipUnBound(other.Select(a => (T2?)a), null, null);
        }
        public static IEnumerable<Tuple<Tuple<T1>, Tuple<T2>>> ZipUnBoundTuple<T1, T2>(this IEnumerable<T1> @this, IEnumerable<T2> other)
        {
            return @this.Select(a => Tuple.Create(a)).ZipUnBound(other.Select(a => Tuple.Create(a)), null, null);
        }
        public static IEnumerable<Tuple<T1, T2, T3>> ZipUnBound<T1, T2, T3>(this IEnumerable<T1> @this, IEnumerable<T2> other, IEnumerable<T3> other2, T1 nilValue1, T2 nilValue2, T3 nilValue3)
        {
            return new[] { @this.Select(a => (object)a), other.Select(a => (object)a), other2.Select(a => (object)a) }.ZipUnBound().Select(a =>
            {
                T1 t1 = nilValue1;
                if (a[0] != null)
                    t1 = (T1)a[0].Item1;

                T2 t2 = nilValue2;
                if (a[1] != null)
                    t2 = (T2)a[1].Item1;

                T3 t3 = nilValue3;
                if (a[2] != null)
                    t3 = (T3)a[2].Item1;

                return Tuple.Create(t1, t2, t3);
            });
        }
        public static IEnumerable<Tuple<T1?, T2?, T3?>> ZipUnBoundNullable<T1, T2, T3>(this IEnumerable<T1> @this, IEnumerable<T2> other, IEnumerable<T3> other2) where T1 : struct where T2 : struct where T3:struct
        {
            return @this.Select(a => (T1?)a).ZipUnBound(other.Select(a => (T2?)a), other2.Select(a=>(T3?)a), null, null, null);
        }
        public static IEnumerable<Tuple<Tuple<T1>, Tuple<T2>, Tuple<T3>>> ZipUnBoundTuple<T1, T2, T3>(this IEnumerable<T1> @this, IEnumerable<T2> other, IEnumerable<T3> other2)
        {
            return @this.Select(a => Tuple.Create(a)).ZipUnBound(other.Select(a => Tuple.Create(a)), other2.Select(a=>Tuple.Create(a)),null, null, null);
        }

        private class ZipUnBoundNilled<T>:LockedList<IList<T>>
        {
            private readonly IEnumerable<IList<T>> _src;
            private readonly T _nilValue;
            public ZipUnBoundNilled(IEnumerable<IList<T>> src, T nilValue)
            {
                _src = src;
                _nilValue = nilValue;
            }
            public override IEnumerator<IList<T>> GetEnumerator()
            {
                return _src.Select(a=>a.AsEnumerable()).ZipUnBound(_nilValue).GetEnumerator();
            }
            public override int Count
            {
                get
                {
                    return _src.Min(a => a.Count);
                }
            }
            public override IList<T> this[int index]
            {
                get
                {
                    return _src.Select(l => l.IsWithinBounds(index) ? l[index] : _nilValue).ToList();
                }
            }
        }

        public static IList<IList<T>> ZipUnBound<T>(this IEnumerable<IList<T>> @this, T nilValue)
        {
            return new ZipUnBoundNilled<T>(@this, nilValue);
        }
        public static IList<IList<T?>> ZipUnBoundNullable<T>(this IEnumerable<IList<T>> @this) where T : struct
        {
            return @this.Select(a => a.Select(x => (T?)x)).ZipUnBound(null);
        }
        public static IList<IList<Tuple<T>>> ZipUnBoundTuple<T>(this IEnumerable<IList<T>> @this)
        {
            return @this.Select(a => a.Select(x => Tuple.Create(x))).ZipUnBound(null);
        }
        public static IList<IList<Tuple<object>>> ZipUnBound(this IEnumerable<IList<object>> @this)
        {
            return @this.Select(a => a.Select(Tuple.Create)).ZipUnBound(null);
        }
        public static IList<Tuple<T1, T2>> ZipUnBound<T1, T2>(this IList<T1> @this, IList<T2> other, T1 nilValue1, T2 nilValue2)
        {
            return new[] { @this.Select(a => (object)a), other.Select(a => (object)a) }.ZipUnBound().Select(a =>
            {
                T1 t1 = nilValue1;
                if (a[0] != null)
                    t1 = (T1)a[0].Item1;

                T2 t2 = nilValue2;
                if (a[1] != null)
                    t2 = (T2)a[1].Item1;

                return Tuple.Create(t1, t2);
            });
        }
        public static IList<Tuple<T1?, T2?>> ZipUnBoundNullable<T1, T2>(this IList<T1> @this, IList<T2> other) where T1 : struct where T2 : struct
        {
            return @this.Select(a => (T1?)a).ZipUnBound(other.Select(a => (T2?)a), null, null);
        }
        public static IList<Tuple<Tuple<T1>, Tuple<T2>>> ZipUnBoundTuple<T1, T2>(this IList<T1> @this, IList<T2> other)
        {
            return @this.Select(a => Tuple.Create(a)).ZipUnBound(other.Select(a => Tuple.Create(a)), null, null);
        }
        public static IList<Tuple<T1, T2, T3>> ZipUnBound<T1, T2, T3>(this IList<T1> @this, IList<T2> other, IList<T3> other2, T1 nilValue1, T2 nilValue2, T3 nilValue3)
        {
            return new[] { @this.Select(a=>(object)a), other.Select(a => (object)a), other2.Select(a => (object)a) }.ZipUnBound().Select(a =>
            {
                T1 t1 = nilValue1;
                if (a[0] != null)
                    t1 = (T1)a[0].Item1;

                T2 t2 = nilValue2;
                if (a[1] != null)
                    t2 = (T2)a[1].Item1;

                T3 t3 = nilValue3;
                if (a[2] != null)
                    t3 = (T3)a[2].Item1;

                return Tuple.Create(t1, t2, t3);
            });
        }
        public static IList<Tuple<T1?, T2?, T3?>> ZipUnBoundNullable<T1, T2, T3>(this IList<T1> @this, IList<T2> other, IList<T3> other2) where T1 : struct where T2 : struct where T3 : struct
        {
            return @this.Select(a => (T1?)a).ZipUnBound(other.Select(a => (T2?)a), other2.Select(a => (T3?)a), null, null, null);
        }
        public static IList<Tuple<Tuple<T1>, Tuple<T2>, Tuple<T3>>> ZipUnBoundTuple<T1, T2, T3>(this IList<T1> @this, IList<T2> other, IList<T3> other2)
        {
            return @this.Select(a => Tuple.Create(a)).ZipUnBound(other.Select(a => Tuple.Create(a)), other2.Select(a => Tuple.Create(a)), null, null, null);
        }
    }
}