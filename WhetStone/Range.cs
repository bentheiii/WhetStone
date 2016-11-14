using System;
using System.Collections.Generic;
using WhetStone.Fielding;
using WhetStone.NumbersMagic;
using WhetStone.LockedStructures;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    public static class range
    {
        private class RangeList<T> : LockedList<T>
        {
            private readonly T _start;
            private readonly T _end;
            private readonly T _step;
            private readonly bool _pos;
            public RangeList(T start, T end, T step, bool inclusive = false, bool pos = true)
            {
                _start = start;
                _end = end;
                _step = step;
                this._pos = pos;
                var gap = end - _start.ToFieldWrapper();
                Count = pos.Indicator(1, -1) * (int)(gap / _step) + ((gap % _step).isZero ? 0 : 1) + (inclusive ? 1 : 0);
            }
            public override IEnumerator<T> GetEnumerator()
            {
                var ret = _start.ToFieldWrapper();
                for (int i = 0; i < Count; i++)
                {
                    yield return ret;
                    if (_pos)
                        ret += _step;
                    else
                        ret -= _step;
                }
            }
            public override bool Contains(T item)
            {
                return item.iswithinPartialExclusive(_start, _end) && ((item.ToFieldWrapper() - _start) % _step).isZero;
            }
            public override int Count { get; }
            public override int IndexOf(T item)
            {
                if (!Contains(item))
                    return -1;
                return _pos.Indicator(1, -1) * (int)((item.ToFieldWrapper() - _start) / _step);
            }
            public override T this[int index]
            {
                get
                {
                    if (index < 0 || index >= Count)
                        throw new IndexOutOfRangeException();
                    if (_pos)
                        return this._start + this._step.ToFieldWrapper() * index;
                    return this._start - this._step.ToFieldWrapper() * index;
                }
            }
        }
        private class RangeList : LockedList<int>
        {
            private readonly int _start;
            private readonly int _end;
            private readonly int _step;
            public RangeList(int start, int end, int step, bool inclusive = false)
            {
                _start = start;
                _end = end;
                _step = step;
                var gap = end - _start;
                Count = (gap / _step) + (gap % _step == 0 ? 0 : 1) + (inclusive ? 1 : 0);
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
                return item.iswithinPartialExclusive(_start, _end) && ((item - _start) % _step) == 0;
            }
            public override int Count { get; }
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
                    if (index < 0 || index >= Count)
                        throw new IndexOutOfRangeException();
                    return this._start + this._step * index;
                }
            }
        }
        public static LockedList<T> Range<T>(T start, T max, T step)
        {
            var field = Fields.getField<T>();
            //try rewrite
            if (!field.Negatable || field.isPositive(step, true))
            {
                return new RangeList<T>(start, max, step);
            }
            return Range(field.Negate(start), field.Negate(max), field.Negate(step)).Select(a => field.Negate(a));
        }
        public static LockedList<T> Range<T>(T start, T max)
        {
            var f = Fields.getField<T>();
            if (f.Compare(start, max) < 0)
                return Range(start, max, f.one);
            if (f.Negatable)
                return Range(start, max, f.negativeone);
            return new RangeList<T>(start, max, f.one, pos: false);
        }
        public static LockedList<T> Range<T>(T max)
        {
            return Range(Fields.getField<T>().zero, max);
        }
        public static LockedList<int> Range(int start, int max, int step)
        {
            if (step >= 0)
                return new RangeList(start, max, step);
            return Range(-start, -max, -step).Select(a => -a);
        }
        public static LockedList<int> Range(int start, int max)
        {
            return Range(start, max, start < max ? 1 : -1);
        }
        public static LockedList<int> Range(int max)
        {
            return Range(0, max);
        }
        public static LockedList<T> IRange<T>(T start, T max, T step)
        {
            var field = Fields.getField<T>();
            //try rewrite
            if (!field.Negatable || field.isPositive(step, true))
            {
                return new RangeList<T>(start, max, step, true);
            }
            return IRange(field.Negate(start), field.Negate(max), field.Negate(step)).Select(a => field.Negate(a));
        }
        public static LockedList<T> IRange<T>(T start, T max)
        {
            var f = Fields.getField<T>();
            if (f.Compare(start, max) < 0)
                return IRange(start, max, f.one);
            if (f.Negatable)
                return IRange(start, max, f.negativeone);
            return new RangeList<T>(start, max, f.one, true, pos: false);
        }
        public static LockedList<T> IRange<T>(T max)
        {
            return IRange(Fields.getField<T>().zero, max);
        }
        public static LockedList<int> IRange(int start, int max, int step)
        {
            //try rewrite
            if (step >= 0)
            {
                return new RangeList(start, max, step, true);
            }
            return IRange(-start, -max, -step).Select(a => -a);
        }
        public static LockedList<int> IRange(int start, int max)
        {
            return IRange(start, max, start < max ? 1 : -1);
        }
        public static LockedList<int> IRange(int max)
        {
            return IRange(0, max);
        }
    }
}
