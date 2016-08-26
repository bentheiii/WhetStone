namespace WhetStone.WordPlay
{
    public static class CommonRegex
    {
        public const string RegexDoubleNoSign = @"((\d+(\.\d+)?)((e|E)(\+|-)?\d+)?))";
        public const string RegexDouble = @"((\+|-)?" + RegexDoubleNoSign;
    }
}
