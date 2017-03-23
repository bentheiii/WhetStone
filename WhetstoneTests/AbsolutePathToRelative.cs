using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;
using WhetStone.Path;

namespace Tests
{
    [TestClass]
    public class AbsolutePathToRelative
    {
        [TestMethod]
        public void Simple()
        {
            string[] paths = {@"C:\", @"C:\filepath.txt", @"C:\dir0\dir00\dir000", @"C:\dir0\dir01\dir010", @"C:\dir1\dir00"};

            foreach (var pair in paths.Join(@join.CartesianType.AllPairs))
            {
                var origin = pair.Item1;
                var dest = pair.Item2;

                var rel = absolutePathToRelative.AbsolutePathToRelative(origin, dest);

                var formed = Path.Combine(origin, rel);
                formed = Path.GetFullPath(formed);
                Assert.AreEqual(dest,formed);
            }
        }
    }
}
