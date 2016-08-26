using System.Collections.Generic;
using System.Text;

namespace WhetStone.Looping
{
    public static class strConcat
    {
        public static string StrConcat<T>(this IEnumerable<T> a, string seperator = ", ")
        {
            StringBuilder b = new StringBuilder();
            bool first = true;
            foreach (T t in a)
            {
                if (!first)
                    b.Append(seperator);
                else
                    first = false;
                b.Append(t);
            }
            return b.ToString();
        }
        public static string StrConcat<K, V>(this IDictionary<K, V> a, string definitionSeperator = ":", string seperator = ", ")
        {
            return a.Select(x => x.Key.ToString() + definitionSeperator + x.Value.ToString()).StrConcat(seperator);
        }
    }
}
