// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Korean;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Korean;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Korean
{
    public class KoreanDatePeriodParserConfiguration : BaseDateTimeOptionsConfiguration, ICJKDatePeriodParserConfiguration
    {

        public static readonly Regex WoMLastRegex = new Regex(DateTimeDefinitions.WoMLastRegex, RegexFlags);
        public static readonly Regex WoMPreviousRegex = new Regex(DateTimeDefinitions.WoMPreviousRegex, RegexFlags);
        public static readonly Regex WoMNextRegex = new Regex(DateTimeDefinitions.WoMNextRegex, RegexFlags);

        public static readonly ImmutableDictionary<string, int> MonthOfYear = DateTimeDefinitions.ParserConfigurationMonthOfYear.ToImmutableDictionary();

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public KoreanDatePeriodParserConfiguration(ICJKCommonDateTimeParserConfiguration config)
            : base(config)
        {
            IntegerExtractor = config.IntegerExtractor;
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            DateExtractor = config.DateExtractor;
            DurationExtractor = config.DurationExtractor;
            DurationParser = config.DurationParser;
            DateParser = config.DateParser;

            DynastyYearRegex = KoreanDateExtractorConfiguration.DynastyYearRegex;
            DynastyStartYear = KoreanDateExtractorConfiguration.DynastyStartYear;
            DynastyYearMap = KoreanDateExtractorConfiguration.DynastyYearMap;
            SimpleCasesRegex = KoreanDatePeriodExtractorConfiguration.SimpleCasesRegex;
            ThisRegex = KoreanDatePeriodExtractorConfiguration.ThisRegex;
            NextRegex = KoreanDatePeriodExtractorConfiguration.NextRegex;
            LastRegex = KoreanDatePeriodExtractorConfiguration.LastRegex;
            YearToYear = KoreanDatePeriodExtractorConfiguration.YearToYear;
            YearToYearSuffixRequired = KoreanDatePeriodExtractorConfiguration.YearToYearSuffixRequired;
            YearRegex = KoreanDatePeriodExtractorConfiguration.YearRegex;
            YearInCJKRegex = KoreanDatePeriodExtractorConfiguration.YearInCJKRegex;
            MonthToMonth = KoreanDatePeriodExtractorConfiguration.MonthToMonth;
            MonthToMonthSuffixRequired = KoreanDatePeriodExtractorConfiguration.MonthToMonthSuffixRequired;
            DayToDay = KoreanDatePeriodExtractorConfiguration.DayToDay;
            MonthDayRange = KoreanDatePeriodExtractorConfiguration.MonthDayRange;
            DayRegexForPeriod = KoreanDatePeriodExtractorConfiguration.DayRegexForPeriod;
            MonthRegex = KoreanDatePeriodExtractorConfiguration.MonthRegex;
            SpecialMonthRegex = KoreanDatePeriodExtractorConfiguration.SpecialMonthRegex;
            SpecialYearRegex = KoreanDatePeriodExtractorConfiguration.SpecialYearRegex;
            YearAndMonth = KoreanDatePeriodExtractorConfiguration.YearAndMonth;
            PureNumYearAndMonth = KoreanDatePeriodExtractorConfiguration.PureNumYearAndMonth;
            SimpleYearAndMonth = KoreanDatePeriodExtractorConfiguration.SimpleYearAndMonth;
            OneWordPeriodRegex = KoreanDatePeriodExtractorConfiguration.OneWordPeriodRegex;
            NumberCombinedWithUnit = KoreanDatePeriodExtractorConfiguration.NumberCombinedWithUnit;
            PastRegex = KoreanDatePeriodExtractorConfiguration.PastRegex;
            FutureRegex = KoreanDatePeriodExtractorConfiguration.FutureRegex;
            WeekWithWeekDayRangeRegex = KoreanDatePeriodExtractorConfiguration.WeekWithWeekDayRangeRegex;
            UnitRegex = KoreanDatePeriodExtractorConfiguration.UnitRegex;
            DurationUnitRegex = KoreanDatePeriodExtractorConfiguration.DurationUnitRegex;
            WeekOfMonthRegex = KoreanDatePeriodExtractorConfiguration.WeekOfMonthRegex;
            WeekOfYearRegex = KoreanDatePeriodExtractorConfiguration.WeekOfYearRegex;
            WeekOfDateRegex = KoreanDatePeriodExtractorConfiguration.WeekOfDateRegex;
            MonthOfDateRegex = KoreanDatePeriodExtractorConfiguration.MonthOfDateRegex;
            WhichWeekRegex = KoreanDatePeriodExtractorConfiguration.WhichWeekRegex;
            FirstLastOfYearRegex = KoreanDatePeriodExtractorConfiguration.FirstLastOfYearRegex;
            SeasonWithYear = KoreanDatePeriodExtractorConfiguration.SeasonWithYear;
            QuarterRegex = KoreanDatePeriodExtractorConfiguration.QuarterRegex;
            DecadeRegex = KoreanDatePeriodExtractorConfiguration.DecadeRegex;
            CenturyRegex = KoreanDatePeriodExtractorConfiguration.CenturyRegex;
            RelativeMonthRegex = KoreanDateExtractorConfiguration.RelativeMonthRegex;
            LaterEarlyPeriodRegex = KoreanDatePeriodExtractorConfiguration.LaterEarlyPeriodRegex;
            DatePointWithAgoAndLater = KoreanDatePeriodExtractorConfiguration.DatePointWithAgoAndLater;
            ReferenceDatePeriodRegex = KoreanDatePeriodExtractorConfiguration.ReferenceDatePeriodRegex;
            ComplexDatePeriodRegex = KoreanDatePeriodExtractorConfiguration.ComplexDatePeriodRegex;
            DurationRelativeDurationUnitRegex = KoreanDateExtractorConfiguration.DurationRelativeDurationUnitRegex;
            RelativeRegex = KoreanDateExtractorConfiguration.RelativeRegex;
            UnitMap = DateTimeDefinitions.ParserConfigurationUnitMap.ToImmutableDictionary();
            CardinalMap = DateTimeDefinitions.ParserConfigurationCardinalMap.ToImmutableDictionary();
            DayOfMonth = DateTimeDefinitions.ParserConfigurationDayOfMonth.ToImmutableDictionary();
            SeasonMap = DateTimeDefinitions.ParserConfigurationSeasonMap.ToImmutableDictionary();

        }

        public IDateTimeExtractor DateExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeParser DurationParser { get; }

        public IDateTimeParser DateParser { get; }

        public IExtractor IntegerExtractor { get; }

        public IExtractor CardinalExtractor { get; }

        public IParser NumberParser { get; }

        public ImmutableDictionary<string, int> DynastyYearMap { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IImmutableDictionary<string, int> CardinalMap { get; }

        public IImmutableDictionary<string, int> DayOfMonth { get; }

        IImmutableDictionary<string, int> ICJKDatePeriodParserConfiguration.MonthOfYear => MonthOfYear;

        public IImmutableDictionary<string, string> SeasonMap { get; }

        public string DynastyStartYear { get; }

        public string TokenBeforeDate => string.Empty;

        public Regex DynastyYearRegex { get; }

        public Regex SimpleCasesRegex { get; }

        public Regex ThisRegex { get; }

        public Regex NextRegex { get; }

        public Regex LastRegex { get; }

        public Regex YearToYear { get; }

        public Regex YearToYearSuffixRequired { get; }

        public Regex YearRegex { get; }

        public Regex RelativeRegex { get; }

        public Regex RelativeMonthRegex { get; }

        public Regex LaterEarlyPeriodRegex { get; }

        public Regex DatePointWithAgoAndLater { get; }

        public Regex ReferenceDatePeriodRegex { get; }

        public Regex ComplexDatePeriodRegex { get; }

        public Regex DurationRelativeDurationUnitRegex { get; }

        public Regex YearInCJKRegex { get; }

        public Regex MonthToMonth { get; }

        public Regex MonthToMonthSuffixRequired { get; }

        public Regex MonthRegex { get; }

        public Regex YearAndMonth { get; }

        public Regex PureNumYearAndMonth { get; }

        public Regex OneWordPeriodRegex { get; }

        public Regex NumberCombinedWithUnit { get; }

        public Regex PastRegex { get; }

        public Regex FutureRegex { get; }

        public Regex WeekWithWeekDayRangeRegex { get; }

        public Regex UnitRegex { get; }

        public Regex DurationUnitRegex { get; }

        public Regex WeekOfMonthRegex { get; }

        public Regex WeekOfYearRegex { get; }

        public Regex WeekOfDateRegex { get; }

        public Regex MonthOfDateRegex { get; }

        public Regex WhichWeekRegex { get; }

        public Regex FirstLastOfYearRegex { get; }

        public Regex SeasonWithYear { get; }

        public Regex QuarterRegex { get; }

        public Regex DecadeRegex { get; }

        public Regex CenturyRegex { get; }

        public Regex DayToDay { get; }

        public Regex MonthDayRange { get; }

        public Regex DayRegexForPeriod { get; }

        public Regex SimpleYearAndMonth { get; }

        public Regex SpecialMonthRegex { get; }

        public Regex SpecialYearRegex { get; }

        Regex ICJKDatePeriodParserConfiguration.WoMLastRegex => WoMLastRegex;

        Regex ICJKDatePeriodParserConfiguration.WoMPreviousRegex => WoMPreviousRegex;

        Regex ICJKDatePeriodParserConfiguration.WoMNextRegex => WoMNextRegex;

        public int TwoNumYear => int.Parse(DateTimeDefinitions.TwoNumYear, CultureInfo.InvariantCulture);

        public int ToMonthNumber(string monthStr)
        {
            return MonthOfYear[monthStr] > 12 ? MonthOfYear[monthStr] % 12 : MonthOfYear[monthStr];
        }

        public bool IsMonthOnly(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.MonthTerms.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal));
        }

        public bool IsWeekend(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.WeekendTerms.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal));
        }

        public bool IsWeekOnly(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.WeekTerms.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal));
        }

        public bool IsYearOnly(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.YearTerms.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal));
        }

        public bool IsThisYear(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.ThisYearTerms.Any(o => trimmedText.Equals(o, StringComparison.Ordinal));
        }

        public bool IsYearToDate(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.YearToDateTerms.Any(o => trimmedText.Equals(o, StringComparison.Ordinal));
        }

        public bool IsLastYear(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.LastYearTerms.Any(o => trimmedText.Equals(o, StringComparison.Ordinal));
        }

        public bool IsNextYear(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.NextYearTerms.Any(o => trimmedText.Equals(o, StringComparison.Ordinal));
        }

        public bool IsYearAfterNext(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.YearAfterNextTerms.Any(o => trimmedText.Equals(o, StringComparison.Ordinal));
        }

        public bool IsYearBeforeLast(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.YearBeforeLastTerms.Any(o => trimmedText.Equals(o, StringComparison.Ordinal));
        }

        public int GetSwiftMonth(string text)
        {
            // Current month: 今月
            var value = 0;

            // @TODO move hardcoded values to resources file

            if (text.Equals("来月", StringComparison.Ordinal))
            {
                value = 1;
            }
            else if (text.Equals("前月", StringComparison.Ordinal) ||
                     text.Equals("先月", StringComparison.Ordinal) ||
                     text.Equals("昨月", StringComparison.Ordinal) ||
                     text.Equals("先々月", StringComparison.Ordinal))
            {
                value = -1;
            }
            else if (text.Equals("再来月", StringComparison.Ordinal))
            {
                value = 2;
            }

            return value;
        }

        public int GetSwiftYear(string text)
        {
            // Current year: 今年
            var value = 0;

            // @TODO move hardcoded values to resources file

            if (text.Equals("来年", StringComparison.Ordinal) ||
                text.Equals("らいねん", StringComparison.Ordinal))
            {
                value = 1;
            }
            else if (text.Equals("昨年", StringComparison.Ordinal) ||
                     text.Equals("前年", StringComparison.Ordinal))
            {
                value = -1;
            }

            return value;
        }
    }
}