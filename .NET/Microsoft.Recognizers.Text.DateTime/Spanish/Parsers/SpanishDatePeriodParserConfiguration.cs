using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Spanish;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishDatePeriodParserConfiguration : BaseOptionsConfiguration, IDatePeriodParserConfiguration
    {
        public int MinYearNum { get; }

        public int MaxYearNum { get; }

        public string TokenBeforeDate { get; }

        #region internalParsers

        public IDateExtractor DateExtractor { get; }

        public IExtractor CardinalExtractor { get; }

        public IExtractor OrdinalExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IExtractor IntegerExtractor { get; }

        public IParser NumberParser { get; }

        public IDateTimeParser DateParser { get; }

        public IDateTimeParser DurationParser { get; }

        #endregion

        #region Regexes

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

        //TODO: config this according to English
        public static readonly Regex NextPrefixRegex = 
            new Regex(DateTimeDefinitions.NextPrefixRegex, RegexOptions.Singleline);

        public static readonly Regex PastPrefixRegex = 
            new Regex(DateTimeDefinitions.PastPrefixRegex, RegexOptions.Singleline);

        public static readonly Regex ThisPrefixRegex = 
            new Regex(DateTimeDefinitions.ThisPrefixRegex, RegexOptions.Singleline);

        public static readonly Regex RelativeRegex = 
            new Regex(DateTimeDefinitions.RelativeRegex, RegexOptions.Singleline);

        public static readonly Regex UnspecificEndOfRangeRegex =
            new Regex(DateTimeDefinitions.UnspecificEndOfRangeRegex, RegexOptions.Singleline);

        Regex IDatePeriodParserConfiguration.NextPrefixRegex => NextPrefixRegex;
        Regex IDatePeriodParserConfiguration.PastPrefixRegex => PastPrefixRegex;
        Regex IDatePeriodParserConfiguration.ThisPrefixRegex => ThisPrefixRegex;
        Regex IDatePeriodParserConfiguration.RelativeRegex => RelativeRegex;
        Regex IDatePeriodParserConfiguration.UnspecificEndOfRangeRegex => UnspecificEndOfRangeRegex;

        #endregion

        #region Dictionaries

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IImmutableDictionary<string, int> CardinalMap { get; }

        public IImmutableDictionary<string, int> DayOfMonth { get; }

        public IImmutableDictionary<string, int> MonthOfYear { get; }

        public IImmutableDictionary<string, string> SeasonMap { get; }

        public IImmutableDictionary<string, int> WrittenDecades { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public IImmutableDictionary<string, int> SpecialDecadeCases { get; }

        #endregion
        
        public SpanishDatePeriodParserConfiguration(ICommonDateTimeParserConfiguration config) : base(config)
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
            UnitMap = config.UnitMap;
            CardinalMap = config.CardinalMap;
            DayOfMonth = config.DayOfMonth;
            MonthOfYear = config.MonthOfYear;
            SeasonMap = config.SeasonMap;
            Numbers = config.Numbers;
            WrittenDecades = config.WrittenDecades;
            SpecialDecadeCases = config.SpecialDecadeCases;
        }

        public int GetSwiftDayOrMonth(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            var swift = 0;

            if (NextPrefixRegex.IsMatch(trimmedText))
            {
                swift = 1;
            }

            if (PastPrefixRegex.IsMatch(trimmedText))
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

            if (PastPrefixRegex.IsMatch(trimmedText))
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
            return ThisPrefixRegex.IsMatch(trimmedText) || NextPrefixRegex.IsMatch(trimmedText);
        }

        public bool IsLastCardinal(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            return PastPrefixRegex.IsMatch(trimmedText);
        }

        public bool IsMonthOnly(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            return (trimmedText.EndsWith("mes") || trimmedText.EndsWith("meses"));
        }

        public bool IsMonthToDate(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            return (trimmedText.Equals("mes a la fecha") || trimmedText.Equals("meses a la fecha"));
        }

        public bool IsWeekend(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            return trimmedText.EndsWith("fin de semana");
        }

        public bool IsWeekOnly(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            return (trimmedText.EndsWith("semana") && !trimmedText.EndsWith("fin de semana"));
        }

        public bool IsYearOnly(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            return (trimmedText.EndsWith("año") || trimmedText.EndsWith("años"));
        }

        public bool IsYearToDate(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            return (trimmedText.Equals("año a la fecha") || trimmedText.Equals("años a la fecha"));
        }
    }
}
