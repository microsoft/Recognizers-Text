using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IMergedExtractorConfiguration : IOptionsConfiguration
    {
        IDateExtractor DateExtractor { get; }

        IDateTimeExtractor TimeExtractor { get; }

        IDateTimeExtractor DateTimeExtractor { get; }

        IDateTimeExtractor DatePeriodExtractor { get; }

        IDateTimeExtractor TimePeriodExtractor { get; }

        IDateTimeExtractor DateTimePeriodExtractor { get; }

        IDateTimeExtractor DurationExtractor { get; }

        IDateTimeExtractor SetExtractor { get; }

        IDateTimeExtractor HolidayExtractor { get; }

        IDateTimeZoneExtractor TimeZoneExtractor { get; }

        IDateTimeListExtractor DateTimeAltExtractor { get; }

        IExtractor IntegerExtractor { get; }

        IEnumerable<Regex> TermFilterRegexes { get; }

        Regex AfterRegex { get; }

        Regex BeforeRegex { get; }

        Regex SinceRegex { get; }

        Regex AroundRegex { get; }

        Regex FromToRegex { get; }

        Regex SingleAmbiguousMonthRegex { get; }

        Regex PrepositionSuffixRegex { get; }

        Regex NumberEndingPattern { get; }

        Regex SuffixAfterRegex { get; }

        Regex UnspecificDatePeriodRegex { get; }

        StringMatcher SuperfluousWordMatcher { get; }

        Dictionary<Regex, Regex> AmbiguityFiltersDict { get; }
    }
}