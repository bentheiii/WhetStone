using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhetStone.Looping;
using WhetStone.Random;

namespace Tests
{
    internal static class MutableListCheck
    {
        public static void check(IList<bool> orig, int? seed = null, bool randomSet = true, bool append = true, bool randominsert = true, bool randomRemove = true)
        {
            check(orig, a => a.Bool(), seed, randomSet, append, randominsert, randomRemove);
        }
        public static void check(IList<int> orig, int? seed = null, bool randomSet = true, bool append = true, bool randominsert = true, bool randomRemove = true)
        {
            check(orig, a=>a.Int(), seed, randomSet, append, randominsert, randomRemove);
        }
        public static void check<T>(IList<T> orig, Func<RandomGenerator, T> create, int? seed = null, bool randomSet = true, bool append = true, bool randominsert = true, bool randomRemove = true, bool wildAction = true)
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
                if(!set(orig, gen, create))
                    throw new AssertFailedException("setrandom failed, seed: "+realseed);
            }
            if (append)
            {
                if (!add(orig, gen, create))
                    throw new AssertFailedException("add failed, seed: " + realseed);
            }
            if (randominsert)
            {
                if (!insert(orig, gen, create))
                    throw new AssertFailedException("insert failed, seed: " + realseed);
            }
            if (wildAction)
            {
                if (!wild(orig,gen, create))
                    throw new AssertFailedException("wild failed, seed: " + realseed);
            }
            if (randomRemove)
            {
                if (!remove(orig, gen))
                    throw new AssertFailedException("remove failed, seed: " + realseed);
            }
        }
        private static bool set<T>(IList<T> orig, RandomGenerator gen, Func<RandomGenerator, T> createval)
        {
            var check = orig.ToList();

            foreach (int _ in range.Range(100))
            {
                var val = createval(gen);
                int ind = gen.Int(orig.Count);

                check[ind] = val;
                orig[ind] = val;
                if (!check.SequenceEqualIndices(orig))
                    return false;
            }

            return true;
        }
        private static bool add<T>(IList<T> orig, RandomGenerator gen, Func<RandomGenerator, T> create)
        {
            var check = orig.ToList();

            foreach (int _ in range.Range(100))
            {
                var val = create(gen);

                check.Add(val);
                orig.Add(val);
                if (!check.SequenceEqualIndices(orig))
                    return false;
            }

            return true;
        }
        private static bool insert<T>(IList<T> orig, RandomGenerator gen, Func<RandomGenerator, T> create)
        {
            var check = orig.ToList();

            foreach (int _ in range.Range(100))
            {
                var val = create(gen);
                int ind = gen.Int(orig.Count);

                check.Insert(ind, val);
                orig.Insert(ind, val);
                if (!check.SequenceEqualIndices(orig))
                    return false;
            }

            return true;
        }
        private static bool remove<T>(IList<T> orig, RandomGenerator gen)
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
        private static bool wild<T>(IList<T> orig, RandomGenerator gen, Func<RandomGenerator, T> create)
        {
            var check = orig.ToList();

            foreach (int _ in range.Range(100))
            {
                var val = create(gen);
                int ind = gen.Int(orig.Count);

                Action<IList<T>> act;

                var actKind = gen.Int(4);

                switch (actKind)
                {
                    case 0:
                        act = x => x[ind] = val;
                        break;
                    case 1:
                        act = x => x.Add(val);
                        break;
                    case 2:
                        act = x => x.Insert(ind, val);
                        break;
                    case 3:
                        act = x => x.RemoveAt(ind);
                        break;
                    default:
                        throw new Exception();
                }

                act(check);
                act(orig);
                if (!check.SequenceEqualIndices(orig))
                    return false;
            }

            return true;
        }
    }
}
