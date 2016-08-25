using System;
using System.Threading;

namespace WhetStone.Random
{
    namespace ThreadEntropy
    {
        public class EntropyRandomGenerator : RandomGenerator, IDisposable
        {
            private volatile int _val = 0;
            private readonly Thread[] _runners;
            public EntropyRandomGenerator(int threadCount = 2, ThreadPriority priority = ThreadPriority.Lowest)
            {
                _runners = new Thread[threadCount];
                for (int i = 0; i < threadCount; i++)
                {
                    _runners[i] = new Thread(ThreadProcedure) {Priority = priority};
                    _runners[i].Start();
                }
            }
            private void ThreadProcedure()
            {
                while (true)
                    _val++;
            }
            protected virtual void Dispose(bool disposing)
            {
                if( disposing) foreach (Thread runner in _runners)
                {
                    runner.Abort();
                }
            }
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            public override int Int(int min, int max)
            {
                _val *= 258745143;
                return (Math.Abs(_val % (max - min)) + min);
            }
        }
    }
}