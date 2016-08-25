using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Funnels;
using WhetStone.WordPlay;
using WhetStone.WordPlay.Parsing;

namespace WhetStone.Units.DataSizes
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
                new Parser<DataSize>($@"^({commonRegex.RegexDouble}) ?(b|bits?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Bit)),
                new Parser<DataSize>($@"^({commonRegex.RegexDouble}) ?(B|bytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Byte)),
                new Parser<DataSize>($@"^({commonRegex.RegexDouble}) ?(KB|kilobytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Kilobyte)),
                new Parser<DataSize>($@"^({commonRegex.RegexDouble}) ?(MB|megabytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Megabyte)),
                new Parser<DataSize>($@"^({commonRegex.RegexDouble}) ?(GB|gigabytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Gigabyte)),
                new Parser<DataSize>($@"^({commonRegex.RegexDouble}) ?(TB|terrabytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Terrabyte)),
                new Parser<DataSize>($@"^({commonRegex.RegexDouble}) ?(PB|pettabytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Pettabyte)),
                new Parser<DataSize>($@"^({commonRegex.RegexDouble}) ?(EB|exabytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Exabyte)),
                new Parser<DataSize>($@"^({commonRegex.RegexDouble}) ?(ZB|zettabytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Zettabyte)),
                new Parser<DataSize>($@"^({commonRegex.RegexDouble}) ?(YB|yottabytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Yottabyte))
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
