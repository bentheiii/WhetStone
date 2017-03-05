using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Comparison;
using WhetStone.Looping;

namespace Tests
{
    [TestClass]
    public class SplitCapture
    {
        [TestMethod]
        public void simpleList()
        {
            var val = new [] {1, 3, 5, 8, 4, 1, 9, 0, 11};
            var c = val.Split((a, b) =>
            {
                if (a.Count == 0)
                    return true;
                return a[0]%2 == b%2;
            });
            Assert.IsTrue(c.SequenceEqual(new[] {new[]  {1,3,5},new[] {8,4},new[] {1,9},new [] {0},new [] {11} }, new EnumerableCompararer<int>()));
        }
        [TestMethod]
        public void simpleEnum()
        {
            var val = new[] { 1, 3, 5, 8, 4, 1, 9, 0, 11 }.AsEnumerable();
            var c = val.Split((a, b) =>
            {
                if (a.Count == 0)
                    return true;
                return a[0] % 2 == b % 2;
            });
            Assert.IsTrue(c.SequenceEqual(new[] { new[] { 1, 3, 5 }, new[] { 8, 4 }, new[] { 1, 9 }, new[] { 0 }, new[] { 11 } }, new EnumerableCompararer<int>()));
        }
    }
}
