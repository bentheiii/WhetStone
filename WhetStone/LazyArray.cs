using System;
using System.Collections;
using System.Collections.Generic;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public class LazyArray<T> : LockedList<T>
    {
        private readonly Func<int, LazyArray<T>, T> _generator;
        private readonly ExpandingArray<T> _data;
        private readonly ExpandingArray<bool> _initialized;
        public LazyArray(Func<int, T> generator) : this((i, array) => generator(i)) { }
        public LazyArray(Func<int, LazyArray<T>, T> generator)
        {
            _generator = generator;
            _data = new ExpandingArray<T>();
            _initialized = new ExpandingArray<bool>();
        }
        public bool Initialized(int index)
        {
            return _initialized[index];
        }
        public void ClearAt(int index)
        {
            _initialized[index] = false;
        }
        public override T this[int ind]
        {
            get
            {
                if (_initialized[ind])
                    return _data[ind];
                T ret = _data[ind] = _generator(ind, this);
                _initialized[ind] = true;
                return ret;
            }
        }
        public override IEnumerator<T> GetEnumerator()
        {
            return countUp.CountUp().Select(a => this[a]).GetEnumerator();
        }
        public override int Count => _data.Count;
        public void ClearAll()
        {
            _data.Clear();
            _initialized.Clear();
        }
        public void fill(IList<T> arr, int start = 0)
        {
            foreach (var tuple in arr.CountBind(start))
            {
                _initialized[tuple.Item2] = true;
                _data[tuple.Item2] = tuple.Item1;
            }
        }
    }
}
