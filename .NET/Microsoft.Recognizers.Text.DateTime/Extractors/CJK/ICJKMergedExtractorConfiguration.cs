using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ICJKMergedExtractorConfiguration : IDateTimeOptionsConfiguration
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

        Regex AfterRegex { get; }

        Regex BeforeRegex { get; }

        Regex SinceSuffixRegex { get; }

        Regex SincePrefixRegex { get; }

        Regex UntilRegex { get; }

        Regex EqualRegex { get; }

        Regex PotentialAmbiguousRangeRegex { get; }

        Regex AmbiguousRangeModifierPrefix { get; }

        Dictionary<Regex, Regex> AmbiguityFiltersDict { get; }
    }
}