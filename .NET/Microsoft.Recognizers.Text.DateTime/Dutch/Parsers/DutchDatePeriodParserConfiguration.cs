using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Dutch;

namespace Microsoft.Recognizers.Text.DateTime.Dutch
{
    public class DutchDatePeriodParserConfiguration : BaseDateTimeOptionsConfiguration, IDatePeriodParserConfiguration
    {
        public static readonly Regex NextPrefixRegex =
            new Regex(DateTimeDefinitions.NextPrefixRegex, RegexFlags);

        public static readonly Regex PreviousPrefixRegex =
            new Regex(DateTimeDefinitions.PreviousPrefixRegex, RegexFlags);

        public static readonly Regex ThisPrefixRegex =
            new Regex(DateTimeDefinitions.ThisPrefixRegex, RegexFlags);

        public static readonly Regex AfterNextSuffixRegex =
            new Regex(DateTimeDefinitions.AfterNextSuffixRegex, RegexFlags);

        public static readonly Regex RelativeRegex =
            new Regex(DateTimeDefinitions.RelativeRegex, RegexFlags);

        public static readonly Regex UnspecificEndOfRangeRegex =
            new Regex(DateTimeDefinitions.UnspecificEndOfRangeRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public DutchDatePeriodParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;
            CardinalExtractor = config.CardinalExtractor;
            OrdinalExtractor = config.OrdinalExtractor;
            IntegerExtractor = config.IntegerExtractor;
            NumberParser = config.NumberParser;
            DateExtractor = config.DateExtractor;
            DurationExtractor = config.DurationExtractor;
            DurationParser = config.DurationParser;
            DateParser = config.DateParser;
            MonthFrontBetweenRegex = DutchDatePeriodExtractorConfiguration.MonthFrontBetweenRegex;
            BetweenRegex = DutchDatePeriodExtractorConfiguration.BetweenRegex;
            MonthFrontSimpleCasesRegex = DutchDatePeriodExtractorConfiguration.MonthFrontSimpleCasesRegex;
            SimpleCasesRegex = DutchDatePeriodExtractorConfiguration.SimpleCasesRegex;
            OneWordPeriodRegex = DutchDatePeriodExtractorConfiguration.OneWordPeriodRegex;
            MonthWithYear = DutchDatePeriodExtractorConfiguration.MonthWithYear;
            MonthNumWithYear = DutchDatePeriodExtractorConfiguration.MonthNumWithYear;
            YearRegex = DutchDatePeriodExtractorConfiguration.YearRegex;
            PastRegex = DutchDatePeriodExtractorConfiguration.PreviousPrefixRegex;
            FutureRegex = DutchDatePeriodExtractorConfiguration.NextPrefixRegex;
            FutureSuffixRegex = DutchDatePeriodExtractorConfiguration.FutureSuffixRegex;
            NumberCombinedWithUnit = DutchDurationExtractorConfiguration.NumberCombinedWithDurationUnit;
            WeekOfMonthRegex = DutchDatePeriodExtractorConfiguration.WeekOfMonthRegex;
            WeekOfYearRegex = DutchDatePeriodExtractorConfiguration.WeekOfYearRegex;
            QuarterRegex = DutchDatePeriodExtractorConfiguration.QuarterRegex;
            QuarterRegexYearFront = DutchDatePeriodExtractorConfiguration.QuarterRegexYearFront;
            AllHalfYearRegex = DutchDatePeriodExtractorConfiguration.AllHalfYearRegex;
            SeasonRegex = DutchDatePeriodExtractorConfiguration.SeasonRegex;
            WhichWeekRegex = DutchDatePeriodExtractorConfiguration.WhichWeekRegex;
            WeekOfRegex = DutchDatePeriodExtractorConfiguration.WeekOfRegex;
            MonthOfRegex = DutchDatePeriodExtractorConfiguration.MonthOfRegex;
            RestOfDateRegex = DutchDatePeriodExtractorConfiguration.RestOfDateRegex;
            LaterEarlyPeriodRegex = DutchDatePeriodExtractorConfiguration.LaterEarlyPeriodRegex;
            WeekWithWeekDayRangeRegex = DutchDatePeriodExtractorConfiguration.WeekWithWeekDayRangeRegex;
            YearPlusNumberRegex = DutchDatePeriodExtractorConfiguration.YearPlusNumberRegex;
            DecadeWithCenturyRegex = DutchDatePeriodExtractorConfiguration.DecadeWithCenturyRegex;
            YearPeriodRegex = DutchDatePeriodExtractorConfiguration.YearPeriodRegex;
            ComplexDatePeriodRegex = DutchDatePeriodExtractorConfiguration.ComplexDatePeriodRegex;
            RelativeDecadeRegex = DutchDatePeriodExtractorConfiguration.RelativeDecadeRegex;
            InConnectorRegex = config.UtilityConfiguration.InConnectorRegex;
            WithinNextPrefixRegex = DutchDatePeriodExtractorConfiguration.WithinNextPrefixRegex;
            ReferenceDatePeriodRegex = DutchDatePeriodExtractorConfiguration.ReferenceDatePeriodRegex;
            AgoRegex = DutchDatePeriodExtractorConfiguration.AgoRegex;
            LaterRegex = DutchDatePeriodExtractorConfiguration.LaterRegex;
            LessThanRegex = DutchDatePeriodExtractorConfiguration.LessThanRegex;
            MoreThanRegex = DutchDatePeriodExtractorConfiguration.MoreThanRegex;
            CenturySuffixRegex = DutchDatePeriodExtractorConfiguration.CenturySuffixRegex;
            NowRegex = DutchDatePeriodExtractorConfiguration.NowRegex;
            TodayNowRegex = new Regex(DateTimeDefinitions.TodayNowRegex, RegexOptions.Singleline);
            SpecialDayRegex = DutchDateExtractorConfiguration.SpecialDayRegex;
            UnitMap = config.UnitMap;
            CardinalMap = config.CardinalMap;
            DayOfMonth = config.DayOfMonth;
            MonthOfYear = config.MonthOfYear;
            SeasonMap = config.SeasonMap;
            SpecialYearPrefixesMap = config.SpecialYearPrefixesMap;
            WrittenDecades = config.WrittenDecades;
            Numbers = config.Numbers;
            SpecialDecadeCases = config.SpecialDecadeCases;
        }

