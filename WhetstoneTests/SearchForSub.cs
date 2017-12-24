using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;

namespace Tests
{
    [TestClass]
    public class SearchForSub
    {
        [TestMethod]
        public void Simple_SearchForSub_enum()
        {
            var haystack = new [] {0, 1, 2, 1, 2, 33, 3, 3,1,2,1}.AsEnumerable();

            void Check(IEnumerable<int> needle, params int[] indices)
            {
                var found = haystack.SearchForSub(needle).Select(a => a.startIndex).ToArray();
                Assert.IsTrue(found.SequenceEqual(indices));
            }

            Check(new [] {1,2}, 1,3,8);
            Check(new [] {1}, 1,3,8,10);
            Check(new [] {33}, 5);
            Check(new [] {3}, 6, 7);
            Check(new []{33,1,2});

            haystack = new[] {97, 45}.AsEnumerable();
            Check(new []{45},1);
        }
        [TestMethod]
        public void Simple_SearchForSub_list()
        {
            var haystack = new[] { 0, 1, 2, 1, 2, 33, 3, 3, 1, 2, 1 };

            void Check(IList<int> needle, params int[] indices)
            {
                var found = haystack.SearchForSub(needle).Select(a => a.startIndex).ToArray();
                Assert.IsTrue(found.SequenceEqual(indices));
            }

            Check(new[] { 1, 2 }, 1, 3, 8);
            Check(new[] { 1 }, 1, 3, 8, 10);
            Check(new[] { 33 }, 5);
            Check(new[] { 3 }, 6, 7);
            Check(new[] { 33, 1, 2 });

            haystack = new[] { 97, 45 };
            Check(new[] { 45 }, 1);
        }
    }
}
