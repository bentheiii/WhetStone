using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;

namespace Tests
{
    [TestClass]
    public class Splice
    {
        [TestMethod]
        public void Simple()
        {
            var val = range.Range(10).Splice(range.Range(5), 2);
            Assert.IsTrue(val.SequenceEqualIndices(0,1,0,1,2,3,4,2,3,4,5,6,7,8,9));
        }
    }
}
