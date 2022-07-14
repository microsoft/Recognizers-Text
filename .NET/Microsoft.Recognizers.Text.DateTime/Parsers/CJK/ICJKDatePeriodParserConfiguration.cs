// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ICJKDatePeriodParserConfiguration : IDateTimeOptionsConfiguration
    {
        IExtractor IntegerExtractor { get; }

        IParser NumberParser { get; }

        IDateTimeExtractor DateExtractor { get; }

        IDateTimeExtractor DurationExtractor { get; }

        IExtractor CardinalExtractor { get; }

        IDateTimeParser DurationParser { get; }

        IDateTimeParser DateParser { get; }

        ImmutableDictionary<string, int> DynastyYearMap { get; }

        IImmutableDictionary<string, string> UnitMap { get; }

        IImmutableDictionary<string, int> CardinalMap { get; }

        IImmutableDictionary<string, int> DayOfMonth { get; }

        IImmutableDictionary<string, int> MonthOfYear { get; }

        IImmutableDictionary<string, string> SeasonMap { get; }

        string DynastyStartYear { get; }

        string TokenBeforeDate { get; }

        int TwoNumYear { get; }

        Regex SimpleCasesRegex { get; }

        Regex DynastyYearRegex { get; }

        Regex YearRegex { get; }

        Regex RelativeRegex { get; }

        Regex RelativeMonthRegex { get; }

        Regex DurationRelativeDurationUnitRegex { get; }

        Regex ThisRegex { get; }

        Regex LastRegex { get; }

        Regex NextRegex { get; }

        Regex YearToYear { get; }

        Regex YearToYearSuffixRequired { get; }

        Regex YearInCJKRegex { get; }

        Regex MonthToMonth { get; }

        Regex MonthToMonthSuffixRequired { get; }

        Regex MonthRegex { get; }

        Regex YearAndMonth { get; }

        Regex PureNumYearAndMonth { get; }

        Regex OneWordPeriodRegex { get; }

        Regex NumberCombinedWithUnit { get; }

        Regex PastRegex { get; }

        Regex FutureRegex { get; }

        Regex WeekWithWeekDayRangeRegex { get; }

        Regex UnitRegex { get; }

        Regex DurationUnitRegex { get; }

        Regex WeekOfMonthRegex { get; }

        Regex WeekOfYearRegex { get; }

        Regex WeekOfDateRegex { get; }

        Regex MonthOfDateRegex { get; }

        Regex WhichWeekRegex { get; }

        Regex FirstLastOfYearRegex { get; }

        Regex SeasonWithYear { get; }

        Regex QuarterRegex { get; }

        Regex DecadeRegex { get; }

        Regex CenturyRegex { get; }

        Regex DayToDay { get; }

        Regex MonthDayRange { get; }

        Regex DayRegexForPeriod { get; }

        Regex SimpleYearAndMonth { get; }

        Regex SpecialMonthRegex { get; }

        Regex SpecialYearRegex { get; }

        public Regex LaterEarlyPeriodRegex { get; }

        public Regex DatePointWithAgoAndLater { get; }

        public Regex ReferenceDatePeriodRegex { get; }

        Regex WoMLastRegex { get; }

        Regex WoMPreviousRegex { get; }

        Regex WoMNextRegex { get; }

        Regex ComplexDatePeriodRegex { get; }

        int ToMonthNumber(string monthStr);

        bool IsMonthOnly(string text);

        bool IsWeekend(string text);

        bool IsWeekOnly(string text);

        bool IsYearOnly(string text);

        bool IsThisYear(string text);

        bool IsYearToDate(string text);

        bool IsLastYear(string text);

        bool IsNextYear(string text);

        bool IsYearAfterNext(string text);

        bool IsYearBeforeLast(string text);

        int GetSwiftMonth(string text);

        int GetSwiftYear(string text);
    }
}
