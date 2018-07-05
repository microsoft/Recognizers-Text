using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.German;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanDatePeriodParserConfiguration : BaseOptionsConfiguration,IDatePeriodParserConfiguration
    {
        public int MinYearNum { get; }

        public int MaxYearNum { get; }

        public string TokenBeforeDate { get; }

        #region internalParsers

        public IDateTimeExtractor DateExtractor { get; }

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

        public static readonly Regex NextPrefixRegex =
            new Regex(
                DateTimeDefinitions.NextPrefixRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex PastPrefixRegex =
            new Regex(
                DateTimeDefinitions.PastPrefixRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex ThisPrefixRegex =
            new Regex(
                DateTimeDefinitions.ThisPrefixRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        Regex IDatePeriodParserConfiguration.NextPrefixRegex => NextPrefixRegex;
        Regex IDatePeriodParserConfiguration.PastPrefixRegex => PastPrefixRegex;
        Regex IDatePeriodParserConfiguration.ThisPrefixRegex => ThisPrefixRegex;
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

        public IImmutableList<string> InStringList { get; }

        public GermanDatePeriodParserConfiguration(ICommonDateTimeParserConfiguration config) : base(config.Options)
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
            PastRegex = GermanDatePeriodExtractorConfiguration.PastPrefixRegex;
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
            WeekOfRegex= GermanDatePeriodExtractorConfiguration.WeekOfRegex;
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

        public int GetSwiftDayOrMonth(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            var swift = 0;
            if (NextPrefixRegex.IsMatch(trimedText))
            {
                swift = 1;
            }
            else if (PastPrefixRegex.IsMatch(trimedText))
            {
                swift = -1;
            }
            return swift;
        }

        public int GetSwiftYear(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            var swift = -10;
            if (NextPrefixRegex.IsMatch(trimedText))
            {
                swift = 1;
            }
            else if (PastPrefixRegex.IsMatch(trimedText))
            {
                swift = -1;
            }
            else if (ThisPrefixRegex.IsMatch(trimedText))
            {
                swift = 0;
            }
            return swift;
        }

        public bool IsFuture(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return (trimedText.StartsWith("dieser") || trimedText.StartsWith("diesen") || trimedText.StartsWith("dieses") || trimedText.StartsWith("diese") || 
                trimedText.StartsWith("nächster") || trimedText.StartsWith("nächstes") || trimedText.StartsWith("nächsten") || trimedText.StartsWith("nächste"));
        }

        public bool IsLastCardinal(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return (trimedText.Equals("letzter") || trimedText.Equals("letztes") || trimedText.Equals("letzten"));
        }

        public bool IsMonthOnly(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return (trimedText.EndsWith("monat") || trimedText.EndsWith("monate") || trimedText.EndsWith("monaten") || trimedText.EndsWith("monats"));
        }

        public bool IsMonthToDate(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return trimedText.Equals("month to date");
        }

        public bool IsWeekend(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return (trimedText.EndsWith("wochenende") || trimedText.EndsWith("wochenendes"));
        }

        public bool IsWeekOnly(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return trimedText.EndsWith("woche");
        }

        public bool IsYearOnly(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return (trimedText.EndsWith("jahr") || trimedText.EndsWith("jahre") || trimedText.EndsWith("jahren") || trimedText.EndsWith("jahres"));
        }

        public bool IsYearToDate(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return trimedText.Equals("year to date");
        }
    }
}
