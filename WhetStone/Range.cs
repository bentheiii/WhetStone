using System;
using System.Collections.Generic;
using NumberStone;
using WhetStone.Fielding;
using WhetStone.LockedStructures;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class range
    {
        #region exclusive positive
        private class RangeListExPos<T> : LockedList<T>
        {
            private readonly FieldWrapper<T> _start;
            private readonly T _end;
            private readonly FieldWrapper<T> _step;
            public RangeListExPos(T start, T end, T step)
            {
                _start = start.ToFieldWrapper();
                _end = end;
                _step = step.ToFieldWrapper();
                var gap = end - _start;
                var ratio = ((double?)gap / (double?)_step);
                if (ratio == null)
                    throw new Exception("gap is nonlinear!");
                Count = ratio.Value <= 0 ? 0 : ratio.Value.Ceil();
            }
            public override IEnumerator<T> GetEnumerator()
            {
                var ret = _start;
                while (ret < _end)
                {
                    yield return ret;
                    ret += _step;
                }
            }
            public override bool Contains(T item)
            {
                return item.iswithinPartialExclusive(_start.val, _end) && ((item - _start) % _step).isZero;
            }
            public override int Count { get; }
            public override int IndexOf(T item)
            {
                if (!Contains(item))
                    return -1;
                return (int)((item - _start) / _step);
            }
            public override T this[int index]
            {
                get
                {
                    if (index < 0 || index >= Count)
                        throw new IndexOutOfRangeException();
                    return this._start + this._step*index;
                }
            }
        }
        private class RangeListExPos : LockedList<int>
        {
            private readonly int _start;
            private readonly int _end;
            private readonly int _step;
            public RangeListExPos(int start, int end, int step)
            {
                _start = start;
                _end = end;
                _step = step;
                var gap = end - _start;
                var ratio = (gap / (double)_step);
                Count = ratio <= 0 ? 0 : ratio.Ceil();
            }
            public override IEnumerator<int> GetEnumerator()
            {
                var ret = _start;
                while (ret < _end)
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
        #endregion
        #region exclusive negative
        private class RangeListExNeg<T> : LockedList<T>
        {
            private readonly FieldWrapper<T> _start;
            private readonly T _end;
            private readonly FieldWrapper<T> _step;
            public RangeListExNeg(T start, T end, T step)
            {
                _start = start.ToFieldWrapper();
                _end = end;
                _step = step.ToFieldWrapper();
                var gap = end - _start;
                var ratio = ((double?)gap / -(double?)_step);
                if (ratio == null)
                    throw new Exception("gap is nonlinear!");
                Count = ratio.Value.Ceil();
            }
            public override IEnumerator<T> GetEnumerator()
            {
                var ret = _start;
                while (ret > _end)
                {
                    yield return ret;
                    ret -= _step;
                }
            }
            public override bool Contains(T item)
            {
                return item.iswithinPartialExclusive(_start.val, _end) && ((item - _start) % _step).isZero;
            }
            public override int Count { get; }
            public override int IndexOf(T item)
            {
                if (!Contains(item))
                    return -1;
                return (int)((item - _start) / -_step);
            }
            public override T this[int index]
            {
                get
                {
                    if (index < 0 || index >= Count)
                        throw new IndexOutOfRangeException();
                    return this._start - this._step * index;
                }
            }
        }
        private class RangeListExNeg : LockedList<int>
        {
            private readonly int _start;
            private readonly int _end;
            private readonly int _step;
            public RangeListExNeg(int start, int end, int step)
            {
                _start = start;
                _end = end;
                _step = step;
                var gap = end - _start;
                var ratio = (gap / (double)-_step);
                Count = ratio <= 0 ? 0 : ratio.Ceil();
            }
            public override IEnumerator<int> GetEnumerator()
            {
                var ret = _start;
                while (ret > _end)
                {
                    yield return ret;
                    ret -= _step;
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
                return (item - _start) / -_step;
            }
            public override int this[int index]
            {
                get
                {
                    if (index < 0 || index >= Count)
                        throw new IndexOutOfRangeException();
                    return this._start - this._step * index;
                }
            }
        }
        #endregion
        #region inclusive positive
        private class RangeListInPos<T> : LockedList<T>
        {
            private readonly FieldWrapper<T> _start;
            private readonly T _end;
            private readonly FieldWrapper<T> _step;
            public RangeListInPos(T start, T end, T step)
            {
                _start = start.ToFieldWrapper();
                _end = end;
                _step = step.ToFieldWrapper();
                var gap = end - _start;
                var ratio = (double?)gap / (double?)_step;
                if (ratio == null)
                    throw new Exception("gap is nonlinear!");
                Count = ratio.Value < 0 ? 0 : ratio.Value.Floor()+1;
            }
            public override IEnumerator<T> GetEnumerator()
            {
                var ret = _start;
                while (ret <= _end)
                {
                    yield return ret;
                    ret += _step;
                }
            }
            public override bool Contains(T item)
            {
                return item.iswithin(_start.val, _end) && ((item - _start) % _step).isZero;
            }
            public override int Count { get; }
            public override int IndexOf(T item)
            {
                if (!Contains(item))
                    return -1;
                return (int)((item - _start) / _step);
            }
            public override T this[int index]
            {
                get
                {
                    if (index < 0 || index >= Count)
                        throw new IndexOutOfRangeException();
                    return this._start + this._step * index;
                }
            }
        }
        private class RangeListInPos : LockedList<int>
        {
            private readonly int _start;
            private readonly int _end;
            private readonly int _step;
            public RangeListInPos(int start, int end, int step)
            {
                _start = start;
                _end = end;
                _step = step;
                var gap = end - _start;
                var ratio = (gap / (double)_step);
                Count = ratio < 0 ? 0 : ratio.Floor()+1;
            }
            public override IEnumerator<int> GetEnumerator()
            {
                var ret = _start;
                while (ret <= _end)
                {
                    yield return ret;
                    ret += _step;
                }
            }
            public override bool Contains(int item)
            {
                return item.iswithin(_start, _end) && ((item - _start) % _step) == 0;
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
        #endregion
        #region inclusive negative
        private class RangeListInNeg<T> : LockedList<T>
        {
            private readonly FieldWrapper<T> _start;
            private readonly T _end;
            private readonly FieldWrapper<T> _step;
            public RangeListInNeg(T start, T end, T step)
            {
                _start = start.ToFieldWrapper();
                _end = end;
                _step = step.ToFieldWrapper();
                var gap = end - _start;
                var ratio = (double?)gap / -(double?)_step;
                if (ratio == null)
                    throw new Exception("gap is nonlinear!");
                Count = ratio.Value < 0 ? 0 : ratio.Value.Floor() + 1;
            }
            public override IEnumerator<T> GetEnumerator()
            {
                var ret = _start;
                while (ret >= _end)
                {
                    yield return ret;
                    ret -= _step;
                }
            }
            public override bool Contains(T item)
            {
                return item.iswithin(_start.val, _end) && ((item - _start) % _step).isZero;
            }
            public override int Count { get; }
            public override int IndexOf(T item)
            {
                if (!Contains(item))
                    return -1;
                return (int)((item - _start) / -_step);
            }
            public override T this[int index]
            {
                get
                {
                    if (index < 0 || index >= Count)
                        throw new IndexOutOfRangeException();
                    return this._start - this._step * index;
                }
            }
        }
        private class RangeListInNeg : LockedList<int>
        {
            private readonly int _start;
            private readonly int _end;
            private readonly int _step;
            public RangeListInNeg(int start, int end, int step)
            {
                _start = start;
                _end = end;
                _step = step;
                var gap = end - _start;
                var ratio = (gap / (double)-_step);
                Count = ratio < 0 ? 0 : ratio.Floor() + 1;
            }
            public override IEnumerator<int> GetEnumerator()
            {
                var ret = _start;
                while (ret >= _end)
                {
                    yield return ret;
                    ret -= _step;
                }
            }
            public override bool Contains(int item)
            {
                return item.iswithin(_start, _end) && ((item - _start) % _step) == 0;
            }
            public override int Count { get; }
            public override int IndexOf(int item)
            {
                if (!Contains(item))
                    return -1;
                return (item - _start) / -_step;
            }
            public override int this[int index]
            {
                get
                {
                    if (index < 0 || index >= Count)
                        throw new IndexOutOfRangeException();
                    return this._start - this._step * index;
                }
            }
        }
        #endregion
        #region Range
        /// <summary>
        /// Get an <see cref="IList{T}"/> of an arithmetic series. 
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="start">The first element of the returned value.</param>
        /// <param name="max">The maximum value of the elements. Exclusive.</param>
        /// <param name="step">The difference between consecutive elements.</param>
        /// <returns>A read-only <see cref="IList{T}"/> with elements from <paramref name="start"/> to <paramref name="max"/> in steps of <paramref name="step"/>.</returns>
        /// <remarks>uses fielding.</remarks>
        public static IList<T> Range<T>(T start, T max, T step)
        {
            if (step.ToFieldWrapper().isNegative)
                return RRange(start,max,(-step.ToFieldWrapper()).val);
            return new RangeListExPos<T>(start, max, step);
        }
        /// <summary>
        /// Get an <see cref="IList{T}"/> of an arithmetic series. 
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="start">The first element of the returned value.</param>
        /// <param name="max">The maximum value of the elements. Exclusive.</param>
        /// <returns>A read-only <see cref="IList{T}"/> with elements from <paramref name="start"/> to <paramref name="max"/> in steps of one.</returns>
        /// <remarks>uses fielding.</remarks>
        public static IList<T> Range<T>(T start, T max)
        {
            var f = Fields.getField<T>();
            return Range(start, max, f.one);
        }
        /// <summary>
        /// Get an <see cref="IList{T}"/> of an arithmetic series. 
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="max">The maximum value of the elements. Exclusive.</param>
        /// <returns>A read-only <see cref="IList{T}"/> with elements from zero to <paramref name="max"/> in steps of one.</returns>
        /// <remarks>uses fielding.</remarks>
        public static IList<T> Range<T>(T max)
        {
            var f = Fields.getField<T>();
            return Range(f.zero, max, f.one);
        }
        /// <summary>
        /// Get an <see cref="IList{T}"/> of an arithmetic series of <see cref="int"/>s. 
        /// </summary>
        /// <param name="start">The first element of the returned value.</param>
        /// <param name="max">The maximum value of the elements. Exclusive.</param>
        /// <returns>A read-only <see cref="IList{T}"/> with elements from <paramref name="start"/> to <paramref name="max"/> in steps of 1.</returns>
        public static IList<int> Range(int start, int max)
        {
            return Range(start, max, 1);
        }
        /// <summary>
        /// Get an <see cref="IList{T}"/> of an arithmetic series of <see cref="int"/>s. 
        /// </summary>
        /// <param name="start">The first element of the returned value.</param>
        /// <param name="max">The maximum value of the elements. Exclusive.</param>
        /// <param name="step">The difference between consecutive elements.</param>
        /// <returns>A read-only <see cref="IList{T}"/> with elements from <paramref name="start"/> to <paramref name="max"/> in steps of <paramref name="step"/>.</returns>
        public static IList<int> Range(int start, int max, int step)
        {
            step.ThrowIfAbsurd(nameof(step),false,allowNeg:true);
            if (step < 0)
                return RRange(start, max, -step);
            return new RangeListExPos(start, max, step);
        }
        /// <summary>
        /// Get an <see cref="IList{T}"/> of an arithmetic series of <see cref="int"/>s. 
        /// </summary>
        /// <param name="max">The maximum value of the elements. Exclusive.</param>
        /// <returns>A read-only <see cref="IList{T}"/> with elements from 0 to <paramref name="max"/> in steps of 1.</returns>
        public static IList<int> Range(int max)
        {
            return Range(0, max);
        }
        #endregion
        #region IRange
        /// <summary>
        /// Get an <see cref="IList{T}"/> of an arithmetic series. 
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="start">The first element of the returned value.</param>
        /// <param name="max">The maximum value of the elements. Inclusive.</param>
        /// <param name="step">The difference between consecutive elements.</param>
        /// <returns>A read-only <see cref="IList{T}"/> with elements from <paramref name="start"/> to <paramref name="max"/> in steps of <paramref name="step"/>.</returns>
        /// <remarks>uses fielding.</remarks>
        public static IList<T> IRange<T>(T start, T max, T step)
        {
            if (step.ToFieldWrapper().isNegative)
                return RIRange(start, max, (-step.ToFieldWrapper()).val);
            return new RangeListInPos<T>(start, max, step);
        }
        /// <summary>
        /// Get an <see cref="IList{T}"/> of an arithmetic series. 
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="start">The first element of the returned value.</param>
        /// <param name="max">The maximum value of the elements. Inclusive.</param>
        /// <returns>A read-only <see cref="IList{T}"/> with elements from <paramref name="start"/> to <paramref name="max"/> in steps of one.</returns>
        /// <remarks>uses fielding.</remarks>
        public static IList<T> IRange<T>(T start, T max)
        {
            var f = Fields.getField<T>();
            return IRange(start, max, f.one);
        }
        /// <summary>
        /// Get an <see cref="IList{T}"/> of an arithmetic series. 
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="max">The maximum value of the elements. Inclusive.</param>
        /// <returns>A read-only <see cref="IList{T}"/> with elements from zero to <paramref name="max"/> in steps of one.</returns>
        /// <remarks>uses fielding.</remarks>
        public static IList<T> IRange<T>(T max)
        {
            var f = Fields.getField<T>();
            return IRange(f.zero, max, f.one);
        }
        /// <summary>
        /// Get an <see cref="IList{T}"/> of an arithmetic series of <see cref="int"/>s. 
        /// </summary>
        /// <param name="start">The first element of the returned value.</param>
        /// <param name="max">The maximum value of the elements. Inclusive.</param>
        /// <returns>A read-only <see cref="IList{T}"/> with elements from <paramref name="start"/> to <paramref name="max"/> in steps of 1.</returns>
        public static IList<int> IRange(int start, int max)
        {
            return IRange(start, max, 1);
        }
        /// <summary>
        /// Get an <see cref="IList{T}"/> of an arithmetic series of <see cref="int"/>s. 
        /// </summary>
        /// <param name="start">The first element of the returned value.</param>
        /// <param name="max">The maximum value of the elements. Inclusive.</param>
        /// <param name="step">The difference between consecutive elements.</param>
        /// <returns>A read-only <see cref="IList{T}"/> with elements from <paramref name="start"/> to <paramref name="max"/> in steps of <paramref name="step"/>.</returns>
        public static IList<int> IRange(int start, int max, int step)
        {
            if (step < 0)
                return RIRange(start, max, -step);
            return new RangeListInPos(start, max, step);
        }
        /// <summary>
        /// Get an <see cref="IList{T}"/> of an arithmetic series of <see cref="int"/>s. 
        /// </summary>
        /// <param name="max">The maximum value of the elements. Inclusive.</param>
        /// <returns>A read-only <see cref="IList{T}"/> with elements from 0 to <paramref name="max"/> in steps of 1.</returns>
        public static IList<int> IRange(int max)
        {
            return IRange(0, max);
        }
        #endregion
        #region RRange
        /// <summary>
        /// Get an <see cref="IList{T}"/> of a descending arithmetic series. 
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="start">The first element of the returned value.</param>
        /// <param name="max">The maximum value of the elements. Exclusive.</param>
        /// <param name="step">The difference between consecutive elements.</param>
        /// <returns>A read-only <see cref="IList{T}"/> with elements from <paramref name="start"/> to <paramref name="max"/> in steps of <paramref name="step"/>.</returns>
        /// <remarks>uses fielding.</remarks>
        public static IList<T> RRange<T>(T start, T max, T step)
        {
            if (step.ToFieldWrapper().isNegative)
                return Range(start, max, (-step.ToFieldWrapper()).val);
            return new RangeListExNeg<T>(start, max, step);
        }
        /// <summary>
        /// Get an <see cref="IList{T}"/> of a descending arithmetic series. 
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="start">The first element of the returned value.</param>
        /// <param name="max">The maximum value of the elements. Exclusive.</param>
        /// <returns>A read-only <see cref="IList{T}"/> with elements from <paramref name="start"/> to <paramref name="max"/> in steps of one.</returns>
        /// <remarks>uses fielding.</remarks>
        public static IList<T> RRange<T>(T start, T max)
        {
            var f = Fields.getField<T>();
            return RRange(start, max, f.one);
        }
        /// <summary>
        /// Get an <see cref="IList{T}"/> of a descending arithmetic series of <see cref="int"/>s. 
        /// </summary>
        /// <param name="start">The first element of the returned value.</param>
        /// <param name="max">The maximum value of the elements. Exclusive.</param>
        /// <returns>A read-only <see cref="IList{T}"/> with elements from <paramref name="start"/> to <paramref name="max"/> in steps of 1.</returns>
        public static IList<int> RRange(int start, int max)
        {
            return RRange(start, max, 1);
        }
        /// <summary>
        /// Get an <see cref="IList{T}"/> of a descending arithmetic series of <see cref="int"/>s. 
        /// </summary>
        /// <param name="start">The first element of the returned value.</param>
        /// <param name="max">The maximum value of the elements. Exclusive.</param>
        /// <param name="step">The difference between consecutive elements.</param>
        /// <returns>A read-only <see cref="IList{T}"/> with elements from <paramref name="start"/> to <paramref name="max"/> in steps of <paramref name="step"/>.</returns>
        public static IList<int> RRange(int start, int max, int step)
        {
            step.ThrowIfAbsurd(nameof(step), false, allowNeg: true);
            if (step < 0)
                return Range(start, max, -step);
            return new RangeListExNeg(start, max, step);
        }
        #endregion
        #region RIRange
        /// <summary>
        /// Get an <see cref="IList{T}"/> of a descending arithmetic series. 
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="start">The first element of the returned value.</param>
        /// <param name="max">The maximum value of the elements. Inclusive.</param>
        /// <param name="step">The difference between consecutive elements.</param>
        /// <returns>A read-only <see cref="IList{T}"/> with elements from <paramref name="start"/> to <paramref name="max"/> in steps of <paramref name="step"/>.</returns>
        /// <remarks>uses fielding.</remarks>
        // ReSharper disable InconsistentNaming
        public static IList<T> RIRange<T>(T start, T max, T step)
        {
            if (step.ToFieldWrapper().isNegative)
                return IRange(start, max, (-step.ToFieldWrapper()).val);
            return new RangeListInNeg<T>(start, max, step);
        }
        /// <summary>
        /// Get an <see cref="IList{T}"/> of a descending arithmetic series. 
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="start">The first element of the returned value.</param>
        /// <param name="max">The maximum value of the elements. Inclusive.</param>
        /// <returns>A read-only <see cref="IList{T}"/> with elements from <paramref name="start"/> to <paramref name="max"/> in steps of one.</returns>
        /// <remarks>uses fielding.</remarks>
        public static IList<T> RIRange<T>(T start, T max)
        {
            var f = Fields.getField<T>();
            return RIRange(start, max, f.one);
        }
        /// <summary>
        /// Get an <see cref="IList{T}"/> of a descending arithmetic series of <see cref="int"/>s. 
        /// </summary>
        /// <param name="start">The first element of the returned value.</param>
        /// <param name="max">The maximum value of the elements. Inclusive.</param>
        /// <returns>A read-only <see cref="IList{T}"/> with elements from <paramref name="start"/> to <paramref name="max"/> in steps of 1.</returns>
        public static IList<int> RIRange(int start, int max)
        {
            return RIRange(start, max, 1);
        }
        /// <summary>
        /// Get an <see cref="IList{T}"/> of a descending arithmetic series of <see cref="int"/>s. 
        /// </summary>
        /// <param name="start">The first element of the returned value.</param>
        /// <param name="max">The maximum value of the elements. Inclusive.</param>
        /// <param name="step">The difference between consecutive elements.</param>
        /// <returns>A read-only <see cref="IList{T}"/> with elements from <paramref name="start"/> to <paramref name="max"/> in steps of <paramref name="step"/>.</returns>
        public static IList<int> RIRange(int start, int max, int step)
        {
            step.ThrowIfAbsurd(nameof(step), false, allowNeg: true);
            if (step < 0)
                return IRange(start, max, -step);
            return new RangeListInNeg(start, max, step);
        }
        // ReSharper restore InconsistentNaming
        #endregion
    }
}
