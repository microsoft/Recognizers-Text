using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDurationExtractorConfiguration
    {
        Regex FollowedUnit { get; }
        Regex NumberCombinedWithUnit { get; }
        Regex AnUnitRegex { get; }
        Regex AllRegex { get; }

        IExtractor CardinalExtractor { get; }
    }
}