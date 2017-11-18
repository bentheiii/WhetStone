using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class zipUnBound
    {
        /// <overloads>Get all the elements in enumerables spliced together, continuing until they all end</overloads>
        /// <summary>
        /// Get all the elements in <see cref="IEnumerable{T}"/>s spliced together, continuing until they all end.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/>s to zip.</param>
        /// <param name="nilValue">The default value to assign when an <see cref="IEnumerable{T}"/> has ended.</param>
        /// <returns><paramref name="this"/> transposed, filling ended <see cref="IEnumerable{T}"/>s with <paramref name="nilValue"/>.</returns>
        public static IEnumerable<IList<T>> ZipUnBound<T>(this IEnumerable<IEnumerable<T>> @this, T nilValue)
        {
            @this.ThrowIfNull(nameof(@this));
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
        /// <summary>
        /// Get all the elements in <see cref="IEnumerable{T}"/>s spliced together, continuing until they all end.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/>s to zip.</param>
        /// <returns><paramref name="this"/> transposed, filling ended <see cref="IEnumerable{T}"/>s with <see langword="null"/>.</returns>
        public static IEnumerable<IList<T?>> ZipUnBoundNullable<T>(this IEnumerable<IEnumerable<T>> @this) where T:struct
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.Select(a => a.Select(x => (T?)x)).ZipUnBound(null);
        }
        /// <summary>
        /// Get all the elements in <see cref="IEnumerable{T}"/>s spliced together, continuing until they all end.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/>s to zip.</param>
        /// <returns><paramref name="this"/> transposed, filling ended <see cref="IEnumerable{T}"/>s with <see langword="null"/>.</returns>
        public static IEnumerable<IList<Tuple<T>>> ZipUnBoundTuple<T>(this IEnumerable<IEnumerable<T>> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.Select(a => a.Select(x => Tuple.Create(x))).ZipUnBound(null);
        }
        /// <summary>
        /// Get all the elements in <see cref="IEnumerable{T}"/>s spliced together, continuing until they all end.
        /// </summary>
        /// <param name="this">The <see cref="IEnumerable{T}"/>s to zip.</param>
        /// <returns><paramref name="this"/> transposed, filling ended <see cref="IEnumerable{T}"/>s with <see langword="null"/>.</returns>
        public static IEnumerable<IList<Tuple<object>>> ZipUnBound(this IEnumerable<IEnumerable<object>> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.Select(a => a.Select(x => Tuple.Create(x))).ZipUnBound(null);
        }
        /// <summary>
        /// Get all the elements in <see cref="IEnumerable{T}"/>s spliced together, continuing until they all end.
        /// </summary>
        /// <typeparam name="T1">The type of the first <see cref="IEnumerable{T}"/>.</typeparam>
        /// <typeparam name="T2">The type of the second <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The first <see cref="IEnumerable{T}"/>.</param>
        /// <param name="other">The second <see cref="IEnumerable{T}"/>.</param>
        /// <param name="nilValue1">The default value to assign when <paramref name="this"/> has ended.</param>
        /// <param name="nilValue2">The default value to assign when <paramref name="other"/> has ended.</param>
        /// <returns><paramref name="this"/> transposed, filling ended <see cref="IEnumerable{T}"/>s with <paramref name="nilValue1"/> or <paramref name="nilValue2"/>.</returns>
        public static IEnumerable<(T1, T2)> ZipUnBound<T1, T2>(this IEnumerable<T1> @this, IEnumerable<T2> other, T1 nilValue1, T2 nilValue2)
        {
            @this.ThrowIfNull(nameof(@this));
            other.ThrowIfNull(nameof(other));
            return new[] {@this.Select(a => (object)a), other.Select(a => (object)a) }.ZipUnBound().Select(a =>
            {
                T1 t1 = nilValue1;
                if (a[0] != null)
                    t1 = (T1)a[0].Item1;

                T2 t2 = nilValue2;
                if (a[1] != null)
                    t2 = (T2)a[1].Item1;

                return (t1, t2);
            });
        }
        /// <summary>
        /// Get all the elements in <see cref="IEnumerable{T}"/>s spliced together, continuing until they all end.
        /// </summary>
        /// <typeparam name="T1">The type of the first <see cref="IEnumerable{T}"/>.</typeparam>
        /// <typeparam name="T2">The type of the second <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The first <see cref="IEnumerable{T}"/>.</param>
        /// <param name="other">The second <see cref="IEnumerable{T}"/>.</param>
        /// <returns><paramref name="this"/> transposed, filling ended <see cref="IEnumerable{T}"/>s with <see langword="null"/>.</returns>
        public static IEnumerable<(T1?, T2?)> ZipUnBoundNullable<T1, T2>(this IEnumerable<T1> @this, IEnumerable<T2> other) where T1 :struct where T2 : struct
        {
            @this.ThrowIfNull(nameof(@this));
            other.ThrowIfNull(nameof(other));
            return @this.Select(a => (T1?)a).ZipUnBound(other.Select(a => (T2?)a), null, null);
        }
        /// <summary>
        /// Get all the elements in <see cref="IEnumerable{T}"/>s spliced together, continuing until they all end.
        /// </summary>
        /// <typeparam name="T1">The type of the first <see cref="IEnumerable{T}"/>.</typeparam>
        /// <typeparam name="T2">The type of the second <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The first <see cref="IEnumerable{T}"/>.</param>
        /// <param name="other">The second <see cref="IEnumerable{T}"/>.</param>
        /// <returns><paramref name="this"/> transposed, filling ended <see cref="IEnumerable{T}"/>s with <see langword="null"/>.</returns>
        public static IEnumerable<(Tuple<T1>, Tuple<T2>)> ZipUnBoundTuple<T1, T2>(this IEnumerable<T1> @this, IEnumerable<T2> other)
        {
            @this.ThrowIfNull(nameof(@this));
            other.ThrowIfNull(nameof(other));
            return @this.Select(a => Tuple.Create(a)).ZipUnBound(other.Select(a => Tuple.Create(a)), null, null);
        }
        /// <summary>
        /// Get all the elements in <see cref="IEnumerable{T}"/>s spliced together, continuing until they all end.
        /// </summary>
        /// <typeparam name="T1">The type of the first <see cref="IEnumerable{T}"/>.</typeparam>
        /// <typeparam name="T2">The type of the second <see cref="IEnumerable{T}"/>.</typeparam>
        /// <typeparam name="T3">The type of the third <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The first <see cref="IEnumerable{T}"/>.</param>
        /// <param name="other">The second <see cref="IEnumerable{T}"/>.</param>
        /// <param name="other2">The third <see cref="IEnumerable{T}"/>.</param>
        /// <param name="nilValue1">The default value to assign when <paramref name="this"/> has ended.</param>
        /// <param name="nilValue2">The default value to assign when <paramref name="other"/> has ended.</param>
        /// <param name="nilValue3">The default value to assign when <paramref name="other2"/> has ended.</param>
        /// <returns><paramref name="this"/> transposed, filling ended <see cref="IEnumerable{T}"/>s with <paramref name="nilValue1"/>, <paramref name="nilValue2"/> or <paramref name="nilValue3"/>.</returns>
        public static IEnumerable<(T1, T2, T3)> ZipUnBound<T1, T2, T3>(this IEnumerable<T1> @this, IEnumerable<T2> other, IEnumerable<T3> other2, T1 nilValue1, T2 nilValue2, T3 nilValue3)
        {
            @this.ThrowIfNull(nameof(@this));
            other.ThrowIfNull(nameof(other));
            other2.ThrowIfNull(nameof(other2));
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

                return (t1, t2, t3);
            });
        }
        /// <summary>
        /// Get all the elements in <see cref="IEnumerable{T}"/>s spliced together, continuing until they all end.
        /// </summary>
        /// <typeparam name="T1">The type of the first <see cref="IEnumerable{T}"/>.</typeparam>
        /// <typeparam name="T2">The type of the second <see cref="IEnumerable{T}"/>.</typeparam>
        /// <typeparam name="T3">The type of the third <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The first <see cref="IEnumerable{T}"/>.</param>
        /// <param name="other">The second <see cref="IEnumerable{T}"/>.</param>
        /// <param name="other2">The third <see cref="IEnumerable{T}"/>.</param>
        /// <returns><paramref name="this"/> transposed, filling ended <see cref="IEnumerable{T}"/>s with null.</returns>
        public static IEnumerable<(T1?, T2?, T3?)> ZipUnBoundNullable<T1, T2, T3>(this IEnumerable<T1> @this, IEnumerable<T2> other, IEnumerable<T3> other2) where T1 : struct where T2 : struct where T3:struct
        {
            @this.ThrowIfNull(nameof(@this));
            other.ThrowIfNull(nameof(other));
            other2.ThrowIfNull(nameof(other2));
            return @this.Select(a => (T1?)a).ZipUnBound(other.Select(a => (T2?)a), other2.Select(a=>(T3?)a), null, null, null);
        }
        /// <summary>
        /// Get all the elements in <see cref="IEnumerable{T}"/>s spliced together, continuing until they all end.
        /// </summary>
        /// <typeparam name="T1">The type of the first <see cref="IEnumerable{T}"/>.</typeparam>
        /// <typeparam name="T2">The type of the second <see cref="IEnumerable{T}"/>.</typeparam>
        /// <typeparam name="T3">The type of the third <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="this">The first <see cref="IEnumerable{T}"/>.</param>
        /// <param name="other">The second <see cref="IEnumerable{T}"/>.</param>
        /// <param name="other2">The third <see cref="IEnumerable{T}"/>.</param>
        /// <returns><paramref name="this"/> transposed, filling ended <see cref="IEnumerable{T}"/>s with null.</returns>
        public static IEnumerable<(Tuple<T1>, Tuple<T2>, Tuple<T3>)> ZipUnBoundTuple<T1, T2, T3>(this IEnumerable<T1> @this, IEnumerable<T2> other, IEnumerable<T3> other2)
        {
            @this.ThrowIfNull(nameof(@this));
            other.ThrowIfNull(nameof(other));
            other2.ThrowIfNull(nameof(other2));
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
                    return _src.Max(a => a.Count);
                }
            }
            public override IList<T> this[int index]
            {
                get
                {
                    if (index < 0)
                        throw new IndexOutOfRangeException();
                    var ret = new List<T>();
                    bool any = false;
                    foreach (var l in _src)
                    {
                        if (index < l.Count)
                        {
                            any = true;
                            ret.Add(l[index]);
                        }
                        else
                            ret.Add(_nilValue);
                    }
                    if (!any)
                        throw new IndexOutOfRangeException();
                    return ret;
                }
            }
        }
        /// <overloads>Get all the elements in enumerables spliced together, continuing until they all end</overloads>
        /// <summary>
        /// Get all the elements in <see cref="IList{T}"/>s spliced together, continuing until they all end.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/>s to zip.</param>
        /// <param name="nilValue">The default value to assign when an <see cref="IList{T}"/> has ended.</param>
        /// <returns><paramref name="this"/> transposed, filling ended <see cref="IList{T}"/>s with <paramref name="nilValue"/>.</returns>
        public static IList<IList<T>> ZipUnBound<T>(this IEnumerable<IList<T>> @this, T nilValue)
        {
            @this.ThrowIfNull(nameof(@this));
            return new ZipUnBoundNilled<T>(@this, nilValue);
        }
        /// <summary>
        /// Get all the elements in <see cref="IList{T}"/>s spliced together, continuing until they all end.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/>s to zip.</param>
        /// <returns><paramref name="this"/> transposed, filling ended <see cref="IList{T}"/>s with <see langword="null"/>.</returns>
        public static IList<IList<T?>> ZipUnBoundNullable<T>(this IEnumerable<IList<T>> @this) where T : struct
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.Select(a => a.Select(x => (T?)x)).ZipUnBound(null);
        }
        /// <summary>
        /// Get all the elements in <see cref="IList{T}"/>s spliced together, continuing until they all end.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The <see cref="IList{T}"/>s to zip.</param>
        /// <returns><paramref name="this"/> transposed, filling ended <see cref="IList{T}"/>s with <see langword="null"/>.</returns>
        public static IList<IList<Tuple<T>>> ZipUnBoundTuple<T>(this IEnumerable<IList<T>> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.Select(a => a.Select(Tuple.Create)).ZipUnBound(null);
        }
        /// <summary>
        /// Get all the elements in <see cref="IList{T}"/>s spliced together, continuing until they all end.
        /// </summary>
        /// <param name="this">The <see cref="IList{T}"/>s to zip.</param>
        /// <returns><paramref name="this"/> transposed, filling ended <see cref="IList{T}"/>s with <see langword="null"/>.</returns>
        public static IList<IList<Tuple<object>>> ZipUnBound(this IEnumerable<IList<object>> @this)
        {
            @this.ThrowIfNull(nameof(@this));
            return @this.Select(a => a.Select(Tuple.Create)).ZipUnBound(null);
        }
        /// <summary>
        /// Get all the elements in <see cref="IList{T}"/>s spliced together, continuing until they all end.
        /// </summary>
        /// <typeparam name="T1">The type of the first <see cref="IList{T}"/>.</typeparam>
        /// <typeparam name="T2">The type of the second <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The first <see cref="IList{T}"/>.</param>
        /// <param name="other">The second <see cref="IList{T}"/>.</param>
        /// <param name="nilValue1">The default value to assign when <paramref name="this"/> has ended.</param>
        /// <param name="nilValue2">The default value to assign when <paramref name="other"/> has ended.</param>
        /// <returns><paramref name="this"/> transposed, filling ended <see cref="IList{T}"/>s with <paramref name="nilValue1"/> or <paramref name="nilValue2"/>.</returns>
        public static IList<(T1, T2)> ZipUnBound<T1, T2>(this IList<T1> @this, IList<T2> other, T1 nilValue1, T2 nilValue2)
        {
            @this.ThrowIfNull(nameof(@this));
            other.ThrowIfNull(nameof(other));
            return new[] { @this.Select(a => (object)a), other.Select(a => (object)a) }.ZipUnBound().Select(a =>
            {
                T1 t1 = nilValue1;
                if (a[0] != null)
                    t1 = (T1)a[0].Item1;

                T2 t2 = nilValue2;
                if (a[1] != null)
                    t2 = (T2)a[1].Item1;

                return (t1, t2);
            });
        }
        /// <summary>
        /// Get all the elements in <see cref="IList{T}"/>s spliced together, continuing until they all end.
        /// </summary>
        /// <typeparam name="T1">The type of the first <see cref="IList{T}"/>.</typeparam>
        /// <typeparam name="T2">The type of the second <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The first <see cref="IList{T}"/>.</param>
        /// <param name="other">The second <see cref="IList{T}"/>.</param>
        /// <returns><paramref name="this"/> transposed, filling ended <see cref="IList{T}"/>s with <see langword="null"/>.</returns>
        public static IList<(T1?, T2?)> ZipUnBoundNullable<T1, T2>(this IList<T1> @this, IList<T2> other) where T1 : struct where T2 : struct
        {
            @this.ThrowIfNull(nameof(@this));
            other.ThrowIfNull(nameof(other));
            return @this.Select(a => (T1?)a).ZipUnBound(other.Select(a => (T2?)a), null, null);
        }
        /// <summary>
        /// Get all the elements in <see cref="IList{T}"/>s spliced together, continuing until they all end.
        /// </summary>
        /// <typeparam name="T1">The type of the first <see cref="IList{T}"/>.</typeparam>
        /// <typeparam name="T2">The type of the second <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The first <see cref="IList{T}"/>.</param>
        /// <param name="other">The second <see cref="IList{T}"/>.</param>
        /// <returns><paramref name="this"/> transposed, filling ended <see cref="IList{T}"/>s with <see langword="null"/>.</returns>
        public static IList<(Tuple<T1>, Tuple<T2>)> ZipUnBoundTuple<T1, T2>(this IList<T1> @this, IList<T2> other)
        {
            @this.ThrowIfNull(nameof(@this));
            other.ThrowIfNull(nameof(other));
            return @this.Select(Tuple.Create).ZipUnBound(other.Select(Tuple.Create), null, null);
        }
        /// <summary>
        /// Get all the elements in <see cref="IList{T}"/>s spliced together, continuing until they all end.
        /// </summary>
        /// <typeparam name="T1">The type of the first <see cref="IList{T}"/>.</typeparam>
        /// <typeparam name="T2">The type of the second <see cref="IList{T}"/>.</typeparam>
        /// <typeparam name="T3">The type of the third <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The first <see cref="IList{T}"/>.</param>
        /// <param name="other">The second <see cref="IList{T}"/>.</param>
        /// <param name="other2">The third <see cref="IList{T}"/>.</param>
        /// <param name="nilValue1">The default value to assign when <paramref name="this"/> has ended.</param>
        /// <param name="nilValue2">The default value to assign when <paramref name="other"/> has ended.</param>
        /// <param name="nilValue3">The default value to assign when <paramref name="other2"/> has ended.</param>
        /// <returns><paramref name="this"/> transposed, filling ended <see cref="IList{T}"/>s with <paramref name="nilValue1"/>, <paramref name="nilValue2"/> or <paramref name="nilValue3"/>.</returns>
        public static IList<(T1, T2, T3)> ZipUnBound<T1, T2, T3>(this IList<T1> @this, IList<T2> other, IList<T3> other2, T1 nilValue1, T2 nilValue2, T3 nilValue3)
        {
            @this.ThrowIfNull(nameof(@this));
            other.ThrowIfNull(nameof(other));
            other2.ThrowIfNull(nameof(other2));
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

                return (t1, t2, t3);
            });
        }
        /// <summary>
        /// Get all the elements in <see cref="IList{T}"/>s spliced together, continuing until they all end.
        /// </summary>
        /// <typeparam name="T1">The type of the first <see cref="IList{T}"/>.</typeparam>
        /// <typeparam name="T2">The type of the second <see cref="IList{T}"/>.</typeparam>
        /// <typeparam name="T3">The type of the third <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The first <see cref="IList{T}"/>.</param>
        /// <param name="other">The second <see cref="IList{T}"/>.</param>
        /// <param name="other2">The third <see cref="IList{T}"/>.</param>
        /// <returns><paramref name="this"/> transposed, filling ended <see cref="IList{T}"/>s with null.</returns>
        public static IList<(T1?, T2?, T3?)> ZipUnBoundNullable<T1, T2, T3>(this IList<T1> @this, IList<T2> other, IList<T3> other2) where T1 : struct where T2 : struct where T3 : struct
        {
            @this.ThrowIfNull(nameof(@this));
            other.ThrowIfNull(nameof(other));
            other2.ThrowIfNull(nameof(other2));
            return @this.Select(a => (T1?)a).ZipUnBound(other.Select(a => (T2?)a), other2.Select(a => (T3?)a), null, null, null);
        }
        /// <summary>
        /// Get all the elements in <see cref="IList{T}"/>s spliced together, continuing until they all end.
        /// </summary>
        /// <typeparam name="T1">The type of the first <see cref="IList{T}"/>.</typeparam>
        /// <typeparam name="T2">The type of the second <see cref="IList{T}"/>.</typeparam>
        /// <typeparam name="T3">The type of the third <see cref="IList{T}"/>.</typeparam>
        /// <param name="this">The first <see cref="IList{T}"/>.</param>
        /// <param name="other">The second <see cref="IList{T}"/>.</param>
        /// <param name="other2">The third <see cref="IList{T}"/>.</param>
        /// <returns><paramref name="this"/> transposed, filling ended <see cref="IList{T}"/>s with null.</returns>
        public static IList<(Tuple<T1>, Tuple<T2>, Tuple<T3>)> ZipUnBoundTuple<T1, T2, T3>(this IList<T1> @this, IList<T2> other, IList<T3> other2)
        {
            @this.ThrowIfNull(nameof(@this));
            other.ThrowIfNull(nameof(other));
            other2.ThrowIfNull(nameof(other2));
            return @this.Select(Tuple.Create).ZipUnBound(other.Select(Tuple.Create), other2.Select(Tuple.Create), null, null, null);
        }
    }
}