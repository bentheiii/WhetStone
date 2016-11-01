using System;
using System.Collections.Generic;
using Numerics;
using WhetStone.Funnels;
using WhetStone.NumbersMagic;
using WhetStone.Units.RotationalSpeeds;
using WhetStone.WordPlay;
using WhetStone.WordPlay.Parsing;

namespace WhetStone.Units.Angles
{
    //arbitrary is radians
    //in most conventions, 0 means aligned with the x axis
    public class Angle : IUnit<Angle>, ScaleMeasurement, DeltaMeasurement, IComparable<Angle>
    {
        public Angle(BigRational val, IUnit<Angle> unit, bool normalize = false) : this(unit.ToArbitrary(val), normalize) { }
        public Angle(BigRational arbitrary, bool normalize = false)
        {
            _sin = new Lazy<double>(() => Math.Sin((double)Arbitrary));
            _cos = new Lazy<double>(() => Math.Cos((double)Arbitrary));
            if (normalize)
                arbitrary = arbitrary.TrueMod(2 * Math.PI);
            this.Arbitrary = arbitrary;
        }
        public BigRational Arbitrary { get; }
        BigRational ScaleMeasurement.Arbitrary
        {
            get
            {
                return this.Arbitrary;
            }
        }
        BigRational DeltaMeasurement.Arbitrary
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
                new Parser<Angle>($@"^({CommonRegex.RegexDouble}) ?(turns?|t)$", m => new Angle(double.Parse(m.Groups[1].Value), Turn)),
                new Parser<Angle>($@"^({CommonRegex.RegexDouble}) ?(°|degrees?|d)$", m => new Angle(double.Parse(m.Groups[1].Value), Degree)),
                new Parser<Angle>($@"^({CommonRegex.RegexDouble}) ?(rad|㎭|radians?|c|r)$", m => new Angle(double.Parse(m.Groups[1].Value), Radian)),
                new Parser<Angle>($@"^({CommonRegex.RegexDouble}) ?(grad|g|gradians?|gon)$", m => new Angle(double.Parse(m.Groups[1].Value), Gradian))
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
            var c = a.Arbitrary + b.Arbitrary;
            return new Angle(c);
        }
        public static Angle operator -(Angle a, Angle b)
        {
            return a + (-b);
        }
        public static BigRational operator /(Angle a, Angle b)
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
