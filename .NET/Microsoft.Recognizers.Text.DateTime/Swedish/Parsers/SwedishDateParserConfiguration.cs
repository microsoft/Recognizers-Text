// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Swedish;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Swedish
{
    public class SwedishDateParserConfiguration : BaseDateTimeOptionsConfiguration, IDateParserConfiguration
    {
        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public SwedishDateParserConfiguration(ICommonDateTimeParserConfiguration config)
             : base(config)
        {
            DateTokenPrefix = DateTimeDefinitions.DateTokenPrefix;

            IntegerExtractor = config.IntegerExtractor;
            OrdinalExtractor = config.OrdinalExtractor;
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            DateExtractor = config.DateExtractor;
            DurationExtractor = config.DurationExtractor;
            DurationParser = config.DurationParser;
            HolidayParser = new BaseHolidayParser(new SwedishHolidayParserConfiguration(this));

            DateRegexes = new SwedishDateExtractorConfiguration(this).DateRegexList;
            OnRegex = SwedishDateExtractorConfiguration.OnRegex;
            SpecialDayRegex = SwedishDateExtractorConfiguration.SpecialDayRegex;
            SpecialDayWithNumRegex = SwedishDateExtractorConfiguration.SpecialDayWithNumRegex;
            NextRegex = SwedishDateExtractorConfiguration.NextDateRegex;
            ThisRegex = SwedishDateExtractorConfiguration.ThisRegex;
            LastRegex = SwedishDateExtractorConfiguration.LastDateRegex;
            UnitRegex = SwedishDateExtractorConfiguration.DateUnitRegex;
            WeekDayRegex = SwedishDateExtractorConfiguration.WeekDayRegex;
            MonthRegex = SwedishDateExtractorConfiguration.MonthRegex;
            WeekDayOfMonthRegex = SwedishDateExtractorConfiguration.WeekDayOfMonthRegex;
            ForTheRegex = SwedishDateExtractorConfiguration.ForTheRegex;
            WeekDayAndDayOfMothRegex = SwedishDateExtractorConfiguration.WeekDayAndDayOfMothRegex;
            WeekDayAndDayRegex = SwedishDateExtractorConfiguration.WeekDayAndDayRegex;
            RelativeMonthRegex = SwedishDateExtractorConfiguration.RelativeMonthRegex;
            StrictRelativeRegex = SwedishDateExtractorConfiguration.StrictRelativeRegex;
            YearSuffix = SwedishDateExtractorConfiguration.YearSuffix;
            RelativeWeekDayRegex = SwedishDateExtractorConfiguration.RelativeWeekDayRegex;
            BeforeAfterRegex = SwedishDateExtractorConfiguration.BeforeAfterRegex;

            RelativeDayRegex = new Regex(DateTimeDefinitions.RelativeDayRegex, RegexFlags);
            NextPrefixRegex = new Regex(DateTimeDefinitions.NextPrefixRegex, RegexFlags);
            PreviousPrefixRegex = new Regex(DateTimeDefinitions.PreviousPrefixRegex, RegexFlags);
            UpcomingPrefixRegex = new Regex(DateTimeDefinitions.UpcomingPrefixRegex, RegexFlags);
            PastPrefixRegex = new Regex(DateTimeDefinitions.PastPrefixRegex, RegexFlags);

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

        public Regex PreviousPrefixRegex { get; }

        public Regex UpcomingPrefixRegex { get; }

        public Regex PastPrefixRegex { get; }

        public Regex BeforeAfterRegex { get; }

        public Regex TasksModeDurationToDatePatterns { get; }

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

            if (NextPrefixRegex.IsMatch(trimmedText))
            {
                swift = 1;
            }

            if (PreviousPrefixRegex.IsMatch(trimmedText))
            {
                swift = -1;
            }

            return swift;
        }

        public bool IsCardinalLast(string text)
        {

            // @TODO move hardcoded values to resources file

            var trimmedText = text.Trim();

            return trimmedText.Equals("last", StringComparison.Ordinal);
        }

        public string Normalize(string text)
        {
            return text;
        }
    }
}
