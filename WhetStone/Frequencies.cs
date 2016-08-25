using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Funnels;
using WhetStone.Units;
using WhetStone.WordPlay;
using WhetStone.WordPlay.Parsing;

namespace WhetStone.Units.Frequencies
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
            GigaHertz = new Frequency(1000 * 1000);
            DefaultParsers = new Lazy<Funnel<string, Frequency>>(() => new Funnel<string, Frequency>(
                new Parser<Frequency>($@"^({commonRegex.RegexDouble}) ?(hz|hertz)$", m => new Frequency(double.Parse(m.Groups[1].Value), Hertz)),
                new Parser<Frequency>($@"^({commonRegex.RegexDouble}) ?(mhz|megahertz)$", m => new Frequency(double.Parse(m.Groups[1].Value), MegaHertz)),
                new Parser<Frequency>($@"^({commonRegex.RegexDouble}) ?(ghz|gigahertz)$", m => new Frequency(double.Parse(m.Groups[1].Value), GigaHertz))
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
