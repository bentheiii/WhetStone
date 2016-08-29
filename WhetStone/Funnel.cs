using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
}
