using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;

namespace Tests
{
    [TestClass]
    public class SequenceEqualIndices
    {
        [TestMethod]
        public void Simple()
        {
            var val00 = range.Range(10);
            var val01 = new [] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
            var val10 = range.Range(9);
            var val20 = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            Assert.IsTrue(val00.SequenceEqualIndices(val01));
            Assert.IsTrue(val01.SequenceEqualIndices(val00));
            Assert.IsTrue(val00.SequenceEqualIndices(val00));
            Assert.IsTrue(val01.SequenceEqualIndices(val01));
            Assert.IsTrue(val10.SequenceEqualIndices(val10));
            Assert.IsTrue(val20.SequenceEqualIndices(val20));

            Assert.IsTrue(!val00.SequenceEqualIndices(val10));
            Assert.IsTrue(!val00.SequenceEqualIndices(val20));
            Assert.IsTrue(!val01.SequenceEqualIndices(val10));
            Assert.IsTrue(!val01.SequenceEqualIndices(val20));
            Assert.IsTrue(!val10.SequenceEqualIndices(val20));
        }
    }
}
