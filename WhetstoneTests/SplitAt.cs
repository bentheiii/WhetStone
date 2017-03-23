using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;
using WhetStone.WordPlay;

namespace Tests
{
    [TestClass]
    public class SplitAt
    {
        [TestMethod]
        public void Simple()
        {
            var val = "abbcccddddeeeee".AsEnumerable().SplitAt(1, 2, 3, 4).Select(a=>a.ConvertToString());
            Assert.IsTrue(val.SequenceEqual("a","bb","ccc","dddd","eeeee"));
            var vall = "abbcccddddeeeee".AsList().SplitAt(1, 2, 3, 4).Select(a => a.ConvertToString());
            Assert.IsTrue(vall.SequenceEqual("a", "bb", "ccc", "dddd", "eeeee"));
        }
        [TestMethod] public void Full()
        {
            var val = "abbcccddddeeeee".AsEnumerable().SplitAt(1, 2, 3, 4, 5).Select(a => a.ConvertToString());
            Assert.IsTrue(val.SequenceEqual("a", "bb", "ccc", "dddd", "eeeee", ""));
            var vall = "abbcccddddeeeee".AsList().SplitAt(1, 2, 3, 4, 5).Select(a => a.ConvertToString());
            Assert.IsTrue(vall.SequenceEqual("a", "bb", "ccc", "dddd", "eeeee", ""));
        }
    }
}
