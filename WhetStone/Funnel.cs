using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Structures.Tries;

// ReSharper disable UnusedMemberInSuper.Global

namespace WhetStone.Funnels
{
    [Serializable]
    public class NoValidProcessorException : Exception
    {
        public NoValidProcessorException(string message) : base(message) {}
    }
    public delegate bool Proccesor<in PT,RT>(PT processed, out RT returnval);
	public delegate bool Proccesor<in PT>(PT processed);
	public delegate bool Proccesor();
	public interface IProccesor<in PT, RT>
    {
        Proccesor<PT, RT> toProcessor();
    }
	public interface IProccesor<in PT>
	{
		Proccesor<PT> toProcessor();
	}
	public interface IProccesor
	{
		Proccesor toProcessor();
	}
    public interface IFunnel<in PT, RT> : IEnumerable<Proccesor<PT, RT>>
    {
        RT Process(PT val);
    }
    public interface IFunnel<in PT> : IEnumerable<Proccesor<PT>>
    {
        void Process(PT val);
    }
    public interface IFunnel : IEnumerable<Proccesor>
    {
        void Process();
    }
    public class Funnel<PT,RT> : IFunnel<PT,RT>
    {
        private IEnumerable<Proccesor<PT,RT>> _procs;
        public RT Process(PT val)
        {
            foreach (Proccesor<PT, RT> p in _procs)
            {
                RT ret;
                if (p(val, out ret))
                    return ret;
            }
            throw new NoValidProcessorException("no usable processor found");
        }
        private Funnel(IEnumerable<Proccesor<PT, RT>> p)
        {
            _procs = p;
        }
        public Funnel()
        {
            _procs = new List<Proccesor<PT,RT>>();
        }
        public Funnel(params Proccesor<PT, RT>[] p)
        {
            _procs = p;
        }
        public Funnel(params IProccesor<PT, RT>[] p)
            : this(p.Select(i => i.toProcessor()))
        {}
        public IEnumerator<Proccesor<PT, RT>> GetEnumerator()
        {
            return this._procs.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
		public void Add(Proccesor<PT, RT> p)
		{
			while (true)
			{
				List<Proccesor<PT, RT>> l = this._procs as List<Proccesor<PT, RT>>;
				if (l == null)
				{
					_procs = new List<Proccesor<PT, RT>>(_procs);
					continue;
				}
				l.Add(p);
				break;
			}
		}
		public void Add(IProccesor<PT, RT> p)
        {
            this.Add(p.toProcessor());
        }
    }
	public class Funnel<PT> : IFunnel<PT>
	{
		private IEnumerable<Proccesor<PT>> _procs;
		public void Process(PT val)
		{
			foreach (Proccesor<PT> p in _procs)
			{
				if (p(val))
					return;
			}
			throw new Exception("no usable processor found");
		}
		private Funnel(IEnumerable<Proccesor<PT>> p)
		{
			_procs = p;
		}
		public Funnel()
		{
			_procs = new List<Proccesor<PT>>();
		}
		public Funnel(params Proccesor<PT>[] p)
		{
			_procs = p;
		}
		public Funnel(params IProccesor<PT>[] p)
			: this(p.Select(i => i.toProcessor()))
		{ }
		public IEnumerator<Proccesor<PT>> GetEnumerator()
		{
			return this._procs.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		public void Add(Proccesor<PT> p)
		{
			while (true)
			{
				List<Proccesor<PT>> l = this._procs as List<Proccesor<PT>>;
				if (l == null)
				{
					_procs = new List<Proccesor<PT>>(_procs);
					continue;
				}
				l.Add(p);
				break;
			}
		}
		public void Add(IProccesor<PT> p)
		{
			this.Add(p.toProcessor());
		}
	}
	public class Funnel : IFunnel
    {
		private IEnumerable<Proccesor> _procs;
		public void Process()
		{
			foreach (Proccesor p in _procs)
			{
				if (p())
					return;
			}
			throw new Exception("no usable processor found");
		}
		private Funnel(IEnumerable<Proccesor> p)
		{
			_procs = p;
		}
		public Funnel()
		{
			_procs = new List<Proccesor>();
		}
		public Funnel(params Proccesor[] p)
		{
			_procs = p;
		}
		public Funnel(params IProccesor[] p)
			: this(p.Select(i => i.toProcessor()))
		{ }
		public IEnumerator<Proccesor> GetEnumerator()
		{
			return this._procs.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		public void Add(Proccesor p)
		{
			while (true)
			{
				List<Proccesor> l = this._procs as List<Proccesor>;
				if (l == null)
				{
					_procs = new List<Proccesor>(_procs);
					continue;
				}
				l.Add(p);
				break;
			}
		}
		public void Add(IProccesor p)
		{
			this.Add(p.toProcessor());
		}
	}
    public class ConditionFunnel<PT, RT> : IFunnel<PT, RT>
    {
        private readonly Funnel<PT, RT> _int = new Funnel<PT, RT>();
        public void Add(Func<PT, bool> filter,Proccesor<PT, RT> p)
        {
            _int.Add((PT processed, out RT returnval) =>
            {
                if (filter(processed))
                {
                    return p(processed, out returnval);
                }
                returnval = default(RT);
                return false;
            });
        }
        public void Add(Func<PT, bool> filter, IProccesor<PT, RT> p)
        {
            Add(filter, p.toProcessor());
        }
        public void Add(Func<PT, RT> p)
        {
            Add(a=>true, p);
        }
        public void Add(Func<PT, bool> filter, Func<PT, RT> p)
        {
            Add(filter,((PT processed, out RT returnval) =>
            {
                returnval = p(processed);
                return true;
            }));
        }
        public RT Process(PT val)
        {
            return _int.Process(val);
        }
        public IEnumerator<Proccesor<PT, RT>> GetEnumerator()
        {
            return _int.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_int).GetEnumerator();
        }
    }
    public class ConditionFunnel<PT> : IFunnel<PT>
    {
        private readonly Funnel<PT> _int = new Funnel<PT>();
        public void Add(Func<PT, bool> filter, Proccesor<PT> p)
        {
            _int.Add(processed => filter(processed) && p(processed));
        }
        public void Add(Func<PT, bool> filter, IProccesor<PT> p)
        {
            Add(filter, p.toProcessor());
        }
        public void Add(Action<PT> p)
        {
            Add(a=>true, p);
        }
        public void Add(Func<PT, bool> filter, Action<PT> p)
        {
            Add(filter, (processed =>
            {
                p(processed);
                return true;
            }));
        }
        public void Process(PT val)
        {
            _int.Process(val);
        }
        public IEnumerator<Proccesor<PT>> GetEnumerator()
        {
            return _int.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_int).GetEnumerator();
        }
    }
    public class ConditionFunnel : IFunnel
    {
        private readonly Funnel _int = new Funnel();
        public void Add(Func<bool> filter, Proccesor p)
        {
            _int.Add(() => filter() && p());
        }
        public void Add(Func<bool> filter, IProccesor p)
        {
            Add(filter, p.toProcessor());
        }
        public void Add(Action p)
        {
            Add(()=>true, p);
        }
        public void Add(Func<bool> filter, Action p)
        {
            Add(filter, (() =>
            {
                p();
                return true;
            }));
        }
        public void Process()
        {
            _int.Process();
        }
        public IEnumerator<Proccesor> GetEnumerator()
        {
            return _int.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_int).GetEnumerator();
        }
    }
    public class QualifierFunnel<PT, IT, RT> : IFunnel<PT, RT>
    {
        private readonly ConditionFunnel<PT,RT> _int = new ConditionFunnel<PT, RT>();
        private readonly Func<PT, IT, bool> _qualifier;
        public QualifierFunnel(Func<PT, IT, bool> qualifier)
        {
            _qualifier = qualifier;
        }
        public virtual void Add(IT iterim, Proccesor<PT, RT> p)
        {
            _int.Add(a=> _qualifier(a,iterim),p);
        }
        public void Add(IT iterim, IProccesor<PT, RT> p)
        {
            Add(iterim, p.toProcessor());
        }
        public void Add(Func<PT, RT> p)
        {
            _int.Add(a =>true, p);
        }
        public void Add(IT iterim, Func<PT, RT> p)
        {
            Add(iterim, ((PT processed, out RT returnval) =>
            {
                returnval = p(processed);
                return true;
            }));
        }
        public RT Process(PT val)
        {
            return _int.Process(val);
        }
        public IEnumerator<Proccesor<PT, RT>> GetEnumerator()
        {
            return _int.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_int).GetEnumerator();
        }
    }
    public class QualifierFunnel<PT, IT> : IFunnel<PT>
    {
        private readonly ConditionFunnel<PT> _int = new ConditionFunnel<PT>();
        private readonly Func<PT, IT, bool> _qualifier;
        public QualifierFunnel(Func<PT, IT, bool> qualifier)
        {
            _qualifier = qualifier;
        }
        public virtual void Add(IT iterim, Proccesor<PT> p)
        {
            _int.Add(a => _qualifier(a, iterim), p);
        }
        public void Add(IT iterim, IProccesor<PT> p)
        {
            Add(iterim, p.toProcessor());
        }
        public void Add(Action<PT> p)
        {
            _int.Add(a => true, p);
        }
        public void Add(IT iterim, Action<PT> p)
        {
            Add(iterim, (processed =>
            {
                p(processed);
                return true;
            }));
        }
        public void Process(PT val)
        {
            _int.Process(val);
        }
        public IEnumerator<Proccesor<PT>> GetEnumerator()
        {
            return _int.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_int).GetEnumerator();
        }
    }
    public class QualifierFunnel<IT> : IFunnel
    {
        private readonly ConditionFunnel _int = new ConditionFunnel();
        private readonly Func<IT, bool> _qualifier;
        public QualifierFunnel(Func<IT, bool> qualifier)
        {
            _qualifier = qualifier;
        }
        public virtual void Add(IT iterim, Proccesor p)
        {
            _int.Add(() => _qualifier(iterim), p);
        }
        public virtual void Add(IT iterim, IProccesor p)
        {
            Add(iterim, p.toProcessor());
        }
        public virtual void Add(Action p)
        {
            _int.Add(() => true, p);
        }
        public virtual void Add(IT iterim, Action p)
        {
            Add(iterim, (() =>
            {
                p();
                return true;
            }));
        }
        public void Process()
        {
            _int.Process();
        }
        public IEnumerator<Proccesor> GetEnumerator()
        {
            return _int.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_int).GetEnumerator();
        }
    }
    public class TypeFunnel<PT, RT> : IFunnel<PT, RT>
    {
        private readonly ConditionFunnel<PT, RT> _int = new ConditionFunnel<PT, RT>();
        public void Add<T>(Proccesor<T, RT> p) where T : class, PT
        {
            _int.Add(pt => pt is T,(PT processed, out RT returnval) =>
            {
                var t = processed as T;
                return p(t, out returnval);
            });
        }
        public void Add<T>(Func<T, RT> p) where T : class, PT
        {
            Add(((T processed, out RT returnval) =>
            {
                returnval = p(processed);
                return true;
            }));
        }
        public void Add<T>(IProccesor<T, RT> p) where T : class, PT
        {
            Add(p.toProcessor());
        }
        public RT Process(PT val)
        {
            return _int.Process(val);
        }
        public IEnumerator<Proccesor<PT, RT>> GetEnumerator()
        {
            return _int.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_int).GetEnumerator();
        }
    }
    public class TypeFunnel<PT> : IFunnel<PT>
    {
        private readonly ConditionFunnel<PT> _int = new ConditionFunnel<PT>();
        public void Add<T>(Proccesor<T> p) where T : class, PT
        {
            _int.Add(a=>a is T ,processed =>
            {
                var t = processed as T;
                return t != null && p(t);
            });
        }
        public void Add<T>(Action<T> p) where T : class, PT
        {
            Add(((T processed) =>
            {
                p(processed);
                return true;
            }));
        }
        public void Add<T>(IProccesor<T> p) where T : class, PT
        {
            Add(p.toProcessor());
        }
        public void Process(PT val)
        {
            _int.Process(val);
        }
        public IEnumerator<Proccesor<PT>> GetEnumerator()
        {
            return _int.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_int).GetEnumerator();
        }
    }

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
