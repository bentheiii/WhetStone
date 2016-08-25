using System;
using System.Collections.Generic;

namespace WhetStone.Fielding
{
    public class ImpotentField<T> : Field<T>
    {
        public override T zero => default(T);
        public override T one => default(T);
        public override T negativeone => default(T);
        public override T naturalbase => default(T);
        public override int Compare(T x, T y) => 0;
        public override T Conjugate(T a) => a;
        public override T Factorial(int x) => default(T);
        public override GenerationType GenType => GenerationType.None;
        public override T Generate(IEnumerable<byte> bytes, Tuple<T, T> bounds = null, object special = null) => default(T);
        public override T Invert(T x) => x;
        public override bool Invertible => false;
        public override bool ModduloAble => false;
        public override bool Negatable => false;
        public override T Negate(T x) => x;
        public override OrderType Order => OrderType.NoOrder;
        public override bool Parsable => false;
        public override T Parse(string s) => default(T);
        public override T Pow(T @base, int x) => @base;
        public override string String(T a) => a.ToString();
        public override T abs(T x) => x;
        public override T add(T a, T b) => a;
        public override T divide(T a, T b) => a;
        public override T fromFraction(double a) => default(T);
        public override T fromFraction(int numerator, int denumerator) => default(T);
        public override T fromInt(int x) => default(T);
        public override T fromInt(ulong x) => default(T);
        public override bool isNegative(T x) => false;
        public override T log(T a) => default(T);
        public override T mod(T a, T b) => a;
        public override T multiply(T a, T b) => a;
        public override T pow(T a, T b) => a;
        public override FieldShape shape => FieldShape.None;
        public override T subtract(T a, T b) => a;
        public override double? toDouble(T a) => null;
    }
}