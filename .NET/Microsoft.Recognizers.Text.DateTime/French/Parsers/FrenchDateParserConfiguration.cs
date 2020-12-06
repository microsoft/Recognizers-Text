using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.French;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchDateParserConfiguration : BaseDateTimeOptionsConfiguration, IDateParserConfiguration
    {
        public FrenchDateParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            DateTokenPrefix = DateTimeDefinitions.DateTokenPrefix;
            IntegerExtractor = config.IntegerExtractor;
            OrdinalExtractor = config.OrdinalExtractor;
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            DurationExtractor = config.DurationExtractor;
            DateExtractor = config.DateExtractor;
            DurationParser = config.DurationParser;
            HolidayParser = new BaseHolidayParser(new FrenchHolidayParserConfiguration(this));

            DateRegexes = new FrenchDateExtractorConfiguration(this).DateRegexList;
            OnRegex = FrenchDateExtractorConfiguration.OnRegex;
            SpecialDayRegex = FrenchDateExtractorConfiguration.SpecialDayRegex;
            SpecialDayWithNumRegex = FrenchDateExtractorConfiguration.SpecialDayWithNumRegex;
            NextRegex = FrenchDateExtractorConfiguration.NextDateRegex;
            ThisRegex = FrenchDateExtractorConfiguration.ThisRegex;
            LastRegex = FrenchDateExtractorConfiguration.LastDateRegex;
            UnitRegex = FrenchDateExtractorConfiguration.DateUnitRegex;
            WeekDayRegex = FrenchDateExtractorConfiguration.WeekDayRegex;
            StrictWeekDay = FrenchDateExtractorConfiguration.StrictWeekDay;
            MonthRegex = FrenchDateExtractorConfiguration.MonthRegex;
            WeekDayOfMonthRegex = FrenchDateExtractorConfiguration.WeekDayOfMonthRegex;
            ForTheRegex = FrenchDateExtractorConfiguration.ForTheRegex;
            WeekDayAndDayOfMothRegex = FrenchDateExtractorConfiguration.WeekDayAndDayOfMothRegex;
            WeekDayAndDayRegex = FrenchDateExtractorConfiguration.WeekDayAndDayRegex;
            RelativeMonthRegex = FrenchDateExtractorConfiguration.RelativeMonthRegex;
            StrictRelativeRegex = FrenchDateExtractorConfiguration.StrictRelativeRegex;
            YearSuffix = FrenchDateExtractorConfiguration.YearSuffix;
            BeforeAfterRegex = FrenchDateExtractorConfiguration.BeforeAfterRegex;
            RelativeWeekDayRegex = FrenchDateExtractorConfiguration.RelativeWeekDayRegex;
            RelativeDayRegex = RegexCache.Get(DateTimeDefinitions.RelativeDayRegex, RegexOptions.Singleline);
            NextPrefixRegex = RegexCache.Get(DateTimeDefinitions.NextPrefixRegex, RegexOptions.Singleline);
            PreviousPrefixRegex = RegexCache.Get(DateTimeDefinitions.PreviousPrefixRegex, RegexOptions.Singleline);
            UpcomingPrefixRegex = RegexCache.Get(DateTimeDefinitions.UpcomingPrefixRegex, RegexOptions.Singleline);
            PastPrefixRegex = RegexCache.Get(DateTimeDefinitions.PastPrefixRegex, RegexOptions.Singleline);

            DayOfMonth = config.DayOfMonth;
            DayOfWeek = config.DayOfWeek;
            MonthOfYear = config.MonthOfYear;
            CardinalMap = config.CardinalMap;
            UnitMap = config.UnitMap;
            UtilityConfiguration = config.UtilityConfiguration;
            SameDayTerms = DateTimeDefinitions.SameDayTerms.ToImmutableList();
            PlusOneDayTerms = DateTimeDefinitions.PlusOneDayTerms.ToImmutableList();
            PlusTwoDayTerms = DateTimeDefinitions.PlusTwoDayTerms.ToImmutableList();
            MinusOneDayTerms = DateTimeDefinitions.MinusOneDayTerms.ToImmutableList();
            MinusTwoDayTerms = DateTimeDefinitions.MinusTwoDayTerms.ToImmutableList();
        }

        public string DateTokenPrefix { get; }

        public IExtractor IntegerExtractor { get; }

        public IExtractor OrdinalExtractor { get; }

        public IExtractor CardinalExtractor { get; }

        public IParser NumberParser { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateExtractor DateExtractor { get; }

        public IDateTimeParser DurationParser { get; }

        public IDateTimeParser HolidayParser { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IEnumerable<Regex> DateRegexes { get; }

        public Regex OnRegex { get; }

        public Regex SpecialDayRegex { get; }

        public Regex SpecialDayWithNumRegex { get; }

        public Regex NextRegex { get; }

        public Regex ThisRegex { get; }

        public Regex LastRegex { get; }

        public Regex UnitRegex { get; }

        public Regex WeekDayRegex { get; }

        public Regex StrictWeekDay { get; }

        public Regex MonthRegex { get; }

        public Regex WeekDayOfMonthRegex { get; }

        public Regex ForTheRegex { get; }

        public Regex WeekDayAndDayOfMothRegex { get; }

        public Regex WeekDayAndDayRegex { get; }

        public Regex RelativeMonthRegex { get; }

        public Regex StrictRelativeRegex { get; }

        public Regex YearSuffix { get; }

        public Regex RelativeWeekDayRegex { get; }

        public Regex RelativeDayRegex { get; }

        public Regex NextPrefixRegex { get; }

        public Regex PreviousPrefixRegex { get; }

        public Regex UpcomingPrefixRegex { get; }

        public Regex PastPrefixRegex { get; }

        public Regex BeforeAfterRegex { get; }

        public IImmutableDictionary<string, int> DayOfMonth { get; }

        public IImmutableDictionary<string, int> DayOfWeek { get; }

        public IImmutableDictionary<string, int> MonthOfYear { get; }

        public IImmutableDictionary<string, int> CardinalMap { get; }

        public IImmutableList<string> SameDayTerms { get; }

        public IImmutableList<string> PlusOneDayTerms { get; }

        public IImmutableList<string> MinusOneDayTerms { get; }

        public IImmutableList<string> PlusTwoDayTerms { get; }

        public IImmutableList<string> MinusTwoDayTerms { get; }

        bool IDateParserConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public static int GetSwiftDay(string text)
        {
            var trimmedText = text.Trim();

            var swift = 0;

            // @TODO move hardcoded values to resource files

            // today
            if (trimmedText.Equals("aujourd'hui", StringComparison.Ordinal) ||
                trimmedText.Equals("auj", StringComparison.Ordinal))
            {
                swift = 0;
            }
            else if (trimmedText.Equals("demain", StringComparison.Ordinal) ||
                     trimmedText.Equals("a2m1", StringComparison.Ordinal) ||
                     trimmedText.Equals("lendemain", StringComparison.Ordinal) ||
                     trimmedText.Equals("jour suivant", StringComparison.Ordinal))
            {
                swift = 1;
            } // yesterday
            else if (trimmedText.Equals("hier", StringComparison.Ordinal))
            {
                swift = -1;
            }
            else if (trimmedText.EndsWith("après demain", StringComparison.Ordinal) || // day after tomorrow
                     trimmedText.EndsWith("après-demain", StringComparison.Ordinal))
            {
                swift = 2;
            }
            else if (trimmedText.StartsWith("avant-hier", StringComparison.Ordinal) || // day before yesterday
                     trimmedText.StartsWith("avant hier", StringComparison.Ordinal))
            {
                swift = -2;
            } // dernier
            else if (trimmedText.EndsWith("dernier", StringComparison.Ordinal))
            {
                swift = -1;
            }

            return swift;
        }

        public int GetSwiftMonthOrYear(string text)
        {
            var trimmedText = text.Trim();

            var swift = 0;

            // @TODO move hardcoded values to resource files

            if (trimmedText.EndsWith("prochaine", StringComparison.Ordinal) ||
                trimmedText.EndsWith("prochain", StringComparison.Ordinal))
            {
                swift = 1;
            }
            else if (trimmedText.Equals("dernière", StringComparison.Ordinal) ||
                     trimmedText.Equals("dernières", StringComparison.Ordinal) ||
                     trimmedText.Equals("derniere", StringComparison.Ordinal) ||
                     trimmedText.Equals("dernieres", StringComparison.Ordinal))
            {
                swift = -1;
            }

            return swift;
        }

        public bool IsCardinalLast(string text)
        {
            var trimmedText = text.Trim();

            // @TODO move hardcoded values to resource files

            return trimmedText.Equals("dernière", StringComparison.Ordinal) ||
                   trimmedText.Equals("dernières", StringComparison.Ordinal) ||
                   trimmedText.Equals("derniere", StringComparison.Ordinal) ||
                   trimmedText.Equals("dernieres", StringComparison.Ordinal);
        }

        public string Normalize(string text)
        {
            return text;
        }
    }
}
