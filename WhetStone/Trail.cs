using System.Collections.Generic;
using System.Linq;
using WhetStone.Looping;
using WhetStone.LockedStructures;

namespace WhetStone
{
    public static class trail
    {
        private class TrailList<T> : LockedList<IList<T>>
        {
            private readonly IList<T> _source;
            private readonly int _trailLength;
            public TrailList(IList<T> source, int trailLength)
            {
                _source = source;
                this._trailLength = trailLength;
            }
            public override IEnumerator<IList<T>> GetEnumerator()
            {
                return Trail((IEnumerable<T>)_source, _trailLength).GetEnumerator();
            }
            public override int Count => _source.Count - _trailLength + 1;
            public override IList<T> this[int index]
            {
                get
                {
                    return _source.SubEnumerable(index, _trailLength).ToArray();
                }
            }
        }
        public static IEnumerable<IList<T>> Trail<T>(this IList<T> @this, int trailLength, bool wrap = false)
        {
            if (wrap)
            {
                @this = @this.Concat(@this.Slice(0,trailLength - 1));
            }
            return new TrailList<T>(@this,trailLength);
        }
        public static IEnumerable<T[]> Trail<T>(this IEnumerable<T> @this, int trailLength, bool wrap = false)
        {
            var buffer = new LinkedList<T>();
            if (wrap)
            {
                @this = @this.Concat(@this.Take(trailLength - 1));
            }
            foreach (T t in @this)
            {
                buffer.AddLast(t);
                while (buffer.Count > trailLength)
                {
                    buffer.RemoveFirst();
                }
                if (buffer.Count == trailLength)
                {
                    yield return buffer.ToArray();
                }
            }
        }
    }
}
