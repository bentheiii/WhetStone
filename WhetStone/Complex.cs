using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.WordsPlay;
using WhetStone.Fielding;
using WhetStone.Funnels;
using WhetStone.Looping;
using WhetStone.Matrix;
using WhetStone.NumbersMagic;
using WhetStone.SystemExtensions;
using WhetStone.Units;
using WhetStone.Units.Angles;
using WhetStone.WordsPlay.Parsing;

namespace WhetStone.Complex
{
    public static class Mathextensions
    {
        public static ComplexNumber pow(this double powbase, ComplexNumber powpower)
        {
            if (powpower.Equals(0))
                return new ComplexNumber(1, 0);
            if (powbase >= 0)
            {
                double r = Math.Pow(powbase, powpower.RealPart);
                double a = powpower.ImaginaryPart * Math.Log(powbase);
                return new ComplexNumber(a, r, ComplexRepresentations.Polar);
            }
            else
            {
                return (-powbase).pow(powpower)*Math.E.pow(-Math.PI*powpower.ImaginaryPart)*ComplexNumber.FromPolar(Math.PI*powpower.RealPart);
            }
        }
        public static ComplexNumber pow(this int powbase, ComplexNumber powpower)
        {
            return ((double)powbase).pow(powpower);
        }
        public static ComplexNumber log(this double logpow, ComplexNumber logbase, int number = 0)
        {
            ComplexNumber a = ((ComplexNumber)logpow).log();
            ComplexNumber b = logbase.log();
            return (a / b) + (number*ComplexNumber.ImaginaryUnit*2*Math.PI);
        }
        public static ComplexNumber log(this int logpow, ComplexNumber logbase, int number = 0)
        {
            return ((double)logpow).log(logbase, number);
        }
    }
    public enum ComplexRepresentations { Polar, Rectangular };
    public sealed class ComplexNumber : IComparable<ComplexNumber>, IComparable<int>, IComparable<double>, IFormattable
    {
        private class ComplexField : Field<ComplexNumber>
        {
            public ComplexField() : base(0, 1, Math.E) { }
            public override ComplexNumber add(ComplexNumber a, ComplexNumber b) => a + b;
            public override ComplexNumber pow(ComplexNumber a, ComplexNumber b) => a.pow(b);
            public override int Compare(ComplexNumber x, ComplexNumber y) => x.CompareTo(y);
            public override ComplexNumber Factorial(int x) => (x.factorial());
            public override ComplexNumber fromInt(int x) => x;
            public override ComplexNumber fromInt(ulong x) => x;
            public override ComplexNumber abs(ComplexNumber x) => (x.abs());
            public override ComplexNumber Conjugate(ComplexNumber a) => a.Conjugate();
            public override ComplexNumber divide(ComplexNumber a, ComplexNumber b) => a / b;
            public override ComplexNumber mod(ComplexNumber a, ComplexNumber b) => a % b;
            public override ComplexNumber fromFraction(int numerator, int denumerator) => FromRectangular(numerator / (double)denumerator);
            public override ComplexNumber Invert(ComplexNumber x) => 1 / x;
            public override bool isNegative(ComplexNumber x) => x.Isreal() && x.RealPart < 0;
            public override ComplexNumber log(ComplexNumber a) => a.log();
            public override ComplexNumber multiply(ComplexNumber a, ComplexNumber b) => a * b;
            public override ComplexNumber Negate(ComplexNumber x) => -x;
            public override ComplexNumber subtract(ComplexNumber a, ComplexNumber b) => a - b;
            public override double? toDouble(ComplexNumber a) => a.Isreal() ? a.RealPart : (double?)null;
            public override bool Parsable => true;
            public override OrderType Order => OrderType.PartialOrder;
            // ReSharper disable once MemberHidesStaticFromOuterClass
            public override ComplexNumber Parse(string s)
            {
                return ComplexNumber.Parse(s);
            }
            public override GenerationType GenType => GenerationType.FromRange;
            public override ComplexNumber Generate(IEnumerable<byte> bytes, Tuple<ComplexNumber, ComplexNumber> bounds = null, object special = null)
            {
                var f = Fields.getField<double>();
                bounds = bounds ?? Tuple.Create(FromRectangular(0), FromRectangular(1, 1));
                return FromRectangular(f.Generate(bytes.Step(), Tuple.Create(bounds.Item1.RealPart, bounds.Item2.RealPart)),
                    f.Generate(bytes.Skip(1).Step(), Tuple.Create(bounds.Item1.ImaginaryPart, bounds.Item2.ImaginaryPart)));
            }
            public override FieldShape shape =>FieldShape.None;
            public override ComplexNumber fromFraction(double a)
            {
                return a;
            }
        }
        public static ComplexNumber ImaginaryUnit { get; }
        public static ComplexNumber Zero { get; }
        public static ComplexNumber One { get; }
        public static ComplexNumber EulersNumber { get; }
        public static ComplexNumber Pi { get; }
        static ComplexNumber()
        {
            ImaginaryUnit = new ComplexNumber(0, 1);
            Zero = new ComplexNumber(0,0);
            One = new ComplexNumber(1,0);
            EulersNumber = new ComplexNumber(Math.E,0);
            Pi = Math.PI;
            Fields.setField(new ComplexField());
        }
        private readonly Lazy<double> _real, _imag, _radius;
        private readonly Lazy<Angle> _angle;
        public double RealPart
        {
            get
            {
                return _real.Value;
            }
        }
        public double ImaginaryPart
        {
            get
            {
                return _imag.Value;
            }
        }
        public double Radius
        {
            get
            {
                return _radius.Value;
            }
        }
        public Angle Angle
        {
            get
            {
                return _angle.Value;
            }
        }
        /// <summary>
        /// if polar, v1 is Angle in radians, v2 is Tadius
        /// if rect, v1 is real, v2 is imaginary
        /// </summary>
        public ComplexNumber(double v1, double v2, ComplexRepresentations r = ComplexRepresentations.Rectangular)
        {
            switch (r)
            {
                case ComplexRepresentations.Rectangular:
                    _real = new Lazy<double>(()=>v1);
                    _imag = new Lazy<double>(()=>v2);
                    _radius = new Lazy<double>(() => Math.Sqrt(RealPart * RealPart + ImaginaryPart * ImaginaryPart));
                    _angle = new Lazy<Angle>(() => Radius == 0 ? new Angle(0, Angle.Turn) : Angle.ATan(ImaginaryPart ,RealPart));
                    break;
                case ComplexRepresentations.Polar:
                    if (v2 < 0)
                    {
                        v2 = -v2;
                        v1 = v1 + Math.PI / 2;
                    }
                    _radius = new Lazy<double>(() => v2);
                    _angle = new Lazy<Angle>(() => Radius == 0 ? new Angle(0) : new Angle(v1, true));
                    _real = new Lazy<double>(() => Radius * Angle.Cos());
                    _imag = new Lazy<double>(() => Radius * Angle.Sin());
                    break;
                default:
                    throw new Exception("unrecognized representation");
            }
        }
        public ComplexNumber(Angle a, double r=1) : this(a.Normalize().InUnits(Angle.Radian), r, ComplexRepresentations.Polar) { }
        public static ComplexNumber FromPolar(double angle, double radius=1)
        {
            return new ComplexNumber(angle, radius, ComplexRepresentations.Polar);
        }
        public static ComplexNumber FromRectangular(double real, double imaginary=0)
        {
            return new ComplexNumber(real, imaginary);
        }
        public static ComplexNumber operator +(ComplexNumber a, ComplexNumber b)
        {
            double r = a.RealPart + b.RealPart;
            double imaginary = a.ImaginaryPart + b.ImaginaryPart;
            return FromRectangular(r, imaginary);
        }
        public static ComplexNumber operator -(ComplexNumber a, ComplexNumber b)
        {
            return a + (-b);
        }
        public static ComplexNumber operator -(ComplexNumber a)
        {
            return a * -1;
        }
        public static ComplexNumber operator +(ComplexNumber a)
        {
            return a;
        }
        public static ComplexNumber operator *(ComplexNumber a, ComplexNumber b)
        {
            double r = a.RealPart * b.RealPart - a.ImaginaryPart * b.ImaginaryPart;
            double i = a.RealPart * b.ImaginaryPart + a.ImaginaryPart * b.RealPart;
            return FromRectangular(r, i);
        }
        public static ComplexNumber operator /(ComplexNumber a, ComplexNumber b)
        {
            double angle = (a.Angle - b.Angle).Normalize().InUnits(Angle.Radian);
            if (b.Radius == 0)
                throw new DivideByZeroException();
            double radius = a.Radius / b.Radius;
            return FromPolar(angle, radius);
        }
        public static ComplexNumber operator %(ComplexNumber a, ComplexNumber b)
        {
            ComplexNumber ct = a / b;
            ComplexNumber c = new ComplexNumber((int)ct.RealPart, (int)ct.ImaginaryPart);
            return a - (b * c);
        }
        public static ComplexNumber operator *(ComplexNumber a, int b)
        {
            return a * (double)b;
        }
        public static ComplexNumber operator *(int b, ComplexNumber a)
        {
            return (double)b * a;
        }
        public static ComplexNumber operator /(ComplexNumber a, int b)
        {
            return a / (double)b;
        }
        public static ComplexNumber operator /(int a, ComplexNumber b)
        {
            double c = a;
            return c / b;
        }
        public static ComplexNumber operator +(ComplexNumber a, int b)
        {
            return a + (double)b;
        }
        public static ComplexNumber operator +(int a, ComplexNumber b)
        {
            return (double)a + b;
        }
        public static ComplexNumber operator -(ComplexNumber a, int b)
        {
            return a - (double)b;
        }
        public static ComplexNumber operator -(int a, ComplexNumber b)
        {
            return (double)a - b;
        }
        public static ComplexNumber operator %(ComplexNumber a, int b)
        {
            return a % (double)b;
        }
        public static ComplexNumber operator %(int a, ComplexNumber b)
        {
            return (double)a % b;
        }
        public static ComplexNumber operator *(ComplexNumber a, double b)
        {
            double real = a.RealPart * b;
            double im = a.ImaginaryPart * b;
            return FromRectangular(real, im);
        }
        public static ComplexNumber operator *(double b, ComplexNumber a)
        {
            double real = a.RealPart * b;
            double im = a.ImaginaryPart * b;
            return FromRectangular(real, im);
        }
        public static ComplexNumber operator /(ComplexNumber a, double b)
        {
            if (b == 0)
                throw new DivideByZeroException();
            double real = a.RealPart / b;
            double im = a.ImaginaryPart / b;
            return FromRectangular(real, im);
        }
        public static ComplexNumber operator /(double a, ComplexNumber b)
        {
            ComplexNumber c = a;
            return c / b;
        }
        public static ComplexNumber operator +(ComplexNumber a, double b)
        {
            double r = a.RealPart + b;
            return FromRectangular(r, a.ImaginaryPart);
        }
        public static ComplexNumber operator +(double a, ComplexNumber b)
        {
            double r = a + b.RealPart;
            return FromRectangular(r, b.ImaginaryPart);
        }
        public static ComplexNumber operator -(ComplexNumber a, double b)
        {
            double r = a.RealPart - b;
            return FromRectangular(r, a.ImaginaryPart);
        }
        public static ComplexNumber operator -(double a, ComplexNumber b)
        {
            double r = a - b.RealPart;
            return FromRectangular(r, -b.ImaginaryPart);
        }
        public static ComplexNumber operator %(ComplexNumber a, double b)
        {
            if (b == 0)
                throw new DivideByZeroException();
            ComplexNumber ct = a / b;
            ComplexNumber c = new ComplexNumber((int)ct.RealPart, (int)ct.ImaginaryPart);
            return a - (b * c);
        }
        public static ComplexNumber operator %(double a, ComplexNumber b)
        {
            ComplexNumber ct = a / b;
            ComplexNumber c = new ComplexNumber((int)ct.RealPart, (int)ct.ImaginaryPart);
            return a - (b * c);
        }
        public static ComplexNumber operator ++(ComplexNumber a)
        {
            a = new ComplexNumber(a.RealPart + 1, a.ImaginaryPart);
            return a;
        }
        public static ComplexNumber operator --(ComplexNumber a)
        {
            a = new ComplexNumber(a.RealPart - 1, a.ImaginaryPart);
            return a;
        }
        public static ComplexNumber operator ~(ComplexNumber a)
        {
            return a.Conjugate();
        }
        public double abs()
        {
            return this.Radius;
        }
        public ComplexNumber pow(double power)
        {
            double a = (this.Angle * power).InUnits(Angle.Radian);
            double r = Math.Pow(this.Radius, power);
            return FromPolar(a, r);
        }
        public ComplexNumber pow(int power)
        {
            return this.pow((double)power);
        }
        public ComplexNumber pow(ComplexNumber powpower)
        {
            if (powpower.Equals(Zero))
                return One;
            if (this.Equals(Zero))
                return Zero;
            var r = this.Radius.pow(powpower.RealPart);
            var a = Math.E.pow(-this.Angle.InUnits(Angle.Radian) * powpower.ImaginaryPart);
            var res = FromPolar(this.Angle.InUnits(Angle.Radian) * powpower.RealPart + Math.Log(this.Radius) * powpower.ImaginaryPart);
            return r * a * res;
        }
        public ComplexNumber log(int logbase, int number=0)
        {
            return this.log((double)logbase, number);
        }
        /// <summary>
        /// number represents Angle representation (θ = θ + 2πk)
        /// </summary>
        public ComplexNumber log(double logbase = Math.E, int number = 0)
        {
            if (this.Equals(Zero))
                throw new ArithmeticException("Cannot log 0");
            if (logbase <= 0)
                throw new ArithmeticException("cannot use logarithm of non-positive base");
            return new ComplexNumber(Math.Log(this.Radius), this.Angle.InUnits(Angle.Radian) + Math.PI * 2 * number) / Math.Log(logbase);
        }
        /// <summary>
        /// number represents Angle representation (θ = θ + 2πk)
        /// </summary>
        public ComplexNumber log(ComplexNumber logbase, int number = 0)
        {
            ComplexNumber a = this.log(number: number);
            ComplexNumber b = logbase.log(number: number);
            return a / b;
        }
        public ComplexNumber Sin()
        {
            ComplexNumber a = this * ImaginaryUnit;
            ComplexNumber b = Math.E.pow(a);
            ComplexNumber c = 1 / b;
            return (b-c) / (2 * ImaginaryUnit);
        }
        public ComplexNumber Asin()
        {
            return -1 * ImaginaryUnit * (this * ImaginaryUnit + (1 - this * this).pow(0.5)).log();
        }
        public ComplexNumber Sinh()
        {
            return ImaginaryUnit * (this / ImaginaryUnit).Sin();
        }
        public ComplexNumber Cos()
        {
            ComplexNumber a = this * ImaginaryUnit;
            ComplexNumber b = Math.E.pow(a);
            ComplexNumber c = 1 / b;
            return (c+b) /2;
        }
        public ComplexNumber Acos()
        {
            return Math.PI / 2 - this.Asin();
        }
        public ComplexNumber Cosh()
        {
            return (this / ImaginaryUnit).Cos();
        }
        public ComplexNumber Tan()
        {
            return Sin()/Cos();
        }
        public ComplexNumber Atan()
        {
            var s = (2 / (1 + (ImaginaryUnit * this))) - 1;
            var l = (s).log();
            return (ImaginaryUnit / 2) * l;
        }
        public ComplexNumber Tanh()
        {
            return ImaginaryUnit * (this / ImaginaryUnit).Tan();
        }
        public IEnumerable<ComplexNumber> roots(int power)
        {
            return Loops.Range(power).Select(a => getnumberedroot(power, a));
        }
        private ComplexNumber getnumberedroot(int power, int number)
        {
            double a = (this.Angle.InUnits(Angle.Radian) + 2 * Math.PI * number) / power;
            double r = Math.Pow(this.Radius, 1.0 / power);
            return FromPolar(a, r);
        }
        public ComplexNumber Conjugate()
        {
            return FromRectangular(this.RealPart, -this.ImaginaryPart);
        }
        public bool Isreal()
        {
            return this.ImaginaryPart == 0;
        }
        public bool Isimaginary()
        {
            return this.RealPart == 0;
        }
        public bool Iscomplex()
        {
            return this.RealPart != 0 && this.ImaginaryPart != 0;
        }
        public bool Iszero()
        {
            return this.RealPart == 0 && this.ImaginaryPart == 0;
        }
        public bool IsGaussian()
        {
            return RealPart.whole() && ImaginaryPart.whole();
        }
        public bool isGaussianPrime()
        {
            if (this.Iszero() || !this.IsGaussian())
                return false;
            if (Iscomplex())
                return ((int)(RealPart*RealPart + ImaginaryPart*ImaginaryPart)).isPrime();
            if (ImaginaryPart == 0)
                return ((int)RealPart.abs()).TrueMod(4) == 3 && ((int)RealPart.abs()).isPrime();
            return ((int)ImaginaryPart.abs()).TrueMod(4) == 3 && ((int)ImaginaryPart.abs()).isPrime();
        }
        public override bool Equals(object obj)
        {
            var complexnum = obj as ComplexNumber;
            if (complexnum != null)
                return complexnum.RealPart == RealPart && complexnum.ImaginaryPart == ImaginaryPart;
            if (!Isreal())
                return false;
            try
            {
                return this.RealPart.Equals(Convert.ToDouble(obj));
            }
            catch (Exception)
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return this.RealPart.GetHashCode() ^ this.ImaginaryPart.GetHashCode() ^ this.Angle.GetHashCode();
        }
        public int CompareTo(ComplexNumber other)
        {
            if (this.Iszero())
            {
                if (other.Iszero())
                    return 0;
                return other.RealPart != 0 ? -other.RealPart.CompareTo(0.0) : -other.ImaginaryPart.CompareTo(0.0);
            }
            if (other.Iszero())
                return -other.CompareTo(this);
            if (other.Angle.Equals(this.Angle))
                return this.Radius.CompareTo(other.Radius);
            throw new ArgumentException("cannot compare complexes of different arguments");
        }
        public int CompareTo(int other)
        {
            return this.CompareTo((double)other);
        }
        public int CompareTo(double other)
        {
            return this.CompareTo((ComplexNumber)other);
        }
        public override string ToString()
        {
            return this.ToString("");
        }
        /// <summary>
        /// accepted format is "R_{whatever format for the double real value}_{whatever format for the double imaginary value}" or
        /// "P_{whatever format for the double radius}_({whatever format for the angle})
        /// the last argument can be discarded to be replaced by the second
        /// </summary>
        /// <param name="format"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public string ToString(string format, IFormatProvider provider)
        {
            if (format == "")
                format = "R_G_G";
            string[] split = format.SmartSplit("_","(",")");
            if (split.Length == 1)
                split = new[] {split[0], "G", "G"};
            if (split.Length == 2)
                split = new[] {split[0], split[1], split[1]};
            if (split.Length != 3)
                throw new FormatException("Format must either be empty or include 1 or 2 dividers (_)");
            if (split[0] == "R")
            {
                string realformat = split[1];
                string imagformat = split[2];
                if (this.Isreal())
                    return this.RealPart.ToString(realformat,provider);
                if (this.Isimaginary())
                    return this.ImaginaryPart.ToString(imagformat,provider) + "i";
                return this.RealPart.ToString(realformat, provider) + (this.ImaginaryPart < 0 ? "" : "+") + this.ImaginaryPart.ToString(imagformat, provider) + "i";
            }
            if (split[1] == "P")
            {
                string radiusformat = split[1];
                string angleformat = split[2];
                if (this.Radius == 1)
                    return "CiS(" + this.Angle.ToString(angleformat) + ")";
                if (this.Radius == 0)
                    return "0";
                return this.Radius.ToString(radiusformat,provider) + "CiS(" + this.Angle.ToString(angleformat) + ")";
            }
            throw new FormatException("Format must either be empty or start with a R or a P");
        }
        public Matrix<double> ToMatrix()
        {
            return new ExplicitMatrix<double>(new[,]
                {
                    {this.RealPart,-this.ImaginaryPart},
                    {this.ImaginaryPart, this.RealPart}
                });
        }
        public static implicit operator ComplexNumber(double d)
        {
            return new ComplexNumber(d, 0);
        }
        public static implicit operator ComplexNumber(int d)
        {
            return (double)d;
        }
        private static readonly Funnel<string, ComplexNumber> DefaultParsers = new Funnel<string, ComplexNumber>
            {
                new Parser<ComplexNumber>($@"^(?<re>{WordPlay.RegexDouble})(((?<im>(\+|-){WordPlay.RegexDoubleNoSign})[i,j])?$", m =>
                    new ComplexNumber(double.Parse(m.Groups["re"].Value),
                        m.Groups["im"].Value.Length == 0 ? 0 : double.Parse(m.Groups["im"].Value))),
                new Parser<ComplexNumber>($@"^(?<im>{WordPlay.RegexDouble})i$", m =>
                    new ComplexNumber(0, m.Groups["im"].Value.Length == 0 ? 0 : double.Parse(m.Groups["im"].Value))),
                new Parser<ComplexNumber>(
                    $@"^(?<r>{WordPlay.RegexDouble})?(CIS|cis|Cis)\((?<a>{WordPlay.RegexDouble})\)$", m =>
                        new ComplexNumber(double.Parse(m.Groups["a"].Value),
                            m.Groups["r"].Value.Length == 0 ? 1 : double.Parse(m.Groups["r"].Value),
                            ComplexRepresentations.Polar)),
                new Parser<ComplexNumber>(
                    $@"^(?<r>{WordPlay.RegexDouble})?(CIS|cis|Cis)\((?<a>{WordPlay.RegexDouble})(°|d|degrees)\)$", m =>
                        new ComplexNumber(double.Parse(m.Groups["a"].Value) * Math.PI / 180,
                            m.Groups["r"].Value.Length == 0 ? 1 : double.Parse(m.Groups["r"].Value),
                            ComplexRepresentations.Polar))
            };
        /// <summary>
        /// accepted formats  are x+yi, x, yi, xCIS(y), xCIS(y°), CIS(x)
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static ComplexNumber Parse(string x)
        {
            return DefaultParsers.Process(x);
        }
    }
    public static class ComplexComparers
    {
        public static readonly IComparer<ComplexNumber> TotalOrder = new TotalOrderClass(), PartialOrder = new PartialOrderClass(); 
        private class TotalOrderClass : IComparer<ComplexNumber>
        {
            public int Compare(ComplexNumber x, ComplexNumber y)
            {
                var radii = x.Radius.CompareTo(y.Radius);
                return radii != 0 ? radii : x.Angle.Arbitrary.CompareTo(y.Angle.Arbitrary);
            }
        }
        private class PartialOrderClass : IComparer<ComplexNumber>
        {
            public int Compare(ComplexNumber x, ComplexNumber y)
            {
                return x.CompareTo(y);
            }
        }
    }
    
}