using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;
using WhetStone.SystemExtensions;

namespace WhetStone.Looping
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
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
            public override int Count => (_source.Count/(double)_chunksize).ceil();
            public override IList<T> this[int index]
            {
                get
                {
                    return _source.Slice(index*_chunksize, length: _chunksize);
                }
            }
        }
        /// <overloads>Groups adjacent elements together in a structure.</overloads>
        /// <summary>
        /// Transforms the list by grouping adjacent elements together into a single member.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/></typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> to be chunked.</param>
        /// <param name="chunkSize">The size of every chunk.</param>
        /// <returns>A new <see cref="IEnumerable{T}"/> of <see cref="IList{T}"/>s, each of which contains <paramref name="chunkSize"/> elements.</returns>
        /// <remarks>If the elements don't evenly divide to <paramref name="chunkSize"/>, the last element of the return value will be shorter.</remarks>
        public static IEnumerable<IList<T>> Chunk<T>(this IEnumerable<T> @this, int chunkSize)
        {
            @this.ThrowIfNull(nameof(@this));
            chunkSize.ThrowIfAbsurd(nameof(chunkSize),false);
            using (var en = @this.GetEnumerator())
            {
                var ret = new ResizingArray<T>(chunkSize);
                while (true)
                {
                    if (ret.Count >= chunkSize)
                    {
                        yield return ret.ToArray();
                        ret.Clear();
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
        }
        /// <summary>
        /// Transforms the list by grouping adjacent elements together into a single member.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IList{T}"/></typeparam>
        /// <param name="this">The <see cref="IList{T}"/> to be chunked.</param>
        /// <param name="chunkSize">The size of every chunk.</param>
        /// <returns>A new <see cref="IList{T}"/> of <see cref="IList{T}"/>s, each of which contains <paramref name="chunkSize"/> elements.</returns>
        /// <remarks>If the elements don't evenly divide to <paramref name="chunkSize"/>, the last element of the return value will be shorter.</remarks>
        public static IList<IList<T>> Chunk<T>(this IList<T> @this, int chunkSize)
        {
            @this.ThrowIfNull(nameof(@this));
            chunkSize.ThrowIfAbsurd(nameof(chunkSize), false);
            return new ChunkList<T>(@this, chunkSize);
        }
    }
}
