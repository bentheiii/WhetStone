using System;
using System.Collections;
using System.Collections.Generic;

namespace WhetStone.Funnels
{
    public class QualifierFunnel<PT, IT, RT> : IFunnel<PT, RT>
    {
        private readonly ConditionFunnel<PT, RT> _int = new ConditionFunnel<PT, RT>();
        private readonly Func<PT, IT, bool> _qualifier;
        public QualifierFunnel(Func<PT, IT, bool> qualifier)
        {
            _qualifier = qualifier;
        }
        public virtual void Add(IT iterim, Proccesor<PT, RT> p)
        {
            _int.Add(a => _qualifier(a, iterim), p);
        }
        public void Add(IT iterim, IProccesor<PT, RT> p)
        {
            Add(iterim, p.toProcessor());
        }
        public void Add(Func<PT, RT> p)
        {
            _int.Add(a => true, p);
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
}