        public static IList<string> MonthTermsPadded { get; private set; } =
           DateTimeDefinitions.MonthTerms.Select(str => $" {str} ").ToList();

        public static IList<string> WeekendTermsPadded { get; private set; } =
            DateTimeDefinitions.WeekendTerms.Select(str => $" {str} ").ToList();

        public static IList<string> WeekTermsPadded { get; private set; } =
            DateTimeDefinitions.WeekTerms.Select(str => $" {str} ").ToList();

        public static IList<string> YearTermsPadded { get; private set; } =
            DateTimeDefinitions.YearTerms.Select(str => $" {str} ").ToList();

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

        public Regex TodayNowRegex { get; }

        public Regex SpecialDayRegex { get; }

        Regex ISimpleDatePeriodParserConfiguration.RelativeRegex => RelativeRegex;

        Regex IDatePeriodParserConfiguration.NextPrefixRegex => NextPrefixRegex;

        Regex IDatePeriodParserConfiguration.PreviousPrefixRegex => PreviousPrefixRegex;

        Regex IDatePeriodParserConfiguration.ThisPrefixRegex => ThisPrefixRegex;

        Regex IDatePeriodParserConfiguration.UnspecificEndOfRangeRegex => UnspecificEndOfRangeRegex;

        Regex IDatePeriodParserConfiguration.AmbiguousPointRangeRegex => null;

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

            if (AfterNextSuffixRegex.IsMatch(trimmedText))
            {
                swift = 2;
            }
            else if (NextPrefixRegex.IsMatch(trimmedText))
            {
                swift = 1;
            }
            else if (PreviousPrefixRegex.IsMatch(trimmedText))
            {
                swift = -1;
            }

            return swift;
        }

        public int GetSwiftYear(string text)
        {
            var swift = -10;

            var trimmedText = text.Trim();

            if (AfterNextSuffixRegex.IsMatch(trimmedText))
            {
                swift = 2;
            }
            else if (NextPrefixRegex.IsMatch(trimmedText))
            {
                swift = 1;
            }
            else if (PreviousPrefixRegex.IsMatch(trimmedText))
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
            return DateTimeDefinitions.FutureTerms.Any(o => trimmedText.StartsWith(o, StringComparison.Ordinal));
        }

        public bool IsLastCardinal(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.LastCardinalTerms.Any(o => trimmedText.Equals(o, StringComparison.Ordinal));
        }

        public bool IsMonthOnly(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.MonthTerms.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal)) ||
                   (MonthTermsPadded.Any(o => trimmedText.Contains(o)) && AfterNextSuffixRegex.IsMatch(trimmedText));
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
                   (WeekendTermsPadded.Any(o => trimmedText.Contains(o)) && AfterNextSuffixRegex.IsMatch(trimmedText));
        }

        public bool IsWeekOnly(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.WeekTerms.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal)) ||
                   (WeekTermsPadded.Any(o => trimmedText.Contains(o)) && AfterNextSuffixRegex.IsMatch(trimmedText));
        }

        public bool IsYearOnly(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.YearTerms.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal)) ||
                   (YearTermsPadded.Any(o => trimmedText.Contains(o)) && AfterNextSuffixRegex.IsMatch(trimmedText)) ||
                   (DateTimeDefinitions.GenericYearTerms.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal)) &&
                    UnspecificEndOfRangeRegex.IsMatch(trimmedText));
        }

        public bool IsFortnight(string text)
        {
            return false;
        }

        public bool IsYearToDate(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.YearToDateTerms.Any(o => trimmedText.Equals(o, StringComparison.Ordinal));
        }
    }
}
