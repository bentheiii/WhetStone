using System;
using System.Collections.Generic;

namespace WhetStone.Looping
{
    public static class enumerationHook
    {
        public static IEnumerable<T> EnumerationHook<T>(this IEnumerable<T> @this, Action<T> preYield = null, Action<T> postYield = null)
        {
            foreach (var t in @this)
            {
                preYield?.Invoke(t);
                yield return t;
                postYield?.Invoke(t);
            }
        }
        //if a function returns false, that function will not be called again
        public static IEnumerable<T> EnumerationHook<T>(this IEnumerable<T> @this, Func<T,bool> preYield = null, Func<T,bool> postYield = null)
        {
            using (var tor = @this.GetEnumerator())
            {
                //phase 0, both active
                if (preYield != null && postYield != null)
                {
                    while (tor.MoveNext())
                    {
                        var t = tor.Current;
                        var anybreak = false;
                        if (!preYield.Invoke(t))
                        {
                            anybreak = true;
                            preYield = null;
                        }
                        yield return t;
                        if (!postYield.Invoke(t))
                        {
                            anybreak = true;
                            postYield = null;
                        }
                        if (anybreak)
                            break;
                    }
                }
                //phase 1, pre-active
                if (preYield != null)
                {
                    while (tor.MoveNext())
                    {
                        var t = tor.Current;
                        bool anybreak = !preYield.Invoke(t);
                        yield return t;
                        if (anybreak)
                            break;
                    }
                }
                //phase 2, post-active (will not be accessed if phase 1 was accessed)
                else if (postYield != null)
                {
                    while (tor.MoveNext())
                    {
                        var t = tor.Current;
                        yield return t;
                        if (!postYield.Invoke(t))
                            break;
                    }
                }
                //phase 3, none active
                while (tor.MoveNext())
                {
                    var t = tor.Current;
                    yield return t;
                }
            }
        }

    }
}
