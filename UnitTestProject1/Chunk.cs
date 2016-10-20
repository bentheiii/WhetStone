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
            Assert.IsTrue(val[0].SequenceEqual(0, 1, 2));
            Assert.IsTrue(val[1].SequenceEqual(3, 4, 5));
            Assert.IsTrue(val[2].SequenceEqual(6, 7, 8));
            Assert.IsTrue(val[3].SequenceEqual(9, 10, 11));
        }
    }
}
