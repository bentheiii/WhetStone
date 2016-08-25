using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Funnels;
using WhetStone.WordPlay;
using WhetStone.WordPlay.Parsing;

namespace WhetStone.Units.Masses
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
                new Parser<Mass>($@"^({commonRegex.RegexDouble}) ?(kg?|kilograms?)$", m => new Mass(double.Parse(m.Groups[1].Value), KiloGram)),
                new Parser<Mass>($@"^({commonRegex.RegexDouble}) ?(g|grams?)$", m => new Mass(double.Parse(m.Groups[1].Value), Gram)),
                new Parser<Mass>($@"^({commonRegex.RegexDouble}) ?(mg|milligrams?)$", m => new Mass(double.Parse(m.Groups[1].Value), Milligram)),
                new Parser<Mass>($@"^({commonRegex.RegexDouble}) ?(t|tons?)$", m => new Mass(double.Parse(m.Groups[1].Value), Tonne)),
                new Parser<Mass>($@"^({commonRegex.RegexDouble}) ?(oz|ounces?)$", m => new Mass(double.Parse(m.Groups[1].Value), Ounce)),
                new Parser<Mass>($@"^({commonRegex.RegexDouble}) ?(lb|pounds?)$", m => new Mass(double.Parse(m.Groups[1].Value), Pound))
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
