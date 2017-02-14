using System.Collections.Generic;
using WhetStone.Looping;
using WhetStone.RecursiveQuerier;

namespace WhetStone.Fielding
{
    public abstract class QueryEnabledField<T> : Field<T>
    {
        public const ulong DefaultMaxFromIntQuery = 256;
        public const ulong DefaultMaxPowBaseQuery = 20;
        public const ulong DefaultMaxPowExpQuery = 16;
        public const ulong DefaultMaxFactorialQuery = 20;
        public ulong MaxFromIntQuery = DefaultMaxFromIntQuery;
        public ulong MaxPowBaseQuery = DefaultMaxPowBaseQuery;
        public ulong MaxPowExpQuery = DefaultMaxPowExpQuery;
        private readonly HalvingQuerier<T> _fromIntQuerier;
        private readonly IDictionary<T, HalvingQuerier<T>> _powDictionary;
        private readonly LazyArray<T> _factorialQuerier;
        protected QueryEnabledField(T zero, T one)
        {
            this.zero = zero;
            this.one = one;
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            _fromIntQuerier = new HalvingQuerier<T>(this.one,this.add,this.zero);
            _powDictionary = new Dictionary<T, HalvingQuerier<T>>();
            _factorialQuerier = new LazyArray<T>((i, array) => i == 0 ? this.one : this.multiply(this.fromInt(i),array[i-1]));
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }
        public override T zero { get; }
        public override T one { get; }
        public override T fromInt(ulong x)
        {
            return x < MaxFromIntQuery ? _fromIntQuerier[(int)x] : base.fromInt(x);
        }
        public override T Pow(T @base, int x)
        {
            //check if exponential is valid
            if (x > 0 && (uint)x < MaxPowExpQuery)
            {
                //check if key for base exists
                if (!_powDictionary.ContainsKey(@base))
                {
                    //check if base is valid
                    var d = toDouble(@base) ?? 0.5;
                    if (d%1.0 != 0 || d > MaxPowBaseQuery)
                        //base isn't valid
                        return base.Pow(@base, x);
                    //base is valid, initialize halver
                    _powDictionary[@base] = new HalvingQuerier<T>(@base,this.multiply, this.one);
                }
                //if it does, then it's valid
                return _powDictionary[@base][x];
            }
            //exponential isn't valid
            return base.Pow(@base, x);
        }
        public void ResetQueriers()
        {
            MaxFromIntQuery = DefaultMaxFromIntQuery;
            MaxPowBaseQuery = DefaultMaxPowBaseQuery;
            MaxPowExpQuery = DefaultMaxPowExpQuery;
            _fromIntQuerier.Clear();
            _powDictionary.Clear();
            _factorialQuerier.Clear();
        }
    }
}