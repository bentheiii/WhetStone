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
        public void SampleEnumerable()
        {
            var val = range.Range(10).AsEnumerable().Cover((-1).Enumerate(3),2,1);
            Assert.IsTrue(val.SequenceEqual(0, 1, -1, 3, -1, 5, -1, 7, 8, 9));
        }
        [TestMethod]
        public void SampleList()
        {
            var val = range.Range(10).Cover((-1).Enumerate(3), 2, 1);
            var exp = new[] {0, 1, -1, 3, -1, 5, -1, 7, 8, 9};
            Assert.IsTrue(val.SequenceEqualIndices(exp));
        }
    }
}
