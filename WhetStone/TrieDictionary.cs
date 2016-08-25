using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using WhetStone.Looping;
using WhetStone.Arrays;
using WhetStone.Comparison;

namespace WhetStone.Tries
{
    internal interface ITrieNode<T, V> : IEnumerable<KeyValuePair<IEnumerable<T>, V>> { }
    
    public class Trie<T, V> : ITrieNode<T, V>, IDictionary<IEnumerable<T>, V>
    {
        public class TrieElement : ITrieNode<T, V>
        {
            private readonly IEqualityComparer<IEnumerable<T>> _comp;
            public TrieElement(IEnumerable<T> key, V value, IEqualityComparer<IEnumerable<T>> comp)
            {
                this._comp = comp;
                this.key = key;
                this.value = value;
            }
            public IEnumerable<T> key { get; }
            public V value { get; }
            public bool ContainsKey(IEnumerable<T> querykey)
            {
                return _comp.Equals(querykey, this.key);
            }
            public IEnumerator<KeyValuePair<IEnumerable<T>, V>> GetEnumerator()
            {
                yield return new KeyValuePair<IEnumerable<T>, V>(key, value);
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        private readonly IDictionary<IList<T>, ITrieNode<T, V>> _children;
        private readonly IEqualityComparer<IEnumerable<T>> _comp;
        private readonly IEqualityComparer<T> _tokencomp;
        public Trie(IEqualityComparer<IEnumerable<T>> comp = null, IEqualityComparer<T> tokencomp = null)
        {
            _tokencomp = tokencomp ?? EqualityComparer<T>.Default;
            _comp = comp ?? EqualityComparer<IEnumerable<T>>.Default;
            _comp = _comp ?? new EnumerableEqualityCompararer<T>();
            _children = new Dictionary<IList<T>, ITrieNode<T, V>>(_comp);
        }
        public void Add(IEnumerable<T> key, V value)
        {
            var k = key.AsList();
            Add(k, value, k);
        }
        public bool Remove(IEnumerable<T> key)
        {
            throw new NotImplementedException();
        }
        public bool TryGetValue(IEnumerable<T> key, out V value)
        {
            bool included;
            KeyValuePair<IList<T>, ITrieNode<T, V>> match;
            if (!key.Any())
                match = _children.FirstOrDefault(a => a.Key.Count == 0, out included);
            else
                match = _children.FirstOrDefault(a => key.StartsWith(a.Key, _tokencomp) && a.Key.Count > 0, out included);
            if (!included)
            {
                value = default(V);
                return false;
            }
            var vs = match.Value as Trie<T, V>;
            if (vs != null)
                return vs.TryGetValue(key.Skip(match.Key.Count), out value);
            if (key.CompareCount(match.Key) != 0)
            {
                value = default(V);
                return false;
            }
            value = (match.Value as TrieElement).value;
            return true;
        }
        public V this[IEnumerable<T> key]
        {
            get
            {
                V val;
                if (TryGetValue(key, out val))
                    return val;
                throw new KeyNotFoundException();
            }
            set
            {
                Add(key, value);
            }
        }
        public ICollection<IEnumerable<T>> Keys
        {
            get
            {
                return new ReadOnlyCollectionBuilder<IEnumerable<T>>(this.Select(a => a.Key));
            }
        }
        public ICollection<V> Values
        {
            get
            {
                return new ReadOnlyCollectionBuilder<V>(this.Select(a => a.Value));
            }
        }
        private void Add(IList<T> key, V value, IEnumerable<T> fullkey)
        {
            if (!key.Any())
            {
                _children[new T[0]] = new TrieElement(fullkey, value, _comp);
                return;
            }
            bool included;
            var match = _children.FirstOrDefault(a => a.Key.StartsWith(key, _tokencomp), out included);
            if (included)
            {
                if (match.Key.CompareCount(key) != 0)
                {
                    //we need to split an existing key
                    _children.Remove(match.Key);
                    var newchild = new Trie<T, V>(_comp, _tokencomp) { { new T[0], value, fullkey } };
                    newchild._children[match.Key.Slice(key.Count)] = match.Value;
                    _children[key] = newchild;
                    return;
                }
                // we add epsilon value
                var trie = match.Value as Trie<T, V>;
                trie?.Add(new T[0], value, fullkey);
                return;
            }
            match = _children.FirstOrDefault(a => a.Key.Any() && key.StartsWith(a.Key, _tokencomp), out included);
            if (included)
            {
                //we need to split this key
                var trie = match.Value as Trie<T, V>;
                if (trie == null)
                {
                    _children.Remove(match);
                    trie = new Trie<T, V>(_comp, _tokencomp) { { new T[0], match.Value.First().Value, match.Value.First().Key } };
                    _children[match.Key] = trie;
                }
                trie.Add(key.Slice(match.Key.Count), value, fullkey);
                return;
            }
            var submatch = _children.Attach(a => a.Key.LongestCommonPrefix(key, _tokencomp)).FirstOrDefault(a => a.Item2.Any(), out included);
            if (included)
            {
                //we need to split both
                match = submatch.Item1;
                var prefix = submatch.Item2.Select(a => a.Item1).ToArray();
                _children.Remove(match.Key);
                var newchild = new Trie<T, V>(_comp, _tokencomp) { { key.Slice(prefix.Length), value, fullkey } };
                newchild._children[match.Key.Slice(prefix.Length)] = match.Value;
                _children[prefix] = newchild;
                return;
            }
            //we have no definitions for anything like this key
            this._children[key] = new TrieElement(fullkey, value, _comp);
        }
        public bool ContainsKey(IEnumerable<T> key)
        {
            V ret;
            return TryGetValue(key, out ret);
        }
        public virtual IEnumerator<KeyValuePair<IEnumerable<T>, V>> GetEnumerator()
        {
            return _children.Values.Concat().GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void Add(KeyValuePair<IEnumerable<T>, V> item)
        {
            Add(item.Key, item.Value);
        }
        public void Clear()
        {
            _children.Clear();
        }
        public bool Contains(KeyValuePair<IEnumerable<T>, V> item)
        {
            V v;
            return TryGetValue(item.Key, out v) && v.Equals(item.Value);
        }
        public void CopyTo(KeyValuePair<IEnumerable<T>, V>[] array, int arrayIndex)
        {
            foreach (var t in this.CountBind(arrayIndex))
            {
                array[t.Item2] = t.Item1;
            }
        }
        public bool Remove(KeyValuePair<IEnumerable<T>, V> item)
        {
            if (this.ContainsKey(item.Key) && this[item.Key].Equals(item.Value))
                return Remove(item.Key);
            return false;
        }
        public int Count
        {
            get
            {
                return _children.Values.Select(a =>
                {
                    var tri = a as Trie<T, V>;
                    return tri?.Count ?? 1;
                }).Sum();
            }
        }
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
        public IEnumerable<KeyValuePair<IEnumerable<T>, V>> CompletionsQuery(IEnumerable<T> prefix)
        {
            if (!prefix.Any())
                return this;
            bool any;
            var match = this._children.FirstOrDefault(a => a.Key.LongestCommonPrefix(prefix, _tokencomp).Any(), out any);
            if (!any)
                return new KeyValuePair<IEnumerable<T>, V>[0];
            var vs = match.Value as Trie<T, V>;
            return vs != null ? vs.CompletionsQuery(prefix.Skip(match.Key.Count).ToArray()) : match.Value;
        }
        public IEnumerable<KeyValuePair<IEnumerable<T>, V>> PrefixesQuery(IEnumerable<T> complete)
        {
            return this._children.Where(a => complete.StartsWith(a.Key, _tokencomp)).Select(a =>
            {
                var vs = a.Value as Trie<T, V>;
                return vs != null ? vs.PrefixesQuery(complete.Skip(a.Key.Count)) : a.Value;
            }).Concat();
        }
    }
}
