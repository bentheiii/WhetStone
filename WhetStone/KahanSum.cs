using WhetStone.Fielding;

namespace WhetStone
{
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
    }
}
