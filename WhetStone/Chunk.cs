using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Arrays;
using WhetStone.LockedStructures;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    public static class chunk
    {
        private class ChunkList<T> : LockedList<IList<T>>
        {
            private readonly IList<T> _source;
            private readonly int _chunksize;
            public ChunkList(IList<T> source, int chunksize)
            {
                _source = source;
                this._chunksize = chunksize;
            }
            public override IEnumerator<IList<T>> GetEnumerator()
            {
                return Chunk((IEnumerable<T>)_source, _chunksize).GetEnumerator();
            }
            public override int Count => _source.Count/_chunksize + (_source.Count%_chunksize == 0 ? 0 : 1);
            public override IList<T> this[int index]
            {
                get
                {
                    return _source.SubEnumerable(index*_chunksize, _chunksize).ToArray();
                }
            }
        }
        public static IEnumerable<IList<T>> Chunk<T>(this IEnumerable<T> @this)
        {
            return Chunk(@this, Math.Sqrt(@this.Count()).floor());
        }
        public static IEnumerable<IList<T>> Chunk<T>(this IEnumerable<T> @this, int chunkSize)
        {
            var en = @this.GetEnumerator();
            var ret = new ResizingArray<T>(chunkSize);
            while (true)
            {
                if (ret.Count >= chunkSize)
                {
                    yield return ret.arr;
                    ret = new ResizingArray<T>(chunkSize);
                }
                if (!en.MoveNext())
                {
                    if (ret.Count > 0)
                        yield return ret.arr;
                    yield break;
                }
                ret.Add(en.Current);
            }
        }
        public static LockedList<IList<T>> Chunk<T>(this IList<T> @this)
        {
            return Chunk(@this, Math.Sqrt(@this.Count).floor());
        }
        public static LockedList<IList<T>> Chunk<T>(this IList<T> @this, int chunkSize)
        {
            return new ChunkList<T>(@this, chunkSize);
        }
    }
}
