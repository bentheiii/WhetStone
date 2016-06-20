using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using WhetStone.Arrays;
using WhetStone.Comparison;
using WhetStone.Guard;
using WhetStone.Looping;

namespace WhetStone.Structures.Tries
{
    internal interface ITrieNode<T, V> : IEnumerable<KeyValuePair<IEnumerable<T>,V>> {}
    internal interface ITrieNode<out T> : IEnumerable<IEnumerable<T>> { }
    public class Trie<T,V> : ITrieNode<T,V>, IDictionary<IEnumerable<T>,V>
    {
        public class TrieElement : ITrieNode<T,V>
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
            public IEnumerator<KeyValuePair<IEnumerable<T>,V>> GetEnumerator()
            {
                yield return new KeyValuePair<IEnumerable<T>, V>(key,value);
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
            var match = _children.First(a => key.StartsWith(a.Key, _tokencomp), out included);
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
                Add(key,value);
            }
        }
        public ICollection<IEnumerable<T>> Keys
        {
            get
            {
                return new ReadOnlyCollectionBuilder<IEnumerable<T>>(this.Select(a=>a.Key));
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
            var match = _children.First(a => a.Key.StartsWith(key,_tokencomp), out included);
            if (included)
            {
                if (match.Key.CompareCount(key) != 0)
                {
                    //we need to split an existing key
                    _children.Remove(match.Key);
                    var newchild = new Trie<T, V>(_comp, _tokencomp) {{new T[0], value, fullkey}};
                    newchild._children[match.Key.Slice(key.Count)] = match.Value;
                    _children[key] = newchild;
                    return;
                }
                // we add epsilon value
                var trie = match.Value as Trie<T,V>;
                trie?.Add(new T[0], value, fullkey);
                return;
            }
            match = _children.First(a => a.Key.Any() && key.StartsWith(a.Key, _tokencomp), out included);
            if (included)
            {
                //we need to split this key
                var trie = match.Value as Trie<T,V>;
                if (trie == null)
                {
                    _children.Remove(match);
                    trie = new Trie<T, V>(_comp, _tokencomp) {{new T[0], match.Value.First().Value, match.Value.First().Key}};
                    _children[match.Key] = trie;
                }
                trie.Add(key.Slice(match.Key.Count), value, fullkey);
                return;
            }
            var submatch = _children.Attach(a => a.Key.SharedPrefix(key, _tokencomp)).First(a => a.Item2.Any(), out included);
            if (included)
            {
                //we need to split both
                match = submatch.Item1;
                var prefix = submatch.Item2.Select(a => a.Item1).ToArray();
                _children.Remove(match.Key);
                var newchild = new Trie<T, V>(_comp, _tokencomp) {{key.Slice(prefix.Length), value, fullkey}};
                newchild._children[match.Key.Slice(prefix.Length)] = match.Value;
                _children[prefix] = newchild;
                return;
            }
            //we have no definitions for anything like this key
            this._children[key] = new TrieElement(fullkey, value, _comp);
        }
        public bool ContainsKey(IEnumerable<T> key)
        {
            bool included;
            var match = _children.First(a => key.StartsWith(a.Key, _tokencomp), out included);
            if (!included)
                return false;
            var vs = match.Value as Trie<T, V>;
            if (vs != null)
                return vs.ContainsKey(key.Skip(match.Key.Count));
            return key.CompareCount(match.Key) == 0;
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
            Add(item.Key,item.Value);
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
            var match = this._children.First(a => a.Key.SharedPrefix(prefix, _tokencomp).Any(), out any);
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
            var match = _children.First(a => key.StartsWith(a.Key, _tokencomp), out included);
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
                _children[new T[0]] = new TrieElement(fullkey,_comp);
                return;
            }
            bool included;
            var match = _children.First(a => a.Key.StartsWith(key, _tokencomp), out included);
            if (included)
            {
                if (match.Key.CompareCount(key) != 0)
                {
                    //we need to split an existing key
                    _children.Remove(match.Key);
                    var newchild = new Trie<T>(_comp, _tokencomp) {{new T[0], fullkey}};
                    newchild._children[match.Key.Slice(key.Count)] = match.Value;
                    _children[key] = newchild;
                    return;
                }
                // we add epsilon value
                var trie = match.Value as Trie<T>;
                trie?.Add(new T[0], fullkey);
                return;
            }
            match = _children.First(a => a.Key.Any() && key.StartsWith(a.Key, _tokencomp), out included);
            if (included)
            {
                //we need to split this key
                var trie = match.Value as Trie<T>;
                if (trie == null)
                {
                    _children.Remove(match);
                    trie = new Trie<T>(_comp, _tokencomp) {{new T[0], match.Value.First()}};
                    _children[match.Key] = trie;
                }
                trie.Add(key.Slice(match.Key.Count), fullkey);
                return;
            }
            var submatch = _children.Attach(a => a.Key.SharedPrefix(key, _tokencomp)).First(a => a.Item2.Any(), out included);
            if (included)
            {
                //we need to split both
                match = submatch.Item1;
                var prefix = submatch.Item2.Select(a => a.Item1).ToArray();
                _children.Remove(match.Key);
                var newchild = new Trie<T>(_comp, _tokencomp) {{key.Slice(prefix.Length), fullkey}};
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
            var match = this._children.First(a => a.Key.SharedPrefix(prefix, _tokencomp).Any(), out any);
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
    public static class TrieExtensions
    {
        public static Trie<int, int> SuffixTrie<T>(this IEnumerable<T> @this, out IDictionary<T,int> queryconverter)
        {
            var ret = new Trie<int,int>(tokencomp: new EqualityFunctionComparer<int>((a,b)=>a != -1 && b != -1 && @this.ElementAt(a).Equals(@this.ElementAt(b)), a=>a.GetHashCode()));
            var count = @this.Count();
            var iter = @this.GetEnumerator();
            queryconverter = new Dictionary<T, int>();
            IGuard<int> ind = new Guard<int>();
            foreach (T f in @this.CountBind().Detach(ind))
            {
                if (!queryconverter.ContainsKey(f))
                    queryconverter[f] = ind.value;
                ret.Add(Loops.Range(ind.value,count,1), ind.value);
            }
            return ret;
        }
        public static IEnumerable<int> ToSuffixTrieQuery<T>(this IEnumerable<T> @this, IDictionary<T, int> queryconverter)
        {
            return @this.Select(a => queryconverter.DefinitionOrDefault(a,-1));
        }
        public static void InsertSuffixes<T>(this IDictionary<IEnumerable<T>, Tuple<IEnumerable<T>,int>> @this, IEnumerable<T> masterkey)
        {
            InsertSuffixes(@this, masterkey, Tuple.Create);
        }
        public static void InsertSuffixes<T, V>(this IDictionary<IEnumerable<T>, V> @this, IEnumerable<T> masterkey, Func<IEnumerable<T>,int,V> value)
        {
            var toadd = masterkey.AsList();
            int i = 0;
            while (toadd.Count > 0)
            {
                @this[toadd] = value(masterkey,i);
                toadd = toadd.Slice(1);
                i++;
            }
        } 
    }
}
namespace WhetStone.Structures.LockedStructures
{
    public abstract class LockedCollection<T> : ICollection<T>, IReadOnlyCollection<T>
    {
        public abstract IEnumerator<T> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void Add(T item)
        {
            throw new NotSupportedException();
        }
        public void Clear()
        {
            throw new NotSupportedException();
        }
        public virtual bool Contains(T item)
        {
            return this.Any(a => a.Equals(item));
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var t in this)
            {
                array[arrayIndex++] = t;
            }
        }
        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }
        public abstract int Count { get; }
        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }
    }
    public abstract class LockedList<T> : LockedCollection<T>, IList<T>, IReadOnlyList<T>
    {
        public override bool Contains(T item)
        {
            return IndexOf(item)!=-1;
        }
        public virtual int IndexOf(T item)
        {
            int ret = 0;
            foreach (var t in this)
            {
                if (t.Equals(item))
                    return ret;
                ret++;
            }
            return -1;
        }
        public void Insert(int index, T item)
        {
            throw new NotSupportedException();
        }
        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }
        public abstract T this[int index] { get; }
        T IList<T>.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                throw new NotSupportedException();
            }
        }
    }
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
    public class LockedListReadOnlyAdaptor<T> : LockedList<T>
    {
        private readonly IReadOnlyList<T> _inner;
        public LockedListReadOnlyAdaptor(IReadOnlyList<T> inner)
        {
            _inner = inner;
        }
        public override IEnumerator<T> GetEnumerator()
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
        public override int IndexOf(T item)
        {
            return _inner.CountBind().FirstOrDefault(a=>a.Item1.Equals(item),Tuple.Create(default(T),-1)).Item2;
        }
        public override T this[int index]
        {
            get
            {
                return _inner[index];
            }
        }
    }
    public class LockedListStringAdaptor : LockedList<char>
    {
        private readonly string _inner;
        public LockedListStringAdaptor(string inner)
        {
            _inner = inner;
        }
        public override IEnumerator<char> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }
        public override int Count
        {
            get
            {
                return _inner.Length;
            }
        }
        public override int IndexOf(char item)
        {
            return _inner.IndexOf(item);
        }
        public override char this[int index]
        {
            get
            {
                return _inner[index];
            }
        }
    }
    public class LockedCollectionReadOnlyAdaptor<T> : LockedCollection<T>
    {
        private readonly IReadOnlyCollection<T> _inner;
        public LockedCollectionReadOnlyAdaptor(IReadOnlyCollection<T> inner)
        {
            _inner = inner;
        }
        public override IEnumerator<T> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }
        public override bool Contains(T item)
        {
            return _inner.Contains(item);
        }
        public override int Count
        {
            get
            {
                return _inner.Count;
            }
        }
    }
    public class LockedDictionaryReadOnlyAdaptor<T,G> : LockedDictionary<T,G>
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
    public class EnumerableCollectionAdaptor<T> : LockedCollection<T>
    {
        private readonly IEnumerable<T> _inner;
        private int? _count;
        public EnumerableCollectionAdaptor(IEnumerable<T> inner)
        {
            _inner = inner;
            var col = inner as ICollection<T>;
            if (col != null)
                _count = col.Count;
            else
            {
                var rol = inner as IReadOnlyCollection<T>;
                if (rol != null)
                    _count = rol.Count;
                else
                {
                    _count = null;
                }
            }
        }
        public override IEnumerator<T> GetEnumerator()
        {
            int c = 0;
            foreach (var t in _inner)
            {
                yield return t;
                c++;
            }
            _count = c;

        }
        public override bool Contains(T item)
        {
            return _inner.Contains(item);
        }
        public override int Count
        {
            get
            {
                if (_count == null)
                    _count = _inner.Count();
                return _count.Value;
            }
        }
    }
    public static class LockedExtensions
    {
        public static LockedList<T> ToLockedList<T>(this IReadOnlyList<T> @this)
        {
             return new LockedListReadOnlyAdaptor<T>(@this);
        }
        public static LockedList<char> ToLockedList(this string @this)
        {
            return new LockedListStringAdaptor(@this);
        }
        public static LockedCollection<T> ToLockedCollection<T>(this IReadOnlyCollection<T> @this)
        {
            return new LockedCollectionReadOnlyAdaptor<T>(@this);
        }
        public static LockedCollection<T> ToLockedCollection<T>(this IEnumerable<T> @this)
        {
            return new EnumerableCollectionAdaptor<T>(@this);
        }
        public static LockedDictionary<T,G> ToLockedDictionary<T,G>(this IReadOnlyDictionary<T,G> @this)
        {
            return new LockedDictionaryReadOnlyAdaptor<T, G>(@this);
        }
    }
}
