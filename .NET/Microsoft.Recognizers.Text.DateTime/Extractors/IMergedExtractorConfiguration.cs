using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IMergedExtractorConfiguration
    {
        IDateTimeExtractor DateExtractor { get; }

        IDateTimeExtractor TimeExtractor { get; }

        IDateTimeExtractor DateTimeExtractor { get; }

        IDateTimeExtractor DatePeriodExtractor { get; }

        IDateTimeExtractor TimePeriodExtractor { get; }

        IDateTimeExtractor DateTimePeriodExtractor { get; }

        IDateTimeExtractor DurationExtractor { get; }

        IDateTimeExtractor SetExtractor { get; }

        IDateTimeExtractor HolidayExtractor { get; }

        IExtractor IntegerExtractor { get; }

        IEnumerable<Regex> FilterWordRegexList { get; }

        Regex AfterRegex { get; }

        Regex BeforeRegex { get; }

        Regex SinceRegex { get; }

        Regex FromToRegex { get; }

        Regex SingleAmbiguousMonthRegex { get; }

        Regex PrepositionSuffixRegex { get; }

        Regex NumberEndingPattern { get; }

    }
}