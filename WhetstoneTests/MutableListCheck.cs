using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;
using WhetStone.Random;

namespace Tests
{
    internal static class MutableListCheck
    {
        public static void check(IList<int> orig, int? seed = null, bool randomSet = true, bool append = true, bool randominsert = true, bool randomRemove = true)
        {
            int realseed = seed ?? 0;
            RandomGenerator gen;
            if (seed.HasValue)
                gen = new LocalRandomGenerator(seed);
            else
                gen = new LocalRandomGenerator(out realseed);

            if (orig.IsReadOnly)
                throw new AssertFailedException("List is read only");

            if (randomSet)
            {
                if(!set(orig, gen))
                    throw new AssertFailedException("setrandom failed, seed: "+realseed);
            }
            if (append)
            {
                if (!add(orig, gen))
                    throw new AssertFailedException("add failed, seed: " + realseed);
            }
            if (randominsert)
            {
                if (!insert(orig, gen))
                    throw new AssertFailedException("insert failed, seed: " + realseed);
            }
            if (randomRemove)
            {
                if (!remove(orig, gen))
                    throw new AssertFailedException("add failed, seed: " + realseed);
            }
        }

        private static bool set(IList<int> orig, RandomGenerator gen)
        {
            var check = orig.ToList();

            foreach (int _ in range.Range(100))
            {
                int val = gen.Int();
                int ind = gen.Int(orig.Count);

                check[ind] = val;
                orig[ind] = val;
                if (!check.SequenceEqualIndices(orig))
                    return false;
            }

            return true;
        }
        private static bool add(IList<int> orig, RandomGenerator gen)
        {
            var check = orig.ToList();

            foreach (int _ in range.Range(100))
            {
                int val = gen.Int();

                check.Add(val);
                orig.Add(val);
                if (!check.SequenceEqualIndices(orig))
                    return false;
            }

            return true;
        }
        private static bool insert(IList<int> orig, RandomGenerator gen)
        {
            var check = orig.ToList();

            foreach (int _ in range.Range(100))
            {
                int val = gen.Int();
                int ind = gen.Int(orig.Count);

                check.Insert(ind, val);
                orig.Insert(ind, val);
                if (!check.SequenceEqualIndices(orig))
                    return false;
            }

            return true;
        }
        private static bool remove(IList<int> orig, RandomGenerator gen)
        {
            var check = orig.ToList();

            foreach (int _ in range.Range(orig.Count))
            {
                int ind = gen.Int(orig.Count);

                check.RemoveAt(ind);
                orig.RemoveAt(ind);
                if (!check.SequenceEqualIndices(orig))
                    return false;
            }

            return true;
        }
    }
}
