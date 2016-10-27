using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    public class SparseArray<T>
    {
        private struct Coordinate : IComparable<Coordinate>
        {
            private readonly int[] _cors;
            public Coordinate(params int[] cors)
            {
                _cors = cors;
            }
            public override int GetHashCode()
            {
                return _cors.GetHashCode() ^ _cors.Length;
            }
            public override bool Equals(object obj)
            {
                Coordinate? c = obj as Coordinate?;
                return c != null && _cors.SequenceEqual(c.Value._cors);
            }
            public override string ToString()
            {
                return _cors.StrConcat();
            }
            public int CompareTo(Coordinate other)
            {
                if (other._cors.Length != _cors.Length)
                    return _cors.Length.CompareTo(other._cors.Length);
                int ret = 0;
                var cors = _cors.Zip(other._cors).GetEnumerator();
                while (ret == 0 && cors.MoveNext())
                    ret = cors.Current.Item1.CompareTo(cors.Current.Item2);
                return ret;
            }
        }
        private readonly IDictionary<Coordinate, T> _values;
        private readonly int[] _dim;
        public SparseArray(int dims, T def = default(T))
        {
            if (dims <= 0)
                throw new ArgumentException("must be positive", nameof(dims));
            this._dim = new int[dims];
            this.defaultValue = def;
            this._values = new SortedDictionary<Coordinate, T>();
        }
        public T defaultValue { get; }
        public T this[params int[] query]
        {
            get
            {
                Coordinate co = new Coordinate(query);
                return _values.ContainsKey(co) ? _values[co] : defaultValue;
            }
            set
            {
                Coordinate co = new Coordinate(query);
                if (query.Length != _dim.Length)
                    throw new ArgumentException("incorrect number of arguments for " + _dim.Length + " rank array");
                for (int i = 0; i < query.Length; i++)
                {
                    if (query[i] > _dim[i])
                        _dim[i] = query[i] + 1;
                }
                this._values[co] = value;
            }
        }
        public int GetLength(int i)
        {
            return _dim[i];
        }
    }
}
