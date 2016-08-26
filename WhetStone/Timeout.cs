using System;
using System.Threading;
using System.Threading.Tasks;
using WhetStone.Timer;

namespace WhetStone.Processes
{
    public static class timeout
    {
        public static bool TimeOut(this Action action, TimeSpan maxtime)
        {
            TimeSpan time;
            return TimeOut(action, maxtime, out time);
        }
        public static bool TimeOut(this Action action, TimeSpan maxtime, out TimeSpan time)
        {
            var tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            int timeOut = (int)maxtime.TotalMilliseconds;
            var task = Task.Factory.StartNew(action, token);
            IdleTimer t = new IdleTimer();
            bool ret = task.Wait(timeOut, token);
            time = t.timeSinceStart;
            return ret;
        }
        public static bool TimeOut<T>(this Func<T> action, TimeSpan maxtime, out TimeSpan time)
        {
            T result;
            return TimeOut(action, maxtime, out time, out result);
        }
        public static bool TimeOut<T>(this Func<T> action, TimeSpan maxtime, out T result)
        {
            TimeSpan time;
            return TimeOut(action, maxtime, out time, out result);
        }
        public static bool TimeOut<T>(this Func<T> action, TimeSpan maxtime, out TimeSpan time, out T result)
        {
            var tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            int timeOut = (int)maxtime.TotalMilliseconds;
            var task = Task<T>.Factory.StartNew(action, token);
            IdleTimer t = new IdleTimer();
            bool ret = task.Wait(timeOut, token);
            time = t.timeSinceStart;
            result = task.Result;
            return ret;
        }
    }
}
