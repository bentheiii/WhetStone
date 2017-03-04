using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class positionBind
    {
        private class PositionBoundList<T> : LockedList<Tuple<T,Position>>
        {
            private readonly IList<T> _source;
            public PositionBoundList(IList<T> source)
            {
                _source = source;
            }
            public override IEnumerator<Tuple<T, Position>> GetEnumerator()
            {
                return _source.AsEnumerable().PositionBind().GetEnumerator();
            }
            public override int Count => _source.Count;
            public override Tuple<T, Position> this[int index]
            {
                get
                {
                    Position ret = Position.None;
                    if (index == 0)
                        ret |= Position.First;
                    if (index == Count-1)
                        ret |= Position.Last;
                    return Tuple.Create(_source[index], ret);
                }
            }
        }
        /// <summary>
        /// A position in an <see cref="IEnumerable{T}"/>.
        /// </summary>
        [Flags] public enum Position
        {
            /// <summary>
            /// All positions have this flag
            /// </summary>
            None = 0,
            /// <summary>
            /// The first position.
            /// </summary>
            First = 1,
            /// <summary>
            /// The first position.
            /// </summary>
            Last = 2,
            /// <summary>
            /// Position in an <see cref="IEnumerable{T}"/> of size 1.
            /// </summary>
            Only = First | Last
        }
        /// <overloads>Attaches positions to elements of enumerables.</overloads>
        /// <summary>
        /// Attaches positions to elements of an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to attach to.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Tuple{T1,T2}"/>, the second element of which is the positions.</returns>
        public static IEnumerable<Tuple<T, Position>> PositionBind<T>(this IEnumerable<T> @this)
        {
            bool first = true;
            using (var num = @this.GetEnumerator())
            {
                bool last = !num.MoveNext();
                while (!last)
                {
                    var v = num.Current;
                    last = !num.MoveNext();
                    Position ret = Position.None;
                    if (first)
                    {
                        ret |= Position.First;
                        first = false;
                    }
                    if (last)
                    {
                        ret |= Position.Last;
                    }
                    yield return Tuple.Create(v, ret);
                }
            }
        }
        /// <overloads>Attaches positions to elements of enumerables.</overloads>
        /// <summary>
        /// Attaches positions to elements of an <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/></typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to attach to.</param>
        /// <returns>An <see cref="IList{T}"/> of <see cref="Tuple{T1,T2}"/>, the second element of which is the positions.</returns>
        public static IList<Tuple<T, Position>> PositionBind<T>(this IList<T> @this)
        {
            return new PositionBoundList<T>(@this);
        }
    }
}
