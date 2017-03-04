using System;
using System.Configuration;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;

namespace Tests
{
    [TestClass]
    public class Cover
    {
        [TestMethod]
        public void IndicesEnumerable()
        {
            var val = range.Range(10).AsEnumerable().Cover((-1).Enumerate(3), range.IRange(2, 6, 2));
            Assert.IsTrue(val.SequenceEqual(0, 1, -1, 3, -1, 5, -1, 7, 8, 9));
        }
        [TestMethod]
        public void SimpleEnumerable()
        {
            var val = range.Range(10).AsEnumerable().Cover((-1).Enumerate(3), 2);
            Assert.IsTrue(val.SequenceEqual(0, 1, -1, -1, -1, 5, 6, 7, 8, 9));
        }
        [TestMethod]
        public void IndicesList()
        {
            var val = range.Range(10).ToList().Cover((-1).Enumerate(3).ToList(), range.IRange(2,6,2).ToList());
            var exp = new[] {0, 1, -1, 3, -1, 5, -1, 7, 8, 9}.ToList();
            Assert.IsTrue(val.SequenceEqualIndices(exp));
            MutableListCheck.check(val);

        }
        [TestMethod]
        public void SimpleList()
        {
            var val = range.Range(10).ToList().Cover((-1).Enumerate(3).ToList(), 2);
            var exp = new[] { 0, 1, -1, -1, -1, 5, 6, 7, 8, 9 }.ToList();
            Assert.IsTrue(val.SequenceEqualIndices(exp));
            MutableListCheck.check(val);
        }
    }
}
