using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Dutch;

namespace Microsoft.Recognizers.Text.DateTime.Dutch
{
    public class DutchDatePeriodParserConfiguration : BaseOptionsConfiguration, IDatePeriodParserConfiguration
    {
        public static readonly Regex NextPrefixRegex =
            new Regex(DateTimeDefinitions.NextPrefixRegex, RegexOptions.Singleline);

        public static readonly Regex PreviousPrefixRegex =
            new Regex(DateTimeDefinitions.PreviousPrefixRegex, RegexOptions.Singleline);

        public static readonly Regex ThisPrefixRegex =
            new Regex(DateTimeDefinitions.ThisPrefixRegex, RegexOptions.Singleline);

        public static readonly Regex AfterNextSuffixRegex =
            new Regex(DateTimeDefinitions.AfterNextSuffixRegex, RegexOptions.Singleline);

        public static readonly Regex RelativeRegex =
            new Regex(DateTimeDefinitions.RelativeRegex, RegexOptions.Singleline);

        public static readonly Regex UnspecificEndOfRangeRegex =
            new Regex(DateTimeDefinitions.UnspecificEndOfRangeRegex, RegexOptions.Singleline);

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
            UnitMap = config.UnitMap;
            CardinalMap = config.CardinalMap;
            DayOfMonth = config.DayOfMonth;
            MonthOfYear = config.MonthOfYear;
            SeasonMap = config.SeasonMap;
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

        Regex IDatePeriodParserConfiguration.NextPrefixRegex => NextPrefixRegex;

        Regex IDatePeriodParserConfiguration.PreviousPrefixRegex => PreviousPrefixRegex;

        Regex IDatePeriodParserConfiguration.ThisPrefixRegex => ThisPrefixRegex;

        Regex IDatePeriodParserConfiguration.RelativeRegex => RelativeRegex;

        Regex IDatePeriodParserConfiguration.UnspecificEndOfRangeRegex => UnspecificEndOfRangeRegex;

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IImmutableDictionary<string, int> CardinalMap { get; }

        public IImmutableDictionary<string, int> DayOfMonth { get; }

        public IImmutableDictionary<string, int> MonthOfYear { get; }

        public IImmutableDictionary<string, string> SeasonMap { get; }

        public IImmutableDictionary<string, int> WrittenDecades { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public IImmutableDictionary<string, int> SpecialDecadeCases { get; }

        public IImmutableList<string> InStringList { get; }

        public int GetSwiftDayOrMonth(string text)
        {
            var swift = 0;

            var trimmedText = text.Trim().ToLowerInvariant();

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

            var trimmedText = text.Trim().ToLowerInvariant();

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
            var trimmedText = text.Trim().ToLowerInvariant();
            return DateTimeDefinitions.FutureTerms.Any(o => trimmedText.StartsWith(o));
        }

        public bool IsLastCardinal(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            return DateTimeDefinitions.LastCardinalTerms.Any(o => trimmedText.Equals(o));
        }

        public bool IsMonthOnly(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            return DateTimeDefinitions.MonthTerms.Any(o => trimmedText.EndsWith(o)) ||
                   (MonthTermsPadded.Any(o => trimmedText.Contains(o)) && AfterNextSuffixRegex.IsMatch(trimmedText));
        }

        public bool IsMonthToDate(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            return DateTimeDefinitions.MonthToDateTerms.Any(o => trimmedText.Equals(o));
        }

        public bool IsWeekend(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            return DateTimeDefinitions.WeekendTerms.Any(o => trimmedText.EndsWith(o)) ||
                   (WeekendTermsPadded.Any(o => trimmedText.Contains(o)) && AfterNextSuffixRegex.IsMatch(trimmedText));
        }

        public bool IsWeekOnly(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            return DateTimeDefinitions.WeekTerms.Any(o => trimmedText.EndsWith(o)) ||
                   (WeekTermsPadded.Any(o => trimmedText.Contains(o)) && AfterNextSuffixRegex.IsMatch(trimmedText));
        }

        public bool IsYearOnly(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            return DateTimeDefinitions.YearTerms.Any(o => trimmedText.EndsWith(o)) ||
                   (YearTermsPadded.Any(o => trimmedText.Contains(o)) && AfterNextSuffixRegex.IsMatch(trimmedText)) ||
                   (DateTimeDefinitions.GenericYearTerms.Any(o => trimmedText.EndsWith(o)) && UnspecificEndOfRangeRegex.IsMatch(trimmedText));
        }

        public bool IsYearToDate(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            return DateTimeDefinitions.YearToDateTerms.Any(o => trimmedText.Equals(o));
        }
    }
}
