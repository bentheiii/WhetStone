using System;
using System.Collections.Generic;
using Numerics;
using WhetStone.Looping;
using WhetStone.WordPlay;

namespace WhetStone.Units
{
    public interface ScaleMeasurement<T> : IFormattable where T : ScaleMeasurement<T>
    {
        //convention: measurement.arbitrary+delta.arbitrary = (measurement+delta).arbitrary
        BigRational Arbitrary { get; }
        IDictionary<string, Tuple<IScaleUnit<T>, string>> scaleDictionary { get; }
    }
    public interface DeltaMeasurement<T> : IFormattable where T : DeltaMeasurement<T>
    {
        BigRational Arbitrary { get; }
        IDictionary<string, Tuple<IDeltaUnit<T>, string>> deltaDictionary { get; }
    }
    public static class UnitExtensions
    {
        public static double InUnits<T>(this T @this, IUnit<T> unit) where T : ScaleMeasurement<T>, DeltaMeasurement<T>
        {
            return (double)unit.FromArbitrary(((ScaleMeasurement<T>)@this).Arbitrary);
        }
        public static double InUnits<T>(this T @this, IScaleUnit<T> unit) where T : ScaleMeasurement<T>
        {
            return (double)unit.FromArbitrary(@this.Arbitrary);
        }
        public static double InUnits<T>(this T @this, IDeltaUnit<T> unit) where T : DeltaMeasurement<T>
        {
            return (double)unit.FromArbitrary(@this.Arbitrary);
        }
        public static BigRational InUnitsExact<T>(this T @this, IUnit<T> unit) where T : ScaleMeasurement<T>, DeltaMeasurement<T>
        {
            return unit.FromArbitrary(((ScaleMeasurement<T>)@this).Arbitrary);
        }
        public static BigRational InUnitsExact<T>(this T @this, IScaleUnit<T> unit) where T : ScaleMeasurement<T>
        {
            return unit.FromArbitrary(@this.Arbitrary);
        }
        public static BigRational InUnitsExact<T>(this T @this, IDeltaUnit<T> unit) where T : DeltaMeasurement<T>
        {
            return unit.FromArbitrary(@this.Arbitrary);
        }
        public static string StringFromUnitDictionary<T>(this T @this, string doubleformat, string defaultunit, IFormatProvider formatProvider, IDictionary<string, Tuple<IScaleUnit<T>, string>> unitDictionary, bool pre = false)
            where T : ScaleMeasurement<T>
        {
            doubleformat = doubleformat ?? "";
            string[] split = doubleformat.SmartSplit("_", "(", ")");
            while (split.Length != 3)
            {
                switch (split.Length)
                {
                    case 0:
                        split = new[] {defaultunit};
                        break;
                    case 1:
                        append.Append(ref split, "G");
                        break;
                    case 2:
                        if (!unitDictionary.ContainsKey(split[0]))
                            throw new FormatException("Unit Specifier not Recognized");
                        append.Append(ref split, unitDictionary[split[0]].Item2);
                        break;
                    default:
                        throw new FormatException("too many arguments");
                }
            }
            if (!unitDictionary.ContainsKey(split[0]))
                throw new FormatException("Unit Specifier not Recognized");
            var val = @this.InUnits(unitDictionary[split[0]].Item1);
            string dat = val.ToString(split[1], formatProvider);
            var id = unitDictionary[split[0]].Item2;
            if (pre)
                return id + dat;
            return dat + id;
        }
        public static string StringFromDeltaDictionary<T>(this T @this, string doubleformat, string defaultunit, IFormatProvider formatProvider, IDictionary<string, Tuple<IDeltaUnit<T>, string>> unitDictionary, bool pre = false)
            where T : DeltaMeasurement<T>
        {
            doubleformat = doubleformat ?? "";
            string[] split = doubleformat.SmartSplit("_", "(", ")");
            while (split.Length != 3)
            {
                switch (split.Length)
                {
                    case 0:
                        split = new[] { defaultunit };
                        break;
                    case 1:
                        append.Append(ref split, "G");
                        break;
                    case 2:
                        if (!unitDictionary.ContainsKey(split[0]))
                            throw new FormatException("Unit Specifier not Recognized");
                        append.Append(ref split, unitDictionary[split[0]].Item2);
                        break;
                    default:
                        throw new FormatException("too many arguments");
                }
            }
            if (!unitDictionary.ContainsKey(split[0]))
                throw new FormatException("Unit Specifier not Recognized");
            var dat = @this.InUnits(unitDictionary[split[0]].Item1).ToString(split[1], formatProvider);
            var id = unitDictionary[split[0]].Item2;
            if (pre)
                return id + dat;
            return dat + id;
        }
    }
    // ReSharper disable once UnusedTypeParameter
    public interface IScaleUnit<T> where T : ScaleMeasurement<T>
    {
        BigRational FromArbitrary(BigRational arb);
        BigRational ToArbitrary(BigRational val);
    }
    // ReSharper disable once UnusedTypeParameter
    public interface IDeltaUnit<T> where T : DeltaMeasurement<T>
    {
        BigRational FromArbitrary(BigRational arb);
        BigRational ToArbitrary(BigRational val);
    }
    public abstract class IUnit<T> : IScaleUnit<T>, IDeltaUnit<T> where T : ScaleMeasurement<T>, DeltaMeasurement<T>
    {
        public abstract BigRational FromArbitrary(BigRational arb);
        public abstract BigRational ToArbitrary(BigRational val);
        public abstract IDictionary<string, Tuple<IUnit<T>, string>> unitDictionary { get; }
        public IDictionary<string, Tuple<IDeltaUnit<T>, string>> deltaDictionary => unitDictionary.Select(a=>Tuple.Create<IDeltaUnit<T>,string>(a.Item1,a.Item2));
        public IDictionary<string, Tuple<IScaleUnit<T>, string>> scaleDictionary => unitDictionary.Select(a => Tuple.Create<IScaleUnit<T>, string>(a.Item1, a.Item2));
        BigRational IScaleUnit<T>.FromArbitrary(BigRational arb)
        {
            return this.FromArbitrary(arb);
        }
        BigRational IDeltaUnit<T>.FromArbitrary(BigRational arb)
        {
            return this.FromArbitrary(arb);
        }
        BigRational IDeltaUnit<T>.ToArbitrary(BigRational val)
        {
            return this.ToArbitrary(val);
        }
        BigRational IScaleUnit<T>.ToArbitrary(BigRational val)
        {
            return this.ToArbitrary(val);
        }
    }
    public class ScaleUnit<T> : IScaleUnit<T> where T : ScaleMeasurement<T>
    {
        private readonly BigRational _faFactor;
        private readonly BigRational _faBias;
        //val = arbitrary*factor + bias
        public ScaleUnit(IDictionary<string, Tuple<IScaleUnit<T>, string>> scaleDictionary, BigRational faFactor,  BigRational faBias = new BigRational())
        {
            _faFactor = faFactor;
            this.scaleDictionary = scaleDictionary;
            _faBias = faBias;
        }
        public BigRational FromArbitrary(BigRational arb)
        {
            return arb * _faFactor + _faBias;
        }
        public BigRational ToArbitrary(BigRational val)
        {
            return (val - _faBias) / _faFactor;
        }
        public IDictionary<string, Tuple<IScaleUnit<T>, string>> scaleDictionary { get; }
    }
    public class DeltaUnit<T> : IDeltaUnit<T> where T : DeltaMeasurement<T>
    {
        private readonly BigRational _faFactor;
        //val = arbitrary*factor
        public DeltaUnit(IDictionary<string, Tuple<IDeltaUnit<T>, string>> deltaDictionary, BigRational faFactor)
        {
            _faFactor = faFactor;
            this.deltaDictionary = deltaDictionary;
        }
        public BigRational FromArbitrary(BigRational arb)
        {
            return arb * _faFactor;
        }
        public BigRational ToArbitrary(BigRational val)
        {
            return val / _faFactor;
        }
        public IDictionary<string, Tuple<IDeltaUnit<T>, string>> deltaDictionary { get; }
    }
}
