using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;

namespace Tests
{
    [TestClass]
    public class Spectypes
    {
        public static void diffButSimilar<T>(IList<T> a, IList<T> b)
        {
            Assert.IsTrue(a.GetType() != b.GetType() && a.SequenceEqualIndices(b));
        }
        public static void diff<T>(IList<T> a, IList<T> b)
        {
            Assert.IsTrue(a.GetType() != b.GetType() && a.Take(100).SequenceEqualIndices(b.Take(100)));
        }
        [TestMethod] public void Range()
        {
            diffButSimilar(range.Range(10), range.Range<int>(10));
            diffButSimilar(range.Range(3,10), range.Range<int>(3,10));
            diffButSimilar(range.Range(3,10,2), range.Range<int>(3,10,2));
        }
        [TestMethod]
        public void RRange()
        {
            diffButSimilar(range.RRange(10,3), range.RRange<int>(10,3));
            diffButSimilar(range.RRange(10, 3,2), range.RRange<int>(10, 3,2));
        }
        [TestMethod]
        public void IRange()
        {
            diffButSimilar(range.IRange(10), range.IRange<int>(10));
            diffButSimilar(range.IRange(3, 10), range.IRange<int>(3, 10));
            diffButSimilar(range.IRange(3, 10, 2), range.IRange<int>(3, 10, 2));
        }
        [TestMethod]
        public void RIRange()
        {
            diffButSimilar(range.RIRange(10,3), range.RIRange<int>(10,3));
            diffButSimilar(range.RIRange(10, 3,2), range.RIRange<int>(10, 3,2));
        }
        [TestMethod]
        public void CountUp()
        {
            diff(countUp.CountUp(), countUp.CountUp<int>());
            diff(countUp.CountUp(10), countUp.CountUp<int>(10));
            diff(countUp.CountUp(10, 3), countUp.CountUp<int>(10, 3));
        }
    }
}
