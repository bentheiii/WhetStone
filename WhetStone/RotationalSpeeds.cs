using System;
using System.Collections.Generic;
using Numerics;
using WhetStone.Funnels;
using WhetStone.Units.Angles;
using WhetStone.WordPlay;
using WhetStone.WordPlay.Parsing;

namespace WhetStone.Units.RotationalSpeeds
{
    //arbitrary is RPS
    public class RotationalSpeed : IUnit<RotationalSpeed>, ScaleMeasurement<RotationalSpeed>, DeltaMeasurement<RotationalSpeed>, IComparable<RotationalSpeed>
    {
        public RotationalSpeed(BigRational val, IDeltaUnit<RotationalSpeed> unit) : this(unit.ToArbitrary(val))
        {
        }
        public RotationalSpeed(BigRational arbitrary)
        {
            this.Arbitrary = arbitrary;
        }
        public BigRational Arbitrary { get; }
        public int CompareTo(RotationalSpeed other)
        {
            return Arbitrary.CompareTo(other.Arbitrary);
        }
        BigRational DeltaMeasurement<RotationalSpeed>.Arbitrary
        {
            get
            {
                return this.Arbitrary;
            }
        }
        public override BigRational FromArbitrary(BigRational arb)
        {
            return arb/Arbitrary;
        }
        public override BigRational ToArbitrary(BigRational val)
        {
            return val*Arbitrary;
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
                new Parser<RotationalSpeed>($@"^({CommonRegex.RegexDouble}) ?(tpm)$",
                    m => new RotationalSpeed(double.Parse(m.Groups[1].Value), TurnsPerMinute)),
                new Parser<RotationalSpeed>($@"^({CommonRegex.RegexDouble}) ?(tps)$",
                    m => new RotationalSpeed(double.Parse(m.Groups[1].Value), TurnsPerSecond)),
                new Parser<RotationalSpeed>($@"^({CommonRegex.RegexDouble}) ?(rps)$",
                    m => new RotationalSpeed(double.Parse(m.Groups[1].Value), RadiansPerSeconds))
                ));
        }
        public static Angle operator *(RotationalSpeed a, TimeSpan b)
        {
            return new Angle(a.Arbitrary*b.TotalSeconds);
        }

        public static BigRational operator *(TimeSpan b, RotationalSpeed a)
        {
            return a.Arbitrary*b.TotalSeconds;
        }
        public static RotationalSpeed operator -(RotationalSpeed a)
        {
            return (-1.0*a);
        }
        public static RotationalSpeed operator *(RotationalSpeed a, double b)
        {
            return new RotationalSpeed(a.Arbitrary*b);
        }
        public static RotationalSpeed operator /(RotationalSpeed a, double b)
        {
            return a*(1/b);
        }
        public static RotationalSpeed operator *(double b, RotationalSpeed a)
        {
            return a*b;
        }
        public static RotationalSpeed operator +(RotationalSpeed a, RotationalSpeed b)
        {
            var c = a.Arbitrary + b.Arbitrary;
            return new RotationalSpeed(c);
        }
        public static RotationalSpeed operator -(RotationalSpeed a, RotationalSpeed b)
        {
            return a + (-b);
        }
        public static BigRational operator /(RotationalSpeed a, RotationalSpeed b)
        {
            return a.Arbitrary/b.Arbitrary;
        }
        public override string ToString()
        {
            return this.ToString("");
        }
        private static readonly IDictionary<string, Tuple<IUnit<RotationalSpeed>, string>> _udic = new Dictionary<string, Tuple<IUnit<RotationalSpeed>, string>>(3)
        {
            ["RPS"] = Tuple.Create<IUnit<RotationalSpeed>, string>(RadiansPerSeconds, "rps"),
            ["TPM"] = Tuple.Create<IUnit<RotationalSpeed>, string>(TurnsPerMinute, "tpm"),
            ["TPS"] = Tuple.Create<IUnit<RotationalSpeed>, string>(TurnsPerSecond, "tps")
        };
        public override IDictionary<string, Tuple<IUnit<RotationalSpeed>, string>> unitDictionary => _udic;
        //accepted formats (RPS|TPM|TPS)_{double format}_{symbol}
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return this.StringFromUnitDictionary(format, "RPS", formatProvider, scaleDictionary);
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
