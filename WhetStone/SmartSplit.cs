using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Arrays;

namespace WhetStone.WordPlay
{
    public static class smartSplit
    {
        public static string[] SmartSplit(this string @this, string divisor, string opener, string closer)
        {
            if (!@this.Balanced(opener, closer, 1))
                throw new ArgumentException("string is not balanced");
            ResizingArray<string> ret = new ResizingArray<string>();
            while (@this.StartsWith(opener))
            {
                ret.Add("");
                @this = @this.Substring(opener.Length);
            }
            int divindex = -2;
            int openerindex = -2;
            while (@this.Length != 0)
            {
                if (@this.StartsWith(opener))
                {
                    @this = @this.Substring(opener.Length);
                    int closerind = @this.IndexOf(closer);
                    ret.Add(@this.Substring(0, closerind));
                    @this = @this.Substring(closerind + closer.Length);
                    continue;
                }
                if (divindex <= -2)
                    divindex = @this.IndexOf(divisor);
                if (openerindex <= -2)
                    openerindex = @this.IndexOf(opener);
                if (divindex == -1 && openerindex == -1)
                {
                    ret.Add(@this);
                    break;
                }
                if (divindex == -1 || (openerindex != -1 && openerindex < divindex))
                {
                    ret.Add(@this.Substring(0, openerindex));
                    @this = @this.Substring(openerindex);
                    divindex -= openerindex;
                    openerindex = -2;
                }
                else
                {
                    ret.Add(@this.Substring(0, divindex));
                    @this = @this.Substring(divisor.Length + divindex);
                    openerindex -= (divindex + divisor.Length);
                    divindex = -2;
                }
            }
            return ret;
        }
    }
}
