﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Spanish;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishDatePeriodParserConfiguration : BaseDateTimeOptionsConfiguration, IDatePeriodParserConfiguration
    {
        // TODO: config this according to English
        public static readonly Regex NextPrefixRegex =
            new Regex(DateTimeDefinitions.NextPrefixRegex, RegexFlags);

        public static readonly Regex NextSuffixRegex =
            new Regex(DateTimeDefinitions.NextSuffixRegex, RegexFlags);

        public static readonly Regex PreviousPrefixRegex =
            new Regex(DateTimeDefinitions.PreviousPrefixRegex, RegexFlags);

        public static readonly Regex PreviousSuffixRegex =
            new Regex(DateTimeDefinitions.PreviousSuffixRegex, RegexFlags);

        public static readonly Regex ThisPrefixRegex =
            new Regex(DateTimeDefinitions.ThisPrefixRegex, RegexFlags);

        public static readonly Regex AfterNextSuffixRegex =
            new Regex(DateTimeDefinitions.AfterNextSuffixRegex, RegexFlags);

        public static readonly Regex RelativeSuffixRegex =
            new Regex(DateTimeDefinitions.RelativeSuffixRegex, RegexFlags);

        public static readonly Regex RelativeRegex =
            new Regex(DateTimeDefinitions.RelativeRegex, RegexFlags);

        public static readonly Regex UnspecificEndOfRangeRegex =
            new Regex(DateTimeDefinitions.UnspecificEndOfRangeRegex, RegexFlags);

        public static readonly Regex AmbiguousPointRangeRegex =
            new Regex(DateTimeDefinitions.AmbiguousPointRangeRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public SpanishDatePeriodParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;
            CardinalExtractor = config.CardinalExtractor;
            OrdinalExtractor = config.OrdinalExtractor;
            IntegerExtractor = config.IntegerExtractor;
            NumberParser = config.NumberParser;
            DurationExtractor = config.DurationExtractor;
            DateExtractor = config.DateExtractor;
            DurationParser = config.DurationParser;
            DateParser = config.DateParser;

            MonthFrontBetweenRegex = SpanishDatePeriodExtractorConfiguration.MonthFrontBetweenRegex;
            BetweenRegex = SpanishDatePeriodExtractorConfiguration.DayBetweenRegex;
            MonthFrontSimpleCasesRegex = SpanishDatePeriodExtractorConfiguration.MonthFrontSimpleCasesRegex;
            SimpleCasesRegex = SpanishDatePeriodExtractorConfiguration.SimpleCasesRegex;
            OneWordPeriodRegex = SpanishDatePeriodExtractorConfiguration.OneWordPeriodRegex;
            MonthWithYear = SpanishDatePeriodExtractorConfiguration.MonthWithYearRegex;
            MonthNumWithYear = SpanishDatePeriodExtractorConfiguration.MonthNumWithYearRegex;
            YearRegex = SpanishDatePeriodExtractorConfiguration.YearRegex;
            PastRegex = SpanishDatePeriodExtractorConfiguration.PastRegex;
            FutureRegex = SpanishDatePeriodExtractorConfiguration.FutureRegex;
            FutureSuffixRegex = SpanishDatePeriodExtractorConfiguration.FutureSuffixRegex;
            NumberCombinedWithUnit = SpanishDurationExtractorConfiguration.NumberCombinedWithUnit;
            WeekOfMonthRegex = SpanishDatePeriodExtractorConfiguration.WeekOfMonthRegex;
            WeekOfYearRegex = SpanishDatePeriodExtractorConfiguration.WeekOfYearRegex;
            QuarterRegex = SpanishDatePeriodExtractorConfiguration.QuarterRegex;
            QuarterRegexYearFront = SpanishDatePeriodExtractorConfiguration.QuarterRegexYearFront;
            AllHalfYearRegex = SpanishDatePeriodExtractorConfiguration.AllHalfYearRegex;
            SeasonRegex = SpanishDatePeriodExtractorConfiguration.SeasonRegex;
            WhichWeekRegex = SpanishDatePeriodExtractorConfiguration.WhichWeekRegex;
            WeekOfRegex = SpanishDatePeriodExtractorConfiguration.WeekOfRegex;
            MonthOfRegex = SpanishDatePeriodExtractorConfiguration.MonthOfRegex;
            RestOfDateRegex = SpanishDatePeriodExtractorConfiguration.RestOfDateRegex;
            LaterEarlyPeriodRegex = SpanishDatePeriodExtractorConfiguration.LaterEarlyPeriodRegex;
            WeekWithWeekDayRangeRegex = SpanishDatePeriodExtractorConfiguration.WeekWithWeekDayRangeRegex;
            YearPlusNumberRegex = SpanishDatePeriodExtractorConfiguration.YearPlusNumberRegex;
            DecadeWithCenturyRegex = SpanishDatePeriodExtractorConfiguration.DecadeWithCenturyRegex;
            YearPeriodRegex = SpanishDatePeriodExtractorConfiguration.YearPeriodRegex;
            ComplexDatePeriodRegex = SpanishDatePeriodExtractorConfiguration.ComplexDatePeriodRegex;
            RelativeDecadeRegex = SpanishDatePeriodExtractorConfiguration.RelativeDecadeRegex;
            InConnectorRegex = config.UtilityConfiguration.InConnectorRegex;
            WithinNextPrefixRegex = SpanishDatePeriodExtractorConfiguration.WithinNextPrefixRegex;
            ReferenceDatePeriodRegex = SpanishDatePeriodExtractorConfiguration.ReferenceDatePeriodRegex;
            AgoRegex = SpanishDatePeriodExtractorConfiguration.AgoRegex;
            LaterRegex = SpanishDatePeriodExtractorConfiguration.LaterRegex;
            LessThanRegex = SpanishDatePeriodExtractorConfiguration.LessThanRegex;
            MoreThanRegex = SpanishDatePeriodExtractorConfiguration.MoreThanRegex;
            CenturySuffixRegex = SpanishDatePeriodExtractorConfiguration.CenturySuffixRegex;
            NowRegex = SpanishDatePeriodExtractorConfiguration.NowRegex;
            SpecialDayRegex = SpanishDateExtractorConfiguration.SpecialDayRegex;
            TodayNowRegex = new Regex(DateTimeDefinitions.TodayNowRegex, RegexOptions.Singleline);

            UnitMap = config.UnitMap;
            CardinalMap = config.CardinalMap;
            DayOfMonth = config.DayOfMonth;
            MonthOfYear = config.MonthOfYear;
            SeasonMap = config.SeasonMap;
            SpecialYearPrefixesMap = config.SpecialYearPrefixesMap;
            Numbers = config.Numbers;
            WrittenDecades = config.WrittenDecades;
            SpecialDecadeCases = config.SpecialDecadeCases;
        }

        public int MinYearNum { get; }

        public int MaxYearNum { get; }

        public string TokenBeforeDate { get; }

        public IDateExtractor DateExtractor { get; }

        public IExtractor CardinalExtractor { get; }

        public IExtractor OrdinalExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IExtractor IntegerExtractor { get; }

        public IParser NumberParser { get; }

        public IDateTimeParser DateParser { get; }

        public IDateTimeParser DurationParser { get; }

        public Regex MonthFrontBetweenRegex { get; }

        public Regex BetweenRegex { get; }

        public Regex MonthFrontSimpleCasesRegex { get; }

        public Regex SimpleCasesRegex { get; }

        public Regex OneWordPeriodRegex { get; }

        public Regex MonthWithYear { get; }

        public Regex MonthNumWithYear { get; }

        public Regex YearRegex { get; }

        public Regex PastRegex { get; }

        public Regex FutureRegex { get; }

        public Regex FutureSuffixRegex { get; }

        public Regex NumberCombinedWithUnit { get; }

        public Regex WeekOfMonthRegex { get; }

        public Regex WeekOfYearRegex { get; }

        public Regex QuarterRegex { get; }

        public Regex QuarterRegexYearFront { get; }

        public Regex AllHalfYearRegex { get; }

        public Regex SeasonRegex { get; }

        public Regex WhichWeekRegex { get; }

        public Regex WeekOfRegex { get; }

        public Regex MonthOfRegex { get; }

        public Regex InConnectorRegex { get; }

        public Regex WithinNextPrefixRegex { get; }

        public Regex RestOfDateRegex { get; }

        public Regex LaterEarlyPeriodRegex { get; }

        public Regex WeekWithWeekDayRangeRegex { get; }

        public Regex YearPlusNumberRegex { get; }

        public Regex DecadeWithCenturyRegex { get; }

        public Regex YearPeriodRegex { get; }

        public Regex ComplexDatePeriodRegex { get; }

        public Regex RelativeDecadeRegex { get; }

        public Regex ReferenceDatePeriodRegex { get; }

        public Regex AgoRegex { get; }

        public Regex LaterRegex { get; }

        public Regex LessThanRegex { get; }

        public Regex MoreThanRegex { get; }

        public Regex CenturySuffixRegex { get; }

        public Regex NowRegex { get; }

        public Regex SpecialDayRegex { get; }

        public Regex TodayNowRegex { get; }

        Regex IDatePeriodParserConfiguration.NextPrefixRegex => NextPrefixRegex;

        Regex IDatePeriodParserConfiguration.PreviousPrefixRegex => PreviousPrefixRegex;

        Regex IDatePeriodParserConfiguration.ThisPrefixRegex => ThisPrefixRegex;

        Regex ISimpleDatePeriodParserConfiguration.RelativeRegex => RelativeRegex;

        Regex IDatePeriodParserConfiguration.UnspecificEndOfRangeRegex => UnspecificEndOfRangeRegex;

        Regex IDatePeriodParserConfiguration.AmbiguousPointRangeRegex => AmbiguousPointRangeRegex;

        bool IDatePeriodParserConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IImmutableDictionary<string, int> CardinalMap { get; }

        public IImmutableDictionary<string, int> DayOfMonth { get; }

        public IImmutableDictionary<string, int> MonthOfYear { get; }

        public IImmutableDictionary<string, string> SeasonMap { get; }

        public IImmutableDictionary<string, string> SpecialYearPrefixesMap { get; }

        public IImmutableDictionary<string, int> WrittenDecades { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public IImmutableDictionary<string, int> SpecialDecadeCases { get; }

        public int GetSwiftDayOrMonth(string text)
        {
            var trimmedText = text.Trim();
            var swift = 0;

            if (AfterNextSuffixRegex.IsMatch(trimmedText))
            {
                swift = 2;
            }
            else if (NextPrefixRegex.IsMatch(trimmedText) || NextSuffixRegex.IsMatch(trimmedText))
            {
                swift = 1;
            }
            else if (PreviousPrefixRegex.IsMatch(trimmedText) || PreviousSuffixRegex.IsMatch(trimmedText))
            {
                swift = -1;
            }

            return swift;
        }

        public int GetSwiftYear(string text)
        {
            var trimmedText = text.Trim();
            var swift = -10;
            if (AfterNextSuffixRegex.IsMatch(trimmedText))
            {
                swift = 2;
            }
            else if (NextPrefixRegex.IsMatch(trimmedText) || NextSuffixRegex.IsMatch(trimmedText))
            {
                swift = 1;
            }
            else if (PreviousPrefixRegex.IsMatch(trimmedText) || PreviousSuffixRegex.IsMatch(trimmedText))
            {
                swift = -1;
            }
            else if (ThisPrefixRegex.IsMatch(trimmedText))
            {
                swift = 0;
            }

            return swift;
        }

        public bool IsFuture(string text)
        {
            var trimmedText = text.Trim();
            return ThisPrefixRegex.IsMatch(trimmedText) || NextPrefixRegex.IsMatch(trimmedText);
        }

        public bool IsLastCardinal(string text)
        {
            var trimmedText = text.Trim();
            return PreviousPrefixRegex.IsMatch(trimmedText);
        }

        public bool IsMonthOnly(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.MonthTerms.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal)) ||
                   (DateTimeDefinitions.MonthTerms.Any(o => trimmedText.Contains(o)) && RelativeSuffixRegex.IsMatch(trimmedText));
        }

        public bool IsMonthToDate(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.MonthToDateTerms.Any(o => trimmedText.Equals(o, StringComparison.Ordinal));
        }

        public bool IsWeekend(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.WeekendTerms.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal)) ||
                   (DateTimeDefinitions.WeekendTerms.Any(o => trimmedText.Contains(o)) && RelativeSuffixRegex.IsMatch(trimmedText));
        }

        public bool IsWeekOnly(string text)
        {
            var trimmedText = text.Trim();
            return (DateTimeDefinitions.WeekTerms.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal)) ||
                   (DateTimeDefinitions.WeekTerms.Any(o => trimmedText.Contains(o)) && RelativeSuffixRegex.IsMatch(trimmedText))) &&
                   !DateTimeDefinitions.WeekendTerms.Any(o => trimmedText.Contains(o));
        }

        public bool IsFortnight(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.FortnightTerms.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal));
        }

        public bool IsYearOnly(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.YearTerms.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal)) ||
                   (DateTimeDefinitions.YearTerms.Any(o => trimmedText.Contains(o)) && RelativeSuffixRegex.IsMatch(trimmedText));
        }

        public bool IsYearToDate(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.YearToDateTerms.Any(o => trimmedText.Equals(o, StringComparison.Ordinal));
        }
    }
}
