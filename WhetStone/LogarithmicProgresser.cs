using System;
using WhetStone.SystemExtensions;

namespace NumberStone
{
    /// <summary>
    /// Stores a (floored) logarithm that is easy to increment.
    /// </summary>
    public class LogarithmicProgresser
    {
        /// <summary>
        /// The base of the logarithm.
        /// </summary>
        public int @base { get; }
        /// <summary>
        /// The antilogarithm.
        /// </summary>
        public long value { get; private set; }
        private long _nextNewLog;
        /// <summary>
        /// the (floored) logarithm.
        /// </summary>
        public int log { get; private set; }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="base">The logarithm's base.</param>
        /// <param name="initialValue">The initial antilogarithm.</param>
        public LogarithmicProgresser(int @base, long initialValue = 1)
        {
            this.@base = @base;
            value = initialValue;
            log = Math.Log(value,@base).floor();
            _nextNewLog = @base.pow(log + 1);
        }
        /// <summary>
        /// increases the antilogarithm.
        /// </summary>
        /// <param name="increment">The amount to increment by.</param>
        /// <returns>The amount by which the logarithm changed.</returns>
        public int Increment(int increment = 1)
        {
            value += increment;
            var ret = 0;
            while (value >= _nextNewLog)
            {
                _nextNewLog *= @base;
                ret += 1;
            }
            log += ret;
            return ret;
        }
        //todo decrement?
    }
}
