using System;
using System.Collections.Generic;
using Numerics;
using WhetStone.Looping;
using WhetStone.WordPlay;

namespace WhetStone.Units
{
    public interface ScaleMeasurement : IFormattable
    {
        //convention: measurement.arbitrary+delta.arbitrary = (measurement+delta).arbitrary
        BigRational Arbitrary { get; }
    }
    public interface DeltaMeasurement : IFormattable
    {
        BigRational Arbitrary { get; }
    }
    public static class UnitExtensions
    {
        public static BigRational InUnits<T>(this T @this, IUnit<T> unit) where T : ScaleMeasurement, DeltaMeasurement
        {
            return unit.FromArbitrary(((ScaleMeasurement)@this).Arbitrary);
        }
        public static BigRational InUnits<T>(this T @this, IScaleUnit<T> unit) where T : ScaleMeasurement
        {
            return unit.FromArbitrary(@this.Arbitrary);
        }
        public static BigRational InUnits<T>(this T @this, IDeltaUnit<T> unit) where T : DeltaMeasurement
        {
            return unit.FromArbitrary(@this.Arbitrary);
        }
        public static string StringFromUnitDictionary<T>(this T @this, string format, string defaultunit, IFormatProvider formatProvider, IDictionary<string, Tuple<IScaleUnit<T>, string>> unitDictionary, bool pre = false)
            where T : ScaleMeasurement
        {
            string[] split = format.SmartSplit("_", "(", ")");
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
            string dat = ((double)val).ToString(split[1], formatProvider);
            var id = unitDictionary[split[0]].Item2;
            if (pre)
                return id + dat;
            return dat + id;
        }
        public static string StringFromDeltaDictionary<T>(this T @this, string format, string defaultunit, IFormatProvider formatProvider, IDictionary<string, Tuple<IDeltaUnit<T>, string>> unitDictionary, bool pre = false)
            where T : DeltaMeasurement
        {
            string[] split = format.SmartSplit("_", "(", ")");
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
            var dat = ((double)@this.InUnits(unitDictionary[split[0]].Item1)).ToString(split[1], formatProvider);
            var id = unitDictionary[split[0]].Item2;
            if (pre)
                return id + dat;
            return dat + id;
        }
    }
    // ReSharper disable once UnusedTypeParameter
    public interface IScaleUnit<T> where T : ScaleMeasurement
    {
        BigRational FromArbitrary(BigRational arb);
        BigRational ToArbitrary(BigRational val);
    }
    // ReSharper disable once UnusedTypeParameter
    public interface IDeltaUnit<T> where T : DeltaMeasurement
    {
        BigRational FromArbitrary(BigRational arb);
        BigRational ToArbitrary(BigRational val);
    }
    public abstract class IUnit<T> : IScaleUnit<T>, IDeltaUnit<T> where T : ScaleMeasurement, DeltaMeasurement
    {
        public abstract BigRational FromArbitrary(BigRational arb);
        public abstract BigRational ToArbitrary(BigRational val);
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
    public class Unit<T> : IUnit<T> where T : ScaleMeasurement, DeltaMeasurement
    {
        private readonly BigRational _faFactor;
        //val = arbitrary*factor
        public Unit(BigRational faFactor)
        {
            _faFactor = faFactor;
        }
        public override BigRational FromArbitrary(BigRational arb)
        {
            return arb * _faFactor;
        }
        public override BigRational ToArbitrary(BigRational val)
        {
            return val / _faFactor;
        }
    }
    public class ScaleUnit<T> : IScaleUnit<T> where T : ScaleMeasurement
    {
        private readonly BigRational _faFactor;
        private readonly BigRational _faBias;
        //val = arbitrary*factor + bias
        public ScaleUnit(BigRational faFactor, BigRational faBias = new BigRational())
        {
            _faFactor = faFactor;
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
    }
    public class DeltaUnit<T> : IDeltaUnit<T> where T : DeltaMeasurement
    {
        private readonly BigRational _faFactor;
        //val = arbitrary*factor
        public DeltaUnit(BigRational faFactor)
        {
            _faFactor = faFactor;
        }
        public BigRational FromArbitrary(BigRational arb)
        {
            return arb * _faFactor;
        }
        public BigRational ToArbitrary(BigRational val)
        {
            return val / _faFactor;
        }
    }
    
}
