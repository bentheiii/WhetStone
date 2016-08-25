using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Looping;
using WhetStone.Fielding;
using WhetStone.Arrays;
using WhetStone.Comparison;

namespace WhetStone.Matrix
{
    public class MatrixBuilder<T>
    {
        private readonly T[][] _rows;
        protected readonly Field<T> _field = Fields.getField<T>();
        public MatrixBuilder(int rows, int collumns, T defVal = default(T))
        {
            if (rows < 1 || collumns < 1)
                throw new Exception("invalid matrix size");
            _rows = fill.Fill(rows, () => fill.Fill(collumns, defVal));
        }
        public MatrixBuilder(Matrix<T> source) : this(source.rows,source.collumns)
        {
            foreach (Tuple<int, int> tuple in range.Range(rows).Join(range.Range(collumns)))
            {
                this._rows[tuple.Item1][tuple.Item2] = source[tuple.Item1, tuple.Item2];
            }
        }
        public MatrixBuilder(T[,] source) : this(source.GetLength(0), source.GetLength(1))
        {
            foreach (Tuple<int, int> tuple in range.Range(rows).Join(range.Range(collumns)))
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
            foreach (var row in range.Range(rows))
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
            _rows[row] = _rows[row].Select(a => _field.multiply(a, factor)).ToArray();
        }
        public void MultColByFactor(int col, T factor)
        {
            if (_field.ToEqualityComparer().Equals(factor, _field.one))
                return;
            foreach (var row in range.Range(rows))
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
                              .Select(tuple => _field.add(_field.multiply(tuple.Item2, factor), tuple.Item1)).ToArray();
        }
        public void AddColByFactor(int sourceCol, int destCol, T factor)
        {
            if (_field.ToEqualityComparer().Equals(factor, _field.zero))
                return;
            foreach (var row in range.Range(rows))
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
            foreach (Tuple<int, int> t in range.Range(rows).Join(range.Range(collumns)))
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
