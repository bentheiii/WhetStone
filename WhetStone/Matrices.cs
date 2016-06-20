using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using WhetStone.Arrays;
using WhetStone.Arrays.Arr2D;
using WhetStone.Comparison;
using WhetStone.Fielding;
using WhetStone.Formulas;
using WhetStone.Looping;
using WhetStone.Shapes;
using WhetStone.SystemExtensions;

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
            return new ExplicitMatrix<G>(Loops.Range(cells).Select(a => bytes.Skip(a).Step(cells)).SelectToArray(a => gen(a)).to2DArr(size.Item2));
        }
        public override Matrix<G> fromFraction(double a)
        {
            return new UnitMatrix<G>(_int.fromFraction(a));
        }
        public override Matrix<G> fromFraction(int a, int b)
        {
            return new UnitMatrix<G>(_int.fromFraction(a,b));
        }
    }
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
                        return field.pow(this.Select(a => field.pow(field.abs(a), ord)).getSum(), field.Invert(ord));
                    }
                    if (order != 1)
                        throw new NotSupportedException("cannot calculate norm of non-vector of more than first order");
                    return this.getCollumns().Select(a => a.Select(field.abs).getSum()).getMax(field);
                case Normtype.Infinity:
                    return this.getRows().Select(a => a.Select(field.abs).getSum()).getMax(field);
                case Normtype.Frobenius:
                    return field.pow((this.conjugate().transpose() * this).Trace(), field.fromFraction(1, 2));
                case Normtype.Entrywise:
                    return field.pow(this.Select(a => field.pow(field.abs(a), ord)).getSum(), field.Invert(ord));
                case Normtype.Max:
                    return this.getMax(field);
                default:
                    throw new ArgumentException($"can't handle {nameof(Normtype)} "+t);
            }
        }
        public Formula<T> CharactaristicPolymonial()
        {
            return new MapMatrix<Formula<T>>((i, j) => (this[i,j] - (i==j ? Formula<T>.x : Field.zero)),rows,collumns).determinant();
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
            return b.isInfinite ? b.LeftMultiply(this) : new LazyMatrix<T>((i, j) => this.Row(i).Zip(b.Collumn(j)).Select(a=>Field.multiply(a.Item1,a.Item2)).getSum(),this.rows,b.collumns);
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
                T max = toadd.Select(a => Field.abs(a)).getMax(Field);
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
            return Loops.Range(rows).SelectToArray(this.Row);
        }
        public virtual IEnumerable<IEnumerable<T>> getCollumns()
        {
            return Loops.Range(rows).SelectToArray(this.Collumn);
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
            return Array2D.Fill(this.rows, this.collumns, (i, i1) => this[i, i1]);
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
                Loops.Range(this.rows)
                     .Select(this.Row)
                     .Select(r => r.Count(new [] {Field.zero}))
                     .getMax(out minrowindex);
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
            foreach (int row in Loops.Range(this.rows))
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
                foreach (int i in Loops.Range(this.rows).Except(row))
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
            foreach (int row in Loops.Range(this.rows))
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
                foreach (int i in Loops.Range(this.rows).Except(row))
                {
                    builder.AddRowByFactor(row, i, field.Negate(builder[i, row]));
                }
            }
            return ret;
        }
        public virtual SparceMatrix<T> toSparce()
        {
            IDictionary<T,int> occurance = new Dictionary<T, int>(Field);
            foreach (T o in this)
            {
                if (occurance.ContainsKey(o))
                    occurance[o]++;
                else
                    occurance[o] = 1;
            }
            return toSparce(occurance.getMax(new FunctionComparer<KeyValuePair<T,int>>(a=>a.Value)).Key);
        }
        public virtual SparceMatrix<T> toSparce(T defval)
        {
            ISet<Tuple<int,int,T>> coors = new HashSet<Tuple<int, int, T>>();
            foreach (Tuple<int, int> cor in Loops.Range(rows).Join(Loops.Range(collumns)))
            {
                if (!Field.ToEqualityComparer().Equals(this[cor.Item1, cor.Item2], defval))
                    coors.Add(new Tuple<int,int,T>(cor.Item1, cor.Item2, this[cor.Item1, cor.Item2]));
            }
            return new SparceMatrix<T>(defval,rows,collumns,coors);
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
            return fromArr(arr.to2DArr(rowlength));
        }
        public static Matrix<T> fromArr(T[,] arr)
        {
            return new ExplicitMatrix<T>(arr);
        }
        public virtual string toPrintable(string openerfirst = "/", string openermid = "|", string openerlast = @"\",
            string closerfirst = @"\", string closermid = "|", string closerlast = "/", string divider = " ")
        {
            return this.to2DArr().ToTablePrintable(openerfirst, openermid, openerlast, closerfirst, closermid, closerlast, divider);
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
            foreach (Tuple<int, int> tuple in Loops.Range(this.rows).Join(Loops.Range(this.collumns)))
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
            foreach (Tuple<int, int> tuple in Loops.Range(this.rows).Join(Loops.Range(this.collumns)))
            {
                ret ^= this[tuple.Item1, tuple.Item2].GetHashCode();
            }
            return ret;
        }
    }
    public class MapMatrix<T> : Matrix<T>
    {
        private readonly Func<int, int, T> _generator;
        public MapMatrix(Func<int, int, T> generator, int rows, int collumns)
        {
            this._generator = generator;
            this.rows = rows;
            this.collumns = collumns;
        }
        public MapMatrix(Func<int, int, T> generator, int size):this(generator,size,size){}
        public override T this[int i, int j]
        {
            get
            {
                if ((i < 0 || i >= rows || j < 0 || j > collumns) && !isInfinite)
                    throw new ArgumentOutOfRangeException();
                return _generator(i, j);
            }
        }
        public override int rows { get; }
        public override int collumns { get; }
    }
    public class LazyMatrix<T> : MapMatrix<T>
    {
        private readonly bool[,] _initialized;
        private readonly T[,] _values;
        public LazyMatrix(Func<int, int, T> generator, int rows, int collumns) : base(generator, rows, collumns)
        {
            _initialized = new bool[rows, collumns];
            _values = new T[rows,collumns];
        }
        public override T this[int i, int j]
        {
            get
            {
                if (!_initialized[i, j])
                {
                    _values[i, j] = base[i, j];
                    _initialized[i, j] = true;
                }
                return _values[i, j];
            }
        }
    }
    public class MemoryMatrix<T> : MapMatrix<T>
    {
        private readonly SparseArray<bool> _initialized;
        private readonly SparseArray<T> _values;
        public MemoryMatrix(Func<int, int, T> generator, int rows, int collumns) : base(generator, rows, collumns)
        {
            _initialized = new SparseArray<bool>(2);
            _values = new SparseArray<T>(2);
        }
        public override T this[int i, int j]
        {
            get
            {
                if (!_initialized[i, j])
                {
                    _values[i, j] = base[i, j];
                    _initialized[i, j] = true;
                }
                return _values[i, j];
            }
        }
    }
    public class ExplicitMatrix<T> : Matrix<T>
    {
        private readonly T[,] _values;
        public ExplicitMatrix(T[,] values)
        {
            _values = values.Copy();
        }
        public ExplicitMatrix(int rows, int collumns, Func<int,int,T> filler)
        {
            this._values = Array2D.Fill(rows, collumns, filler);
        }
        public override T this[int i, int j]
        {
            get
            {
                return this._values[i, j];
            }
        }
        public override int rows
        {
            get
            {
                return this._values.GetLength(0);
            }
        }
        public override int collumns
        {
            get
            {
                return this._values.GetLength(1);
            }
        }
    }
    public class SparceMatrix<T> : Matrix<T>
    {
        private readonly SparseArray<T> _arr;
        public override T this[int i, int j]
        {
            get
            {
                return _arr[i, j];
            }
        }
        public override int rows { get; }
        public override int collumns { get; }
        public SparceMatrix(int size, IEnumerable<Tuple<int,int,T>> coordinates) : this(size, size, coordinates) { }
        public SparceMatrix(int rows, int collumns, IEnumerable<Tuple<int,int,T>> coordinates) : this(default(T), rows, collumns, coordinates) { }
        public SparceMatrix(T defValue, int rows, int collumns, IEnumerable<Tuple<int,int,T>> coordinates)
        {
            this.rows = rows;
            this.collumns = collumns;
            _arr = new SparseArray<T>(2,defValue);
            foreach (Tuple<int, int, T> coordinate in coordinates)
            {
                _arr[coordinate.Item1, coordinate.Item2] = coordinate.Item3;
            }
        }
    }
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
            return new MapMatrix<T>((i0, i1) => this[i0,i1].ToFieldWrapper() + b[i0,i1],b.rows,b.collumns);
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
                return new UnitMatrix<T>(factor.ToFieldWrapper()*matrix.factor);
            return factor*b;
        }
        public override Matrix<T> Multiply(T x)
        {
            return new UnitMatrix<T>(factor.ToFieldWrapper()*x);
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
            return factor+"*I";
        }
        public override T[,] to2DArr()
        {
            throw new NotSupportedException(NOT_SUPPORTED_STRING);
        }
        public override IEnumerable<T> Collumn(int i)
        {
            foreach (int row in Loops.Count())
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
            return new UnitMatrix<T>(1/factor.ToFieldWrapper());
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
            return new MapMatrix<T>((i, i1) => this[i,i1],newrows,newcols);
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
        public override SparceMatrix<T> toSparce()
        {
            throw new NotSupportedException(NOT_SUPPORTED_STRING);
        }
        public override SparceMatrix<T> toSparce(T defval)
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
            foreach (int i in Loops.Count())
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
    public class MatrixBuilder<T>
    {
        private readonly T[][] _rows;
        protected readonly Field<T> _field = Fields.getField<T>();
        public MatrixBuilder(int rows, int collumns, T defVal = default(T))
        {
            if (rows < 1 || collumns < 1)
                throw new Exception("invalid matrix size");
            _rows = ArrayExtensions.Fill(rows, () => ArrayExtensions.Fill(collumns, defVal));
        }
        public MatrixBuilder(Matrix<T> source) : this(source.rows,source.collumns)
        {
            foreach (Tuple<int, int> tuple in Loops.Range(rows).Join(Loops.Range(collumns)))
            {
                this._rows[tuple.Item1][tuple.Item2] = source[tuple.Item1, tuple.Item2];
            }
        }
        public MatrixBuilder(T[,] source) : this(source.GetLength(0), source.GetLength(1))
        {
            foreach (Tuple<int, int> tuple in Loops.Range(rows).Join(Loops.Range(collumns)))
            {
                this._rows[tuple.Item1][tuple.Item2] = source[tuple.Item1, tuple.Item2];
            }
        }
        public int rows => this._rows.Length;
        public int collumns => this._rows[0].Length;
        public T this[int r, int c]
        {
            get
            {
                return _rows[r][c];
            }
            set
            {
                _rows[r][c] = value;
            }
        }
        public void SwapRows(int i, int j)
        {
            var temp = _rows[i];
            _rows[i] = _rows[j];
            _rows[j] = temp;
        }
        public void SwapCols(int i, int j)
        {
            if (i==j)
                return;
            foreach (var row in Loops.Range(rows))
            {
                var temp = this[row,i];
                this[row, i] = this[row, j];
                this[row, j] = temp;
            }
        }
        public void MultRowByFactor(int row, T factor)
        {
            if (_field.ToEqualityComparer().Equals(factor,_field.one))
                return;
            _rows[row] = _rows[row].SelectToArray(a => _field.multiply(a, factor));
        }
        public void MultColByFactor(int col, T factor)
        {
            if (_field.ToEqualityComparer().Equals(factor, _field.one))
                return;
            foreach (var row in Loops.Range(rows))
            {
                this[row, col] = _field.multiply(this[row, col], factor);
            }
        }
        public void AddRowByFactor(int sourceRow, int destRow, T factor)
        {
            if (_field.ToEqualityComparer().Equals(factor, _field.zero))
                return;
            _rows[destRow] =
                _rows[destRow].Zip(_rows[sourceRow])
                              .SelectToArray(tuple => _field.add(_field.multiply(tuple.Item2, factor), tuple.Item1));
        }
        public void AddColByFactor(int sourceCol, int destCol, T factor)
        {
            if (_field.ToEqualityComparer().Equals(factor, _field.zero))
                return;
            foreach (var row in Loops.Range(rows))
            {
                this[row, destCol] = _field.add(this[row,destCol],_field.multiply(this[row, sourceCol], factor));
            }
        }
        public Matrix<T> ToMatrix()
        {
            return new ExplicitMatrix<T>(this.ToArr());
        }
        public T[,] ToArr()
        {
            T[,] ret = new T[rows,collumns];
            foreach (Tuple<int, int> t in Loops.Range(rows).Join(Loops.Range(collumns)))
            {
                ret[t.Item1, t.Item2] = this[t.Item1, t.Item2];
            }
            return ret;
        }
        public Matrix<T> toMutableMatrix()
        {
            return new MapMatrix<T>((i, i1) => this[i,i1],rows,collumns);
        }
    }
    
}
