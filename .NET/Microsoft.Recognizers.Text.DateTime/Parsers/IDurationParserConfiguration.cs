using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDurationParserConfiguration : IDateTimeOptionsConfiguration
    {
        IExtractor CardinalExtractor { get; }

        IDateTimeExtractor DurationExtractor { get; }

        IParser NumberParser { get; }

        Regex NumberCombinedWithUnit { get; }

        Regex AnUnitRegex { get; }

        Regex PrefixArticleRegex { get; }

        Regex DuringRegex { get; }

        Regex AllDateUnitRegex { get; }

        Regex HalfDateUnitRegex { get; }

        Regex SuffixAndRegex { get; }

        Regex FollowedUnit { get; }

        Regex ConjunctionRegex { get; }

        Regex InexactNumberRegex { get; }

        Regex InexactNumberUnitRegex { get; }

        Regex DurationUnitRegex { get; }

        Regex SpecialNumberUnitRegex { get; }

        bool CheckBothBeforeAfter { get; }

        IImmutableDictionary<string, string> UnitMap { get; }

        IImmutableDictionary<string, long> UnitValueMap { get; }

        IImmutableDictionary<string, double> DoubleNumbers { get; }

    }
}
