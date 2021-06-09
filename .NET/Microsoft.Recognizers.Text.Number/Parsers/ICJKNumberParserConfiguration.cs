using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number
{
    public interface ICJKNumberParserConfiguration : INumberParserConfiguration
    {
        ImmutableDictionary<char, double> ZeroToNineMap { get; }

        ImmutableDictionary<char, long> RoundNumberMapChar { get; }

        ImmutableDictionary<char, char> FullToHalfMap { get; }

        // A UnitMap sorted by key length is required to ensure the correct replacement order.
        ImmutableSortedDictionary<string, string> UnitMap { get; }

        ImmutableDictionary<char, char> TratoSimMap { get; }

        ImmutableList<char> RoundDirectList { get; }

        ImmutableList<char> TenChars { get; }

        Regex FracSplitRegex { get; }

        Regex DigitNumRegex { get; }

        Regex SpeGetNumberRegex { get; }

        Regex PercentageRegex { get; }

        Regex PercentageNumRegex { get; }

        Regex PointRegex { get; }

        Regex DoubleAndRoundRegex { get; }

        Regex PairRegex { get; }

        Regex DozenRegex { get; }

        Regex RoundNumberIntegerRegex { get; }

        char ZeroChar { get; }

        char PairChar { get; }
    }
}
