using System;

namespace WhetStone.Matrix
{
    public class MapMatrix<T> : Matrix<T>
    {
        private readonly Func<int, int, T> _generator;
        public MapMatrix(Func<int, int, T> generator, int rows, int collumns)
        {
            this._generator = generator;
            this.rows = rows;
            this.collumns = collumns;
        }
        public MapMatrix(Func<int, int, T> generator, int size) : this(generator, size, size) { }
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
}
