using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Number;
using System.Collections.Immutable;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDurationExtractorConfiguration : IOptionsConfiguration
    {
        Regex FollowedUnit { get; }

        Regex NumberCombinedWithUnit { get; }

        Regex AnUnitRegex { get; }

        Regex AllRegex { get; }

        Regex HalfRegex { get; }

        Regex SuffixAndRegex { get; }

        Regex ConjunctionRegex { get; }

        Regex InExactNumberRegex { get; }

        Regex InExactNumberUnitRegex { get; }

        Regex RelativeDurationUnitRegex { get; }

        Regex DurationUnitRegex { get; }

        Regex DurationConnectorRegex { get; }

        IExtractor CardinalExtractor { get; }

        IImmutableDictionary<string, string> UnitMap { get; }

        IImmutableDictionary<string, long> UnitValueMap { get; }
    }
}