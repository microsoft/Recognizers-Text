using System;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ICJKSetExtractorConfiguration : IDateTimeOptionsConfiguration
    {
        Regex LastRegex { get; }

        Regex EachPrefixRegex { get; }

        Regex EachUnitRegex { get; }

        Regex UnitRegex { get; }

        Regex EachDayRegex { get; }

        IDateTimeExtractor DurationExtractor { get; }

        IDateTimeExtractor TimeExtractor { get; }

        IDateTimeExtractor DateExtractor { get; }

        IDateTimeExtractor DateTimeExtractor { get; }

        IDateTimeExtractor DatePeriodExtractor { get; }

        IDateTimeExtractor TimePeriodExtractor { get; }

        IDateTimeExtractor DateTimePeriodExtractor { get; }
    }
}