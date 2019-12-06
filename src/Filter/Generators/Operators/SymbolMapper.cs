namespace Panner.Filter
{
    using System.Collections.Generic;
    using System.Linq;

    public static class SymbolMapper
    {
        private static readonly Dictionary<string, Operator> map = new Dictionary<string, Operator>()
        {
            { "=", Operator.Equal },
            { "!=", Operator.NotEquals },
            { ">", Operator.GreaterThan },
            { "<", Operator.LessThan },
            { "%=%", Operator.Contains },
            { "!%=%", Operator.DoesNotContain },
        };

        public static IEnumerable<string> Symbols => map.Keys.OrderByDescending(x => x.Length);

        public static Operator Map(string symbol) => map[symbol];
    }


}
