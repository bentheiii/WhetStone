using System.Collections.Generic;
using WhetStone.Fielding;

namespace NumberStone
{
    //todo does this work?

    public class KahanSum
    {
        public double Sum { get; private set; } = 0;
        private double _compensation = 0;
        public void Add(double item)
        {
            var y = item - _compensation;
            var t = Sum + y;
            _compensation = (t - Sum) - y;
            Sum = t;
        }
        public void Add(IEnumerable<double> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }
    }
    public class KahanSum<T>
    {
        public T Sum { get; private set; }
        private FieldWrapper<T> _compensation;
        public KahanSum()
        {
            Field<T> f = Fields.getField<T>();
            Sum = f.zero;
            _compensation = f.zero;
        }
        public void Add(T item)
        {
            var y = item - _compensation;
            var t = Sum + y;
            _compensation = (t - Sum) - y;
            Sum = t;
        }
        public void Add(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }
    }
}
