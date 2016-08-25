using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using WhetStone.Arrays;
using WhetStone.Comparison;
using WhetStone.Fielding;
using WhetStone.Looping;
using WhetStone.Shapes;

namespace WhetStone.Matrix
{
    public abstract class Matrix<T> : IEnumerable<T>
    {
        static Matrix()
        {
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(T).TypeHandle);
            Fields.setField(new MatrixField<T>(Fields.getField<T>()));
        } 
        protected static readonly Field<T> Field = Fields.getField<T>();
        public virtual bool isInfinite
        {
            get
            {
                return false;
            }
        }
        public abstract T this[int i, int j] { get; }
        public abstract int rows { get; }
        public abstract int collumns { get; }
        public Size Size
        {
            get
            {
                return new Size(collumns, rows);
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }
        public virtual bool isSquare
        {
            get
            {
                return (this.rows == this.collumns);
            }
        }
        public VectorType isVector
        {
            get
            {
                if (isInfinite || (rows != 1 && collumns != 1) || (rows * collumns) <= 0)
                    return VectorType.None;
                return this.rows == 1 ? VectorType.Row : VectorType.Collumn;
            }
        }
        public virtual Matrix<T> subMatrix(int i, int j)
        {
            if (!Size.IsWithin(j,i))
                throw new ArgumentOutOfRangeException("both i and j must be within the matrix size");
            return new MapMatrix<T>((a, b) =>
            {
                if (a >= i)
                    a++;
                if (b >= j)
                    b++;
                return this[a, b];
            }, this.rows - 1, this.collumns - 1);
        }
        public T Cofactor(int i, int j)
        {
            T ret = this.subMatrix(i, j).determinant();
            if ((i + j) % 2 == 1)
                ret = Field.Negate(ret);
            return ret;
        }
        public Matrix<T> Cofactor()
        {
            return new LazyMatrix<T>(this.Cofactor,this.rows, this.collumns);
        }
        public enum Normtype { Natural, Infinity, Entrywise, Frobenius, Max }
        public virtual T norm(int order = 2, Normtype t = Normtype.Natural)
        {
            var field = Fields.getField<T>();
            T ord = field.fromInt(order);
            switch (t)
            {
                case Normtype.Natural:
                    if (order == 0)
                        return field.fromInt(rows);
                    if (this.isVector != VectorType.Collumn)
                    {
                        return field.pow(this.Select(a => field.pow(field.abs(a), ord)).GetSum(), field.Invert(ord));
                    }
                    if (order != 1)
                        throw new NotSupportedException("cannot calculate norm of non-vector of more than first order");
                    return this.getCollumns().Select(a => a.Select(field.abs).GetSum()).GetMax(field);
                case Normtype.Infinity:
                    return this.getRows().Select(a => a.Select(field.abs).GetSum()).GetMax(field);
                case Normtype.Frobenius:
                    return field.pow((this.conjugate().transpose() * this).Trace(), field.fromFraction(1, 2));
                case Normtype.Entrywise:
                    return field.pow(this.Select(a => field.pow(field.abs(a), ord)).GetSum(), field.Invert(ord));
                case Normtype.Max:
                    return this.GetMax(field);
                default:
                    throw new ArgumentException($"can't handle {nameof(Normtype)} "+t);
            }
        }
        public virtual T determinant()
        {
            return this.JordanDeterminant();
        }
        public virtual bool invertible()
        {
            return this.isSquare && Field.Invertible && !this.determinant().Equals(Field.zero);
        }
        public virtual Matrix<T> inverse()
        {
            return this.JordanInvert();
        }
        public virtual Matrix<T> transpose()
        {
            return new MapMatrix<T>((i, j) => this[j, i], this.collumns, this.rows);
        }
        public virtual bool canAdd(Matrix<T> b)
        {
            return (this.rows == b.rows && this.collumns == b.collumns) || isInfinite || b.isInfinite;
        }
        public virtual Matrix<T> Add(Matrix<T> b)
        {
            if (!this.canAdd(b))
                throw new ArithmeticException("can't add incompatible matrices");
            return b.isInfinite ? b.Add(this) : new MapMatrix<T>((i, i1) => Field.add(this[i,i1],b[i,i1]), b.rows, b.collumns);
        }
        public virtual bool canMultiply(Matrix<T> b)
        {
            return this.collumns == b.rows || isInfinite || b.isInfinite;
        }
        public virtual Matrix<T> Multiply(Matrix<T> b)
        {
            if (!this.canMultiply(b))
                throw new ArithmeticException("can't multiply incompatible matrices");
            return b.isInfinite ? b.LeftMultiply(this) : new LazyMatrix<T>((i, j) => this.Row(i).Zip(b.Collumn(j)).Select(a=>Field.multiply(a.Item1,a.Item2)).GetSum(),this.rows,b.collumns);
        }
        public virtual Matrix<T> LeftMultiply(Matrix<T> b)
        {
            return b.Multiply(this);
        } 
        public virtual Matrix<T> Multiply(T x)
        {
            return this.MapTransform(a => Field.multiply(a, x));
        }
        public virtual Matrix<T> conjugate()
        {
            return this.MapTransform(Field.Conjugate);
        }
        public virtual Matrix<T> LazyTransform(Func<T, T> gen)
        {
            return new LazyMatrix<T>((i, i1) => gen(this[i,i1]),this.rows,this.collumns);
        }
        public virtual Matrix<T> MapTransform(Func<T, T> gen)
        {
            return new MapMatrix<T>((i, i1) => gen(this[i, i1]), this.rows, this.collumns);
        }
        public static Matrix<T> getIdent(int size)
        {
            return getIdent(size, Field.one);
        }
        public static Matrix<T> getIdent()
        {
            return getIdent(Field.one);
        }
        public static Matrix<T> getIdent(T val)
        {
            return new UnitMatrix<T>(val);
        }
        public static Matrix<T> getIdent(int size, T val)
        {
            return
                new MapMatrix<T>(
                    (i, i1) => (i == i1) ? val : Field.zero, size,
                    size);
        }
        public static Matrix<T> getZero()
        {
            return getIdent(Field.zero);
        }
        public static Matrix<T> getZero(int size)
        {
            return getZero(size, size);
        }
        public static Matrix<T> getZero(int row, int cols)
        {
            return new MapMatrix<T>((i, i1) => Field.zero,row,cols);
        }
        public virtual Matrix<T> pow(int p)
        {
            if (!this.isSquare)
                throw new ArithmeticException("cant raise a non-square matrix to a power");
            if (p == 0)
            {
                return getIdent(this.rows);
            }
            if (p < 0)
            {
                if (!this.invertible())
                    throw new ArithmeticException("cant raise an invertible matrix to a negative power");
                return this.inverse().pow(-p);
            }
            if (p == 1)
            {
                return this;
            }
            Matrix<T> part = this.pow(p / 2);
            Matrix<T> ret = part.Multiply(part);
            if (p % 2 == 1)
                ret *= this;
            return ret;
        }
        public Matrix<T> pow(Matrix<T> p)
        {
            T v;
            if (!p.isIdent(out v))
            {
                throw new ArithmeticException("can only raise a matrix to the power of an identity matrix");
            }
            double pow = Field.toDouble(v) ?? 0.5;
            if (pow % 1 != 0)
                throw new ArithmeticException("can't raise a matrix to a non-integer number");
            return this.pow((int)(pow));
        }
        public Matrix<T> exp(T tolerance, T @base)
        {
            return (this * Field.log(@base)).exp(tolerance);
        }
        public virtual Matrix<T> exp(T tolerance)
        {
            if (!this.isSquare)
                throw new Exception("can't exponentiate a non-square matrix");
            Matrix<T> ret = getZero(this.rows);
            Matrix<T> pow = getIdent(this.rows);
            T counter = Field.zero;
            T factorial = Field.one;
            while (true)
            {
                var toadd = pow * Field.Invert(factorial);
                ret += toadd;
                counter = Field.add(counter, Field.one);
                factorial = Field.multiply(factorial, counter);
                T max = toadd.Select(a => Field.abs(a)).GetMax(Field);
                if (Field.Compare(max, tolerance) < 0)
                {
                    break;
                }
                pow *= this;
            }
            return ret;
        }
        public virtual T Trace()
        {
            if (!this.isSquare)
                throw new ArithmeticException("cannot trace non-square matrix");
            T ret = Field.zero;
            for (int i = 0; i < this.rows; i++)
            {
                ret = Field.add(ret, this[i, i]);
            }
            return ret;
        }
        public T dotProduct(Matrix<T> b)
        {
            if (!this.canAdd(b))
                throw new ArithmeticException("can't inner product incompatible matrices");
            return b.conjugate().transpose().Multiply(this).Trace();
        }
        public virtual IEnumerable<IEnumerable<T>> getRows()
        {
            return range.Range(rows).Select(this.Row);
        }
        public virtual IEnumerable<IEnumerable<T>> getCollumns()
        {
            return range.Range(rows).Select(this.Collumn);
        }
        public virtual IEnumerable<T> Row(int i)
        {

            T[] ret = new T[this.collumns];
            for (int j = 0; j < this.collumns; j++)
            {
                ret[j] = this[i, j];
            }
            return ret;
        }
        public virtual IEnumerable<T> Collumn(int i)
        {
            T[] ret = new T[this.rows];
            for (int j = 0; j < this.rows; j++)
            {
                ret[j] = this[j, i];
            }
            return ret;
        }
        public virtual T[,] to2DArr()
        {
            return fill2D.Fill2D(this.rows, this.collumns, (i, i1) => this[i, i1]);
        }
        public virtual bool isIdent(out T val)
        {
            val = this[0, 0];
            if (!this.isSquare)
                return false;
            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.collumns; j++)
                {
                    if (!this[i, j].Equals((i == j ? this[1, 1] : Field.zero)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public virtual T CofactorDeterminant()
        {
            int minrowindex;
            int minrow =
                range.Range(this.rows)
                     .Select(this.Row)
                     .Select(r => r.Count(new [] {Field.zero}))
                     .GetMax(out minrowindex);
            return this.CofactorDeterminant(minrowindex);
        }
        public virtual T CofactorDeterminant(int row)
        {
            if (!this.isSquare)
                throw new ArithmeticException("can't get determinant of non-square matrix");
            if (this.rows == 1)
                return this[0, 0];
            T ret = Field.zero;
            for (int i = 0; i < this.collumns; i++)
            {
                if (!this[row, i].Equals(Field.zero))
                    ret = Field.add(ret, Field.multiply(this.Cofactor(row, i), this[row, i]));
            }
            return ret;
        }
        public virtual Matrix<T> CofactorInvert()
        {
            if (!this.isSquare)
                throw new ArithmeticException("cannot invert non-square matrix");
            T det = this.determinant();
            if (det.Equals(Field.zero))
                throw new ArithmeticException("matrix is invertible");
            return new LazyMatrix<T>((i, i1) => Field.divide(this.Cofactor(i1, i), det), this.rows, this.collumns);
        }
        public virtual Matrix<T> JordanInvert()
        {
            var field = Fields.getField<T>();
            if (!this.isSquare)
                throw new ArithmeticException("cannot invert non-square matrix");
            MatrixBuilder<T> ret = new MatrixBuilder<T>(this.to2DArr().Concat(getIdent(this.rows).to2DArr(), 1));
            foreach (int row in range.Range(this.rows))
            {
                int pivotsearcher = row;
                while (pivotsearcher < this.rows && ret[pivotsearcher, row].Equals(field.zero))
                {
                    pivotsearcher++;
                }
                if (pivotsearcher == this.rows)
                    throw new ArithmeticException("matrix is singular!");
                if (pivotsearcher != row)
                    ret.SwapRows(pivotsearcher, row);
                ret.MultRowByFactor(row, field.Invert(ret[row, row]));
                foreach (int i in range.Range(this.rows).Except(row))
                {
                    ret.AddRowByFactor(row, i, field.Negate(ret[i, row]));
                }
            }
            var m = ret.toMutableMatrix();
            return new MapMatrix<T>((i, j) => m[i, j + this.rows], this.rows, this.rows);
        }
        public virtual T JordanDeterminant()
        {
            var field = Fields.getField<T>();
            if (!this.isSquare)
                throw new ArithmeticException("cannot get determinant of non-square matrix");
            var ret = field.one;
            MatrixBuilder<T> builder = new MatrixBuilder<T>(this.to2DArr());
            foreach (int row in range.Range(this.rows))
            {
                int pivotsearcher = row;
                while (pivotsearcher < this.rows && builder[pivotsearcher, row].Equals(field.zero))
                {
                    pivotsearcher++;
                }
                if (pivotsearcher == this.rows)
                    return field.zero;
                if (pivotsearcher != row)
                {
                    builder.SwapRows(pivotsearcher, row);
                    ret = field.Negate(ret);
                }
                ret = field.multiply(ret, builder[row, row]);
                builder.MultRowByFactor(row, field.Invert(builder[row, row]));
                foreach (int i in range.Range(this.rows).Except(row))
                {
                    builder.AddRowByFactor(row, i, field.Negate(builder[i, row]));
                }
            }
            return ret;
        }
        public enum VectorType { Row, Collumn, None };
        public static Matrix<T> fromArr(T[] arr, VectorType v = VectorType.Collumn)
        {
            if (v == VectorType.None)
                throw new ArgumentException("cannot process vector type \"None\"");
            return fromArr(arr, v == VectorType.Collumn ? arr.Length : 1);
        }
        public static Matrix<T> fromArr(T[] arr, int rowlength)
        {
            return fromArr(arr.To2DArr(rowlength));
        }
        public static Matrix<T> fromArr(T[,] arr)
        {
            return new ExplicitMatrix<T>(arr);
        }
        public virtual string toPrintable(string openerfirst = "/", string openermid = "|", string openerlast = @"\",
            string closerfirst = @"\", string closermid = "|", string closerlast = "/", string divider = " ")
        {
            return this.to2DArr().Str2DConcat(openerfirst, openermid, openerlast, closerfirst, closermid, closerlast, divider);
        }
        public virtual Matrix<T> resize(int newrows, int newcols)
        {
            if (rows != newrows || collumns != newcols)
                throw new ArgumentException("this matrix is already well-sized");
            return this;
        } 
        public Matrix<T> resize(int newsize)
        {
            return resize(newsize, newsize);
        }
        public static Matrix<T> operator -(Matrix<T> a)
        {
            return a.Multiply(Field.negativeone);
        }
        public static Matrix<T> operator ~(Matrix<T> a)
        {
            return a.conjugate();
        }
        public static Matrix<T> operator +(Matrix<T> a, Matrix<T> b)
        {
            return a.Add(b);
        }
        public static Matrix<T> operator -(Matrix<T> a, Matrix<T> b)
        {
            return a.Add(-b);
        }
        public static Matrix<T> operator *(Matrix<T> a, Matrix<T> b)
        {
            return a.Multiply(b);
        }
        public static Matrix<T> operator *(Matrix<T> a, T b)
        {
            return a.Multiply(b);
        }
        public static Matrix<T> operator *(T b, Matrix<T> a)
        {
            return a.Multiply(b);
        }
        public virtual IEnumerator<T> GetEnumerator()
        {
            foreach (Tuple<int, int> tuple in range.Range(this.rows).Join(range.Range(this.collumns)))
            {
                yield return this[tuple.Item1, tuple.Item2];
            }
        }
        public override bool Equals(object obj)
        {
            Matrix<T> matrix = obj as Matrix<T>;
            if (matrix != null)
            {
                if (matrix.isInfinite)
                    return matrix.Equals(this);
                var f = Fields.getField<T>();
                if (matrix.rows != this.rows || matrix.collumns != this.collumns)
                    return false;
                return (matrix - this).All(a => f.ToEqualityComparer().Equals(a, f.zero));
            }
            return false;
        }
        public override int GetHashCode()
        {
            int ret = 0;
            foreach (Tuple<int, int> tuple in range.Range(this.rows).Join(range.Range(this.collumns)))
            {
                ret ^= this[tuple.Item1, tuple.Item2].GetHashCode();
            }
            return ret;
        }
    }
}
