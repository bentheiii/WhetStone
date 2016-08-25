using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Tries;

namespace WhetStone.Funnels
{
    public class PrefixFunnel<T, RT> : IFunnel<IEnumerable<T>, RT>
    {
        public PrefixFunnel(bool removePrefix = true)
        {
            RemovePrefix = removePrefix;
        }
        private readonly Trie<T, Tuple<int, Proccesor<IEnumerable<T>, RT>>> _processors = new Trie<T, Tuple<int, Proccesor<IEnumerable<T>, RT>>>();
        private int _proccount = 0;
        public bool RemovePrefix { get; }
        public RT Process(IEnumerable<T> val)
        {
            var validproc = _processors.PrefixesQuery(val).OrderBy(a=>a.Value.Item1).Select(a=>a.Value.Item2);
            foreach (var p in validproc)
            {
                RT ret;
                if (p(val, out ret))
                    return ret;
            }
            throw new NoValidProcessorException("no usable processor found");
        }
        public IEnumerator<Proccesor<IEnumerable<T>, RT>> GetEnumerator()
        {
            return _processors.Select(a => a.Value.Item2).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void Add(Proccesor<IEnumerable<T>, RT> p)
        {
            Add(new T[0], p);
        }
        public void Add(IEnumerable<T> prefix, Proccesor<IEnumerable<T>, RT> p)
        {
            // ReSharper disable once ImplicitlyCapturedClosure
            Proccesor<IEnumerable<T>, RT> proc =
                (IEnumerable<T> processed, out RT returnval) => p(RemovePrefix ? processed.Skip(prefix.Count()) : processed, out returnval);
            _processors.Add(prefix, Tuple.Create(_proccount++,proc));
        }
        public void Add(Func<IEnumerable<T>, RT> p)
        {
            Add(new T[0], p);
        }
        public void Add(IEnumerable<T> prefix, Func<IEnumerable<T>, RT> p)
        {
            // ReSharper disable once ImplicitlyCapturedClosure
            Proccesor<IEnumerable<T>, RT> proc =
                (IEnumerable<T> processed, out RT returnval) =>
                {
                    returnval = p(RemovePrefix ? processed.Skip(prefix.Count()) : processed);
                    return true;
                };
            _processors.Add(prefix, Tuple.Create(_proccount++, proc));
        }
    }
    public class PrefixFunnel<T> : IFunnel<IEnumerable<T>>
    {
        public PrefixFunnel(bool removePrefix = true)
        {
            RemovePrefix = removePrefix;
        }
        private readonly Trie<T, Tuple<int, Proccesor<IEnumerable<T>>>> _processors = new Trie<T, Tuple<int, Proccesor<IEnumerable<T>>>>();
        private int _proccount = 0;
        public bool RemovePrefix { get; }
        public void Process(IEnumerable<T> val)
        {
            var validproc = _processors.PrefixesQuery(val).OrderBy(a => a.Value.Item1).Select(a => a.Value.Item2);
            foreach (var p in validproc)
            {
                if (p(val))
                    return;
            }
            throw new NoValidProcessorException("no usable processor found");
        }
        public IEnumerator<Proccesor<IEnumerable<T>>> GetEnumerator()
        {
            return _processors.Select(a => a.Value.Item2).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void Add(Proccesor<IEnumerable<T>> p)
        {
            Add(new T[0], p);
        }
        public void Add(IEnumerable<T> prefix, Proccesor<IEnumerable<T>> p)
        {
            // ReSharper disable once ImplicitlyCapturedClosure
            Proccesor<IEnumerable<T>> proc =
                processed => p(RemovePrefix ? processed.Skip(prefix.Count()) : processed);
            _processors.Add(prefix, Tuple.Create(_proccount++, proc));
        }
        public void Add(Action<IEnumerable<T>> p)
        {
            Add(new T[0], p);
        }
        public void Add(IEnumerable<T> prefix, Action<IEnumerable<T>> p)
        {
            // ReSharper disable once ImplicitlyCapturedClosure
            Proccesor<IEnumerable<T>> proc =
                processed =>
                {
                    p(RemovePrefix ? processed.Skip(prefix.Count()) : processed);
                    return true;
                };
            _processors.Add(prefix, Tuple.Create(_proccount++, proc));
        }
    }
}