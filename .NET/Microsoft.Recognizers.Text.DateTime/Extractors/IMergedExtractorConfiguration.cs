using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IMergedExtractorConfiguration
    {
        IExtractor DateExtractor { get; }

        IExtractor TimeExtractor { get; }

        IExtractor DateTimeExtractor { get; }

        IExtractor DatePeriodExtractor { get; }

        IExtractor TimePeriodExtractor { get; }

        IExtractor DateTimePeriodExtractor { get; }

        IExtractor DurationExtractor { get; }

        IExtractor SetExtractor { get; }

        IExtractor HolidayExtractor { get; }

        IExtractor IntegerExtractor { get; }

        Regex AfterRegex { get; }

        Regex BeforeRegex { get; }

        Regex SinceRegex { get; }

        Regex FromToRegex { get; }

        Regex SingleAmbiguousMonthRegex { get; }

        Regex PrepositionSuffixRegex { get; }

        Regex NumberEndingPattern { get; }

    }
}