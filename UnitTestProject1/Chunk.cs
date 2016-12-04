using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class Chunk
    {
        [TestMethod]
        public void Simple()
        {
            var val = range.Range(12).ToArray().Chunk(3).ToArray();
            Assert.IsTrue(val[0].SequenceEqualIndices(0, 1, 2));
            Assert.IsTrue(val[1].SequenceEqualIndices(3, 4, 5));
            Assert.IsTrue(val[2].SequenceEqualIndices(6, 7, 8));
            Assert.IsTrue(val[3].SequenceEqualIndices(9, 10, 11));
            Assert.IsTrue(val.CountBind().Select(a => Tuple.Create(a.Item1, val[a.Item2])).All(a => a.Item1.SequenceEqual(a.Item2)));
        }
    }
}
