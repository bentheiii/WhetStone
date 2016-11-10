using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public static class switchUnbound
    {
        private class SwitchUnboundList<T> : LockedList<T>
        {
            private readonly IList<IList<T>> _sources;
            private readonly T _def;
            public SwitchUnboundList(IList<IList<T>> sources, T def)
            {
                _sources = sources;
                _def = def;
            }
            public override IEnumerator<T> GetEnumerator()
            {
                return _sources.AsEnumerable().SwitchUnbound(_def).GetEnumerator();
            }
            public override int Count => _sources.Select(a => a.Count).Max() * _sources.Count;
            public override T this[int index]
            {
                get
                {
                    int sourceind = index % _sources.Count;
                    if (_sources[index/_sources.Count].Count > sourceind)
                        return _sources[index / _sources.Count][sourceind];
                    return _def;
                }
            }
        }
        public static IEnumerable<T> SwitchUnbound<T>(this IEnumerable<IEnumerable<T>> @this, T def = default(T))
        {
            var numerators = @this.Select(a => a.GetEnumerator()).ToArray();
            var buffer = new List<T>(numerators.Length);
            while (numerators.Any(a => a != null))
            {
                for (int i = 0; i < numerators.Length; i++)
                {
                    IEnumerator<T> e = numerators[i];
                    if (e != null && e.MoveNext())
                        buffer.Add(e.Current);
                    else
                    {
                        numerators[i] = null;
                        buffer.Add(def);
                    }
                }
                if (numerators.Any(a => a != null))
                {
                    foreach (T t in buffer)
                    {
                        yield return t;
                    }
                    buffer.Clear();
                }
            }
        }
        public static LockedList<T> SwitchUnbound<T>(this IList<IList<T>> @this, T def = default(T))
        {
            return new SwitchUnboundList<T>(@this,def);
        }
    }
}
