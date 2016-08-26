using System.Collections.Generic;
using System.Linq;
using WhetStone.LockedStructures;

namespace WhetStone.Looping
{
    public static class @switch
    {
        public class SwitchList<T> : LockedList<T>
        {
            public readonly IEnumerable<IList<T>> _sources;
            public SwitchList(IEnumerable<IList<T>> sources)
            {
                _sources = sources;
            }
            public override IEnumerator<T> GetEnumerator()
            {
                IEnumerable<IEnumerator<T>> numerators = _sources.Select(a => a.GetEnumerator()).ToArray();
                bool returned = true;
                while (returned)
                {
                    returned = false;
                    foreach (IEnumerator<T> e in numerators)
                    {
                        if (!e.MoveNext())
                            continue;
                        returned = true;
                        yield return e.Current;
                    }
                }
            }
            public override int Count => _sources.Min(a => a.Count)*_sources.Count();
            public override T this[int index]
            {
                get
                {
                    int sourceind = index%_sources.Count();
                    return _sources.ElementAt(index/_sources.Count())[sourceind];
                }
            }
        }
        public static IEnumerable<T> Switch<T>(this IEnumerable<IEnumerable<T>> @this)
        {
            IEnumerable<IEnumerator<T>> numerators = @this.Select(a => a.GetEnumerator()).ToArray();
            bool returned = true;
            while (returned)
            {
                returned = false;
                foreach (IEnumerator<T> e in numerators)
                {
                    if (!e.MoveNext())
                        continue;
                    returned = true;
                    yield return e.Current;
                }
            }
        }
        public static LockedList<T> Switch<T>(this IEnumerable<IList<T>> @this)
        {
            return new SwitchList<T>(@this);
        }
    }
}
