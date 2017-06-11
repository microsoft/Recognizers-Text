using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDatePeriodParserConfiguration
    {
        string TokenBeforeDate { get; }

        IExtractor DateExtractor { get; }
        IExtractor CardinalExtractor { get; }
        IParser NumberParser { get; }
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
        Regex NumberCombinedWithUnit { get; }
        Regex WeekOfMonthRegex { get; }
        Regex WeekOfYearRegex { get; }
        Regex QuarterRegex { get; }
        Regex QuarterRegexYearFront { get; }
        Regex SeasonRegex { get; }

        IImmutableDictionary<string, string> UnitMap { get; }
        IImmutableDictionary<string, int> CardinalMap { get; }
        IImmutableDictionary<string, int> DayOfMonth { get; }
        IImmutableDictionary<string, int> MonthOfYear { get; }
        IImmutableDictionary<string, string> SeasonMap { get; }

        int GetSwiftMonth(string text);
        bool IsFuture(string text);
        bool IsYearToDate(string text);
        bool IsMonthToDate(string text);
        bool IsWeekOnly(string text);
        bool IsWeekend(string text);
        bool IsMonthOnly(string text);
        bool IsYearOnly(string text);
        int GetSwiftYear(string text);
        int GetSwiftDay(string text);
        bool IsLastCardinal(string text);
    }
}
