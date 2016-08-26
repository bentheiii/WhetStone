using System.Collections.Generic;
using System.Linq;

namespace WhetStone.Looping
{
    public static class switchUnbound
    {
        public static IEnumerable<T> SwitchUnbound<T>(this IEnumerable<IEnumerable<T>> @this, T def = default(T))
        {
            var numerators = @this.Select(a => a.GetEnumerator()).ToArray();
            var buffer = new List<T>(numerators.Length);
            while (numerators.Any(a => a != null))
            {
                for (int i = 0; i < numerators.Length; i++)
                {
                    IEnumerator<T> e = numerators[i];
                    if (e != null && e.MoveNext())
                        buffer.Add(e.Current);
                    else
                    {
                        numerators[i] = null;
                        buffer.Add(def);
                    }
                }
                if (numerators.Any(a => a != null))
                {
                    foreach (T t in buffer)
                    {
                        yield return t;
                    }
                    buffer.Clear();
                }
            }
        }
    }
}
