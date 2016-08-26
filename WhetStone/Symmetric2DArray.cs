using System;

namespace WhetStone.Looping
{
    public class Symmetric2DArray<T>
    {
        private readonly T[] _data;
        private int Size { get; }
        public bool Reflexive { get; }
        public Symmetric2DArray(int size, bool reflexive = true)
        {
            this.Size = size;
            if (Size < 0)
                throw new ArgumentException("must be non-negative", nameof(size));
            Reflexive = reflexive;
            _data = new T[(this.Size * (this.Size + (reflexive ? 1 : -1))) / 2];
        }
        private int getindex(int r, int c)
        {
            if (r >= this.Size || c >= this.Size)
                throw new IndexOutOfRangeException();
            if (r > c)
                return getindex(c, r);
            if (this.Reflexive)
                return (int)(r * (this.Size - (r + 1) / 2.0) + c);
            if (r == c)
                throw new IndexOutOfRangeException("this matrix is non-reflexive");
            return (int)(r * (this.Size - (r + 3) / 2.0) + c - 1);
        }
        public int length
        {
            get
            {
                return _data.Length;
            }
        }
        public T this[int row, int col]
        {
            get
            {
                if (row >= Size || col >= Size)
                    throw new IndexOutOfRangeException();
                return _data[this.getindex(row, col)];
            }
            set
            {
                if (row >= Size || col >= Size)
                    throw new IndexOutOfRangeException();
                _data[this.getindex(row, col)] = value;
            }
        }
        public int GetLength(int i)
        {
            switch (i)
            {
                case 1:
                case 0:
                    return this.Size;
                default:
                    throw new IndexOutOfRangeException();
            }
        }
        public T[,] toArr()
        {
            T[,] ret = new T[this.Size, this.Size];
            for (int i = 0; i < ret.GetLength(0); i++)
            {
                for (int j = 0; j < ret.GetLength(1); j++)
                    ret[i, j] = ((i == j && !this.Reflexive) ? default(T) : this[i, j]);
            }
            return ret;
        }
    }
}
