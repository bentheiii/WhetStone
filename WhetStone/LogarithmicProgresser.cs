using WhetStone.SystemExtensions;

namespace NumberStone
{
    public class LogarithmicProgresser
    {
        public int @base { get; }
        public long value { get; private set; }
        private long nextNewLog;
        public int log { get; private set; }
        public LogarithmicProgresser(int @base, long initialValue = 1)
        {
            this.@base = @base;
            value = initialValue;
            log = value.log(@base).floor();
            nextNewLog = @base.pow(log + 1);
        }
        public int Increment(int increment = 1)
        {
            value += increment;
            var ret = 0;
            while (value >= nextNewLog)
            {
                nextNewLog *= @base;
                ret += 1;
            }
            log += ret;
            return ret;
        }
    }
}
