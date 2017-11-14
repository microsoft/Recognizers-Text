using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.English;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishDatePeriodParserConfiguration : IDatePeriodParserConfiguration
    {
        public string TokenBeforeDate { get; }

        #region internalParsers

        public IDateTimeExtractor DateExtractor { get; }

        public IExtractor CardinalExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

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
        public Regex NumberCombinedWithUnit { get; }
        public Regex WeekOfMonthRegex { get; }
        public Regex WeekOfYearRegex { get; }
        public Regex QuarterRegex { get; }
        public Regex QuarterRegexYearFront { get; }
        public Regex SeasonRegex { get; }
        public Regex WhichWeekRegex { get; }
        public Regex WeekOfRegex { get; }
        public Regex MonthOfRegex { get; }
        public Regex InConnectorRegex { get; }
        public Regex RestOfDateRegex { get; }
        public Regex LaterEarlyPeriodRegex { get; }
        public Regex WeekWithWeekDayRangeRegex { get; }

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
        #endregion

        public IImmutableList<string> InStringList { get; }

        public EnglishDatePeriodParserConfiguration(ICommonDateTimeParserConfiguration config)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            DateExtractor = config.DateExtractor;
            DurationExtractor = config.DurationExtractor;
            DurationParser = config.DurationParser;
            DateParser = config.DateParser;
            MonthFrontBetweenRegex = EnglishDatePeriodExtractorConfiguration.MonthFrontBetweenRegex;
            BetweenRegex = EnglishDatePeriodExtractorConfiguration.BetweenRegex;
            MonthFrontSimpleCasesRegex = EnglishDatePeriodExtractorConfiguration.MonthFrontSimpleCasesRegex;
            SimpleCasesRegex = EnglishDatePeriodExtractorConfiguration.SimpleCasesRegex;
            OneWordPeriodRegex = EnglishDatePeriodExtractorConfiguration.OneWordPeriodRegex;
            MonthWithYear = EnglishDatePeriodExtractorConfiguration.MonthWithYear;
            MonthNumWithYear = EnglishDatePeriodExtractorConfiguration.MonthNumWithYear;
            YearRegex = EnglishDatePeriodExtractorConfiguration.YearRegex;
            PastRegex = EnglishDatePeriodExtractorConfiguration.PastPrefixRegex;
            FutureRegex = EnglishDatePeriodExtractorConfiguration.NextPrefixRegex;
            NumberCombinedWithUnit = EnglishDurationExtractorConfiguration.NumberCombinedWithDurationUnit;
            WeekOfMonthRegex = EnglishDatePeriodExtractorConfiguration.WeekOfMonthRegex;
            WeekOfYearRegex = EnglishDatePeriodExtractorConfiguration.WeekOfYearRegex;
            QuarterRegex = EnglishDatePeriodExtractorConfiguration.QuarterRegex;
            QuarterRegexYearFront = EnglishDatePeriodExtractorConfiguration.QuarterRegexYearFront;
            SeasonRegex = EnglishDatePeriodExtractorConfiguration.SeasonRegex;
            WhichWeekRegex = EnglishDatePeriodExtractorConfiguration.WhichWeekRegex;
            WeekOfRegex= EnglishDatePeriodExtractorConfiguration.WeekOfRegex;
            MonthOfRegex = EnglishDatePeriodExtractorConfiguration.MonthOfRegex;
            RestOfDateRegex = EnglishDatePeriodExtractorConfiguration.RestOfDateRegex;
            LaterEarlyPeriodRegex = EnglishDatePeriodExtractorConfiguration.LaterEarlyPeriodRegex;
            WeekWithWeekDayRangeRegex = EnglishDatePeriodExtractorConfiguration.WeekWithWeekDayRangeRegex;
            InConnectorRegex = config.UtilityConfiguration.InConnectorRegex;
            UnitMap = config.UnitMap;
            CardinalMap = config.CardinalMap;
            DayOfMonth = config.DayOfMonth;
            MonthOfYear = config.MonthOfYear;
            SeasonMap = config.SeasonMap;
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
            return (trimedText.StartsWith("this") || trimedText.StartsWith("next"));
        }

        public bool IsLastCardinal(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return trimedText.Equals("last");
        }

        public bool IsMonthOnly(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return trimedText.EndsWith("month");
        }

        public bool IsMonthToDate(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return trimedText.Equals("month to date");
        }

        public bool IsWeekend(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return trimedText.EndsWith("weekend");
        }

        public bool IsWeekOnly(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return trimedText.EndsWith("week");
        }

        public bool IsYearOnly(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return trimedText.EndsWith("year");
        }

        public bool IsYearToDate(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return trimedText.Equals("year to date");
        }
    }
}
