using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Comparison;

namespace WhetStone.Functional
{
    public static class Functional
    {
        public static Func<T1, R> Partial<T0, T1, R>(this Func<T0, T1, R> @this, T0 value=default(T0))
        {
            return x => @this(value, x);
        }
        public static Func<T, Tuple<R0,R1>> Adjoin<T, R0, R1>(this Func<T,R0> f0, Func<T,R1> f1)
        {
            return x => Tuple.Create(f0(x), f1(x));
        }
        public static Func<T, Tuple<R0, R1, R2>> Adjoin<T, R0, R1, R2>(this Func<T, R0> f0, Func<T, R1> f1, Func<T, R2> f2)
        {
            return x => Tuple.Create(f0(x), f1(x), f2(x));
        }
        public static Func<T, IEnumerable<R>> Adjoin<T, R>(params Func<T, R>[] @this)
        {
            return x => @this.Select(a => a(x));
        }
        public static Func<R0, R2> Compose<R0, R1, R2>(this Func<R1, R2> @this, Func<R0, R1> other)
        {
            return x => @this(other(x));
        }
        public static Func<R0, R2> Pipe<R0, R1, R2>(this Func<R0, R1> @this, Func<R1, R2> other)
        {
            return x => other(@this(x));
        }
        public static Func<T,R> Memoize<T,R>(this Func<T, R> @this, IEqualityComparer<T> comp = null)
        {
            comp = comp ?? EqualityComparer<T>.Default;
            var dic = new Dictionary<T, R>(comp);
            return x =>
            {
                if (!dic.ContainsKey(x))
                    dic[x] = @this(x);
                return dic[x];
            };
        }
        public static Func<T1, T2, R> Memoize<T1, T2, R>(this Func<T1, T2, R> @this, IEqualityComparer<T1> comp1 = null , IEqualityComparer<T2> comp2 = null)
        {
            return @this.Attach().Memoize(new TupleEqualityComparer<T1, T2>(comp1, comp2)).Detach();
        }
        public static Func<T1, T2, T3, R> Memoize<T1, T2, T3, R>(this Func<T1, T2, T3, R> @this, IEqualityComparer<T1> comp1 = null, IEqualityComparer<T2> comp2 = null, IEqualityComparer<T3> comp3 = null)
        {
            return @this.Attach().Memoize(new TupleEqualityComparer<T1, T2, T3>(comp1, comp2, comp3)).Detach();
        }
        public static Func<Tuple<T1, T2>, R> Attach<T1, T2, R>(this Func<T1, T2, R> @this)
        {
            return x => @this(x.Item1, x.Item2);
        }
        public static Func<Tuple<T1, T2, T3>, R> Attach<T1, T2, T3, R>(this Func<T1, T2, T3, R> @this)
        {
            return x => @this(x.Item1, x.Item2, x.Item3);
        }
        public static Func<Tuple<T1, T2, T3, T4>, R> Attach<T1, T2, T3, T4, R>(this Func<T1, T2, T3, T4, R> @this)
        {
            return x => @this(x.Item1, x.Item2, x.Item3, x.Item4);
        }
        public static Func<T1, T2, R> Detach<T1, T2, R>(this Func<Tuple<T1, T2>, R> @this)
        {
            return (i1, i2) => @this(Tuple.Create(i1, i2));
        }
        public static Func<T1, T2, T3, R> Detach<T1, T2, T3, R>(this Func<Tuple<T1, T2, T3>, R> @this)
        {
            return (i1, i2, i3) => @this(Tuple.Create(i1, i2, i3));
        }
        public static Func<T1, T2, T3, T4, R> Detach<T1, T2, T3, T4, R>(this Func<Tuple<T1, T2, T3, T4>, R> @this)
        {
            return (i1, i2, i3, i4) => @this(Tuple.Create(i1, i2, i3, i4));
        }
    }
}
