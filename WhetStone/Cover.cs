using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public static class cover
    {
        public static IEnumerable<T> Cover<T>(this IEnumerable<T> @this, IEnumerable<T> cover, int start = 0, int covergap = 0)
        {
            using (var tor = @this.GetEnumerator())
            {
                foreach (var _ in range.Range(start))
                {
                    if (!tor.MoveNext())
                        yield break;
                    yield return tor.Current;
                }
                foreach (var c in cover)
                {
                    if (!tor.MoveNext())
                        yield break;
                    yield return c;
                    foreach (var _ in range.Range(covergap))
                    {
                        if (!tor.MoveNext())
                            yield break;
                        yield return tor.Current;
                    }
                }
                while (tor.MoveNext())
                {
                    yield return tor.Current;
                }
            }
        }
        public static IEnumerable<T> Cover<T>(this IEnumerable<T> @this, params T[] cover)
        {
            return @this.Cover(cover.AsEnumerable());
        }
        public static IEnumerable<T> Cover<T>(this IEnumerable<T> @this, T cover, int start = 0)
        {
            return @this.Cover(cover.Enumerate(),start);
        }

        private class CoverList<T> : LockedList<T>
        {
            private readonly IList<T> _source;
            private readonly IList<T> _cover;
            private readonly int _start;
            private readonly int _gap;
            public CoverList(IList<T> source, IList<T> cover, int start, int gap)
            {
                _source = source;
                _cover = cover;
                _start = start;
                _gap = gap;
            }
            public override IEnumerator<T> GetEnumerator()
            {
                return _source.AsEnumerable().Cover(_cover,_start,_gap).GetEnumerator();
            }
            public override int Count => _source.Count;
            public override T this[int index]
            {
                get
                {
                    if (index >= _start && (index - _start)%(_gap+1) == 0)
                    {
                        var i = (index - _start)/(_gap+1);
                        if (i < _cover.Count)
                            return _cover[i];
                    }
                    return _source[index];
                }
            }
        }
        public static IList<T> Cover<T>(this IList<T> @this, IList<T> cover, int start = 0, int covergap = 0)
        {
            return new CoverList<T>(@this,cover,start,covergap);
        }
        public static IList<T> Cover<T>(this IList<T> @this, params T[] cover)
        {
            return @this.Cover(cover.AsList());
        }
        public static IList<T> Cover<T>(this IList<T> @this, T cover, int start)
        {
            return @this.Cover(cover.Enumerate(),start);
        }
    }
}
