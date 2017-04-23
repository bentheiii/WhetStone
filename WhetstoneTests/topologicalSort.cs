using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumberStone;
using WhetStone.Looping;

namespace Tests
{
    [TestClass]
    public class topologicalSort
    {
        [TestMethod]
        public void Simple()
        {
            int[] elements = range.IRange(1, 15).ToArray();
            var ordered = elements.Attach(extractDependants)
            .TopologicalSort().ToArray();
            foreach (int i in ordered.Indices())
            {
                var deps = extractDependants(ordered[i]);
                if (deps.Count() == 1)
                    continue;
                foreach (int dep in deps)
                {
                    var j = ((IList<int>)ordered).IndexOf(dep);
                    Assert.IsTrue(i > j);
                }
            }
        }
        
        private static ICollection<int> extractDependants(int a)
        {
            var x = a.Primefactors();
            return x.DistinctSorted().Select(y => a / y).ToArray();
        }
    }
}
