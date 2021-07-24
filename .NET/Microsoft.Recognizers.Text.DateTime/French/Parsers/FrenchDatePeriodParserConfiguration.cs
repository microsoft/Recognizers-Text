﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.French;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchDatePeriodParserConfiguration : BaseDateTimeOptionsConfiguration, IDatePeriodParserConfiguration
    {
        // @TODO move to resources - French - relative
        public static readonly Regex NextPrefixRegex =
            new Regex(@"(prochain|prochaine)\b", RegexFlags);

        public static readonly Regex PastPrefixRegex =
            new Regex(@"(dernier)\b", RegexFlags);

        public static readonly Regex ThisPrefixRegex =
            new Regex(@"(ce|cette)\b", RegexFlags);

        public static readonly Regex NextSuffixRegex =
            new Regex(DateTimeDefinitions.NextSuffixRegex, RegexFlags);

        public static readonly Regex PastSuffixRegex =
            new Regex(DateTimeDefinitions.PastSuffixRegex, RegexFlags);

        public static readonly Regex RelativeRegex =
            new Regex(DateTimeDefinitions.RelativeRegex, RegexFlags);

        public static readonly Regex UnspecificEndOfRangeRegex =
            new Regex(DateTimeDefinitions.UnspecificEndOfRangeRegex, RegexFlags);

        public static readonly Regex AmbiguousPointRangeRegex =
            new Regex(DateTimeDefinitions.AmbiguousPointRangeRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public FrenchDatePeriodParserConfiguration(ICommonDateTimeParserConfiguration config)
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

            MonthFrontBetweenRegex = FrenchDatePeriodExtractorConfiguration.MonthFrontBetweenRegex;
            BetweenRegex = FrenchDatePeriodExtractorConfiguration.BetweenRegex;
            MonthFrontSimpleCasesRegex = FrenchDatePeriodExtractorConfiguration.MonthFrontSimpleCasesRegex;
            SimpleCasesRegex = FrenchDatePeriodExtractorConfiguration.SimpleCasesRegex;
            OneWordPeriodRegex = FrenchDatePeriodExtractorConfiguration.OneWordPeriodRegex;
            MonthWithYear = FrenchDatePeriodExtractorConfiguration.MonthWithYear;
            MonthNumWithYear = FrenchDatePeriodExtractorConfiguration.MonthNumWithYear;
            YearRegex = FrenchDatePeriodExtractorConfiguration.YearRegex;
            PastRegex = FrenchDatePeriodExtractorConfiguration.PastPrefixRegex;
            FutureRegex = FrenchDatePeriodExtractorConfiguration.NextPrefixRegex;
            FutureSuffixRegex = FrenchDatePeriodExtractorConfiguration.FutureSuffixRegex;
            NumberCombinedWithUnit = FrenchDurationExtractorConfiguration.NumberCombinedWithDurationUnit;
            WeekOfMonthRegex = FrenchDatePeriodExtractorConfiguration.WeekOfMonthRegex;
            WeekOfYearRegex = FrenchDatePeriodExtractorConfiguration.WeekOfYearRegex;
            QuarterRegex = FrenchDatePeriodExtractorConfiguration.QuarterRegex;
            QuarterRegexYearFront = FrenchDatePeriodExtractorConfiguration.QuarterRegexYearFront;
            AllHalfYearRegex = FrenchDatePeriodExtractorConfiguration.AllHalfYearRegex;
            SeasonRegex = FrenchDatePeriodExtractorConfiguration.SeasonRegex;
            WhichWeekRegex = FrenchDatePeriodExtractorConfiguration.WhichWeekRegex;
            WeekOfRegex = FrenchDatePeriodExtractorConfiguration.WeekOfRegex;
            MonthOfRegex = FrenchDatePeriodExtractorConfiguration.MonthOfRegex;
            RestOfDateRegex = FrenchDatePeriodExtractorConfiguration.RestOfDateRegex;
            LaterEarlyPeriodRegex = FrenchDatePeriodExtractorConfiguration.LaterEarlyPeriodRegex;
            WeekWithWeekDayRangeRegex = FrenchDatePeriodExtractorConfiguration.WeekWithWeekDayRangeRegex;
            YearPlusNumberRegex = FrenchDatePeriodExtractorConfiguration.YearPlusNumberRegex;
            DecadeWithCenturyRegex = FrenchDatePeriodExtractorConfiguration.DecadeWithCenturyRegex;
            YearPeriodRegex = FrenchDatePeriodExtractorConfiguration.YearPeriodRegex;
            ComplexDatePeriodRegex = FrenchDatePeriodExtractorConfiguration.ComplexDatePeriodRegex;
            RelativeDecadeRegex = FrenchDatePeriodExtractorConfiguration.RelativeDecadeRegex;
            InConnectorRegex = config.UtilityConfiguration.InConnectorRegex;
            WithinNextPrefixRegex = FrenchDatePeriodExtractorConfiguration.WithinNextPrefixRegex;
            ReferenceDatePeriodRegex = FrenchDatePeriodExtractorConfiguration.ReferenceDatePeriodRegex;
            AgoRegex = FrenchDatePeriodExtractorConfiguration.AgoRegex;
            LaterRegex = FrenchDatePeriodExtractorConfiguration.LaterRegex;
            LessThanRegex = FrenchDatePeriodExtractorConfiguration.LessThanRegex;
            MoreThanRegex = FrenchDatePeriodExtractorConfiguration.MoreThanRegex;
            CenturySuffixRegex = FrenchDatePeriodExtractorConfiguration.CenturySuffixRegex;
            NowRegex = FrenchDatePeriodExtractorConfiguration.NowRegex;
            SpecialDayRegex = FrenchDateExtractorConfiguration.SpecialDayRegex;
            TodayNowRegex = new Regex(DateTimeDefinitions.TodayNowRegex, RegexOptions.Singleline);

            UnitMap = config.UnitMap;
            CardinalMap = config.CardinalMap;
            DayOfMonth = config.DayOfMonth;
            MonthOfYear = config.MonthOfYear;
            SeasonMap = config.SeasonMap;
            SpecialYearPrefixesMap = config.SpecialYearPrefixesMap;
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

        Regex ISimpleDatePeriodParserConfiguration.RelativeRegex => RelativeRegex;

        Regex IDatePeriodParserConfiguration.NextPrefixRegex => NextPrefixRegex;

        Regex IDatePeriodParserConfiguration.PreviousPrefixRegex => PastPrefixRegex;

        Regex IDatePeriodParserConfiguration.ThisPrefixRegex => ThisPrefixRegex;

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

        public IImmutableList<string> InStringList { get; }

        public int GetSwiftDayOrMonth(string text)
        {
            var swift = 0;

            var trimmedText = text.Trim();

            // @TODO move hardcoded values to resources file
            // @TODO Add 'upcoming' key word

            // example: "nous serons ensemble cette fois la semaine prochaine" - "We'll be together this time next week"
            if (trimmedText.EndsWith("prochain", StringComparison.Ordinal) ||
                trimmedText.EndsWith("prochaine", StringComparison.Ordinal))
            {
                swift = 1;
            }

            // example: Je l'ai vue pas plus tard que la semaine derniere - "I saw her only last week"
            if (trimmedText.EndsWith("dernière", StringComparison.Ordinal) ||
                trimmedText.EndsWith("dernières", StringComparison.Ordinal) ||
                trimmedText.EndsWith("derniere", StringComparison.Ordinal) ||
                trimmedText.EndsWith("dernieres", StringComparison.Ordinal))
            {
                swift = -1;
            }

            return swift;
        }

        public int GetSwiftYear(string text)
        {
            var swift = -10;

            var trimmedText = text.Trim();

            // @TODO move hardcoded values to resources file

            if (trimmedText.EndsWith("prochain", StringComparison.Ordinal) ||
                trimmedText.EndsWith("prochaine", StringComparison.Ordinal))
            {
                swift = 1;
            }

            if (trimmedText.EndsWith("dernières", StringComparison.Ordinal) ||
                trimmedText.EndsWith("dernière", StringComparison.Ordinal) ||
                trimmedText.EndsWith("dernieres", StringComparison.Ordinal) ||
                trimmedText.EndsWith("derniere", StringComparison.Ordinal) ||
                trimmedText.EndsWith("dernier", StringComparison.Ordinal))
            {
                swift = -1;
            }
            else if (trimmedText.StartsWith("cette", StringComparison.Ordinal))
            {
                swift = 0;
            }

            return swift;
        }

        public bool IsFuture(string text)
        {
            var trimmedText = text.Trim();

            return DateTimeDefinitions.FutureStartTerms.Any(o => trimmedText.StartsWith(o, StringComparison.Ordinal)) ||
                   DateTimeDefinitions.FutureEndTerms.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal));
        }

        public bool IsLastCardinal(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.LastCardinalTerms.Any(o => trimmedText.Equals(o, StringComparison.Ordinal));
        }

        public bool IsMonthOnly(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.MonthTerms.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal));
        }

        public bool IsMonthToDate(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.MonthToDateTerms.Any(o => trimmedText.Equals(o, StringComparison.Ordinal));
        }

        public bool IsWeekend(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.WeekendTerms.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal));
        }

        public bool IsWeekOnly(string text)
        {
            var trimmedText = text.Trim();

            return (DateTimeDefinitions.WeekTerms.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal)) ||
                   (DateTimeDefinitions.WeekTerms.Any(o => trimmedText.Contains(o)) &&
                   (NextSuffixRegex.IsMatch(trimmedText) || PastSuffixRegex.IsMatch(trimmedText)))) &&
                   !DateTimeDefinitions.WeekendTerms.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal));
        }

        public bool IsFortnight(string text)
        {
            return false;
        }

        public bool IsYearOnly(string text)
        {
            var trimmedText = text.Trim();

            return DateTimeDefinitions.YearTerms.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal));
        }

        public bool IsYearToDate(string text)
        {
            var trimmedText = text.Trim();

            return DateTimeDefinitions.YearToDateTerms.Any(o => trimmedText.Equals(o, StringComparison.Ordinal));
        }
    }
}
