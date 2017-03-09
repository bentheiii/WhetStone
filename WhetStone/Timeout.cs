using System;
using System.Threading;
using System.Threading.Tasks;
using WhetStone.SystemExtensions;

namespace WhetStone.Processes
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class timeout
    {
        /// <summary>
        /// Perform an <see cref="Action"/>, or cancel it if it exceeds a timeout.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> to invoke.</param>
        /// <param name="maxtime">The timeout to cancel <paramref name="action"/> if exceeded.</param>
        /// <returns>Whether the action completed within the allotted time.</returns>
        public static bool TimeOut(this Action action, TimeSpan maxtime)
        {
            action.ThrowIfNull(nameof(action));
            var tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            int timeOut = (int)maxtime.TotalMilliseconds;
            var task = Task.Factory.StartNew(action, token);
            bool ret = task.Wait(timeOut, token);
            return ret;
        }
        /// <summary>
        /// Perform an <see cref="Func{TResult}"/>, or cancel it if it exceeds a timeout.
        /// </summary>
        /// <param name="action">The <see cref="Func{TResult}"/> to invoke.</param>
        /// <param name="maxtime">The timeout to cancel <paramref name="action"/> if exceeded.</param>
        /// <param name="result">The output of <paramref name="action"/>.</param>
        /// <returns>Whether the action completed within the allotted time.</returns>
        public static bool TimeOut<T>(this Func<T> action, TimeSpan maxtime, out T result)
        {
            action.ThrowIfNull(nameof(action));
            var tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            int timeOut = (int)maxtime.TotalMilliseconds;
            var task = Task<T>.Factory.StartNew(action, token);
            bool ret = task.Wait(timeOut, token);
            result = task.Result;
            return ret;
        }
    }
}
