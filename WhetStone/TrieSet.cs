using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Looping;
using WhetStone.Arrays;
using WhetStone.Comparison;

namespace WhetStone.Tries
{
    internal interface ITrieNode<out T> : IEnumerable<IEnumerable<T>> { }
    public class Trie<T> : ITrieNode<T>, ICollection<IEnumerable<T>>
    {
        public class TrieElement : ITrieNode<T>
        {
            private readonly IEqualityComparer<IEnumerable<T>> _comp;
            public TrieElement(IEnumerable<T> key, IEqualityComparer<IEnumerable<T>> comp)
            {
                this._comp = comp;
                this.key = key;
            }
            public IEnumerable<T> key { get; }
            public bool ContainsKey(IEnumerable<T> querykey)
            {
                return _comp.Equals(querykey, this.key);
            }
            public IEnumerator<IEnumerable<T>> GetEnumerator()
            {
                yield return key;
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        private readonly IDictionary<IList<T>, ITrieNode<T>> _children;
        private readonly IEqualityComparer<IEnumerable<T>> _comp;
        private readonly IEqualityComparer<T> _tokencomp;
        public Trie(IEqualityComparer<IEnumerable<T>> comp = null, IEqualityComparer<T> tokencomp = null)
        {
            _tokencomp = tokencomp ?? EqualityComparer<T>.Default;
            _comp = comp ?? EqualityComparer<IEnumerable<T>>.Default;
            _comp = _comp ?? new EnumerableEqualityCompararer<T>();
            _children = new Dictionary<IList<T>, ITrieNode<T>>(_comp);
        }
        public void Add(IEnumerable<T> key)
        {
            var k = (key as T[]) ?? key.ToArray();
            Add(k, k);
        }
        public bool Contains(IEnumerable<T> key)
        {
            bool included;
            if (!key.Any())
                return _children.Any(a => a.Key.Count == 0);
            var match = _children.FirstOrDefault(a => key.StartsWith(a.Key, _tokencomp) && a.Key.Count > 0, out included);
            if (!included)
            {
                return false;
            }
            var vs = match.Value as Trie<T>;
            if (vs != null)
                return vs.Contains(key.Skip(match.Key.Count));
            return key.CompareCount(match.Key) == 0;
        }
        public void CopyTo(IEnumerable<T>[] array, int arrayIndex)
        {
            foreach (var t in this.CountBind(arrayIndex))
            {
                array[t.Item2] = t.Item1;
            }
        }
        public bool Remove(IEnumerable<T> item)
        {
            throw new NotImplementedException();
        }
        private void Add(IList<T> key, IEnumerable<T> fullkey)
        {
            if (!key.Any())
            {
                _children[new T[0]] = new TrieElement(fullkey, _comp);
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
                    var newchild = new Trie<T>(_comp, _tokencomp) { { new T[0], fullkey } };
                    newchild._children[match.Key.Slice(key.Count)] = match.Value;
                    _children[key] = newchild;
                    return;
                }
                // we add epsilon value
                var trie = match.Value as Trie<T>;
                trie?.Add(new T[0], fullkey);
                return;
            }
            match = _children.FirstOrDefault(a => a.Key.Any() && key.StartsWith(a.Key, _tokencomp), out included);
            if (included)
            {
                //we need to split this key
                var trie = match.Value as Trie<T>;
                if (trie == null)
                {
                    _children.Remove(match);
                    trie = new Trie<T>(_comp, _tokencomp) { { new T[0], match.Value.First() } };
                    _children[match.Key] = trie;
                }
                trie.Add(key.Slice(match.Key.Count), fullkey);
                return;
            }
            var submatch = _children.Attach(a => a.Key.LongestCommonPrefix(key, _tokencomp)).FirstOrDefault(a => a.Item2.Any(), out included);
            if (included)
            {
                //we need to split both
                match = submatch.Item1;
                var prefix = submatch.Item2.Select(a => a.Item1).ToArray();
                _children.Remove(match.Key);
                var newchild = new Trie<T>(_comp, _tokencomp) { { key.Slice(prefix.Length), fullkey } };
                newchild._children[match.Key.Slice(prefix.Length)] = match.Value;
                _children[prefix] = newchild;
                return;
            }
            //we have no definitions for anything like this key
            this._children[key] = new TrieElement(fullkey, _comp);
        }
        public virtual IEnumerator<IEnumerable<T>> GetEnumerator()
        {
            return _children.Values.Concat().GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void Clear()
        {
            _children.Clear();
        }
        public int Count
        {
            get
            {
                return _children.Values.Select(a =>
                {
                    var tri = a as Trie<T>;
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
        public IEnumerable<IEnumerable<T>> CompletionsQuery(IEnumerable<T> prefix)
        {
            if (!prefix.Any())
                return this;
            bool any;
            var match = this._children.FirstOrDefault(a => a.Key.LongestCommonPrefix(prefix, _tokencomp).Any(), out any);
            if (!any)
                return new IEnumerable<T>[0];
            var vs = match.Value as Trie<T>;
            return vs != null ? vs.CompletionsQuery(prefix.Skip(match.Key.Count).ToArray()) : match.Value;
        }
        public IEnumerable<IEnumerable<T>> PrefixesQuery(IEnumerable<T> complete)
        {
            return this._children.Where(a => complete.StartsWith(a.Key, _tokencomp)).Select(a =>
            {
                var vs = a.Value as Trie<T>;
                return vs != null ? vs.PrefixesQuery(complete.Skip(a.Key.Count)) : a.Value;
            }).Concat();
        }
    }
}
