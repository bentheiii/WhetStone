using System;
using System.Collections.Generic;
using Numerics;
using WhetStone.Funnels;
using WhetStone.WordPlay;
using WhetStone.WordPlay.Parsing;

namespace WhetStone.Units.Frequencies
{
    //arbitrary is hertz
    public class Frequency : IUnit<Frequency>, ScaleMeasurement<Frequency>, DeltaMeasurement<Frequency>, IComparable<Frequency>
    {
        public Frequency(BigRational val, IDeltaUnit<Frequency> unit) : this(unit.ToArbitrary(val)) { }
        public Frequency(BigRational arbitrary)
        {
            this.Arbitrary = arbitrary;
        }
        public BigRational Arbitrary { get; }
        public int CompareTo(Frequency other)
        {
            return Arbitrary.CompareTo(other.Arbitrary);
        }
        BigRational DeltaMeasurement<Frequency>.Arbitrary
        {
            get
            {
                return this.Arbitrary;
            }
        }
        public override BigRational FromArbitrary(BigRational arb)
        {
            return arb / Arbitrary;
        }
        public override BigRational ToArbitrary(BigRational val)
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
                new Parser<Frequency>($@"^({CommonRegex.RegexDouble}) ?(hz|hertz)$", m => new Frequency(double.Parse(m.Groups[1].Value), Hertz)),
                new Parser<Frequency>($@"^({CommonRegex.RegexDouble}) ?(mhz|megahertz)$", m => new Frequency(double.Parse(m.Groups[1].Value), MegaHertz)),
                new Parser<Frequency>($@"^({CommonRegex.RegexDouble}) ?(ghz|gigahertz)$", m => new Frequency(double.Parse(m.Groups[1].Value), GigaHertz))
                ));
        }
        public static BigRational operator *(Frequency a, TimeSpan b)
        {
            return a.Arbitrary * b.TotalSeconds;
        }
        public static TimeSpan operator /(BigRational a, Frequency b)
        {
            return TimeSpan.FromSeconds((double)(a / b.Arbitrary));
        }
        public static BigRational operator *(TimeSpan b, Frequency a)
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
            var c = a.Arbitrary + b.Arbitrary;
            return new Frequency(c);
        }
        public static Frequency operator -(Frequency a, Frequency b)
        {
            return a + (-b);
        }
        public static BigRational operator /(Frequency a, Frequency b)
        {
            return a.Arbitrary / b.Arbitrary;
        }
        public override string ToString()
        {
            return this.ToString("");
        }
        private static readonly IDictionary<string, Tuple<IUnit<Frequency>, string>> _udic = new Dictionary<string, Tuple<IUnit<Frequency>, string>>(3)
        {
            ["H"] = Tuple.Create<IUnit<Frequency>, string>(Hertz, "hz"),
            ["M"] = Tuple.Create<IUnit<Frequency>, string>(MegaHertz, "mhz"),
            ["G"] = Tuple.Create<IUnit<Frequency>, string>(GigaHertz, "ghz")
        };
        public override IDictionary<string, Tuple<IUnit<Frequency>, string>> unitDictionary => _udic;
        //accepted formats (H|M|G|R)_{double format}_{symbol}
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return this.StringFromUnitDictionary(format, "H", formatProvider, scaleDictionary);
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
