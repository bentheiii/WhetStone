using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;

namespace Tests
{
    [TestClass]
    public class ResizingArray
    {
        [TestMethod] public void Simple_resizing_array()
        {
            var arr = new ResizingArray<int>();

            void check(params int[] elements)
            {
                Assert.IsTrue(elements.SequenceEqual(arr.arrView));
            }

            check();
            arr.Add(1);
            arr.Add(2);
            check(1,2);
            arr.AddRange(range.IRange(3,5).Where(a=>a<10).AsEnumerable());
            check(1,2,3,4,5);
            arr.AddRange(range.IRange(6,8));
            check(1,2,3,4,5,6,7,8);
        }
    }
}
