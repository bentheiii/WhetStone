using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;


namespace Tests
{
    [TestClass]
    public class ListDoubleSidedTest
    {
        [TestMethod]
        public void Simple()
        {
            ListDoubleSided<int> val = new ListDoubleSided<int>();
            Assert.IsTrue(val.Count == 0);
            val.Add(6);
            Assert.IsTrue(val.SequenceEqualIndices(6));
            val.Insert(0,0);
            Assert.IsTrue(val.CountBind().Select(a => Tuple.Create(a.Item1, val[a.Item2])).All(a => a.Item1.Equals(a.Item2)));
            Assert.IsTrue(val.SequenceEqualIndices(0,6));
            val.Insert(1,5);
            Assert.IsTrue(val.CountBind().Select(a => Tuple.Create(a.Item1, val[a.Item2])).All(a => a.Item1.Equals(a.Item2)));
            Assert.IsTrue(val.SequenceEqualIndices(0, 5, 6));
            val.Insert(2,2);
            Assert.IsTrue(val.CountBind().Select(a => Tuple.Create(a.Item1, val[a.Item2])).All(a => a.Item1.Equals(a.Item2)));
            val.RemoveAt(1);
            Assert.IsTrue(val.SequenceEqualIndices(0, 2, 6));
            Assert.IsTrue(val.CountBind().Select(a => Tuple.Create(a.Item1, val[a.Item2])).All(a => a.Item1.Equals(a.Item2)));
        }
    }
}
