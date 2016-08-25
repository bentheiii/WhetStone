using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Arrays;

namespace WhetStone.Arrays
{
    public static class str2DConcat
    {
        public static string Str2DConcat<T>(this T[,] arr, string openerfirst = "/", string openermid = "|", string openerlast = @"\",
                                                     string closerfirst = @"\", string closermid = "|", string closerlast = "/", string divider = " ",
                                                     string linediv = null)
        {
            linediv = linediv ?? Environment.NewLine;
            var cols = arr.Collumns();
            int[] collengths = cols.Select(a => a.Max(x => x.ToString().Length)).ToArray();
            StringBuilder ret = new StringBuilder();
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                string opener = openermid;
                if (i == 0)
                    opener = openerfirst;
                if (i == arr.GetLength(0) - 1)
                    opener = openerlast;
                ret.Append(opener);
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    if (j > 0)
                        ret.Append(divider);
                    ret.Append(arr[i, j].ToString().PadLeft(collengths[j]));
                }
                string closer = closermid;
                if (i == 0)
                    closer = closerfirst;
                if (i == arr.GetLength(0) - 1)
                    closer = closerlast;
                ret.Append(closer + linediv);
            }
            return ret.ToString();
        }
        public static string Str2DConcat<T>(this IEnumerable<IEnumerable<T>> @this, string instanceopener = "[", string instancecloser = "]",
            string instancediv = ", ", string linediv = null)
        {
            linediv = linediv ?? Environment.NewLine;
            return @this.Select(a => instanceopener + a.StrConcat(instancediv) + instancecloser).StrConcat(linediv);
        }
    }
}
