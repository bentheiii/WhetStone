using System;
using System.Collections;
using System.Collections.Generic;

namespace WhetStone.Funnels
{
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
            Add((T processed, out RT returnval) =>
            {
                returnval = p(processed);
                return true;
            });
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
            _int.Add(a => a is T, processed =>
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
}