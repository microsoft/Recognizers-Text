using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDatePeriodParserConfiguration : IOptionsConfiguration
    {
        string TokenBeforeDate { get; }

        IDateTimeExtractor DateExtractor { get; }

        IExtractor CardinalExtractor { get; }

        IExtractor OrdinalExtractor { get; }

        IExtractor IntegerExtractor { get; }

        IParser NumberParser { get; }

        IDateTimeExtractor DurationExtractor { get; }

        IDateTimeParser DurationParser { get; }

        IDateTimeParser DateParser { get; }

        Regex MonthFrontBetweenRegex { get; }

        Regex BetweenRegex { get; }

        Regex MonthFrontSimpleCasesRegex { get; }

        Regex SimpleCasesRegex { get; }

        Regex OneWordPeriodRegex { get; }

        Regex MonthWithYear { get; }

        Regex MonthNumWithYear { get; }

        Regex YearRegex { get; }

        Regex PastRegex { get; }

        Regex FutureRegex { get; }

        Regex FutureSuffixRegex { get; }

        Regex NumberCombinedWithUnit { get; }

        Regex WeekOfMonthRegex { get; }

        Regex WeekOfYearRegex { get; }

        Regex QuarterRegex { get; }

        Regex QuarterRegexYearFront { get; }

        Regex AllHalfYearRegex { get; }

        Regex SeasonRegex { get; }

        Regex WhichWeekRegex { get; }

        Regex WeekOfRegex { get; }

        Regex MonthOfRegex { get; }

        Regex InConnectorRegex { get; }

        Regex WithinNextPrefixRegex { get; }

        Regex NextPrefixRegex { get; }

        Regex PastPrefixRegex { get; }

        Regex ThisPrefixRegex { get; }

        Regex RestOfDateRegex { get; }

        Regex LaterEarlyPeriodRegex { get; }

        Regex WeekWithWeekDayRangeRegex { get; }

        Regex YearPlusNumberRegex { get; }

        Regex DecadeWithCenturyRegex { get; }

        Regex YearPeriodRegex { get; }

        Regex ComplexDatePeriodRegex { get; }

        Regex RelativeDecadeRegex { get; }

        Regex ReferenceDatePeriodRegex { get; }

        Regex AgoRegex { get; }

        Regex LaterRegex { get; }

        Regex LessThanRegex { get; }

        Regex MoreThanRegex { get; }

        Regex CenturySuffixRegex { get; }

        IImmutableDictionary<string, string> UnitMap { get; }

        IImmutableDictionary<string, int> CardinalMap { get; }

        IImmutableDictionary<string, int> DayOfMonth { get; }

        IImmutableDictionary<string, int> MonthOfYear { get; }

        IImmutableDictionary<string, string> SeasonMap { get; }

        IImmutableDictionary<string, int> WrittenDecades { get; }

        IImmutableDictionary<string, int> Numbers { get; }

        IImmutableDictionary<string, int> SpecialDecadeCases { get; }

        bool IsFuture(string text);

        bool IsYearToDate(string text);

        bool IsMonthToDate(string text);

        bool IsWeekOnly(string text);

        bool IsWeekend(string text);

        bool IsMonthOnly(string text);

        bool IsYearOnly(string text);

        int GetSwiftYear(string text);

        int GetSwiftDayOrMonth(string text);

        bool IsLastCardinal(string text);
    }
}
