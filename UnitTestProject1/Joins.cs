using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Comparison;
using WhetStone.Looping;

namespace Tests
{
    [TestClass]
    public class Joins
    {
        [TestMethod] public void Double()
        {
            var val1 = new[] {0, 1, 2, 3, 4};
            var val2 = new[] {'a', 'b', 'c'};
            var val = val1.Join(val2);
            Assert.IsTrue(val[0].Equals(Tuple.Create(0,'a')));
            Assert.IsTrue(val[6].Equals(Tuple.Create(1, 'b')));
            Assert.IsTrue(val[14].Equals(Tuple.Create(4, 'c')));

            Assert.AreEqual(val.Count, 15);

            Assert.IsTrue(val.SequenceEqual( new []
            {
                Tuple.Create(0,'a'),Tuple.Create(1,'a'),Tuple.Create(2,'a'),Tuple.Create(3,'a'),Tuple.Create(4,'a'),
                Tuple.Create(0,'b'),Tuple.Create(1,'b'),Tuple.Create(2,'b'),Tuple.Create(3,'b'),Tuple.Create(4,'b'),
                Tuple.Create(0,'c'),Tuple.Create(1,'c'),Tuple.Create(2,'c'),Tuple.Create(3,'c'),Tuple.Create(4,'c'),
            }, new TupleEqualityComparer<int,char>()));
        }
        [TestMethod]
        public void Multi()
        {
            var val = new IList<int>[] {range.Range(5), range.Range(2), range.Range(3)}.Join();
            Assert.IsTrue(val[0].SequenceEqual(new[] {0, 0, 0}));
            Assert.IsTrue(val[6].SequenceEqual(new[] {1, 1, 0 }));
            Assert.IsTrue(val[14].SequenceEqual(new[] {4, 0, 1 }));

            Assert.AreEqual(val.Count,30);

            Assert.IsTrue(val.SequenceEqual( new []
            {
                new [] {0,0,0},new [] {1,0,0},new [] {2,0,0},new [] {3,0,0},new [] {4,0,0},
                new [] {0,1,0},new [] {1,1,0},new [] {2,1,0},new [] {3,1,0},new [] {4,1,0},
                new [] {0,0,1},new [] {1,0,1},new [] {2,0,1},new [] {3,0,1},new [] {4,0,1},
                new [] {0,1,1},new [] {1,1,1},new [] {2,1,1},new [] {3,1,1},new [] {4,1,1},
                new [] {0,0,2},new [] {1,0,2},new [] {2,0,2},new [] {3,0,2},new [] {4,0,2},
                new [] {0,1,2},new [] {1,1,2},new [] {2,1,2},new [] {3,1,2},new [] {4,1,2},
            }, new EnumerableEqualityCompararer<int>()));
        }
        [TestMethod]
        public void MetaAllPairs()
        {
            var val = range.Range(5).Join(3);
            Assert.IsTrue(val[0].SequenceEqual(new [] {0,0,0}));
            Assert.IsTrue(val[10].SequenceEqual(new [] {0,2,0}));
            Assert.IsTrue(val[32].SequenceEqual(new[] { 2, 1, 1 }));

            Assert.AreEqual(val.Count,125);

            Assert.IsTrue(val.Select(a=>a[0]+a[1]*5+a[2]*25).SequenceEqual(range.Range(125)));
        }
        [TestMethod]
        public void MetaMonoDescending()
        {
            var val = range.Range(5).Join(3, join.CartesianType.NoSymmatry);
            Assert.IsTrue(val[0].SequenceEqual(new[] { 0, 0, 0 }));
            Assert.IsTrue(val[10].SequenceEqual(new[] { 3, 2, 0 }));
            Assert.IsTrue(val[32].SequenceEqual(new[] { 4,3,3 }));

            Assert.AreEqual(val.Count, 35);

            Assert.IsTrue(val.Select(a => a[0] + a[1] * 10 + a[2] * 100).SequenceEqual(new int[35]
            {
                0,1,2,3,4,11,12,13,14,22,23,24,33,34,44,111,112,113,114,122,123,124,133,134,144,222,223,224,233,234,244,333,334,344,444
            }));
        }
        [TestMethod]
        public void MetaDescending()
        {
            var val = range.Range(5).Join(3, join.CartesianType.NoReflexive | join.CartesianType.NoSymmatry);
            Assert.IsTrue(val[0].SequenceEqual(new[] { 2, 1, 0 }));
            Assert.IsTrue(val[6].SequenceEqual(new[] { 3, 2, 1 }));
            Assert.IsTrue(val[8].SequenceEqual(new[] { 4, 3, 1 }));

            Assert.AreEqual(val.Count, 10);

            Assert.IsTrue(val.Select(a => a[0] + a[1] * 10 + a[2] * 100).SequenceEqual(new int[10]
            {
                12,13,14,23,24,34,123,124,134,234
            }));
        }
        [TestMethod]
        public void MetaNoReflexive()
        {
            var val = range.Range(5).Join(3, join.CartesianType.NoReflexive);
            Assert.IsTrue(val[0].SequenceEqual(new[] { 2, 1, 0 }));
            Assert.IsTrue(val[10].SequenceEqual(new[] { 2, 4, 0 }));
            Assert.IsTrue(val[48].SequenceEqual(new[] { 1, 0, 4 }));

            Assert.AreEqual(val.Count, 60);

            Assert.IsTrue(val.Select(a => a[0] + a[1]*10 + a[2]*100).SequenceEqual(new int[60]
            {
                12, 13, 14, 21, 23, 24, 31, 32, 34, 41, 42, 43, 102, 103, 104, 120, 123, 124, 130, 132, 134, 140, 142, 143, 201, 203, 204, 210, 213,
                214, 230, 231, 234, 240, 241, 243, 301, 302, 304, 310, 312, 314, 320, 321, 324, 340, 341, 342, 401, 402, 403, 410, 412, 413, 420, 421,
                423, 430, 431, 432
            }));
        }
        [TestMethod] public void keepsorder()
        {
            const int n = 5, length = 3;
            int trials = 0;
            foreach (join.CartesianType cartesianType in new [] {join.CartesianType.AllPairs, join.CartesianType.NoReflexive, join.CartesianType.NoSymmatry, join.CartesianType.NoReflexive| join.CartesianType.NoSymmatry})
            {
                trials++;
                var valList = range.Range(n).Join(length, cartesianType);
                var valEnumerable = range.Range(n).AsEnumerable().Join(length, cartesianType);
                Assert.IsTrue(valList.Count == valEnumerable.Count());
                Assert.IsTrue(valList.Count > 0);
                Assert.IsTrue(valList.SequenceEqual(valEnumerable,new EnumerableEqualityCompararer<int>()));
            }
            Assert.AreEqual(4,trials);
        }
    }
}
