using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Arrays;
using WhetStone.Fielding;
using WhetStone.Looping;

namespace WhetStone.Matrix
{
    public class MatrixField<G> : Field<Matrix<G>>
    {
        private readonly Field<G> _int;
        public MatrixField(Field<G> i)
        {
            this._int = i;
        }
        public override Matrix<G> zero => new UnitMatrix<G>(_int.zero);
        public override Matrix<G> one => new UnitMatrix<G>(_int.one);
        public override Matrix<G> naturalbase => new UnitMatrix<G>(_int.naturalbase);
        public override Matrix<G> add(Matrix<G> a, Matrix<G> b) => a + b;
        public override Matrix<G> pow(Matrix<G> a, Matrix<G> b) => a.pow(b);
        public override int Compare(Matrix<G> x, Matrix<G> y) => _int.Compare(x.determinant(), y.determinant());
        public override Matrix<G> fromInt(int x) => new UnitMatrix<G>(_int.fromInt(x));
        public override Matrix<G> fromInt(ulong x) => new UnitMatrix<G>(_int.fromInt(x));
        public override Matrix<G> abs(Matrix<G> x) => x;
        public override Matrix<G> Conjugate(Matrix<G> a) => a.conjugate();
        public override Matrix<G> divide(Matrix<G> a, Matrix<G> b) => a * b.inverse();
        public override Matrix<G> Invert(Matrix<G> x) => x.inverse();
        public override Matrix<G> log(Matrix<G> a)
        {
            throw new NotSupportedException("can't log a matrix");
        }
        public override Matrix<G> multiply(Matrix<G> a, Matrix<G> b) => a * b;
        public override Matrix<G> Negate(Matrix<G> x) => -x;
        public override Matrix<G> subtract(Matrix<G> a, Matrix<G> b) => a - b;
        public override double? toDouble(Matrix<G> a) => _int.toDouble(a.determinant());
        public override Matrix<G> mod(Matrix<G> a, Matrix<G> b)
        {
            throw new NotSupportedException();
        }
        public override bool ModduloAble => false;
        public override GenerationType GenType => _int.GenType == GenerationType.None ? GenerationType.None : GenerationType.Special;
        public override Matrix<G> Generate(IEnumerable<byte> bytes, Tuple<Matrix<G>, Matrix<G>> bounds = null, object special = null)
        {
            var size = special as Tuple<int, int, object> ?? Tuple.Create(0, 0, (object)null);
            int cells = size.Item1 * size.Item2;
            Tuple<G, G> gbounds = null;
            if (bounds != null && bounds.Item1.Any() && bounds.Item2.Any())
                gbounds = Tuple.Create(bounds.Item1[0, 0], bounds.Item2[0, 0]);
            Func<IEnumerable<byte>, G> gen = null;
            switch (_int.GenType)
            {
                case GenerationType.FromBytes:
                    gen = bytes1 => _int.Generate(bytes1);
                    break;
                case GenerationType.FromRange:
                    gen = bytes1 => _int.Generate(bytes1, gbounds);
                    break;
                case GenerationType.Special:
                    gen = bytes1 => _int.Generate(bytes1, gbounds, size.Item3);
                    break;
            }
            return new ExplicitMatrix<G>(range.Range(cells).Select(a => bytes.Skip(a).Step(cells)).Select(a => gen(a)).To2DArr(size.Item2));
        }
        public override Matrix<G> fromFraction(double a)
        {
            return new UnitMatrix<G>(_int.fromFraction(a));
        }
        public override Matrix<G> fromFraction(int a, int b)
        {
            return new UnitMatrix<G>(_int.fromFraction(a, b));
        }
    }
}
