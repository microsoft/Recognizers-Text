using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDurationExtractorConfiguration
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

        IExtractor CardinalExtractor { get; }
    }
}