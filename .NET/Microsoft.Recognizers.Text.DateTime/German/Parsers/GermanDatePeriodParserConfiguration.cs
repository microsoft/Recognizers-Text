using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.German;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanDatePeriodParserConfiguration : BaseOptionsConfiguration, IDatePeriodParserConfiguration
    {
        public static readonly Regex NextPrefixRegex =
            new Regex(DateTimeDefinitions.NextPrefixRegex, RegexOptions.Singleline);

        public static readonly Regex PreviousPrefixRegex =
            new Regex(DateTimeDefinitions.PreviousPrefixRegex, RegexOptions.Singleline);

        public static readonly Regex ThisPrefixRegex =
            new Regex(DateTimeDefinitions.ThisPrefixRegex, RegexOptions.Singleline);

        public static readonly Regex RelativeRegex =
            new Regex(DateTimeDefinitions.RelativeRegex, RegexOptions.Singleline);

        public static readonly Regex UnspecificEndOfRangeRegex =
            new Regex(DateTimeDefinitions.UnspecificEndOfRangeRegex, RegexOptions.Singleline);

        public GermanDatePeriodParserConfiguration(ICommonDateTimeParserConfiguration config)
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
            MonthFrontBetweenRegex = GermanDatePeriodExtractorConfiguration.MonthFrontBetweenRegex;
            BetweenRegex = GermanDatePeriodExtractorConfiguration.BetweenRegex;
            MonthFrontSimpleCasesRegex = GermanDatePeriodExtractorConfiguration.MonthFrontSimpleCasesRegex;
            SimpleCasesRegex = GermanDatePeriodExtractorConfiguration.SimpleCasesRegex;
            OneWordPeriodRegex = GermanDatePeriodExtractorConfiguration.OneWordPeriodRegex;
            MonthWithYear = GermanDatePeriodExtractorConfiguration.MonthWithYear;
            MonthNumWithYear = GermanDatePeriodExtractorConfiguration.MonthNumWithYear;
            YearRegex = GermanDatePeriodExtractorConfiguration.YearRegex;
            PastRegex = GermanDatePeriodExtractorConfiguration.PreviousPrefixRegex;
            FutureRegex = GermanDatePeriodExtractorConfiguration.NextPrefixRegex;
            FutureSuffixRegex = GermanDatePeriodExtractorConfiguration.FutureSuffixRegex;
            NumberCombinedWithUnit = GermanDurationExtractorConfiguration.NumberCombinedWithDurationUnit;
            WeekOfMonthRegex = GermanDatePeriodExtractorConfiguration.WeekOfMonthRegex;
            WeekOfYearRegex = GermanDatePeriodExtractorConfiguration.WeekOfYearRegex;
            QuarterRegex = GermanDatePeriodExtractorConfiguration.QuarterRegex;
            QuarterRegexYearFront = GermanDatePeriodExtractorConfiguration.QuarterRegexYearFront;
            AllHalfYearRegex = GermanDatePeriodExtractorConfiguration.AllHalfYearRegex;
            SeasonRegex = GermanDatePeriodExtractorConfiguration.SeasonRegex;
            WhichWeekRegex = GermanDatePeriodExtractorConfiguration.WhichWeekRegex;
            WeekOfRegex = GermanDatePeriodExtractorConfiguration.WeekOfRegex;
            MonthOfRegex = GermanDatePeriodExtractorConfiguration.MonthOfRegex;
            RestOfDateRegex = GermanDatePeriodExtractorConfiguration.RestOfDateRegex;
            LaterEarlyPeriodRegex = GermanDatePeriodExtractorConfiguration.LaterEarlyPeriodRegex;
            WeekWithWeekDayRangeRegex = GermanDatePeriodExtractorConfiguration.WeekWithWeekDayRangeRegex;
            YearPlusNumberRegex = GermanDatePeriodExtractorConfiguration.YearPlusNumberRegex;
            DecadeWithCenturyRegex = GermanDatePeriodExtractorConfiguration.DecadeWithCenturyRegex;
            YearPeriodRegex = GermanDatePeriodExtractorConfiguration.YearPeriodRegex;
            ComplexDatePeriodRegex = GermanDatePeriodExtractorConfiguration.ComplexDatePeriodRegex;
            RelativeDecadeRegex = GermanDatePeriodExtractorConfiguration.RelativeDecadeRegex;
            InConnectorRegex = config.UtilityConfiguration.InConnectorRegex;
            WithinNextPrefixRegex = GermanDatePeriodExtractorConfiguration.WithinNextPrefixRegex;
            ReferenceDatePeriodRegex = GermanDatePeriodExtractorConfiguration.ReferenceDatePeriodRegex;
            AgoRegex = GermanDatePeriodExtractorConfiguration.AgoRegex;
            LaterRegex = GermanDatePeriodExtractorConfiguration.LaterRegex;
            LessThanRegex = GermanDatePeriodExtractorConfiguration.LessThanRegex;
            MoreThanRegex = GermanDatePeriodExtractorConfiguration.MoreThanRegex;
            CenturySuffixRegex = GermanDatePeriodExtractorConfiguration.CenturySuffixRegex;
            UnitMap = config.UnitMap;
            CardinalMap = config.CardinalMap;
            DayOfMonth = config.DayOfMonth;
            MonthOfYear = config.MonthOfYear;
            SeasonMap = config.SeasonMap;
            WrittenDecades = config.WrittenDecades;
            Numbers = config.Numbers;
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
            var trimmedText = text.Trim().ToLowerInvariant();
            var swift = 0;
            if (NextPrefixRegex.IsMatch(trimmedText))
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
            var trimmedText = text.Trim().ToLowerInvariant();
            var swift = -10;
            if (NextPrefixRegex.IsMatch(trimmedText))
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
            return DateTimeDefinitions.MonthTerms.Any(o => trimmedText.EndsWith(o));
        }

        public bool IsMonthToDate(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            return DateTimeDefinitions.MonthToDateTerms.Any(o => trimmedText.Equals(o));
        }

        public bool IsWeekend(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            return DateTimeDefinitions.WeekendTerms.Any(o => trimmedText.EndsWith(o));
        }

        public bool IsWeekOnly(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            return DateTimeDefinitions.WeekTerms.Any(o => trimmedText.EndsWith(o));
        }

        public bool IsYearOnly(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            return DateTimeDefinitions.YearTerms.Any(o => trimmedText.EndsWith(o));
        }

        public bool IsYearToDate(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            return DateTimeDefinitions.YearToDateTerms.Any(o => trimmedText.Equals(o));
        }
    }
}
