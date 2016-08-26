using System;
using System.Collections.Generic;
using WhetStone.Looping;
using WhetStone.WordPlay;

namespace WhetStone.Units
{
    public interface ScaleMeasurement : IFormattable
    {
        //convention: measurement.arbitrary+delta.arbitrary = (measurement+delta).arbitrary
        double Arbitrary { get; }
    }
    public interface DeltaMeasurement : IFormattable
    {
        double Arbitrary { get; }
    }
    public static class UnitExtensions
    {
        public static double InUnits<T>(this T @this, IUnit<T> unit) where T : ScaleMeasurement, DeltaMeasurement
        {
            return unit.FromArbitrary(((ScaleMeasurement)@this).Arbitrary);
        }
        public static double InUnits<T>(this T @this, IScaleUnit<T> unit) where T : ScaleMeasurement
        {
            return unit.FromArbitrary(@this.Arbitrary);
        }
        public static double InUnits<T>(this T @this, IDeltaUnit<T> unit) where T : DeltaMeasurement
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
            var dat = @this.InUnits(unitDictionary[split[0]].Item1).ToString(split[1], formatProvider);
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
            var dat = @this.InUnits(unitDictionary[split[0]].Item1).ToString(split[1], formatProvider);
            var id = unitDictionary[split[0]].Item2;
            if (pre)
                return id + dat;
            return dat + id;
        }
    }
    // ReSharper disable once UnusedTypeParameter
    public interface IScaleUnit<T> where T : ScaleMeasurement
    {
        double FromArbitrary(double arb);
        double ToArbitrary(double val);
    }
    // ReSharper disable once UnusedTypeParameter
    public interface IDeltaUnit<T> where T : DeltaMeasurement
    {
        double FromArbitrary(double arb);
        double ToArbitrary(double val);
    }
    public abstract class IUnit<T> : IScaleUnit<T>, IDeltaUnit<T> where T : ScaleMeasurement, DeltaMeasurement
    {
        public abstract double FromArbitrary(double arb);
        public abstract double ToArbitrary(double val);
        double IScaleUnit<T>.FromArbitrary(double arb)
        {
            return this.FromArbitrary(arb);
        }
        double IDeltaUnit<T>.FromArbitrary(double arb)
        {
            return this.FromArbitrary(arb);
        }
        double IDeltaUnit<T>.ToArbitrary(double val)
        {
            return this.ToArbitrary(val);
        }
        double IScaleUnit<T>.ToArbitrary(double val)
        {
            return this.ToArbitrary(val);
        }
    }
    public class Unit<T> : IUnit<T> where T : ScaleMeasurement, DeltaMeasurement
    {
        private readonly double _faFactor;
        //val = arbitrary*factor
        public Unit(double faFactor)
        {
            _faFactor = faFactor;
        }
        public override double FromArbitrary(double arb)
        {
            return arb * _faFactor;
        }
        public override double ToArbitrary(double val)
        {
            return val / _faFactor;
        }
    }
    public class ScaleUnit<T> : IScaleUnit<T> where T : ScaleMeasurement
    {
        private readonly double _faFactor;
        private readonly double _faBias;
        //val = arbitrary*factor + bias
        public ScaleUnit(double faFactor, double faBias = 0)
        {
            _faFactor = faFactor;
            _faBias = faBias;
        }
        public double FromArbitrary(double arb)
        {
            return arb * _faFactor + _faBias;
        }
        public double ToArbitrary(double val)
        {
            return (val - _faBias) / _faFactor;
        }
    }
    public class DeltaUnit<T> : IDeltaUnit<T> where T : DeltaMeasurement
    {
        private readonly double _faFactor;
        //val = arbitrary*factor
        public DeltaUnit(double faFactor)
        {
            _faFactor = faFactor;
        }
        public double FromArbitrary(double arb)
        {
            return arb * _faFactor;
        }
        public double ToArbitrary(double val)
        {
            return val / _faFactor;
        }
    }
    
}
