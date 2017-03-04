using WhetStone.Fielding;

namespace NumberStone
{
    //todo does this work?
    /// <summary>
    /// A mutable <see cref="double"/> sum that compensates for floating-point errors.
    /// </summary>
    /// <remarks>using the Khan method.</remarks>
    public class KahanSum
    {
        /// <summary>
        /// Get the current sum of all added items.
        /// </summary>
        public double Sum { get; private set; } = 0;
        private double _compensation = 0;
        /// <summary>
        /// Adds an item to the sum.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(double item)
        {
            var y = item - _compensation;
            var t = Sum + y;
            _compensation = (t - Sum) - y;
            Sum = t;
        }
    }
    /// <summary>
    /// A mutable generic sum that compensates for floating-point errors.
    /// </summary>
    /// <remarks>
    /// <para>using the Khan method.</para>
    /// <para>Uses fielding for addition.</para>
    /// </remarks>
    public class KahanSum<T>
    {
        /// <summary>
        /// Get the current sum of all added items.
        /// </summary>
        public T Sum { get; private set; }
        private FieldWrapper<T> _compensation;
        /// <summary>
        /// Constructor.
        /// </summary>
        public KahanSum()
        {
            Field<T> f = Fields.getField<T>();
            Sum = f.zero;
            _compensation = f.zero;
        }
        /// <summary>
        /// Adds an item to the sum.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(T item)
        {
            var y = item - _compensation;
            var t = Sum + y;
            _compensation = (t - Sum) - y;
            Sum = t;
        }
    }
}
