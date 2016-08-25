using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Fielding;

namespace WhetStone.NumbersMagic
{
    public static class isWithin
    {
        public static bool iswithinexclusive(this int x, int border1, int border2)
        {
            double min;
            double max;
            if (border1 < border2)
            {
                min = border1;
                max = border2;
            }
            else
            {
                min = border2;
                max = border1;
            }
            return x > min && x < max;
        }
        public static bool iswithin(this int x, int border1, int border2)
        {
            return iswithinexclusive(x, border1, border2) || x == border1 || x == border2;
        }
        public static bool iswithin<T>(this T x, T border1, T border2)
        {
            return iswithinexclusive(x, border1, border2) || x.Equals(border1) || x.Equals(border2);
        }
        public static bool iswithinPartialExclusive(this int x, int border1, int border2)
        {
            return iswithinexclusive(x, border1, border2) || (x == border1 && x != border2);
        }
        public static bool iswithinPartialExclusive(this double x, double border1, double border2)
        {
            return iswithinexclusive(x, border1, border2) || (x == border1 && x != border2);
        }
        public static bool iswithinPartialExclusive(this float x, float border1, float border2)
        {
            return iswithinexclusive(x, border1, border2) || (x == border1 && x != border2);
        }
        public static bool iswithinPartialExclusive<T>(this T x, T border1, T border2)
        {
            return iswithinexclusive(x, border1, border2) || (x.Equals(border1) && !x.Equals(border2));
        }
        public static bool iswithin(this double x, double border1, double border2)
        {
            return iswithinexclusive(x, border1, border2) || x == border1 || x == border2;
        }
        public static bool iswithin(this float x, float border1, float border2)
        {
            return iswithinexclusive(x, border1, border2) || x == border1 || x == border2;
        }
        public static bool iswithinexclusive(this double x, double border1, double border2)
        {
            minmax.MinMax(ref border1, ref border2);
            return x > border1 && x < border2;
        }
        public static bool iswithinexclusive(this float x, float border1, float border2)
        {
            minmax.MinMax(ref border1, ref border2);
            return x > border1 && x < border2;
        }
        public static bool iswithinexclusive<T>(this T x, T border1, T border2)
        {
            minmax.MinMax(ref border1, ref border2);
            return x.ToFieldWrapper() > border1 && x.ToFieldWrapper() < border2;
        }
    }
}
