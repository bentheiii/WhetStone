using System.Collections.Generic;

namespace WhetStone.LockedStructures
{
    internal class LockedDictionaryReadOnlyAdaptor<T,G> : LockedDictionary<T,G>
    {
        private readonly IReadOnlyDictionary<T,G> _inner;
        public LockedDictionaryReadOnlyAdaptor(IReadOnlyDictionary<T, G> inner)
        {
            _inner = inner;
        }
        public override IEnumerator<KeyValuePair<T, G>> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }
        public override int Count
        {
            get
            {
                return _inner.Count;
            }
        }
        public override bool TryGetValue(T key, out G value)
        {
            return _inner.TryGetValue(key, out value);
        }
    }
}