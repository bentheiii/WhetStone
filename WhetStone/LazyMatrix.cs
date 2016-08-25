using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Matrix
{
    public class LazyMatrix<T> : MapMatrix<T>
    {
        private readonly bool[,] _initialized;
        private readonly T[,] _values;
        public LazyMatrix(Func<int, int, T> generator, int rows, int collumns) : base(generator, rows, collumns)
        {
            _initialized = new bool[rows, collumns];
            _values = new T[rows, collumns];
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
}
