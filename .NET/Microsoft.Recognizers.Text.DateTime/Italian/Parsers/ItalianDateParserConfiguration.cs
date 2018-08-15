﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Italian;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Italian
{
    public class ItalianDateParserConfiguration : BaseOptionsConfiguration, IDateParserConfiguration
    {
        public string DateTokenPrefix { get; }

        public IExtractor IntegerExtractor { get; }

        public IExtractor OrdinalExtractor { get; }

        public IExtractor CardinalExtractor { get; }

        public IParser NumberParser { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor DateExtractor { get; }

        public IDateTimeParser DurationParser { get; }

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

        public Regex RelativeMonthRegex { get; }

        public Regex YearSuffix { get; }

        public Regex RelativeWeekDayRegex { get; }

        public IImmutableDictionary<string, int> DayOfMonth { get; }

        public IImmutableDictionary<string, int> DayOfWeek { get; }

        public IImmutableDictionary<string, int> MonthOfYear { get; }

        public IImmutableDictionary<string, int> CardinalMap { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public ItalianDateParserConfiguration(ICommonDateTimeParserConfiguration config) : base(config.Options)
        {
            DateTokenPrefix = DateTimeDefinitions.DateTokenPrefix;
            IntegerExtractor = config.IntegerExtractor;
            OrdinalExtractor = config.OrdinalExtractor;
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            DurationExtractor = config.DurationExtractor;
            DateExtractor = config.DateExtractor;
            DurationParser = config.DurationParser;
            DateRegexes = ItalianDateExtractorConfiguration.DateRegexList;
            OnRegex = ItalianDateExtractorConfiguration.OnRegex;
            SpecialDayRegex = ItalianDateExtractorConfiguration.SpecialDayRegex;
            SpecialDayWithNumRegex = ItalianDateExtractorConfiguration.SpecialDayWithNumRegex;
            NextRegex = ItalianDateExtractorConfiguration.NextRegex;
            ThisRegex = ItalianDateExtractorConfiguration.ThisRegex;
            LastRegex = ItalianDateExtractorConfiguration.LastRegex;
            UnitRegex = ItalianDateExtractorConfiguration.DateUnitRegex;
            WeekDayRegex = ItalianDateExtractorConfiguration.WeekDayRegex;
            StrictWeekDay = ItalianDateExtractorConfiguration.StrictWeekDay;
            MonthRegex = ItalianDateExtractorConfiguration.MonthRegex;
            WeekDayOfMonthRegex = ItalianDateExtractorConfiguration.WeekDayOfMonthRegex;
            ForTheRegex = ItalianDateExtractorConfiguration.ForTheRegex;
            WeekDayAndDayOfMothRegex = ItalianDateExtractorConfiguration.WeekDayAndDayOfMothRegex;
            RelativeMonthRegex = ItalianDateExtractorConfiguration.RelativeMonthRegex;
            YearSuffix = ItalianDateExtractorConfiguration.YearSuffix;
            RelativeWeekDayRegex = ItalianDateExtractorConfiguration.RelativeWeekDayRegex;
            DayOfMonth = config.DayOfMonth;
            DayOfWeek = config.DayOfWeek;
            MonthOfYear = config.MonthOfYear;
            CardinalMap = config.CardinalMap;
            UnitMap = config.UnitMap;
            UtilityConfiguration = config.UtilityConfiguration;
        }

        public int GetSwiftDay(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();

            var swift = 0;
            if (trimedText.Equals("aujourd'hui") || trimedText.Equals("auj")) //today
            {
                swift = 0;
            }
            else if (trimedText.Equals("demain") || trimedText.Equals("a2m1") || 
                     trimedText.Equals("lendemain") || trimedText.Equals("jour suivant"))
            {
                swift = 1;
            }
            else if (trimedText.Equals("hier")) // yesterday
            {
                swift = -1;
            }
            else if (trimedText.EndsWith("après demain") || // day after tomorrow
                     trimedText.EndsWith("après-demain"))
            {
                swift = 2;
            }
            else if (trimedText.StartsWith("avant-hier") || // day before yesterday
                     trimedText.StartsWith("avant hier"))
            {
                swift = -2;
            }
            else if (trimedText.EndsWith("dernier")) // dernier
            {
                swift = -1;
            }
            return swift;
        }

        public int GetSwiftMonth(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            var swift = 0;
            if (trimedText.EndsWith("prochaine") || trimedText.EndsWith("prochain"))
            {
                swift = 1;
            }
            else if (trimedText.Equals("dernière") || trimedText.Equals("dernières") ||
                    trimedText.Equals("derniere") || trimedText.Equals("dernieres"))
            {
                swift = -1;
            }
            return swift;
        }

        public bool IsCardinalLast(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            return (trimedText.Equals("dernière") || trimedText.Equals("dernières") ||
                    trimedText.Equals("derniere") || trimedText.Equals("dernieres"));
        }
    }
}
