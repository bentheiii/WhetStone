using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Comparison;
using WhetStone.Dictionaries;
using WhetStone.Guard;
using WhetStone.Looping;

namespace WhetStone.Tries
{
    public static class toSuffixTrie
    {
        public static Trie<int, int> ToSuffixTrie<T>(this IEnumerable<T> @this, out IDictionary<T, int> queryconverter)
        {
            var ret = new Trie<int, int>(tokencomp: new EqualityFunctionComparer<int>((a, b) => a != -1 && b != -1 && @this.ElementAt(a).Equals(@this.ElementAt(b)), a => a.GetHashCode()));
            var count = @this.Count();
            var iter = @this.GetEnumerator();
            queryconverter = new Dictionary<T, int>();
            IGuard<int> ind = new Guard<int>();
            foreach (T f in @this.CountBind().Detach(ind))
            {
                if (!queryconverter.ContainsKey(f))
                    queryconverter[f] = ind.value;
                ret.Add(range.Range(ind.value, count, 1), ind.value);
            }
            return ret;
        }
        public static IEnumerable<int> ToSuffixTrieQuery<T>(this IEnumerable<T> @this, IDictionary<T, int> queryconverter)
        {
            return @this.Select(a => queryconverter.ValueOrDefault(a, -1));
        }
    }
}
