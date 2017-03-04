using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;

namespace Tests
{
    [TestClass]
    public class Cache
    {
        [TestMethod]
        public void EnumerableInf()
        {
            int count = 0;
            IEnumerable<int> src = range.Range(100).AsEnumerable().Select(a =>
            {
                count++;
                return a;
            }).Cache();

            foreach (int i in range.Range(100))
            {
                Assert.AreEqual(src.Sum(),4950);
                Assert.AreEqual(src.ElementAt(i),i);
                Assert.IsTrue(count <= 100);
            }
        }

        [TestMethod]
        public void EnumerableBound()
        {
            int count = 0;
            IEnumerable<int> src = range.Range(100).AsEnumerable().Select(a =>
            {
                count++;
                return a;
            }).Cache(10);

            foreach (int i in range.Range(10))
            {
                Assert.AreEqual(src.Take(10).Sum(), 45);
                Assert.AreEqual(src.ElementAt(i), i);
                Assert.IsTrue(count <= 10);
            }

            foreach (int i in range.Range(100))
            {
                Assert.AreEqual(src.Take(100).Sum(), 4950);
                Assert.AreEqual(src.ElementAt(i), i);
            }
        }

        [TestMethod]
        public void ListInf()
        {
            int count = 0;
            IList<int> src = range.Range(100).ToList().Select(a =>
            {
                count++;
                return a;
            },a=>a).Cache();

            foreach (int i in range.Range(100))
            {
                Assert.AreEqual(src.Sum(), 4950);
                Assert.AreEqual(src[i], i);
                Assert.IsTrue(count <= 100);
            }

            MutableListCheck.check(src);
        }

        [TestMethod]
        public void ListBound()
        {
            int count = 0;
            IList<int> src = range.Range(100).ToList().Select(a =>
            {
                count++;
                return a;
            }, a => a).Cache(10);

            foreach (int i in range.Range(10))
            {
                Assert.AreEqual(src.Take(10).Sum(), 45);
                Assert.AreEqual(src[i], i);
                Assert.IsTrue(count <= 10);
            }

            foreach (int i in range.Range(100))
            {
                Assert.AreEqual(src.Take(100).Sum(), 4950);
                Assert.AreEqual(src[i], i);
            }

            MutableListCheck.check(src);
        }
    }
}
