using System;
using WhetStone.Looping;
using WhetStone.Fielding;

namespace WhetStone.RecursiveQuerier
{
    public class HalvingQuerier<T> : LazyArray<T>
    {
        public HalvingQuerier(T baseValue, Func<T, T, T> agg, T seed) : base((i, l) =>
        {
            if (i == 0)
                return seed;
            if (i % 2 == 1 || l.Initialized(i - 1))
                return agg(l[i - 1], baseValue);
            var x = l[i / 2];
            return agg(x, x);
        }){}
    }
    public class PowQuerier<T> : HalvingQuerier<T>
    {
        public PowQuerier(T baseValue) : base(baseValue, Fields.getField<T>().multiply, Fields.getField<T>().one) { }
    }
    public class ProdQuerier<T> : HalvingQuerier<T>
    {
        public ProdQuerier(T baseValue) : base(baseValue, Fields.getField<T>().add, Fields.getField<T>().zero) { }
    }
    public class FactorialQuerier<T> : LazyArray<T>
    {
        private static readonly Field<T> Field = Fields.getField<T>();
        public FactorialQuerier() : base((i, l) => i == 0 ? Field.one : Field.multiply(l[i-1],Field.fromInt(i))){ }
    }
}
