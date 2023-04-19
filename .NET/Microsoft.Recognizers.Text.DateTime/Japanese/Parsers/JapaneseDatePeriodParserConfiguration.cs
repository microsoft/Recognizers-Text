﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Japanese;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Japanese;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseDatePeriodParserConfiguration : BaseDateTimeOptionsConfiguration, ICJKDatePeriodParserConfiguration
    {

        public static readonly Regex WoMLastRegex = new Regex(DateTimeDefinitions.WoMLastRegex, RegexFlags, RegexTimeOut);
        public static readonly Regex WoMPreviousRegex = new Regex(DateTimeDefinitions.WoMPreviousRegex, RegexFlags, RegexTimeOut);
        public static readonly Regex WoMNextRegex = new Regex(DateTimeDefinitions.WoMNextRegex, RegexFlags, RegexTimeOut);

        public static readonly ImmutableDictionary<string, int> MonthOfYear = DateTimeDefinitions.ParserConfigurationMonthOfYear.ToImmutableDictionary();

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex NextMonthRegex = new Regex(DateTimeDefinitions.ParserConfigurationNextMonthRegex, RegexFlags, RegexTimeOut);
        private static readonly Regex AfterNextMonthRegex = new Regex(DateTimeDefinitions.ParserConfigurationAfterNextMonthRegex, RegexFlags, RegexTimeOut);
        private static readonly Regex LastMonthRegex = new Regex(DateTimeDefinitions.ParserConfigurationLastMonthRegex, RegexFlags, RegexTimeOut);
        private static readonly Regex NextYearRegex = new Regex(DateTimeDefinitions.ParserConfigurationNextYearRegex, RegexFlags, RegexTimeOut);
        private static readonly Regex AfterNextYearRegex = new Regex(DateTimeDefinitions.ParserConfigurationAfterNextYearRegex, RegexFlags, RegexTimeOut);
        private static readonly Regex LastYearRegex = new Regex(DateTimeDefinitions.ParserConfigurationLastYearRegex, RegexFlags, RegexTimeOut);
        private static readonly Regex ThisYearRegex = new Regex(DateTimeDefinitions.ParserConfigurationThisYearRegex, RegexFlags, RegexTimeOut);

        public JapaneseDatePeriodParserConfiguration(ICJKCommonDateTimeParserConfiguration config)
            : base(config)
        {
            IntegerExtractor = config.IntegerExtractor;
            NumberParser = config.NumberParser;
            DateExtractor = config.DateExtractor;
            DurationExtractor = config.DurationExtractor;
            CardinalExtractor = config.CardinalExtractor;
            DurationParser = config.DurationParser;
            DateParser = config.DateParser;

            DynastyYearRegex = JapaneseDateExtractorConfiguration.DynastyYearRegex;
            DynastyStartYear = JapaneseDateExtractorConfiguration.DynastyStartYear;
            DynastyYearMap = JapaneseDateExtractorConfiguration.DynastyYearMap;
            SimpleCasesRegex = JapaneseDatePeriodExtractorConfiguration.SimpleCasesRegex;
            ThisRegex = JapaneseDatePeriodExtractorConfiguration.ThisRegex;
            NextRegex = JapaneseDatePeriodExtractorConfiguration.NextRegex;
            LastRegex = JapaneseDatePeriodExtractorConfiguration.LastRegex;
            YearToYear = JapaneseDatePeriodExtractorConfiguration.YearToYear;
            YearToYearSuffixRequired = JapaneseDatePeriodExtractorConfiguration.YearToYearSuffixRequired;
            YearRegex = JapaneseDatePeriodExtractorConfiguration.YearRegex;
            YearInCJKRegex = JapaneseDatePeriodExtractorConfiguration.YearInCJKRegex;
            MonthToMonth = JapaneseDatePeriodExtractorConfiguration.MonthToMonth;
            MonthToMonthSuffixRequired = JapaneseDatePeriodExtractorConfiguration.MonthToMonthSuffixRequired;
            DayToDay = JapaneseDatePeriodExtractorConfiguration.DayToDay;
            MonthDayRange = JapaneseDatePeriodExtractorConfiguration.MonthDayRange;
            DayRegexForPeriod = JapaneseDatePeriodExtractorConfiguration.DayRegexForPeriod;
            MonthRegex = JapaneseDatePeriodExtractorConfiguration.MonthRegex;
            SpecialMonthRegex = JapaneseDatePeriodExtractorConfiguration.SpecialMonthRegex;
            SpecialYearRegex = JapaneseDatePeriodExtractorConfiguration.SpecialYearRegex;
            YearAndMonth = JapaneseDatePeriodExtractorConfiguration.YearAndMonth;
            PureNumYearAndMonth = JapaneseDatePeriodExtractorConfiguration.PureNumYearAndMonth;
            SimpleYearAndMonth = JapaneseDatePeriodExtractorConfiguration.SimpleYearAndMonth;
            OneWordPeriodRegex = JapaneseDatePeriodExtractorConfiguration.OneWordPeriodRegex;
            NumberCombinedWithUnit = JapaneseDatePeriodExtractorConfiguration.NumberCombinedWithUnit;
            PastRegex = JapaneseDatePeriodExtractorConfiguration.PastRegex;
            FutureRegex = JapaneseDatePeriodExtractorConfiguration.FutureRegex;
            WeekWithWeekDayRangeRegex = JapaneseDatePeriodExtractorConfiguration.WeekWithWeekDayRangeRegex;
            UnitRegex = JapaneseDatePeriodExtractorConfiguration.UnitRegex;
            DurationUnitRegex = JapaneseDatePeriodExtractorConfiguration.DurationUnitRegex;
            WeekOfMonthRegex = JapaneseDatePeriodExtractorConfiguration.WeekOfMonthRegex;
            WeekOfYearRegex = JapaneseDatePeriodExtractorConfiguration.WeekOfYearRegex;
            WeekOfDateRegex = JapaneseDatePeriodExtractorConfiguration.WeekOfDateRegex;
            MonthOfDateRegex = JapaneseDatePeriodExtractorConfiguration.MonthOfDateRegex;
            WhichWeekRegex = JapaneseDatePeriodExtractorConfiguration.WhichWeekRegex;
            FirstLastOfYearRegex = JapaneseDatePeriodExtractorConfiguration.FirstLastOfYearRegex;
            SeasonWithYear = JapaneseDatePeriodExtractorConfiguration.SeasonWithYear;
            QuarterRegex = JapaneseDatePeriodExtractorConfiguration.QuarterRegex;
            DecadeRegex = JapaneseDatePeriodExtractorConfiguration.DecadeRegex;
            CenturyRegex = JapaneseDatePeriodExtractorConfiguration.CenturyRegex;
            RelativeRegex = JapaneseDateExtractorConfiguration.RelativeRegex;
            RelativeMonthRegex = JapaneseDateExtractorConfiguration.RelativeMonthRegex;
            LaterEarlyPeriodRegex = JapaneseDatePeriodExtractorConfiguration.LaterEarlyPeriodRegex;
            DatePointWithAgoAndLater = JapaneseDatePeriodExtractorConfiguration.DatePointWithAgoAndLater;
            ReferenceDatePeriodRegex = JapaneseDatePeriodExtractorConfiguration.ReferenceDatePeriodRegex;
            ComplexDatePeriodRegex = JapaneseDatePeriodExtractorConfiguration.ComplexDatePeriodRegex;
            DurationRelativeDurationUnitRegex = JapaneseDateExtractorConfiguration.DurationRelativeDurationUnitRegex;
            UnitMap = DateTimeDefinitions.ParserConfigurationUnitMap.ToImmutableDictionary();
            CardinalMap = DateTimeDefinitions.ParserConfigurationCardinalMap.ToImmutableDictionary();
            DayOfMonth = DateTimeDefinitions.ParserConfigurationDayOfMonth.ToImmutableDictionary();
            SeasonMap = DateTimeDefinitions.ParserConfigurationSeasonMap.ToImmutableDictionary();

        }

        public IDateTimeExtractor DateExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IExtractor CardinalExtractor { get; }

        public IDateTimeParser DurationParser { get; }

        public IDateTimeParser DateParser { get; }

        public IExtractor IntegerExtractor { get; }

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
            return DateTimeDefinitions.YearTerms.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal) || trimmedText.StartsWith(o, StringComparison.Ordinal));
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

            if (NextMonthRegex.IsMatch(text))
            {
                value = 1;
            }
            else if (LastMonthRegex.IsMatch(text))
            {
                value = -1;
            }
            else if (AfterNextMonthRegex.IsMatch(text))
            {
                value = 2;
            }

            return value;
        }

        public int GetSwiftYear(string text)
        {
            var value = -10;

            if (AfterNextYearRegex.IsMatch(text))
            {
                value = 2;
            }
            else if (NextYearRegex.IsMatch(text))
            {
                value = 1;
            }
            else if (LastYearRegex.IsMatch(text))
            {
                value = -1;
            }
            else if (ThisYearRegex.IsMatch(text))
            {
                // Current year: 今年
                value = 0;
            }

            return value;
        }
    }
}