using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WhetStone.Data;
using WhetStone.Funnels;
using WhetStone.Net;
using WhetStone.PermanentObject;
using WhetStone.WordPlay;
using WhetStone.WordPlay.Parsing;

namespace WhetStone.Units.Moneys
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
            DollarUS = new Money(1 / 1.12);
            NewShekel = new Money(0.26, DollarUS);
            Yen = new Money(0.009, DollarUS);

            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(_exchangeRatesPermaPath));
            _exchangeRatePerma = new SyncPermaObject<string>(Encoding.ASCII.GetString, Encoding.ASCII.GetBytes, _exchangeRatesPermaPath, false, FileAccess.ReadWrite, FileShare.ReadWrite, valueIfCreated: "");

            DefaultParsers = new Lazy<Funnel<string, Money>>(() => new Funnel<string, Money>(
                new Parser<Money>($@"^({commonRegex.RegexDouble}) ?(\$|dollars?)$", m => new Money(double.Parse(m.Groups[1].Value), DollarUS)),
                new Parser<Money>($@"^\$({commonRegex.RegexDouble})$", m => new Money(double.Parse(m.Groups[1].Value), DollarUS)),
                new Parser<Money>($@"^({commonRegex.RegexDouble}) ?(INS|ins|Israeli New Sheckels?|israeli new sheckel)$", m => new Money(double.Parse(m.Groups[1].Value), NewShekel)),
                new Parser<Money>($@"^₪({commonRegex.RegexDouble})$", m => new Money(double.Parse(m.Groups[1].Value), NewShekel)),
                new Parser<Money>($@"^({commonRegex.RegexDouble}) ?(euros?)$", m => new Money(double.Parse(m.Groups[1].Value), Euro)),
                new Parser<Money>($@"^€({commonRegex.RegexDouble})$", m => new Money(double.Parse(m.Groups[1].Value), Euro)),
                new Parser<Money>($@"^({commonRegex.RegexDouble}) ?(yen)$", m => new Money(double.Parse(m.Groups[1].Value), Yen)),
                new Parser<Money>($@"^¥({commonRegex.RegexDouble})$", m => new Money(double.Parse(m.Groups[1].Value), Yen))
                ));
        }

        public static bool updateRates()
        {

            Exception error;
            return updateRates(out error);
        }
        public static bool updateRates(out Exception error)
        {
            return updateRates(TimeSpan.FromHours(12), out error);
        }
        public static bool updateRates(TimeSpan updateTimeTolerance, out Exception error)
        {
            error = null;
            if (_exchangeRatePerma.timeSinceUpdate() < updateTimeTolerance && _initialized)
                return false;
            var doc = (_exchangeRatePerma.timeSinceUpdate() < updateTimeTolerance && _exchangeRatePerma.value.Length > 0) ? toXmlDoc.ToXmlDoc(_exchangeRatePerma.value) : loadXml(out error);
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
            return this.StringFromUnitDictionary(format, "E", formatProvider, unitDictionary, true);
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
