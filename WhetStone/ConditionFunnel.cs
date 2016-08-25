using System;
using System.Collections;
using System.Collections.Generic;

namespace WhetStone.Funnels
{
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
            Add(a => true, p);
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
            Add(() => true, p);
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
}