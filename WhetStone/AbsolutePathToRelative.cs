using System;
using System.Collections.Generic;
using System.Linq;
using WhetStone.Looping;

namespace WhetStone.Path
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class absolutePathToRelative
    {
        ///<summary>
        ///Returns the <paramref name="destination"/> path as a relative path, relative to <paramref name="origin"/>
        ///</summary>
        ///<param name="origin">The "working directory", from which, if you follow the return value, you will reach <paramref name="destination"/></param>
        ///<param name="destination">The destination to reach from <paramref name="origin"/></param>
        ///<returns>The relative path</returns>
        public static string AbsolutePathToRelative(string origin, string destination)
        {
            if (origin.Substring(1, 2) != @":\")
                throw new Exception("argument is not an absolute path!");
            if (destination.Substring(1, 2) != @":\")
                throw new Exception("argument is not an absolute path!");
            origin = origin.Remove(1, 1);
            destination = destination.Remove(1, 1);
            IList<string> osplit = origin.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            IList<string> dsplit = destination.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            while (osplit[0].Equals(dsplit[0]))
            {
                osplit = osplit.Skip(1);
                dsplit = dsplit.Skip(1);
                if (!(osplit.Any() && dsplit.Any()))
                    break;
            }
            IList<string> ret = osplit.Select(a => "..").Concat(dsplit);
            return ret.StrConcat("\\");
        }
    }
}
