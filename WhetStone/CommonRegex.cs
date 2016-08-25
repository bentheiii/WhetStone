using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.WordPlay
{
    public static class commonRegex
    {
        public const string RegexDoubleNoSign = @"((\d+(\.\d+)?)((e|E)(\+|-)?\d+)?))";
        public const string RegexDouble = @"((\+|-)?" + RegexDoubleNoSign;
    }
}
