using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishDatePeriodParserConfiguration : IDatePeriodParserConfiguration
    {
        public string TokenBeforeDate { get; }

        #region internalsParsers

        public IExtractor DateExtractor { get; }

        public IExtractor CardinalExtractor { get; }

        public IParser NumberParser { get; }

        public IDateTimeParser DateParser { get; }

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

        #endregion

        #region Dictionaries
        public IImmutableDictionary<string, string> UnitMap { get; }

        public IImmutableDictionary<string, int> CardinalMap { get; }

        public IImmutableDictionary<string, int> DayOfMonth { get; }

        public IImmutableDictionary<string, int> MonthOfYear { get; }

        public IImmutableDictionary<string, string> SeasonMap { get; }

        #endregion

        public SpanishDatePeriodParserConfiguration(ICommonDateTimeParserConfiguration config)
        {
            TokenBeforeDate = "en ";
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            DateExtractor = config.DateExtractor;
            DateParser = config.DateParser;
            MonthFrontBetweenRegex = SpanishDatePeriodExtractorConfiguration.MonthFrontBetweenRegex;
            BetweenRegex = SpanishDatePeriodExtractorConfiguration.BetweenRegex;
            MonthFrontSimpleCasesRegex = SpanishDatePeriodExtractorConfiguration.MonthFrontSimpleCasesRegex;
            SimpleCasesRegex = SpanishDatePeriodExtractorConfiguration.SimpleCasesRegex;
            OneWordPeriodRegex = SpanishDatePeriodExtractorConfiguration.OneWordPeriodRegex;
            MonthWithYear = SpanishDatePeriodExtractorConfiguration.MonthWithYear;
            MonthNumWithYear = SpanishDatePeriodExtractorConfiguration.MonthNumWithYear;
            YearRegex = SpanishDatePeriodExtractorConfiguration.YearRegex;
            PastRegex = SpanishDatePeriodExtractorConfiguration.PastRegex;
            FutureRegex = SpanishDatePeriodExtractorConfiguration.FutureRegex;
            NumberCombinedWithUnit = SpanishDurationExtractorConfiguration.NumberCombinedWithUnit;
            WeekOfMonthRegex = SpanishDatePeriodExtractorConfiguration.WeekOfMonthRegex;
            WeekOfYearRegex = SpanishDatePeriodExtractorConfiguration.WeekOfYearRegex;
            QuarterRegex = SpanishDatePeriodExtractorConfiguration.QuarterRegex;
            QuarterRegexYearFront = SpanishDatePeriodExtractorConfiguration.QuarterRegexYearFront;
            SeasonRegex = SpanishDatePeriodExtractorConfiguration.SeasonRegex;
            UnitMap = config.UnitMap;
            CardinalMap = config.CardinalMap;
            DayOfMonth = config.DayOfMonth;
            MonthOfYear = config.MonthOfYear;
            SeasonMap = config.SeasonMap;
        }

        public int GetSwiftDay(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            var swift = 0;
            
            //TODO: Replace with a regex
            if (trimedText.StartsWith("proximo") || trimedText.StartsWith("próximo") ||
                trimedText.StartsWith("proxima") || trimedText.StartsWith("próxima"))
            {
                swift = 1;
            }

            //TODO: Replace with a regex
            if (trimedText.StartsWith("ultimo") || trimedText.StartsWith("último") ||
                trimedText.StartsWith("ultima") || trimedText.StartsWith("última"))
            {
                swift = -1;
            }
            return swift;
        }

        public int GetSwiftMonth(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            var swift = 0;

            //TODO: Replace with a regex
            if (trimedText.StartsWith("proximo") || trimedText.StartsWith("próximo") ||
                trimedText.StartsWith("proxima") || trimedText.StartsWith("próxima"))
            {
                swift = 1;
            }

            //TODO: Replace with a regex
            if (trimedText.StartsWith("ultimo") || trimedText.StartsWith("último") ||
                trimedText.StartsWith("ultima") || trimedText.StartsWith("última"))
            {
                swift = -1;
            }
            return swift;
        }

        public int GetSwiftYear(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            var swift = -10;
            if (trimedText.StartsWith("proximo") || trimedText.StartsWith("próximo") ||
                trimedText.StartsWith("proxima") || trimedText.StartsWith("próxima"))
            {
                swift = 1;
            }
            if (trimedText.StartsWith("ultimo") || trimedText.StartsWith("último") ||
                trimedText.StartsWith("ultima") || trimedText.StartsWith("última"))
            {
                swift = -1;
            }
            else if (trimedText.StartsWith("este") || trimedText.StartsWith("esta"))
            {
                swift = 0;
            }
            return swift;
        }

        public bool IsFuture(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return (
                trimedText.StartsWith("este") ||
                trimedText.StartsWith("proximo") || trimedText.StartsWith("próximo") ||
                trimedText.StartsWith("proxima") || trimedText.StartsWith("próxima"));
        }

        public bool IsLastCardinal(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return (
                trimedText.Equals("ultimo") || trimedText.Equals("último") ||
                trimedText.Equals("ultima") || trimedText.Equals("última"));
        }

        public bool IsMonthOnly(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return (trimedText.EndsWith("mes") || trimedText.EndsWith("meses"));
        }

        public bool IsMonthToDate(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return (trimedText.Equals("mes a la fecha") || trimedText.Equals("meses a la fecha"));
        }

        public bool IsWeekend(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return trimedText.EndsWith("fin de semana");
        }

        public bool IsWeekOnly(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return (trimedText.EndsWith("semana") && !trimedText.EndsWith("fin de semana"));
        }

        public bool IsYearOnly(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return (trimedText.EndsWith("año") || trimedText.EndsWith("años"));
        }

        public bool IsYearToDate(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return (trimedText.Equals("año a la fecha") || trimedText.Equals("años a la fecha"));
        }
    }
}
