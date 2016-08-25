using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Units.Time
{
    public static class TimeExtentions
    {
        public static TimeSpan Divide(this TimeSpan t, double divisor)
        {
            return multiply(t, 1.0 / divisor);
        }
        public static double Divide(this TimeSpan t, TimeSpan divisor)
        {
            return (t.Ticks / (double)divisor.Ticks);
        }
        public static TimeSpan multiply(this TimeSpan t, double factor)
        {
            return new TimeSpan((long)(t.Ticks * factor));
        }
    }
}
