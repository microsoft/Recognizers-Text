using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDurationExtractorConfiguration : IOptionsConfiguration
    {
        Regex FollowedUnit { get; }

        Regex NumberCombinedWithUnit { get; }

        Regex AnUnitRegex { get; }

        Regex DuringRegex { get; }

        Regex AllRegex { get; }

        Regex HalfRegex { get; }

        Regex SuffixAndRegex { get; }

        Regex ConjunctionRegex { get; }

        Regex InexactNumberRegex { get; }

        Regex InexactNumberUnitRegex { get; }

        Regex RelativeDurationUnitRegex { get; }

        Regex DurationUnitRegex { get; }

        Regex DurationConnectorRegex { get; }

        Regex LessThanRegex { get; }

        Regex MoreThanRegex { get; }

        IExtractor CardinalExtractor { get; }

        IImmutableDictionary<string, string> UnitMap { get; }

        IImmutableDictionary<string, long> UnitValueMap { get; }
    }
}