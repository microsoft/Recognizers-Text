using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number
{
    public interface ICJKNumberParserConfiguration : INumberParserConfiguration
    {
        ImmutableDictionary<char, double> ZeroToNineMap { get; }

        ImmutableDictionary<char, long> RoundNumberMapChar { get; }

        ImmutableDictionary<char, char> FullToHalfMap { get; }

        ImmutableDictionary<string, string> UnitMap { get; }

        ImmutableDictionary<char, char> TratoSimMap { get; }

        ImmutableList<char> RoundDirectList { get; }

        Regex FracSplitRegex { get; }

        Regex DigitNumRegex { get; }

        Regex SpeGetNumberRegex { get; }

        Regex PercentageRegex { get; }

        Regex PointRegex { get; }

        Regex DoubleAndRoundRegex { get; }

        Regex PairRegex { get; }

        Regex DozenRegex { get; }

        Regex RoundNumberIntegerRegex { get; }
    }
}
