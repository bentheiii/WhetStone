using System;
using System.Collections.Generic;
using WhetStone.Fielding;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class countUp
    {
        private class CountList<T> : LockedList<T>
        {
            private readonly T _start;
            private readonly FieldWrapper<T> _step;
            public CountList(T start, T step)
            {
                _start = start;
                _step = step.ToFieldWrapper();
                if (_step.isZero)
                    throw new ArgumentException(nameof(step)+" is zero");
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
                return (_step.Field.subtract(item,_start) % _step).isZero;
            }
            public override int Count { get; } = int.MaxValue;
            public override int IndexOf(T item)
            {
                if (!Contains(item))
                    return -1;
                return (int)(_step.Field.subtract(item, _start) / _step);
            }
            public override T this[int index]
            {
                get
                {
                    return this._start + this._step * index;
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
                return (item - _start)%_step == 0;
            }
            public override int Count { get; } = int.MaxValue;
            public override int IndexOf(int item)
            {
                if (!Contains(item))
                    return -1;
                return (item - _start)/_step;
            }
            public override int this[int index]
            {
                get
                {
                    return this._start + this._step*index;
                }
            }
        }
        /// <overloads>Get an infinite enumerable counting up from a value</overloads>
        /// <summary>
        /// Get an infinite <see cref="IList{T}"/> of <see cref="int"/>s counting up from 0 in steps of 1. 
        /// </summary>
        /// <returns>A read-only, infinite <see cref="IList{T}"/> of <see cref="int"/>s.</returns>
        public static IList<int> CountUp()
        {
            return CountUp(0);
        }
        /// <overloads>Get an infinite enumerable counting up from a value</overloads>
        /// <summary>
        /// Get an infinite <see cref="IList{T}"/> of <see cref="int"/>s counting up from <paramref name="start"/> in steps of 1. 
        /// </summary>
        /// <param name="start">The first element of the <see cref="IList{T}"/></param>
        /// <returns>A read-only, infinite <see cref="IList{T}"/> of <see cref="int"/>s.</returns>
        public static IList<int> CountUp(int start)
        {
            return CountUp(start, 1);
        }
        /// <overloads>Get an infinite enumerable counting up from a value</overloads>
        /// <summary>
        /// Get an infinite <see cref="IList{T}"/> of <see cref="int"/>s counting up from <paramref name="start"/> in steps of <paramref name="step"/>. 
        /// </summary>
        /// <param name="start">The first element of the <see cref="IList{T}"/></param>
        /// <param name="step">The difference between any two consecutive elements.</param>
        /// <returns>A read-only, infinite <see cref="IList{T}"/> of <see cref="int"/>s.</returns>
        public static IList<int> CountUp(int start, int step)
        {
            if (step == 0)
                throw new ArgumentOutOfRangeException(nameof(step));
            return new CountList(start, step);
        }
        /// <summary>
        /// Get an infinite <see cref="IList{T}"/> of generic type. counting up from <paramref name="start"/> in steps of <paramref name="step"/>. 
        /// </summary>
        /// <param name="start">The first element of the <see cref="IList{T}"/></param>
        /// <param name="step">The difference between any two consecutive elements.</param>
        /// <returns>A read-only, infinite <see cref="IList{T}"/>.</returns>
        /// <remarks>This function uses fielding to generate the addition function.</remarks>
        public static IList<T> CountUp<T>(T start, T step)
        {
            return new CountList<T>(start, step);
        }
        /// <summary>
        /// Get an infinite <see cref="IList{T}"/> of generic type. counting up from <paramref name="start"/> in steps of 1 (uses fielding to get generic 1). 
        /// </summary>
        /// <param name="start">The first element of the <see cref="IList{T}"/></param>
        /// <returns>A read-only, infinite <see cref="IList{T}"/>.</returns>
        /// <remarks>This function uses fielding to generate the addition function and the step member.</remarks>
        public static IList<T> CountUp<T>(T start)
        {
            return CountUp(start, Fields.getField<T>().one);
        }
        /// <summary>
        /// Get an infinite <see cref="IList{T}"/> of generic type. counting up from 0 in steps of 1 (uses fielding to get generic 0 and 1). 
        /// </summary>
        /// <returns>A read-only, infinite <see cref="IList{T}"/>.</returns>
        /// <remarks>This function uses fielding to generate the addition function, the step member, and initial member.</remarks>
        public static IList<T> CountUp<T>()
        {
            return CountUp(Fields.getField<T>().zero);
        }
    }
}
