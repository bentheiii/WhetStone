using System;
using System.Collections.Generic;
using WhetStone.Looping;

namespace WhetStone.LockedStructures
{
    public abstract class LockedDictionary<T, G> : LockedCollection<KeyValuePair<T, G>>, IDictionary<T, G>, IReadOnlyDictionary<T,G>
    {
        public virtual bool ContainsKey(T key)
        {
            G val;
            return this.TryGetValue(key, out val);
        }
        public void Add(T key, G value)
        {
            throw new NotSupportedException();
        }
        public bool Remove(T key)
        {
            throw new NotSupportedException();
        }
        public abstract bool TryGetValue(T key, out G value);
        public G this[T key]
        {
            get
            {
                G ret;
                if (!TryGetValue(key, out ret))
                    throw new KeyNotFoundException();
                return ret;
            }
            set
            {
                throw new NotSupportedException();
            }
        }
        IEnumerable<T> IReadOnlyDictionary<T, G>.Keys
        {
            get
            {
                return Keys;
            }
        }
        IEnumerable<G> IReadOnlyDictionary<T, G>.Values
        {
            get
            {
                return Values;
            }
        }
        public virtual ICollection<T> Keys
        {
            get
            {
                return this.Select(a=>a.Key).ToLockedCollection();
            }
        }
        public virtual ICollection<G> Values
        {
            get
            {
                return this.Select(a => a.Value).ToLockedCollection();
            }
        }
    }
}