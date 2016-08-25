using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Funnels;
using WhetStone.WordPlay;
using WhetStone.WordPlay.Parsing;

namespace WhetStone.Units.Lengths
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
                new Parser<Length>($@"^({commonRegex.RegexDouble}) ?(m|meters?)$", m => new Length(double.Parse(m.Groups[1].Value), Meter)),
                new Parser<Length>($@"^({commonRegex.RegexDouble}) ?(cm|centimeters?)$", m => new Length(double.Parse(m.Groups[1].Value), CentiMeter)),
                new Parser<Length>($@"^({commonRegex.RegexDouble}) ?(mm|millimeters?)$", m => new Length(double.Parse(m.Groups[1].Value), MilliMeter)),
                new Parser<Length>($@"^({commonRegex.RegexDouble}) ?(km|kilometers?)$", m => new Length(double.Parse(m.Groups[1].Value), KiloMeter)),
                new Parser<Length>($@"^({commonRegex.RegexDouble}) ?(ft|foot|feet)$", m => new Length(double.Parse(m.Groups[1].Value), Foot)),
                new Parser<Length>($@"^({commonRegex.RegexDouble}) ?(yd|yards?)$", m => new Length(double.Parse(m.Groups[1].Value), Yard)),
                new Parser<Length>($@"^({commonRegex.RegexDouble}) ?(mi|miles?)$", m => new Length(double.Parse(m.Groups[1].Value), Mile)),
                new Parser<Length>($@"^({commonRegex.RegexDouble}) ?(au|astronomical units?)$",
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
