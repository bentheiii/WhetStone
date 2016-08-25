using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Fielding;
using WhetStone.Looping;

namespace WhetStone.Matrix
{
    public class UnitMatrix<T> : Matrix<T>
    {
        public T factor { get; }
        public UnitMatrix(T factor)
        {
            this.factor = factor;
        }
        public override T this[int i, int j]
        {
            get
            {
                return i == j ? factor : Field.zero;
            }
        }
        public const int UNDEF_SIZE = -1;
        public override int rows
        {
            get
            {
                return UNDEF_SIZE;
            }
        }
        public override int collumns
        {
            get
            {
                return UNDEF_SIZE;
            }
        }
        public override Matrix<T> Add(Matrix<T> b)
        {
            UnitMatrix<T> matrix = b as UnitMatrix<T>;
            if (matrix != null)
                return new UnitMatrix<T>(factor.ToFieldWrapper() + matrix.factor);
            return new MapMatrix<T>((i0, i1) => this[i0, i1].ToFieldWrapper() + b[i0, i1], b.rows, b.collumns);
        }
        public override bool isInfinite => true;
        public override Matrix<T> pow(int p)
        {
            return new UnitMatrix<T>(factor.ToFieldWrapper().pow(p));
        }
        public override Matrix<T> Multiply(Matrix<T> b)
        {
            UnitMatrix<T> matrix = b as UnitMatrix<T>;
            if (matrix != null)
                return new UnitMatrix<T>(factor.ToFieldWrapper() * matrix.factor);
            return factor * b;
        }
        public override Matrix<T> Multiply(T x)
        {
            return new UnitMatrix<T>(factor.ToFieldWrapper() * x);
        }
        public override Matrix<T> LeftMultiply(Matrix<T> b)
        {
            return this.Multiply(b);
        }
        const string NOT_SUPPORTED_STRING = "function not supported for infinite matrix, try resizing it first";
        public override IEnumerator<T> GetEnumerator()
        {
            throw new NotSupportedException(NOT_SUPPORTED_STRING);
        }
        public override bool Equals(object obj)
        {
            Matrix<T> matrix = obj as Matrix<T>;
            if (matrix != null)
            {
                UnitMatrix<T> unit = matrix as UnitMatrix<T>;
                if (unit != null)
                    return factor.ToFieldWrapper().Equals(unit.factor);
                foreach (Tuple<T, int, int> tuple in matrix.to2DArr().CoordinateBind())
                {
                    if (!this[tuple.Item2, tuple.Item3].ToFieldWrapper().Equals(tuple.Item1))
                        return false;
                }
                return true;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return factor.GetHashCode();
        }
        public override string ToString()
        {
            return factor + "*I";
        }
        public override T[,] to2DArr()
        {
            throw new NotSupportedException(NOT_SUPPORTED_STRING);
        }
        public override IEnumerable<T> Collumn(int i)
        {
            foreach (int row in countUp.CountUp())
            {
                if (row == i)
                    yield return factor;
                else
                    yield return Field.zero;
            }
        }
        public override Matrix<T> LazyTransform(Func<T, T> gen)
        {
            throw new NotSupportedException(NOT_SUPPORTED_STRING);
        }
        public override Matrix<T> MapTransform(Func<T, T> gen)
        {
            throw new NotSupportedException(NOT_SUPPORTED_STRING);
        }
        public override IEnumerable<T> Row(int i)
        {
            return Collumn(i);
        }
        public override T Trace()
        {
            throw new NotSupportedException(NOT_SUPPORTED_STRING);
        }
        public override Matrix<T> conjugate()
        {
            return new UnitMatrix<T>(~factor.ToFieldWrapper());
        }
        public override T determinant()
        {
            if (factor.ToFieldWrapper().Equals(Field.zero) || factor.ToFieldWrapper().Equals(Field.one))
                return factor;
            throw new NotSupportedException(NOT_SUPPORTED_STRING);
        }
        public override Matrix<T> exp(T tolerance)
        {
            return new UnitMatrix<T>(Field.naturalbase.ToFieldWrapper().pow(factor));
        }
        public override Matrix<T> inverse()
        {
            return new UnitMatrix<T>(1 / factor.ToFieldWrapper());
        }
        public override bool isIdent(out T val)
        {
            val = factor;
            return true;
        }
        public override T norm(int order = 2, Normtype t = Normtype.Natural)
        {
            if (factor.ToFieldWrapper().Equals(Field.zero))
                return Field.zero;
            switch (t)
            {
                case Normtype.Natural:
                case Normtype.Infinity:
                case Normtype.Max:
                    if (order != 1 && t == Normtype.Natural)
                        throw new NotSupportedException("cannot calculate norm of non-vector of more than first order");
                    return factor;
            }
            throw new NotSupportedException(NOT_SUPPORTED_STRING);
        }
        public override Matrix<T> resize(int newrows, int newcols)
        {
            return new MapMatrix<T>((i, i1) => this[i, i1], newrows, newcols);
        }
        public override bool isSquare
        {
            get
            {
                return true;
            }
        }
        public override Matrix<T> subMatrix(int i, int j)
        {
            throw new NotSupportedException(NOT_SUPPORTED_STRING);
        }
        public override string toPrintable(string openerfirst = "/", string openermid = "|", string openerlast = "\\", string closerfirst = "\\",
                                           string closermid = "|", string closerlast = "/", string divider = " ")
        {
            throw new NotSupportedException(NOT_SUPPORTED_STRING);
        }
        public override Matrix<T> transpose()
        {
            return this;
        }
        public override T CofactorDeterminant()
        {
            throw new NotSupportedException(NOT_SUPPORTED_STRING);
        }
        public override T CofactorDeterminant(int row)
        {
            throw new NotSupportedException(NOT_SUPPORTED_STRING);
        }
        public override Matrix<T> CofactorInvert()
        {
            throw new NotSupportedException(NOT_SUPPORTED_STRING);
        }
        public override T JordanDeterminant()
        {
            throw new NotSupportedException(NOT_SUPPORTED_STRING);
        }
        public override Matrix<T> JordanInvert()
        {
            throw new NotSupportedException(NOT_SUPPORTED_STRING);
        }
        public override bool canAdd(Matrix<T> b)
        {
            return true;
        }
        public override bool canMultiply(Matrix<T> b)
        {
            return true;
        }
        public override IEnumerable<IEnumerable<T>> getCollumns()
        {
            foreach (int i in countUp.CountUp())
            {
                yield return Collumn(i);
            }
        }
        public override IEnumerable<IEnumerable<T>> getRows()
        {
            return getCollumns();
        }
        public override bool invertible()
        {
            return !factor.ToFieldWrapper().Equals(Field.zero) && Field.Invertible;
        }
    }
}
