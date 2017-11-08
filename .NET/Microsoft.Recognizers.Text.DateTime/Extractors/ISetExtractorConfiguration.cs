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

        Regex SetWeekDayRegex { get; }

        Regex SetEachRegex { get; }

        IDateTimeExtractor DurationExtractor { get; }

        IDateTimeExtractor TimeExtractor { get; }

        IDateTimeExtractor DateExtractor { get; }

        IDateTimeExtractor DateTimeExtractor { get; }

        IDateTimeExtractor DatePeriodExtractor { get; }

        IDateTimeExtractor TimePeriodExtractor { get; }

        IDateTimeExtractor DateTimePeriodExtractor { get; }
        
    }
}