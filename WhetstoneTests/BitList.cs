using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumberStone;
using WhetStone.Looping;
using WhetStone.SystemExtensions;

namespace Tests
{
    [TestClass]
    public class BitListTest
    {
        [TestMethod] public void setGet()
        {
            var val = new BitList(100);
            Assert.IsTrue(val.SequenceEqualIndices(range.Range(100).Select(a=>false)));
            foreach (int i in val.Indices())
            {
                Assert.IsFalse(val[i]);
            }
            val[0] = val[1] = val[4] = val[9] = val[16] = val[25] = val[36] = val[49] = val[64] = val[81] = true;
            foreach (int i in range.Range(10).Select(a=>a*a).Skip(1))
            {
                Assert.IsFalse(val[i+1]);
                Assert.IsTrue(val[i]);
            }
        }
        [TestMethod] public void Add()
        {
            var val = new BitList(100);
            val[0] = val[1] = val[4] = val[9] = val[16] = val[25] = val[36] = val[49] = val[64] = val[81] = true;
            IList<bool> comp = new List<bool>(range.Range(100).Select(a=>new [] {0,1,4,9,16,25,36,49,64,81}.Contains(a)));
            Assert.IsTrue(val.SequenceEqualIndices(comp));

            val.Add(true);
            comp.Add(true);

            Assert.IsTrue(val.SequenceEqualIndices(comp));

            val.Insert(0,false);
            comp.Insert(0,false);

            Assert.IsTrue(val.SequenceEqualIndices(comp));

            val.Insert(6, true);
            comp.Insert(6, true);

            Assert.IsTrue(val.SequenceEqualIndices(comp));

            val.Insert(86, false);
            comp.Insert(86, false);

            Assert.IsTrue(val.SequenceEqualIndices(comp));

            while (val.Count < 640)
            {
                val.Insert(18, comp.Count%2==0);
                comp.Insert(18, comp.Count % 2 == 0);
                Assert.IsTrue(val.SequenceEqualIndices(comp), val.Count.ToString());
            }
        }
        [TestMethod]
        public void Remove()
        {
            var val = new BitList(100);
            val[0] = val[1] = val[4] = val[9] = val[16] = val[25] = val[36] = val[49] = val[64] = val[81] = true;
            IList<bool> comp = new List<bool>(range.Range(100).Select(a => new[] { 0, 1, 4, 9, 16, 25, 36, 49, 64, 81 }.Contains(a)));
            Assert.IsTrue(val.SequenceEqualIndices(comp));

            val.RemoveAt(99);
            comp.RemoveAt(99);

            Assert.IsTrue(val.SequenceEqualIndices(comp));

            val.RemoveAt(0);
            comp.RemoveAt(0);

            Assert.IsTrue(val.SequenceEqualIndices(comp));

            val.RemoveAt(6);
            comp.RemoveAt(6);

            Assert.IsTrue(val.SequenceEqualIndices(comp));

            val.RemoveAt(86);
            comp.RemoveAt(86);

            Assert.IsTrue(val.SequenceEqualIndices(comp));

            while (val.Count > 64)
            {
                val.RemoveAt(63);
                comp.RemoveAt(63);
                Assert.IsTrue(val.SequenceEqualIndices(comp), val.Count.ToString());
            }

        }
        [TestMethod] public void SetAll()
        {
            var val = new BitList(600);
            val[30] = val[64] = val[85] = val[99] = val[125] = val[199] = true;
            val.SetRange(10,190,true);
            Assert.AreEqual(600,val.Count);
            foreach (var index in val.Indices())
            {
                Assert.AreEqual(index.iswithinPartialExclusive(10, 200), val[index], index.ToString());
            }
        }
        [TestMethod]
        public void Gentest()
        {
            var val = new BitList(10);
            val[0] = val[1] = val[4] /*= val[9] = val[16] = val[25] = val[36] = val[49] = val[64] = val[81]*/ = true;

            MutableListCheck.check(val, 604071330);
        }
    }
}
