using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using WhetStone.Arrays;
using WhetStone.Units.Angles;
using WhetStone.Units.Rotationals;
using WhetStone.Data;
using WhetStone.Funnels;
using WhetStone.Net;
using WhetStone.NumbersMagic;
using WhetStone.PermanentObject;
using WhetStone.WordsPlay;
using WhetStone.WordsPlay.Parsing;

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
        internal static string StringFromUnitDictionary<T>(this T @this, string format, string defaultunit, IFormatProvider formatProvider, IDictionary<string, Tuple<IScaleUnit<T>, string>> unitDictionary, bool pre = false)
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
                        ArrayExtensions.Append(ref split, "G");
                        break;
                    case 2:
                        if (!unitDictionary.ContainsKey(split[0]))
                            throw new FormatException("Unit Specifier not Recognized");
                        ArrayExtensions.Append(ref split, unitDictionary[split[0]].Item2);
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
        internal static string StringFromDeltaDictionary<T>(this T @this, string format, string defaultunit, IFormatProvider formatProvider, IDictionary<string, Tuple<IDeltaUnit<T>, string>> unitDictionary, bool pre = false)
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
                        ArrayExtensions.Append(ref split, "G");
                        break;
                    case 2:
                        if (!unitDictionary.ContainsKey(split[0]))
                            throw new FormatException("Unit Specifier not Recognized");
                        ArrayExtensions.Append(ref split, unitDictionary[split[0]].Item2);
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

    namespace Angles
    {
        //arbitrary is radians
        //in most conventions, 0 means aligned with the x axis
        public class Angle : IUnit<Angle>, ScaleMeasurement, DeltaMeasurement, IComparable<Angle>
        {
            public Angle(double val, IUnit<Angle> unit, bool normalize = false) : this(unit.ToArbitrary(val), normalize) { }
            public Angle(double arbitrary, bool normalize = false)
            {
                _sin = new Lazy<double>(() => Math.Sin(Arbitrary));
                _cos = new Lazy<double>(() => Math.Cos(Arbitrary));
                if (normalize)
                    arbitrary = arbitrary.TrueMod(2 * Math.PI);
                this.Arbitrary = arbitrary;
            }
            public double Arbitrary { get; }
            double ScaleMeasurement.Arbitrary
            {
                get
                {
                    return this.Arbitrary;
                }
            }
            double DeltaMeasurement.Arbitrary
            {
                get
                {
                    return this.Arbitrary;
                }
            }
            public override double FromArbitrary(double arb)
            {
                return arb / Arbitrary;
            }
            public override double ToArbitrary(double val)
            {
                return val * Arbitrary;
            }
            public int CompareTo(Angle other)
            {
                return Arbitrary.CompareTo(other.Arbitrary);
            }

            private readonly Lazy<double> _sin, _cos;
            public double Sin() => _sin.Value;
            public double Cos() => _cos.Value;
            public double Tan() => Sin() / Cos();

            public Angle Normalize()
            {
                return Normalized ? this : new Angle(Arbitrary, true);
            }
            public bool Normalized => Arbitrary.iswithinPartialExclusive(0, 2 * Math.PI);
            public static Angle ASin(double x) => new Angle(Math.Asin(x));
            public static Angle ACos(double x) => new Angle(Math.Acos(x));
            public static Angle ATan(double x) => new Angle(Math.Atan(x));
            public static Angle ATan(double y, double x)
            {
                var r = Math.Atan2(y, x);
                return new Angle(r, true);
            }
            private static readonly Lazy<Funnel<string, Angle>> DefaultParsers =
                new Lazy<Funnel<string, Angle>>(() => new Funnel<string, Angle>(
                    new Parser<Angle>($@"^({WordPlay.RegexDouble}) ?(turns?|t)$", m => new Angle(double.Parse(m.Groups[1].Value), Turn)),
                    new Parser<Angle>($@"^({WordPlay.RegexDouble}) ?(°|degrees?|d)$", m => new Angle(double.Parse(m.Groups[1].Value), Degree)),
                    new Parser<Angle>($@"^({WordPlay.RegexDouble}) ?(rad|㎭|radians?|c|r)$", m => new Angle(double.Parse(m.Groups[1].Value), Radian)),
                    new Parser<Angle>($@"^({WordPlay.RegexDouble}) ?(grad|g|gradians?|gon)$", m => new Angle(double.Parse(m.Groups[1].Value), Gradian))
                    ));
            public static Angle Parse(string s)
            {
                return DefaultParsers.Value.Process(s);
            }

            public static readonly Angle Radian = new Angle(1), Degree = new Angle(Math.PI / 180), Turn = new Angle(2 * Math.PI),
                                         Gradian = new Angle(Math.PI / 200), RightAngle = new Angle(Math.PI / 2);
            public static RotationalSpeed operator /(Angle a, TimeSpan b)
            {
                return new RotationalSpeed(a.Arbitrary / b.TotalSeconds);
            }
            public static Angle operator -(Angle a)
            {
                return (-1.0 * a);
            }
            public static Angle operator *(Angle a, double b)
            {
                return new Angle(a.Arbitrary * b);
            }
            public static Angle operator /(Angle a, double b)
            {
                return a * (1 / b);
            }
            public static Angle operator *(double b, Angle a)
            {
                return a * b;
            }
            public static Angle operator +(Angle a, Angle b)
            {
                double c = a.Arbitrary + b.Arbitrary;
                return new Angle(c);
            }
            public static Angle operator -(Angle a, Angle b)
            {
                return a + (-b);
            }
            public static double operator /(Angle a, Angle b)
            {
                return a.Arbitrary / b.Arbitrary;
            }
            public override string ToString()
            {
                return this.ToString("");
            }
            //accepted formats (R|D|G|T)_{double format}_{symbol}
            public string ToString(string format, IFormatProvider formatProvider)
            {
                IDictionary<string, Tuple<IScaleUnit<Angle>, string>> unitDictionary = new Dictionary<string, Tuple<IScaleUnit<Angle>, string>>(4);
                unitDictionary["R"] = Tuple.Create<IScaleUnit<Angle>, string>(Radian, "rad");
                unitDictionary["D"] = Tuple.Create<IScaleUnit<Angle>, string>(Degree, "\u00b0");
                unitDictionary["G"] = Tuple.Create<IScaleUnit<Angle>, string>(Gradian, "gon");
                unitDictionary["T"] = Tuple.Create<IScaleUnit<Angle>, string>(Turn, "\u03c4");
                return this.StringFromUnitDictionary(format, "R", formatProvider, unitDictionary);
            }
            public override int GetHashCode()
            {
                return Arbitrary.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                var an = obj as Angle;
                return an != null && an.Arbitrary == this.Arbitrary;
            }
        }
    }
    namespace Lengths
    {
        //arbitrary is meters
        public class Length : IUnit<Length>, ScaleMeasurement, DeltaMeasurement, IComparable<Length>
        {
            public Length(double val, IUnit<Length> unit) : this(unit.ToArbitrary(val)) { }
            public Length(double arbitrary)
            {
                this.Arbitrary = arbitrary;
            }
            public double Arbitrary { get; }
            double ScaleMeasurement.Arbitrary
            {
                get
                {
                    return this.Arbitrary;
                }
            }
            double DeltaMeasurement.Arbitrary
            {
                get
                {
                    return this.Arbitrary;
                }
            }
            public override double FromArbitrary(double arb)
            {
                return arb / Arbitrary;
            }
            public override double ToArbitrary(double val)
            {
                return val * Arbitrary;
            }
            public int CompareTo(Length other)
            {
                return Arbitrary.CompareTo(other.Arbitrary);
            }

            private static readonly Lazy<Funnel<string, Length>> DefaultParsers;
            public static Length Parse(string s)
            {
                return DefaultParsers.Value.Process(s);
            }

            public static readonly Length Meter, CentiMeter, MilliMeter, KiloMeter, Foot, Yard, Mile, LightSecond, LightYear, Parsec, AstronomicalUnit;
            static Length()
            {
                Meter = new Length(1);
                CentiMeter = new Length(0.01);
                MilliMeter = new Length(0.001);
                KiloMeter = new Length(1000);
                Foot = new Length(0.3048);
                Yard = new Length(3, Foot);
                Mile = new Length(1760, Yard);
                LightSecond = new Length(299792458);
                LightYear = new Length(9460730472580800);
                AstronomicalUnit = new Length(149597870700);
                Parsec = new Length(648000 / Math.PI, AstronomicalUnit);
                DefaultParsers = new Lazy<Funnel<string, Length>>(() => new Funnel<string, Length>(
                    new Parser<Length>($@"^({WordPlay.RegexDouble}) ?(m|meters?)$", m => new Length(double.Parse(m.Groups[1].Value), Meter)),
                    new Parser<Length>($@"^({WordPlay.RegexDouble}) ?(cm|centimeters?)$", m => new Length(double.Parse(m.Groups[1].Value), CentiMeter)),
                    new Parser<Length>($@"^({WordPlay.RegexDouble}) ?(mm|millimeters?)$", m => new Length(double.Parse(m.Groups[1].Value), MilliMeter)),
                    new Parser<Length>($@"^({WordPlay.RegexDouble}) ?(km|kilometers?)$", m => new Length(double.Parse(m.Groups[1].Value), KiloMeter)),
                    new Parser<Length>($@"^({WordPlay.RegexDouble}) ?(ft|foot|feet)$", m => new Length(double.Parse(m.Groups[1].Value), Foot)),
                    new Parser<Length>($@"^({WordPlay.RegexDouble}) ?(yd|yards?)$", m => new Length(double.Parse(m.Groups[1].Value), Yard)),
                    new Parser<Length>($@"^({WordPlay.RegexDouble}) ?(mi|miles?)$", m => new Length(double.Parse(m.Groups[1].Value), Mile)),
                    new Parser<Length>($@"^({WordPlay.RegexDouble}) ?(au|astronomical units?)$",
                        m => new Length(double.Parse(m.Groups[1].Value), AstronomicalUnit))
                    ));
            }
            public static Length operator -(Length a)
            {
                return (-1.0 * a);
            }
            public static Length operator *(Length a, double b)
            {
                return new Length(a.Arbitrary * b);
            }
            public static Length operator /(Length a, double b)
            {
                return a * (1 / b);
            }
            public static Length operator *(double b, Length a)
            {
                return a * b;
            }
            public static Length operator +(Length a, Length b)
            {
                double c = a.Arbitrary + b.Arbitrary;
                return new Length(c);
            }
            public static Length operator -(Length a, Length b)
            {
                return a + (-b);
            }
            public static double operator /(Length a, Length b)
            {
                return a.Arbitrary / b.Arbitrary;
            }
            public override string ToString()
            {
                return this.ToString("");
            }
            //accepted formats (M|CM|MM|KM|F|Y|MI|LS|LY|P|AU)_{double format}_{symbol}
            public string ToString(string format, IFormatProvider formatProvider)
            {
                IDictionary<string, Tuple<IScaleUnit<Length>, string>> unitDictionary = new Dictionary<string, Tuple<IScaleUnit<Length>, string>>(11);
                unitDictionary["M"] = Tuple.Create<IScaleUnit<Length>, string>(Meter, "m");
                unitDictionary["CM"] = Tuple.Create<IScaleUnit<Length>, string>(CentiMeter, "cm");
                unitDictionary["MM"] = Tuple.Create<IScaleUnit<Length>, string>(MilliMeter, "mm");
                unitDictionary["KM"] = Tuple.Create<IScaleUnit<Length>, string>(KiloMeter, "km");
                unitDictionary["F"] = Tuple.Create<IScaleUnit<Length>, string>(Foot, "f");
                unitDictionary["Y"] = Tuple.Create<IScaleUnit<Length>, string>(Yard, "yd");
                unitDictionary["MI"] = Tuple.Create<IScaleUnit<Length>, string>(Mile, "mi");
                unitDictionary["LS"] = Tuple.Create<IScaleUnit<Length>, string>(LightSecond, "ls");
                unitDictionary["LY"] = Tuple.Create<IScaleUnit<Length>, string>(LightYear, "ly");
                unitDictionary["P"] = Tuple.Create<IScaleUnit<Length>, string>(Parsec, "p");
                unitDictionary["AU"] = Tuple.Create<IScaleUnit<Length>, string>(AstronomicalUnit, "au");
                return this.StringFromUnitDictionary(format, "M", formatProvider, unitDictionary);
            }
            public override int GetHashCode()
            {
                return Arbitrary.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                var an = obj as Length;
                return an != null && an.Arbitrary == this.Arbitrary;
            }
        }
    }
    namespace Time
    {
        public static class TimeExtentions
        {
            public enum TimeRoundPoint
            {
                Days,
                Hours,
                Minutes,
                Seconds,
                None
            }
            public static TimeSpan Divide(this TimeSpan t, double divisor)
            {
                return multiply(t, 1.0 / divisor);
            }
            public static double Divide(this TimeSpan t, TimeSpan divisor)
            {
                return (t.Ticks / (double)divisor.Ticks);
            }
            public static TimeSpan multiply(this TimeSpan t, double factor)
            {
                return new TimeSpan((long)(t.Ticks * factor));
            }
        }
    }
    namespace Masses
    {
        //arbitrary is kilogram
        public class Mass : IUnit<Mass>, ScaleMeasurement, DeltaMeasurement, IComparable<Mass>
        {
            public Mass(double val, IUnit<Mass> unit) : this(unit.ToArbitrary(val)) { }
            public Mass(double arbitrary)
            {
                this.Arbitrary = arbitrary;
            }
            public double Arbitrary { get; }
            double ScaleMeasurement.Arbitrary
            {
                get
                {
                    return this.Arbitrary;
                }
            }
            double DeltaMeasurement.Arbitrary
            {
                get
                {
                    return this.Arbitrary;
                }
            }
            public override double FromArbitrary(double arb)
            {
                return arb / Arbitrary;
            }
            public override double ToArbitrary(double val)
            {
                return val * Arbitrary;
            }
            public int CompareTo(Mass other)
            {
                return Arbitrary.CompareTo(other.Arbitrary);
            }

            private static readonly Lazy<Funnel<string, Mass>> DefaultParsers;
            public static Mass Parse(string s)
            {
                return DefaultParsers.Value.Process(s);
            }

            public static readonly Mass Milligram, Gram, KiloGram, Tonne, Ounce, Pound;
            static Mass()
            {
                KiloGram = new Mass(1);
                Gram = new Mass(0.001);
                Tonne = new Mass(1000);
                Milligram = new Mass(1E-06);
                Pound = new Mass(0.45359237);
                Ounce = new Mass(28.349523125, Gram);
                DefaultParsers = new Lazy<Funnel<string, Mass>>(() => new Funnel<string, Mass>(
                    new Parser<Mass>($@"^({WordPlay.RegexDouble}) ?(kg?|kilograms?)$", m => new Mass(double.Parse(m.Groups[1].Value), KiloGram)),
                    new Parser<Mass>($@"^({WordPlay.RegexDouble}) ?(g|grams?)$", m => new Mass(double.Parse(m.Groups[1].Value), Gram)),
                    new Parser<Mass>($@"^({WordPlay.RegexDouble}) ?(mg|milligrams?)$", m => new Mass(double.Parse(m.Groups[1].Value), Milligram)),
                    new Parser<Mass>($@"^({WordPlay.RegexDouble}) ?(t|tons?)$", m => new Mass(double.Parse(m.Groups[1].Value), Tonne)),
                    new Parser<Mass>($@"^({WordPlay.RegexDouble}) ?(oz|ounces?)$", m => new Mass(double.Parse(m.Groups[1].Value), Ounce)),
                    new Parser<Mass>($@"^({WordPlay.RegexDouble}) ?(lb|pounds?)$", m => new Mass(double.Parse(m.Groups[1].Value), Pound))
                    ));
            }
            public static Mass operator -(Mass a)
            {
                return (-1.0 * a);
            }
            public static Mass operator *(Mass a, double b)
            {
                return new Mass(a.Arbitrary * b);
            }
            public static Mass operator /(Mass a, double b)
            {
                return a * (1 / b);
            }
            public static Mass operator *(double b, Mass a)
            {
                return a * b;
            }
            public static Mass operator +(Mass a, Mass b)
            {
                double c = a.Arbitrary + b.Arbitrary;
                return new Mass(c);
            }
            public static Mass operator -(Mass a, Mass b)
            {
                return a + (-b);
            }
            public static double operator /(Mass a, Mass b)
            {
                return a.Arbitrary / b.Arbitrary;
            }
            public override string ToString()
            {
                return this.ToString("");
            }
            //accepted formats (K|M|G|T|O|L)_{double format}_{symbol}
            public string ToString(string format, IFormatProvider formatProvider)
            {
                IDictionary<string, Tuple<IScaleUnit<Mass>, string>> unitDictionary = new Dictionary<string, Tuple<IScaleUnit<Mass>, string>>(11);
                unitDictionary["K"] = Tuple.Create<IScaleUnit<Mass>, string>(KiloGram, "kg");
                unitDictionary["M"] = Tuple.Create<IScaleUnit<Mass>, string>(Milligram, "mg");
                unitDictionary["G"] = Tuple.Create<IScaleUnit<Mass>, string>(Gram, "g");
                unitDictionary["T"] = Tuple.Create<IScaleUnit<Mass>, string>(Tonne, "t");
                unitDictionary["O"] = Tuple.Create<IScaleUnit<Mass>, string>(Ounce, "oz");
                unitDictionary["L"] = Tuple.Create<IScaleUnit<Mass>, string>(Pound, "lb");
                return this.StringFromUnitDictionary(format, "K", formatProvider, unitDictionary);
            }
            public override int GetHashCode()
            {
                return Arbitrary.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                var an = obj as Mass;
                return an != null && an.Arbitrary == this.Arbitrary;
            }
        }
    }
    namespace DataSizes
    {
        //arbitrary is megabyte
        public class DataSize : IUnit<DataSize>, ScaleMeasurement, DeltaMeasurement, IComparable<DataSize>
        {
            public DataSize(double val, IUnit<DataSize> unit) : this(unit.ToArbitrary(val)) { }
            public DataSize(double arbitrary)
            {
                this.Arbitrary = arbitrary;
            }
            public double Arbitrary { get; }
            double ScaleMeasurement.Arbitrary
            {
                get
                {
                    return this.Arbitrary;
                }
            }
            double DeltaMeasurement.Arbitrary
            {
                get
                {
                    return this.Arbitrary;
                }
            }
            public override double FromArbitrary(double arb)
            {
                return arb / Arbitrary;
            }
            public override double ToArbitrary(double val)
            {
                return val * Arbitrary;
            }
            public int CompareTo(DataSize other)
            {
                return Arbitrary.CompareTo(other.Arbitrary);
            }

            private static readonly Lazy<Funnel<string, DataSize>> DefaultParsers;
            public static DataSize Parse(string s)
            {
                return DefaultParsers.Value.Process(s);
            }

            public static readonly DataSize Bit, Byte, Kilobyte, Megabyte, Gigabyte, Terrabyte, Pettabyte, Exabyte, Zettabyte, Yottabyte;
            static DataSize()
            {
                Megabyte = new DataSize(1);
                Gigabyte = Megabyte * 1024;
                Terrabyte = Gigabyte * 1024;
                Pettabyte = Terrabyte * 1024;
                Exabyte = Pettabyte * 1024;
                Zettabyte = Exabyte * 1024;
                Yottabyte = Zettabyte * 1024;

                Kilobyte = Megabyte / 1024;
                Byte = Kilobyte / 1024;
                Bit = Byte / 8;

                DefaultParsers = new Lazy<Funnel<string, DataSize>>(() => new Funnel<string, DataSize>(
                    new Parser<DataSize>($@"^({WordPlay.RegexDouble}) ?(b|bits?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Bit)),
                    new Parser<DataSize>($@"^({WordPlay.RegexDouble}) ?(B|bytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Byte)),
                    new Parser<DataSize>($@"^({WordPlay.RegexDouble}) ?(KB|kilobytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Kilobyte)),
                    new Parser<DataSize>($@"^({WordPlay.RegexDouble}) ?(MB|megabytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Megabyte)),
                    new Parser<DataSize>($@"^({WordPlay.RegexDouble}) ?(GB|gigabytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Gigabyte)),
                    new Parser<DataSize>($@"^({WordPlay.RegexDouble}) ?(TB|terrabytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Terrabyte)),
                    new Parser<DataSize>($@"^({WordPlay.RegexDouble}) ?(PB|pettabytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Pettabyte)),
                    new Parser<DataSize>($@"^({WordPlay.RegexDouble}) ?(EB|exabytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Exabyte)),
                    new Parser<DataSize>($@"^({WordPlay.RegexDouble}) ?(ZB|zettabytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Zettabyte)),
                    new Parser<DataSize>($@"^({WordPlay.RegexDouble}) ?(YB|yottabytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Yottabyte))
                    ));
            }
            public static DataSize operator -(DataSize a)
            {
                return (-1.0 * a);
            }
            public static DataSize operator *(DataSize a, double b)
            {
                return new DataSize(a.Arbitrary * b);
            }
            public static DataSize operator /(DataSize a, double b)
            {
                return a * (1 / b);
            }
            public static DataSize operator *(double b, DataSize a)
            {
                return a * b;
            }
            public static DataSize operator +(DataSize a, DataSize b)
            {
                double c = a.Arbitrary + b.Arbitrary;
                return new DataSize(c);
            }
            public static DataSize operator -(DataSize a, DataSize b)
            {
                return a + (-b);
            }
            public static double operator /(DataSize a, DataSize b)
            {
                return a.Arbitrary / b.Arbitrary;
            }
            public override string ToString()
            {
                return this.ToString("");
            }
            //accepted formats (b|B|K|M|G|T|P|E|Z|Y)_{double format}_{symbol}
            public string ToString(string format, IFormatProvider formatProvider)
            {
                IDictionary<string, Tuple<IScaleUnit<DataSize>, string>> unitDictionary = new Dictionary<string, Tuple<IScaleUnit<DataSize>, string>>(11);
                unitDictionary["b"] = Tuple.Create<IScaleUnit<DataSize>, string>(Bit, "b");
                unitDictionary["B"] = Tuple.Create<IScaleUnit<DataSize>, string>(Byte, "B");
                unitDictionary["K"] = Tuple.Create<IScaleUnit<DataSize>, string>(Kilobyte, "KB");
                unitDictionary["M"] = Tuple.Create<IScaleUnit<DataSize>, string>(Megabyte, "MB");
                unitDictionary["G"] = Tuple.Create<IScaleUnit<DataSize>, string>(Gigabyte, "GB");
                unitDictionary["T"] = Tuple.Create<IScaleUnit<DataSize>, string>(Terrabyte, "TB");
                unitDictionary["P"] = Tuple.Create<IScaleUnit<DataSize>, string>(Pettabyte, "PB");
                unitDictionary["E"] = Tuple.Create<IScaleUnit<DataSize>, string>(Exabyte, "EB");
                unitDictionary["Z"] = Tuple.Create<IScaleUnit<DataSize>, string>(Zettabyte, "ZB");
                unitDictionary["Y"] = Tuple.Create<IScaleUnit<DataSize>, string>(Yottabyte, "YB");
                return this.StringFromUnitDictionary(format, "M", formatProvider, unitDictionary);
            }
            public override int GetHashCode()
            {
                return Arbitrary.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                var an = obj as DataSize;
                return an != null && an.Arbitrary == this.Arbitrary;
            }
        }
    }
    namespace Moneys
    {
        //arbitrary is euro
        public class Money : IUnit<Money>, ScaleMeasurement, DeltaMeasurement, IComparable<Money>
        {
            public Money(double val, IUnit<Money> unit) : this(unit.ToArbitrary(val)) { }
            public Money(double arbitrary)
            {
                this.Arbitrary = arbitrary;
            }
            public double Arbitrary { get; }
            double ScaleMeasurement.Arbitrary
            {
                get
                {
                    return this.Arbitrary;
                }
            }
            double DeltaMeasurement.Arbitrary
            {
                get
                {
                    return this.Arbitrary;
                }
            }
            public override double FromArbitrary(double arb)
            {
                return arb / Arbitrary;
            }
            public override double ToArbitrary(double val)
            {
                return val * Arbitrary;
            }
            public int CompareTo(Money other)
            {
                return Arbitrary.CompareTo(other.Arbitrary);
            }

            private static readonly Lazy<Funnel<string, Money>> DefaultParsers;
            public static Money Parse(string s)
            {
                return DefaultParsers.Value.Process(s);
            }

            private static readonly SyncPermaObject<string> _exchangeRatePerma;
            private static bool _initialized;
            private static readonly string _exchangeRatesPermaPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\WhetStone\" + "__WhetStone_Units_Money_ExchangeRates.xml";
            // ReSharper disable once InconsistentNaming
            public static Money DollarUS { get; private set; }
            public static Money NewShekel { get; private set; }
            public static Money Euro { get; }
            public static Money Yen { get; private set; }
            static Money()
            {
                Euro = new Money(1);
                DollarUS = new Money(1/1.12);
                NewShekel = new Money(0.26, DollarUS);
                Yen = new Money(0.009, DollarUS);

                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(_exchangeRatesPermaPath));
                _exchangeRatePerma = new SyncPermaObject<string>(Encoding.ASCII.GetString, Encoding.ASCII.GetBytes, _exchangeRatesPermaPath, false, FileAccess.ReadWrite, FileShare.ReadWrite, valueIfCreated: "");

                DefaultParsers = new Lazy<Funnel<string, Money>>(() => new Funnel<string, Money>(
                    new Parser<Money>($@"^({WordPlay.RegexDouble}) ?(\$|dollars?)$", m => new Money(double.Parse(m.Groups[1].Value), DollarUS)),
                    new Parser<Money>($@"^\$({WordPlay.RegexDouble})$", m => new Money(double.Parse(m.Groups[1].Value), DollarUS)),
                    new Parser<Money>($@"^({WordPlay.RegexDouble}) ?(INS|ins|Israeli New Sheckels?|israeli new sheckel)$", m => new Money(double.Parse(m.Groups[1].Value), NewShekel)),
                    new Parser<Money>($@"^₪({WordPlay.RegexDouble})$", m => new Money(double.Parse(m.Groups[1].Value), NewShekel)),
                    new Parser<Money>($@"^({WordPlay.RegexDouble}) ?(euros?)$", m => new Money(double.Parse(m.Groups[1].Value), Euro)),
                    new Parser<Money>($@"^€({WordPlay.RegexDouble})$", m => new Money(double.Parse(m.Groups[1].Value), Euro)),
                    new Parser<Money>($@"^({WordPlay.RegexDouble}) ?(yen)$", m => new Money(double.Parse(m.Groups[1].Value), Yen)),
                    new Parser<Money>($@"^¥({WordPlay.RegexDouble})$", m => new Money(double.Parse(m.Groups[1].Value), Yen))
                    ));
            }

            public static bool updateRates()
            {
                
                Exception error;
                return updateRates(out error);
            }
            public static bool updateRates( out Exception error)
            {
                return updateRates(TimeSpan.FromHours(12), out error);
            }
            public static bool updateRates(TimeSpan updateTimeTolerance,  out Exception error)
            {
                error = null;
                if (_exchangeRatePerma.timeSinceUpdate() < updateTimeTolerance && _initialized)
                    return false;
                var doc = (_exchangeRatePerma.timeSinceUpdate() < updateTimeTolerance && _exchangeRatePerma.value.Length > 0) ? XPathMarksman.getDocFromString(_exchangeRatePerma.value) : loadXml(out error);
                if (error != null)
                {
                    return false;
                }
                XmlElement root = doc.DocumentElement;
                XmlNamespaceManager nsm = new XmlNamespaceManager(doc.NameTable);
                nsm.AddNamespace("gesmes", @"http://www.gesmes.org/xml/2002-08-01");
                nsm.AddNamespace("def", @"http://www.ecb.int/vocabulary/2002-08-01/eurofxref");
                DollarUS = new Money(1.0 / getrate(root, "USD", nsm));
                NewShekel = new Money(1.0 / getrate(root, "ILS", nsm));
                Yen = new Money(1.0 / getrate(root, "JPY", nsm));
                _initialized = true;
                return true;
            }
            private static XmlDocument loadXml(out Exception error)
            {
                XmlDocument doc = WebGuard.LoadXmlDocumentFromUrl(@"http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml", out error);
                _exchangeRatePerma.value = doc.InnerXml;
                return doc;
            }
            private static double getrate(XmlNode root, string identifier, XmlNamespaceManager xnsm)
            {
                return double.Parse(root.SelectSingleNode("//def:Cube[@currency=\"" + identifier + "\"]", xnsm).Attributes["rate"].InnerText);
            }

            public static Money operator -(Money a)
            {
                return (-1.0 * a);
            }
            public static Money operator *(Money a, double b)
            {
                return new Money(a.Arbitrary * b);
            }
            public static Money operator /(Money a, double b)
            {
                return a * (1 / b);
            }
            public static Money operator *(double b, Money a)
            {
                return a * b;
            }
            public static Money operator +(Money a, Money b)
            {
                double c = a.Arbitrary + b.Arbitrary;
                return new Money(c);
            }
            public static Money operator -(Money a, Money b)
            {
                return a + (-b);
            }
            public static double operator /(Money a, Money b)
            {
                return a.Arbitrary / b.Arbitrary;
            }
            public override string ToString()
            {
                return this.ToString("");
            }
            //accepted formats (D|S|E|Y)_{double format}_{symbol}
            public string ToString(string format, IFormatProvider formatProvider)
            {
                IDictionary<string, Tuple<IScaleUnit<Money>, string>> unitDictionary = new Dictionary<string, Tuple<IScaleUnit<Money>, string>>(11);
                unitDictionary["D"] = Tuple.Create<IScaleUnit<Money>, string>(DollarUS, "$");
                unitDictionary["S"] = Tuple.Create<IScaleUnit<Money>, string>(NewShekel, "₪");
                unitDictionary["E"] = Tuple.Create<IScaleUnit<Money>, string>(Euro, "€");
                unitDictionary["Y"] = Tuple.Create<IScaleUnit<Money>, string>(Yen, "¥");
                return this.StringFromUnitDictionary(format, "E", formatProvider, unitDictionary,true);
            }
            public override int GetHashCode()
            {
                return Arbitrary.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                var an = obj as Money;
                return an != null && an.Arbitrary == this.Arbitrary;
            }
        }
    }
    namespace Temperature
    {
        //arbitrary is kelvin
        public class Temperature : ScaleMeasurement, IComparable<Temperature>
        {
            public Temperature(double val, IScaleUnit<Temperature> unit) : this(unit.ToArbitrary(val)) { }
            public Temperature(double arbitrary)
            {
                if (arbitrary < 0)
                    throw new Exception("temperature scale cannot be negative");
                this.Arbitrary = arbitrary;
            }
            public double Arbitrary { get; }
            public int CompareTo(Temperature other)
            {
                return Arbitrary.CompareTo(other.Arbitrary);
            }

            private static readonly Lazy<Funnel<string, Temperature>> DefaultParsers;
            public static Temperature Parse(string s)
            {
                return DefaultParsers.Value.Process(s);
            }

            public static readonly IScaleUnit<Temperature> Kelvin, Fahrenheit, Celsius;
            static Temperature()
            {
                Kelvin = new ScaleUnit<Temperature>(1);
                Celsius = new ScaleUnit<Temperature>(1, -273.15);
                Fahrenheit = new ScaleUnit<Temperature>(9/5.0, -459.67);
                DefaultParsers = new Lazy<Funnel<string, Temperature>>(() => new Funnel<string, Temperature>(
                    new Parser<Temperature>($@"^({WordPlay.RegexDouble}) ?(k|kelvin)$", m => new Temperature(double.Parse(m.Groups[1].Value), Kelvin)),
                    new Parser<Temperature>($@"^({WordPlay.RegexDouble}) ?(f|fahrenheit)$", m => new Temperature(double.Parse(m.Groups[1].Value), Fahrenheit)),
                    new Parser<Temperature>($@"^({WordPlay.RegexDouble}) ?(c|celsius)$", m => new Temperature(double.Parse(m.Groups[1].Value), Celsius))
                    ));
            }
            public static Temperature operator +(Temperature t, TemperatureDelta d)
            {
                return new Temperature(t.Arbitrary+d.Arbitrary);
            }
            public static Temperature operator +(TemperatureDelta d, Temperature t)
            {
                return new Temperature(t.Arbitrary + d.Arbitrary);
            }
            public static Temperature operator -(Temperature t, TemperatureDelta d)
            {
                return new Temperature(t.Arbitrary - d.Arbitrary);
            }
            public static TemperatureDelta operator -(Temperature t, Temperature d)
            {
                return new TemperatureDelta(t.Arbitrary - d.Arbitrary);
            }
            public override string ToString()
            {
                return this.ToString("");
            }
            //accepted formats (K|F|C)_{double format}_{symbol}
            public string ToString(string format, IFormatProvider formatProvider)
            {
                IDictionary<string, Tuple<IScaleUnit<Temperature>, string>> unitDictionary = new Dictionary<string, Tuple<IScaleUnit<Temperature>, string>>(11);
                unitDictionary["K"] = Tuple.Create(Kelvin, "K");
                unitDictionary["F"] = Tuple.Create(Fahrenheit, "F");
                unitDictionary["C"] = Tuple.Create(Celsius, "C");
                return this.StringFromUnitDictionary(format, "C", formatProvider, unitDictionary);
            }
            public override int GetHashCode()
            {
                return Arbitrary.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                var an = obj as Temperature;
                return an != null && an.Arbitrary == this.Arbitrary;
            }
        }
        public class TemperatureDelta : DeltaMeasurement, IDeltaUnit<TemperatureDelta>, IComparable<TemperatureDelta>
        {
            public TemperatureDelta(double val, IDeltaUnit<TemperatureDelta> unit) : this(unit.ToArbitrary(val)) { }
            public TemperatureDelta(double arbitrary)
            {
                this.Arbitrary = arbitrary;
            }
            public double Arbitrary { get; }
            public int CompareTo(TemperatureDelta other)
            {
                return Arbitrary.CompareTo(other.Arbitrary);
            }
            double DeltaMeasurement.Arbitrary
            {
                get
                {
                    return this.Arbitrary;
                }
            }
            public double FromArbitrary(double arb)
            {
                return arb * Arbitrary;
            }
            public double ToArbitrary(double val)
            {
                return val / Arbitrary;
            }

            private static readonly Lazy<Funnel<string, TemperatureDelta>> DefaultParsers;
            public static TemperatureDelta Parse(string s)
            {
                return DefaultParsers.Value.Process(s);
            }

            public static readonly TemperatureDelta Kelvin, Fahrenheit, Celsius;
            static TemperatureDelta()
            {
                Kelvin = new TemperatureDelta(1);
                Celsius = Kelvin;
                Fahrenheit = new TemperatureDelta(9/5.0);
                DefaultParsers = new Lazy<Funnel<string, TemperatureDelta>>(() => new Funnel<string, TemperatureDelta>(
                    new Parser<TemperatureDelta>($@"^({WordPlay.RegexDouble}) ?(k|kelvin)$", m => new TemperatureDelta(double.Parse(m.Groups[1].Value), Kelvin)),
                    new Parser<TemperatureDelta>($@"^({WordPlay.RegexDouble}) ?(f|fahrenheit)$", m => new TemperatureDelta(double.Parse(m.Groups[1].Value), Fahrenheit)),
                    new Parser<TemperatureDelta>($@"^({WordPlay.RegexDouble}) ?(c|celsius)$", m => new TemperatureDelta(double.Parse(m.Groups[1].Value), Celsius))
                    ));
            }
            public static TemperatureDelta operator -(TemperatureDelta a)
            {
                return (-1.0 * a);
            }
            public static TemperatureDelta operator *(TemperatureDelta a, double b)
            {
                return new TemperatureDelta(a.Arbitrary * b);
            }
            public static TemperatureDelta operator /(TemperatureDelta a, double b)
            {
                return a * (1 / b);
            }
            public static TemperatureDelta operator *(double b, TemperatureDelta a)
            {
                return a * b;
            }
            public static TemperatureDelta operator +(TemperatureDelta a, TemperatureDelta b)
            {
                double c = a.Arbitrary + b.Arbitrary;
                return new TemperatureDelta(c);
            }
            public static TemperatureDelta operator -(TemperatureDelta a, TemperatureDelta b)
            {
                return a + (-b);
            }
            public static double operator /(TemperatureDelta a, TemperatureDelta b)
            {
                return a.Arbitrary / b.Arbitrary;
            }
            public override string ToString()
            {
                return this.ToString("");
            }
            //accepted formats (K|F|C)_{double format}_{symbol}
            public string ToString(string format, IFormatProvider formatProvider)
            {
                IDictionary<string, Tuple<IDeltaUnit<TemperatureDelta>, string>> unitDictionary = new Dictionary<string, Tuple<IDeltaUnit<TemperatureDelta>, string>>(11);
                unitDictionary["K"] = Tuple.Create<IDeltaUnit<TemperatureDelta>, string>(Kelvin, "K");
                unitDictionary["F"] = Tuple.Create<IDeltaUnit<TemperatureDelta>, string>(Fahrenheit, "F");
                unitDictionary["C"] = Tuple.Create<IDeltaUnit<TemperatureDelta>, string>(Celsius, "C");
                return this.StringFromDeltaDictionary(format, "C", formatProvider, unitDictionary);
            }
            public override int GetHashCode()
            {
                return Arbitrary.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                var an = obj as TemperatureDelta;
                return an != null && an.Arbitrary == this.Arbitrary;
            }
        }
    }
    namespace GraphicDistances
    {
        public class GraphicsLength : IUnit<GraphicsLength>, ScaleMeasurement, DeltaMeasurement, IComparable<GraphicsLength>
        {
            public GraphicsLength(double val, IDeltaUnit<GraphicsLength> unit) : this(unit.ToArbitrary(val)) { }
            public GraphicsLength(double arbitrary)
            {
                this.Arbitrary = arbitrary;
            }
            public double Arbitrary { get; }
            public int CompareTo(GraphicsLength other)
            {
                return Arbitrary.CompareTo(other.Arbitrary);
            }
            double DeltaMeasurement.Arbitrary
            {
                get
                {
                    return this.Arbitrary;
                }
            }
            public override double FromArbitrary(double arb)
            {
                return arb / Arbitrary;
            }
            public override double ToArbitrary(double val)
            {
                return val * Arbitrary;
            }

            private static readonly Lazy<Funnel<string, GraphicsLength>> DefaultParsers;
            public static GraphicsLength Parse(string s)
            {
                return DefaultParsers.Value.Process(s);
            }

            public static readonly GraphicsLength Pixel;
            static GraphicsLength()
            {
                Pixel = new GraphicsLength(1);
                DefaultParsers = new Lazy<Funnel<string, GraphicsLength>>(() => new Funnel<string, GraphicsLength>(
                    new Parser<GraphicsLength>($@"^({WordPlay.RegexDouble}) ?(p|pixels?)$", m => new GraphicsLength(double.Parse(m.Groups[1].Value), Pixel))
                    ));
            }
            public static GraphicsLength operator -(GraphicsLength a)
            {
                return (-1.0 * a);
            }
            public static GraphicsLength operator *(GraphicsLength a, double b)
            {
                return new GraphicsLength(a.Arbitrary * b);
            }
            public static GraphicsLength operator /(GraphicsLength a, double b)
            {
                return a * (1 / b);
            }
            public static GraphicsLength operator *(double b, GraphicsLength a)
            {
                return a * b;
            }
            public static GraphicsLength operator +(GraphicsLength a, GraphicsLength b)
            {
                double c = a.Arbitrary + b.Arbitrary;
                return new GraphicsLength(c);
            }
            public static GraphicsLength operator -(GraphicsLength a, GraphicsLength b)
            {
                return a + (-b);
            }
            public static double operator /(GraphicsLength a, GraphicsLength b)
            {
                return a.Arbitrary / b.Arbitrary;
            }
            public override string ToString()
            {
                return this.ToString("");
            }
            //accepted formats (P)_{double format}_{symbol}
            public string ToString(string format, IFormatProvider formatProvider)
            {
                IDictionary<string, Tuple<IDeltaUnit<GraphicsLength>, string>> unitDictionary = new Dictionary<string, Tuple<IDeltaUnit<GraphicsLength>, string>>(1);
                unitDictionary["P"] = Tuple.Create<IDeltaUnit<GraphicsLength>, string>(Pixel, "P");
                return this.StringFromDeltaDictionary(format, "P", formatProvider, unitDictionary);
            }
            public override int GetHashCode()
            {
                return Arbitrary.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                var an = obj as GraphicsLength;
                return an != null && an.Arbitrary == this.Arbitrary;
            }
        }
    }
    namespace Frequencies
    {
        //arbitrary is hertz
        public class Frequency : IUnit<Frequency>, ScaleMeasurement, DeltaMeasurement, IComparable<Frequency>
        {
            public Frequency(double val, IDeltaUnit<Frequency> unit) : this(unit.ToArbitrary(val)) { }
            public Frequency(double arbitrary)
            {
                this.Arbitrary = arbitrary;
            }
            public double Arbitrary { get; }
            public int CompareTo(Frequency other)
            {
                return Arbitrary.CompareTo(other.Arbitrary);
            }
            double DeltaMeasurement.Arbitrary
            {
                get
                {
                    return this.Arbitrary;
                }
            }
            public override double FromArbitrary(double arb)
            {
                return arb / Arbitrary;
            }
            public override double ToArbitrary(double val)
            {
                return val * Arbitrary;
            }

            private static readonly Lazy<Funnel<string, Frequency>> DefaultParsers;
            public static Frequency Parse(string s)
            {
                return DefaultParsers.Value.Process(s);
            }

            // ReSharper disable once InconsistentNaming
            public static readonly Frequency Hertz, GigaHertz, MegaHertz;
            static Frequency()
            {
                Hertz = new Frequency(1);
                MegaHertz = new Frequency(1000);
                GigaHertz = new Frequency(1000*1000);
                DefaultParsers = new Lazy<Funnel<string, Frequency>>(() => new Funnel<string, Frequency>(
                    new Parser<Frequency>($@"^({WordPlay.RegexDouble}) ?(hz|hertz)$", m => new Frequency(double.Parse(m.Groups[1].Value), Hertz)),
                    new Parser<Frequency>($@"^({WordPlay.RegexDouble}) ?(mhz|megahertz)$", m => new Frequency(double.Parse(m.Groups[1].Value), MegaHertz)),
                    new Parser<Frequency>($@"^({WordPlay.RegexDouble}) ?(ghz|gigahertz)$", m => new Frequency(double.Parse(m.Groups[1].Value), GigaHertz))
                    ));
            }
            public static double operator *(Frequency a, TimeSpan b)
            {
                return a.Arbitrary * b.TotalSeconds;
            }
            public static TimeSpan operator /(double a, Frequency b)
            {
                return TimeSpan.FromSeconds(a / b.Arbitrary);
            }
            public static double operator *(TimeSpan b, Frequency a)
            {
                return a.Arbitrary * b.TotalSeconds;
            }
            public static Frequency operator -(Frequency a)
            {
                return (-1.0 * a);
            }
            public static Frequency operator *(Frequency a, double b)
            {
                return new Frequency(a.Arbitrary * b);
            }
            public static Frequency operator /(Frequency a, double b)
            {
                return a * (1 / b);
            }
            public static Frequency operator *(double b, Frequency a)
            {
                return a * b;
            }
            public static Frequency operator +(Frequency a, Frequency b)
            {
                double c = a.Arbitrary + b.Arbitrary;
                return new Frequency(c);
            }
            public static Frequency operator -(Frequency a, Frequency b)
            {
                return a + (-b);
            }
            public static double operator /(Frequency a, Frequency b)
            {
                return a.Arbitrary / b.Arbitrary;
            }
            public override string ToString()
            {
                return this.ToString("");
            }
            //accepted formats (H|M|G|R)_{double format}_{symbol}
            public string ToString(string format, IFormatProvider formatProvider)
            {
                IDictionary<string, Tuple<IScaleUnit<Frequency>, string>> unitDictionary = new Dictionary<string, Tuple<IScaleUnit<Frequency>, string>>(1);
                unitDictionary["H"] = Tuple.Create<IScaleUnit<Frequency>, string>(Hertz, "hz");
                unitDictionary["M"] = Tuple.Create<IScaleUnit<Frequency>, string>(MegaHertz, "mhz");
                unitDictionary["G"] = Tuple.Create<IScaleUnit<Frequency>, string>(GigaHertz, "ghz");
                return this.StringFromUnitDictionary(format, "H", formatProvider, unitDictionary);
            }
            public override int GetHashCode()
            {
                return Arbitrary.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                var an = obj as Frequency;
                return an != null && an.Arbitrary == this.Arbitrary;
            }
        }
    }
    namespace Rotationals
    {
        //arbitrary is RPS
        public class RotationalSpeed : IUnit<RotationalSpeed>, ScaleMeasurement, DeltaMeasurement, IComparable<RotationalSpeed>
        {
            public RotationalSpeed(double val, IDeltaUnit<RotationalSpeed> unit) : this(unit.ToArbitrary(val)) { }
            public RotationalSpeed(double arbitrary)
            {
                this.Arbitrary = arbitrary;
            }
            public double Arbitrary { get; }
            public int CompareTo(RotationalSpeed other)
            {
                return Arbitrary.CompareTo(other.Arbitrary);
            }
            double DeltaMeasurement.Arbitrary
            {
                get
                {
                    return this.Arbitrary;
                }
            }
            public override double FromArbitrary(double arb)
            {
                return arb / Arbitrary;
            }
            public override double ToArbitrary(double val)
            {
                return val * Arbitrary;
            }

            private static readonly Lazy<Funnel<string, RotationalSpeed>> DefaultParsers;
            public static RotationalSpeed Parse(string s)
            {
                return DefaultParsers.Value.Process(s);
            }

            // ReSharper disable once InconsistentNaming
            public static readonly RotationalSpeed TurnsPerMinute, TurnsPerSecond, RadiansPerSeconds;
            static RotationalSpeed()
            {
                RadiansPerSeconds = new RotationalSpeed(1);
                TurnsPerSecond = new RotationalSpeed(Math.PI);
                TurnsPerMinute = new RotationalSpeed(1.0/60, TurnsPerSecond);
                DefaultParsers = new Lazy<Funnel<string, RotationalSpeed>>(() => new Funnel<string, RotationalSpeed>(
                    new Parser<RotationalSpeed>($@"^({WordPlay.RegexDouble}) ?(tpm)$", m => new RotationalSpeed(double.Parse(m.Groups[1].Value), TurnsPerMinute)),
                    new Parser<RotationalSpeed>($@"^({WordPlay.RegexDouble}) ?(tps)$", m => new RotationalSpeed(double.Parse(m.Groups[1].Value), TurnsPerSecond)),
                    new Parser<RotationalSpeed>($@"^({WordPlay.RegexDouble}) ?(rps)$", m => new RotationalSpeed(double.Parse(m.Groups[1].Value), RadiansPerSeconds))
                    ));
            }
            public static Angle operator *(RotationalSpeed a, TimeSpan b)
            {
                return new Angle(a.Arbitrary * b.TotalSeconds);
            }
            
            public static double operator *(TimeSpan b, RotationalSpeed a)
            {
                return a.Arbitrary * b.TotalSeconds;
            }
            public static RotationalSpeed operator -(RotationalSpeed a)
            {
                return (-1.0 * a);
            }
            public static RotationalSpeed operator *(RotationalSpeed a, double b)
            {
                return new RotationalSpeed(a.Arbitrary * b);
            }
            public static RotationalSpeed operator /(RotationalSpeed a, double b)
            {
                return a * (1 / b);
            }
            public static RotationalSpeed operator *(double b, RotationalSpeed a)
            {
                return a * b;
            }
            public static RotationalSpeed operator +(RotationalSpeed a, RotationalSpeed b)
            {
                double c = a.Arbitrary + b.Arbitrary;
                return new RotationalSpeed(c);
            }
            public static RotationalSpeed operator -(RotationalSpeed a, RotationalSpeed b)
            {
                return a + (-b);
            }
            public static double operator /(RotationalSpeed a, RotationalSpeed b)
            {
                return a.Arbitrary / b.Arbitrary;
            }
            public override string ToString()
            {
                return this.ToString("");
            }
            //accepted formats (RPS|TPM|TPS)_{double format}_{symbol}
            public string ToString(string format, IFormatProvider formatProvider)
            {
                IDictionary<string, Tuple<IScaleUnit<RotationalSpeed>, string>> unitDictionary = new Dictionary<string, Tuple<IScaleUnit<RotationalSpeed>, string>>(1);
                unitDictionary["RPS"] = Tuple.Create<IScaleUnit<RotationalSpeed>, string>(RadiansPerSeconds, "rps");
                unitDictionary["TPM"] = Tuple.Create<IScaleUnit<RotationalSpeed>, string>(TurnsPerMinute, "tpm");
                unitDictionary["TPS"] = Tuple.Create<IScaleUnit<RotationalSpeed>, string>(TurnsPerSecond, "tps");
                return this.StringFromUnitDictionary(format, "RPS", formatProvider, unitDictionary);
            }
            public override int GetHashCode()
            {
                return Arbitrary.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                var an = obj as RotationalSpeed;
                return an != null && an.Arbitrary == this.Arbitrary;
            }
        }
    }
}
