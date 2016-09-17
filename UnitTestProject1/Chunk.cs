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
            Assert.IsTrue(val[0].SequenceEqual(new[] { 0, 1, 2}));
            Assert.IsTrue(val[1].SequenceEqual(new[] { 3, 4, 5 }));
            Assert.IsTrue(val[2].SequenceEqual(new[] { 6, 7, 8 }));
            Assert.IsTrue(val[3].SequenceEqual(new[] { 9, 10, 11 }));
        }
    }
}
