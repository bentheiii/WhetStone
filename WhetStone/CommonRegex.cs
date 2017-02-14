namespace WhetStone.WordPlay
{
    /// <summary>
    /// A static library containing common regex patterns for general use.
    /// </summary>
    public static class CommonRegex
    {
        /// <summary>
        /// A regex pattern representing a double without a sign. Either decimal (1.618) or exponential (1618e-3)
        /// </summary>
        public const string RegexDoubleNoSign = @"((\d+(\.\d+)?)((e|E)(\+|-)?\d+)?)";
        /// <summary>
        /// A regex pattern representing a double. Either decimal (-1.618) or exponential (-1618e-3)
        /// </summary>
        public const string RegexDouble = @"((\+|-)?" + RegexDoubleNoSign + ")";
    }
}
