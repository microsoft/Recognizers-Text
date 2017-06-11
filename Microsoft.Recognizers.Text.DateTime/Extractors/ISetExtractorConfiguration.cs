using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ISetExtractorConfiguration
    {
        Regex LastRegex { get; }
        Regex EachPrefixRegex { get; }
        Regex PeriodicRegex { get; }
        Regex EachUnitRegex { get; }
        Regex EachDayRegex { get; }
        Regex BeforeEachDayRegex { get; }

        IExtractor DurationExtractor { get; }
        IExtractor TimeExtractor { get; }
        IExtractor DateExtractor { get; }
        IExtractor DateTimeExtractor { get; }
    }
}