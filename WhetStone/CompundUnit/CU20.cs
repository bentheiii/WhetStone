using System;
using System.Collections.Generic;
using System.Linq;
using Numerics;
using WhetStone.Looping;

namespace WhetStone.Units
{
    
    public class CompundUnit2Num0Denum<T0, T1> : IUnit<CompundUnit2Num0Denum<T0, T1>>, ScaleMeasurement<CompundUnit2Num0Denum<T0, T1>>, DeltaMeasurement<CompundUnit2Num0Denum<T0, T1>>, IComparable<CompundUnit2Num0Denum<T0, T1>>
        where T0 : IUnit<T0>, ScaleMeasurement<T0>, DeltaMeasurement<T0> where T1 : IUnit<T1>, ScaleMeasurement<T1>, DeltaMeasurement<T1>
    {
        public CompundUnit2Num0Denum(T0 i0, T1 i1, bool creadeUdic = true)
        {
            Arbitrary = ((DeltaMeasurement<T0>)i0).Arbitrary * ((DeltaMeasurement<T1>)i1).Arbitrary;
            if (_udic == null && creadeUdic)
            {
                _defunit = i0.unitDictionary.First().Value.Item2 + "*" + i1.unitDictionary.First().Value.Item2;
                _udic = i0.unitDictionary.Join(i1.unitDictionary).Select(a =>
                {
                    var u0 = a.Item1;
                    var u1 = a.Item2;
                    return new KeyValuePair<string, Tuple<IUnit<CompundUnit2Num0Denum<T0, T1>>, string>>(u0.Key + "|" + u1.Key,
                        Tuple.Create((IUnit<CompundUnit2Num0Denum<T0, T1>>)new CompundUnit2Num0Denum<T0, T1>((T0)u0.Value.Item1, (T1)u1.Value.Item1, false),
                            u0.Value.Item2 + "*" + u1.Value.Item2));
                }).ToDictionary();
            }
        }
        private CompundUnit2Num0Denum(BigRational arb)
        {
            Arbitrary = arb;
        }
        public BigRational Arbitrary { get; }
        public override BigRational FromArbitrary(BigRational arb)
        {
            return arb / Arbitrary;
        }
        public override BigRational ToArbitrary(BigRational val)
        {
            return val * Arbitrary;
        }
        private static IDictionary<string, Tuple<IUnit<CompundUnit2Num0Denum<T0, T1>>, string>> _udic = null;
        private static string _defunit;
        public override IDictionary<string, Tuple<IUnit<CompundUnit2Num0Denum<T0, T1>>, string>> unitDictionary => _udic;
        //format symbols is *unit symbol for T0*|*unit symbol for T1*
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return this.StringFromUnitDictionary(format, _defunit, formatProvider, scaleDictionary);
        }
        BigRational ScaleMeasurement<CompundUnit2Num0Denum<T0, T1>>.Arbitrary
        {
            get
            {
                return this.Arbitrary;
            }
        }
        BigRational DeltaMeasurement<CompundUnit2Num0Denum<T0, T1>>.Arbitrary
        {
            get
            {
                return this.Arbitrary;
            }
        }
        public int CompareTo(CompundUnit2Num0Denum<T0,T1> other)
        {
            return Arbitrary.CompareTo(other.Arbitrary);
        }
        public static CompundUnit2Num0Denum<T0,T1> operator +(CompundUnit2Num0Denum<T0, T1> @this, CompundUnit2Num0Denum<T0, T1> a)
        {
            return new CompundUnit2Num0Denum<T0, T1>(@this.Arbitrary+a.Arbitrary);
        }
        public static CompundUnit2Num0Denum<T0, T1> operator +(CompundUnit2Num0Denum<T0, T1> @this, CompundUnit2Num0Denum<T1, T0> a)
        {
            return new CompundUnit2Num0Denum<T0, T1>(@this.Arbitrary + a.Arbitrary);
        }
        public static CompundUnit2Num0Denum<T0, T1> operator -(CompundUnit2Num0Denum<T0, T1> @this, CompundUnit2Num0Denum<T0, T1> a)
        {
            return new CompundUnit2Num0Denum<T0, T1>(@this.Arbitrary - a.Arbitrary);
        }
        public static CompundUnit2Num0Denum<T0, T1> operator -(CompundUnit2Num0Denum<T0, T1> @this, CompundUnit2Num0Denum<T1, T0> a)
        {
            return new CompundUnit2Num0Denum<T0, T1>(@this.Arbitrary - a.Arbitrary);
        }
        public static T0 operator /(CompundUnit2Num0Denum<T0, T1> @this, T1 div)
        {
            return (T0)typeof(T0).GetConstructor(new[] { typeof(BigRational) }).Invoke(new object[] { @this.Arbitrary / ((DeltaMeasurement<T1>)div).Arbitrary });
        }
        public static T1 operator /(CompundUnit2Num0Denum<T0, T1> @this, T0 div)
        {
            return (T1)typeof(T1).GetConstructor(new[] { typeof(BigRational) }).Invoke(new object[] { @this.Arbitrary / ((DeltaMeasurement<T1>)div).Arbitrary });
        }
        public static BigRational operator /(CompundUnit2Num0Denum<T0, T1> @this, CompundUnit2Num0Denum<T0, T1> a)
        {
            return @this.Arbitrary/a.Arbitrary;
        }
        public static BigRational operator /(CompundUnit2Num0Denum<T0, T1> @this, CompundUnit2Num0Denum<T1, T0> a)
        {
            return @this.Arbitrary / a.Arbitrary;
        }
        public static CompundUnit2Num0Denum<T0, T1> operator *(CompundUnit2Num0Denum<T0, T1> @this, BigRational a)
        {
            return new CompundUnit2Num0Denum<T0, T1>(@this.Arbitrary * a);
        }
        public static CompundUnit2Num0Denum<T0, T1> operator *(BigRational a, CompundUnit2Num0Denum<T0, T1> @this)
        {
            return new CompundUnit2Num0Denum<T0, T1>(@this.Arbitrary * a);
        }
        public static CompundUnit2Num0Denum<T0, T1> operator /(CompundUnit2Num0Denum<T0, T1> @this, BigRational a)
        {
            return new CompundUnit2Num0Denum<T0, T1>(@this.Arbitrary / a);
        }
    }

    public static partial class CompundUnitExtentions
    {
        public static T div<T>(this CompundUnit2Num0Denum<T, T> @this, T denum) where T : IUnit<T>, ScaleMeasurement<T>, DeltaMeasurement<T>
        {
            return (T)typeof(T).GetConstructor(new[] { typeof(BigRational) }).Invoke(new object[] { @this.Arbitrary / ((DeltaMeasurement<T>)denum).Arbitrary });
        }
        public static CompundUnit2Num0Denum<T0, T1> mul<T0,T1>(this T0 u0, T1 u1) where T0 : IUnit<T0>, ScaleMeasurement<T0>, DeltaMeasurement<T0> where T1 : IUnit<T1>, ScaleMeasurement<T1>, DeltaMeasurement<T1>
        {
            return new CompundUnit2Num0Denum<T0, T1>(u0, u1);
        }
    }
    
}
