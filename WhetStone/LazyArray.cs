using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Looping;

namespace WhetStone.Arrays
{
    public class LazyArray<T> : IEnumerable<T>
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
        public void RemoveAt(int index)
        {
            _initialized[index] = false;
        }
        public T this[int ind]
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
        public IEnumerator<T> GetEnumerator()
        {
            return countUp.CountUp().Select(a => this[a]).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void Clear()
        {
            _data.Clear();
            _initialized.Clear();
        }
    }
}
