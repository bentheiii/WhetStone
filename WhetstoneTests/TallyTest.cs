using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;
using WhetStone.Tuples;

namespace Tests
{
    [TestClass]
    public class TallyTest
    {
        [TestMethod]
        public void Simple()
        {
            var val = new TypeTally<double>().TallyAggregate((a, b) => a + b,0.0, a => a > 1)
                .TallyAny().TallyAny(a => a < 0, true).TallyCount();
            var tests = new[]
            {
                Tuple.Create(new[] {0.05, 0.05, 0.6, 0.0, 0.1}, Tuple.Create(0.8, true, false, 5)),
                Tuple.Create(new[] {0.05, 0.05, 0.6, 0.0, 0.1, 0.2}, Tuple.Create(1.0, true, false, 6)),
                Tuple.Create(new double[0], Tuple.Create(0.0, false, false, 0)),
                Tuple.Create(new[] {0.05, 0.5, 0.6, 0.0, 0.1, 0.2}, Tuple.Create(1.15, true, false, 2)),
                Tuple.Create(new[] {0.05, -0.05, 0.6, 0.0, 0.1, 0.2}, Tuple.Create(0.0, true, true, 1)),
            };

            foreach (var test in tests)
            {
                var result = val.Do(test.Item1);
                Assert.AreEqual(result.Item1, test.Item2.Item1,1e-3);
                Assert.AreEqual(result.Item2, test.Item2.Item2);
                Assert.AreEqual(result.Item3, test.Item2.Item3);
                Assert.AreEqual(result.Item4, test.Item2.Item4);
            }
        }
    }
}
