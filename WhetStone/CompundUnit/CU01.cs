using System;
using System.Collections.Generic;
using System.Linq;
using Numerics;
using WhetStone.Looping;

namespace WhetStone.Units
{
    public class CompundUnit0Num1Denum<T0>
        : IUnit<CompundUnit0Num1Denum<T0>>, ScaleMeasurement<CompundUnit0Num1Denum<T0>>, DeltaMeasurement<CompundUnit0Num1Denum<T0>>,
            IComparable<CompundUnit0Num1Denum<T0>>
        where T0 : IUnit<T0>, ScaleMeasurement<T0>, DeltaMeasurement<T0>
    {
        public CompundUnit0Num1Denum(T0 i0, bool creadeUdic = true)
        {
            Arbitrary = 1.0/((DeltaMeasurement<T0>)i0).Arbitrary;
            if (_udic == null && creadeUdic)
            {
                _defunit = "1/"+i0.unitDictionary.First().Value.Item2;
                _udic = (IDictionary<string, Tuple<IUnit<CompundUnit0Num1Denum<T0>>, string>>)i0.unitDictionary.Select(tuple => Tuple.Create(new CompundUnit0Num1Denum<T0>((T0)tuple.Item1, false), "1/"+tuple.Item2));
            }
        }
        private CompundUnit0Num1Denum(BigRational arb)
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
        public override IDictionary<string, Tuple<IUnit<CompundUnit0Num1Denum<T0>>, string>> unitDictionary => _udic;
        private static IDictionary<string, Tuple<IUnit<CompundUnit0Num1Denum<T0>>, string>> _udic = null;
        private static string _defunit;
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return this.StringFromUnitDictionary(format, _defunit, formatProvider, scaleDictionary);
        }
        public int CompareTo(CompundUnit0Num1Denum<T0> other)
        {
            return Arbitrary.CompareTo(other.Arbitrary);
        }

        public static CompundUnit0Num1Denum<T0> operator +(CompundUnit0Num1Denum<T0> @this, CompundUnit0Num1Denum<T0> a)
        {
            return new CompundUnit0Num1Denum<T0>(@this.Arbitrary + a.Arbitrary);
        }
        public static CompundUnit0Num1Denum<T0> operator -(CompundUnit0Num1Denum<T0> @this, CompundUnit0Num1Denum<T0> a)
        {
            return new CompundUnit0Num1Denum<T0>(@this.Arbitrary - a.Arbitrary);
        }
        public static BigRational operator /(CompundUnit0Num1Denum<T0> @this, CompundUnit0Num1Denum<T0> a)
        {
            return @this.Arbitrary/a.Arbitrary;
        }
        public static CompundUnit0Num1Denum<T0> operator *(CompundUnit0Num1Denum<T0> @this, BigRational a)
        {
            return new CompundUnit0Num1Denum<T0>(@this.Arbitrary * a);
        }
        public static CompundUnit0Num1Denum<T0> operator *(BigRational a, CompundUnit0Num1Denum<T0> @this)
        {
            return new CompundUnit0Num1Denum<T0>(@this.Arbitrary * a);
        }
        public static BigRational operator *(T0 a, CompundUnit0Num1Denum<T0> @this)
        {
            return ((ScaleMeasurement<T0>)a).Arbitrary * @this.Arbitrary;
        }
        public static CompundUnit0Num1Denum<T0> operator /(CompundUnit0Num1Denum<T0> @this, BigRational a)
        {
            return new CompundUnit0Num1Denum<T0>(@this.Arbitrary * a);
        }
        public static T0 operator /(BigRational a, CompundUnit0Num1Denum<T0> @this)
        {
            return (T0)typeof(T0).GetConstructor(new[] { typeof(BigRational) }).Invoke(new object[] { a/@this.Arbitrary });
        }
    }
    public static partial class CompundUnitExtentions
    {
        public static CompundUnit0Num1Denum<T> Inv<T>(this T @this) where T : IUnit<T>, ScaleMeasurement<T>, DeltaMeasurement<T>
        {
            return new CompundUnit0Num1Denum<T>(@this);
        }
    }
}
