using System.Collections.Generic;
using WhetStone.Looping;
using WhetStone.RecursiveQuerier;

namespace WhetStone.Fielding
{
    public class QueryEnabledField<T> : Field<T>
    {
        public const ulong DefaultMaxFromIntQuery = 256;
        public const ulong DefaultMaxPowBaseQuery = 20;
        public const ulong DefaultMaxPowExpQuery = 16;
        public const ulong DefaultMaxFactorialQuery = 20;
        public ulong MaxFromIntQuery = DefaultMaxFromIntQuery;
        public ulong MaxPowBaseQuery = DefaultMaxPowBaseQuery;
        public ulong MaxPowExpQuery = DefaultMaxPowExpQuery;
        public ulong MaxFactorialQuery = DefaultMaxFactorialQuery;

        private readonly HalvingQuerier<T> _fromIntQuerier;
        private readonly IDictionary<T, HalvingQuerier<T>> _powDictionary;
        private readonly LazyArray<T> _factorialQuerier;
        public QueryEnabledField(T zero, T one, T naturalbase) : base(zero,one,naturalbase)
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            _fromIntQuerier = new HalvingQuerier<T>(this.one,this.add,this.zero);
            _powDictionary = new Dictionary<T, HalvingQuerier<T>>();
            _factorialQuerier = new LazyArray<T>((i, array) => i == 0 ? this.one : this.multiply(this.fromInt(i),array[i-1]));
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }
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
                    if (d%1 != 0 || d > MaxPowBaseQuery)
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
        public override T Factorial(int x)
        {
            return (x>0 && (ulong)x < MaxFactorialQuery) ? _factorialQuerier[x] : base.Factorial(x);
        }
        public void ResetQueriers()
        {
            MaxFromIntQuery = DefaultMaxFromIntQuery;
            MaxPowBaseQuery = DefaultMaxPowBaseQuery;
            MaxPowExpQuery = DefaultMaxPowExpQuery;
            MaxFactorialQuery = DefaultMaxFactorialQuery;
            _fromIntQuerier.Clear();
            _powDictionary.Clear();
            _factorialQuerier.Clear();
        }
    }
}