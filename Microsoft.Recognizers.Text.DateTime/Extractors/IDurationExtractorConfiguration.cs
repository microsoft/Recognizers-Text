using System.Text.RegularExpressions;

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

        IExtractor CardinalExtractor { get; }
    }
}