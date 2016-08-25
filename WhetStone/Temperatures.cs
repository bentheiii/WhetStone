using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Funnels;
using WhetStone.WordPlay;
using WhetStone.WordPlay.Parsing;

namespace WhetStone.Units.Temperatures
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
            Fahrenheit = new ScaleUnit<Temperature>(9 / 5.0, -459.67);
            DefaultParsers = new Lazy<Funnel<string, Temperature>>(() => new Funnel<string, Temperature>(
                new Parser<Temperature>($@"^({commonRegex.RegexDouble}) ?(k|kelvin)$", m => new Temperature(double.Parse(m.Groups[1].Value), Kelvin)),
                new Parser<Temperature>($@"^({commonRegex.RegexDouble}) ?(f|fahrenheit)$", m => new Temperature(double.Parse(m.Groups[1].Value), Fahrenheit)),
                new Parser<Temperature>($@"^({commonRegex.RegexDouble}) ?(c|celsius)$", m => new Temperature(double.Parse(m.Groups[1].Value), Celsius))
                ));
        }
        public static Temperature operator +(Temperature t, TemperatureDelta d)
        {
            return new Temperature(t.Arbitrary + d.Arbitrary);
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
            Fahrenheit = new TemperatureDelta(9 / 5.0);
            DefaultParsers = new Lazy<Funnel<string, TemperatureDelta>>(() => new Funnel<string, TemperatureDelta>(
                new Parser<TemperatureDelta>($@"^({commonRegex.RegexDouble}) ?(k|kelvin)$", m => new TemperatureDelta(double.Parse(m.Groups[1].Value), Kelvin)),
                new Parser<TemperatureDelta>($@"^({commonRegex.RegexDouble}) ?(f|fahrenheit)$", m => new TemperatureDelta(double.Parse(m.Groups[1].Value), Fahrenheit)),
                new Parser<TemperatureDelta>($@"^({commonRegex.RegexDouble}) ?(c|celsius)$", m => new TemperatureDelta(double.Parse(m.Groups[1].Value), Celsius))
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
