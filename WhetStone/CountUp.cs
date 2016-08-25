using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Fielding;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public static class countUp
    {
        private class CountList<T> : LockedList<T>
        {
            private readonly T _start;
            private readonly T _step;
            public CountList(T start, T step)
            {
                _start = start;
                _step = step;
            }
            public override IEnumerator<T> GetEnumerator()
            {
                var ret = _start.ToFieldWrapper();
                for (int i = 0; i < Count; i++)
                {
                    yield return ret;
                    ret += _step;
                }
            }
            public override bool Contains(T item)
            {
                return ((item.ToFieldWrapper() - _start) % _step).isZero;
            }
            public override int Count { get; } = int.MaxValue;
            public override int IndexOf(T item)
            {
                if (!Contains(item))
                    return -1;
                return (int)((item.ToFieldWrapper() - _start) / _step);
            }
            public override T this[int index]
            {
                get
                {
                    return this._start + this._step.ToFieldWrapper() * index;
                }
            }
        }
        private class CountList : LockedList<int>
    {
        private readonly int _start;
        private readonly int _step;
        public CountList(int start, int step)
        {
            _start = start;
            _step = step;
        }
        public override IEnumerator<int> GetEnumerator()
        {
            var ret = _start;
            for (int i = 0; i < Count; i++)
            {
                yield return ret;
                ret += _step;
            }
        }
        public override bool Contains(int item)
        {
            return (item - _start) % _step == 0;
        }
        public override int Count { get; } = int.MaxValue;
        public override int IndexOf(int item)
        {
            if (!Contains(item))
                return -1;
            return (item - _start) / _step;
        }
        public override int this[int index]
        {
            get
            {
                return this._start + this._step * index;
            }
        }
    }
        public static LockedList<int> CountUp(int start = 0, int step = 1)
        {
            return new CountList(start, step);
        }
        public static LockedList<T> CountUp<T>(T start, T step)
        {
            return new CountList<T>(start, step);
        }
        public static LockedList<T> CountUp<T>(T start)
        {
            return CountUp(start, Fields.getField<T>().one);
        }
        public static LockedList<T> CountUp<T>()
        {
            return CountUp(Fields.getField<T>().zero);
        }
    }
}
