using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Funnels;
using WhetStone.WordPlay;
using WhetStone.WordPlay.Parsing;

namespace WhetStone.Units.GraphicDistances
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
                new Parser<GraphicsLength>($@"^({commonRegex.RegexDouble}) ?(p|pixels?)$", m => new GraphicsLength(double.Parse(m.Groups[1].Value), Pixel))
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
