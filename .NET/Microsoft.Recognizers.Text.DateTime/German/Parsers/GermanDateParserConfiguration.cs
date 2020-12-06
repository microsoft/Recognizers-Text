using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.German;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanDateParserConfiguration : BaseDateTimeOptionsConfiguration, IDateParserConfiguration
    {
        public GermanDateParserConfiguration(ICommonDateTimeParserConfiguration config)
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
            HolidayParser = new BaseHolidayParser(new GermanHolidayParserConfiguration(this));

            DateRegexes = new GermanDateExtractorConfiguration(this).DateRegexList;
            OnRegex = GermanDateExtractorConfiguration.OnRegex;
            SpecialDayRegex = GermanDateExtractorConfiguration.SpecialDayRegex;
            SpecialDayWithNumRegex = GermanDateExtractorConfiguration.SpecialDayWithNumRegex;
            NextRegex = GermanDateExtractorConfiguration.NextDateRegex;
            ThisRegex = GermanDateExtractorConfiguration.ThisRegex;
            LastRegex = GermanDateExtractorConfiguration.LastDateRegex;
            UnitRegex = GermanDateExtractorConfiguration.DateUnitRegex;
            WeekDayRegex = GermanDateExtractorConfiguration.WeekDayRegex;
            MonthRegex = GermanDateExtractorConfiguration.MonthRegex;
            WeekDayOfMonthRegex = GermanDateExtractorConfiguration.WeekDayOfMonthRegex;
            ForTheRegex = GermanDateExtractorConfiguration.ForTheRegex;
            WeekDayAndDayOfMothRegex = GermanDateExtractorConfiguration.WeekDayAndDayOfMothRegex;
            WeekDayAndDayRegex = GermanDateExtractorConfiguration.WeekDayAndDayRegex;
            RelativeMonthRegex = GermanDateExtractorConfiguration.RelativeMonthRegex;
            StrictRelativeRegex = GermanDateExtractorConfiguration.StrictRelativeRegex;
            YearSuffix = GermanDateExtractorConfiguration.YearSuffix;
            BeforeAfterRegex = GermanDateExtractorConfiguration.BeforeAfterRegex;
            RelativeWeekDayRegex = GermanDateExtractorConfiguration.RelativeWeekDayRegex;
            RelativeDayRegex = RegexCache.Get(DateTimeDefinitions.RelativeDayRegex, RegexOptions.Singleline);
            NextPrefixRegex = RegexCache.Get(DateTimeDefinitions.NextPrefixRegex, RegexOptions.Singleline);
            AfterNextPrefixRegex = RegexCache.Get(DateTimeDefinitions.AfterNextPrefixRegex, RegexOptions.Singleline);
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

        public IEnumerable<Regex> DateRegexes { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public Regex OnRegex { get; }

        public Regex SpecialDayRegex { get; }

        public Regex SpecialDayWithNumRegex { get; }

        public Regex NextRegex { get; }

        public Regex ThisRegex { get; }

        public Regex LastRegex { get; }

        public Regex UnitRegex { get; }

        public Regex WeekDayRegex { get; }

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

        public Regex AfterNextPrefixRegex { get; }

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

        public int GetSwiftMonthOrYear(string text)
        {
            var trimmedText = text.Trim();
            var swift = 0;

            var afterNextMatch = AfterNextPrefixRegex.Match(text);

            if (afterNextMatch.Success)
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

        public bool IsCardinalLast(string text)
        {
            var trimmedText = text.Trim();

            // @TODO move hardcoded values to resources file

            return trimmedText.Equals("letzten", StringComparison.Ordinal);
        }

        public string Normalize(string text)
        {
            return text;
        }
    }
}
