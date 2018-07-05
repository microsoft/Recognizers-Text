using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.French;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchDatePeriodParserConfiguration : BaseOptionsConfiguration, IDatePeriodParserConfiguration
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
                @"(prochain|prochaine)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex PastPrefixRegex =
            new Regex(
                @"(dernier)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex ThisPrefixRegex =
            new Regex(
                @"(ce|cette)\b",
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

        public FrenchDatePeriodParserConfiguration(ICommonDateTimeParserConfiguration config) : base(config.Options)
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
            UnitMap = config.UnitMap;
            CardinalMap = config.CardinalMap;
            DayOfMonth = config.DayOfMonth;
            MonthOfYear = config.MonthOfYear;
            SeasonMap = config.SeasonMap;
            WrittenDecades = config.WrittenDecades;
            SpecialDecadeCases = config.SpecialDecadeCases;
        }
        public int GetSwiftDayOrMonth(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            var swift = 0;

            //TODO: Replace with a regex
            //TODO: Add 'upcoming' key word
            
            // example: "nous serons ensemble cette fois la semaine prochaine" - "We'll be together this time next week"
            if (trimedText.EndsWith("prochain") || trimedText.EndsWith("prochaine"))
            {
                swift = 1;
            }

            //TODO: Replace with a regex

            // example: Je l'ai vue pas plus tard que la semaine derniere - "I saw her only last week" 
            if (trimedText.EndsWith("dernière") || trimedText.EndsWith("dernières") ||
                trimedText.EndsWith("derniere") || trimedText.EndsWith("dernieres")) 
            {
                swift = -1;
            }
            return swift;
        }

        public int GetSwiftYear(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            var swift = -10;
            if (trimedText.EndsWith("prochain") || trimedText.EndsWith("prochaine"))
            {
                swift = 1;
            }

            if (trimedText.EndsWith("dernières") || trimedText.EndsWith("dernière") ||
                trimedText.EndsWith("dernieres") || trimedText.EndsWith("derniere") || trimedText.EndsWith("dernier"))
            {
                swift = -1;
            }
            else if (trimedText.StartsWith("cette"))
            {
                swift = 0;
            }

            return swift;
        }

        public bool IsFuture(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return (
                trimedText.StartsWith("cette") ||
                trimedText.EndsWith("prochaine") || trimedText.EndsWith("prochain"));
        }

        public bool IsLastCardinal(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return (
                trimedText.Equals("dernières") || trimedText.Equals("dernière") ||
                trimedText.Equals("dernieres") || trimedText.Equals("derniere")||trimedText.Equals("dernier"));
        }

        public bool IsMonthOnly(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return trimedText.EndsWith("mois");
        }

        public bool IsMonthToDate(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return trimedText.Equals("mois à ce jour");
        }

        public bool IsWeekend(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return (trimedText.EndsWith("fin de semaine") || trimedText.EndsWith("le weekend"));
        }

        public bool IsWeekOnly(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return (trimedText.EndsWith("semaine") && !trimedText.EndsWith("fin de semaine"));
        }

        public bool IsYearOnly(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return (trimedText.EndsWith("années") || trimedText.EndsWith("ans")
                || (trimedText.EndsWith("l'annees") || trimedText.EndsWith("l'annee"))
                );
        }

        public bool IsYearToDate(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return (trimedText.Equals("année à ce jour") || trimedText.Equals("an à ce jour"));
        }
    }
}
