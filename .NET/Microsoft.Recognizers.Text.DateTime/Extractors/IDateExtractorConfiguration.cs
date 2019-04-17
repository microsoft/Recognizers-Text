using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateExtractorConfiguration : IOptionsConfiguration
    {
        IEnumerable<Regex> DateRegexList { get; }

        IEnumerable<Regex> ImplicitDateList { get; }

        Regex OfMonth { get; }

        Regex MonthEnd { get; }

        Regex WeekDayEnd { get; }

        Regex DateUnitRegex { get; }

        Regex ForTheRegex { get; }

        Regex WeekDayAndDayOfMonthRegex { get; }

        Regex WeekDayAndDayRegex { get; }

        Regex RelativeMonthRegex { get; }

        Regex WeekDayRegex { get; }

        Regex PrefixArticleRegex { get; }

        Regex YearSuffix { get; }

        Regex MoreThanRegex { get; }

        Regex LessThanRegex { get; }

        Regex InConnectorRegex { get; }

        Regex SinceYearSuffixRegex { get; }

        Regex RangeUnitRegex { get; }

        Regex RangeConnectorSymbolRegex { get; }

        IExtractor IntegerExtractor { get; }

        IExtractor OrdinalExtractor { get; }

        IParser NumberParser { get; }

        IDateTimeExtractor DurationExtractor { get; }

        IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        IImmutableDictionary<string, int> DayOfWeek { get; }

        IImmutableDictionary<string, int> MonthOfYear { get; }
    }
}