﻿using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Resources.English;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishDateTimePeriodParserConfiguration : IDateTimePeriodParserConfiguration
    {
        public IExtractor DateExtractor { get; }

        public IExtractor TimeExtractor { get; }

        public IExtractor DateTimeExtractor { get; }

        public IExtractor CardinalExtractor { get; }

        public IExtractor DurationExtractor { get; }

        public IParser NumberParser { get; }

        public IDateTimeParser DateParser { get; }

        public IDateTimeParser TimeParser { get; }

        public IDateTimeParser DateTimeParser { get; }

        public IDateTimeParser DurationParser { get; }

        public Regex PureNumberFromToRegex { get; }

        public Regex PureNumberBetweenAndRegex { get; }

        public Regex SpecificTimeOfDayRegex { get; }

        public Regex TimeOfDayRegex { get; }

        public Regex PastRegex { get; }

        public Regex FutureRegex { get; }

        public Regex NumberCombinedWithUnitRegex { get; }

        public Regex UnitRegex { get; }

        public Regex PeriodTimeOfDayWithDateRegex { get; }

        public Regex RelativeTimeUnitRegex { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public EnglishDateTimePeriodParserConfiguration(ICommonDateTimeParserConfiguration config)
        {
            DateExtractor = config.DateExtractor;
            TimeExtractor = config.TimeExtractor;
            DateTimeExtractor = config.DateTimeExtractor;
            CardinalExtractor = config.CardinalExtractor;
            DurationExtractor = config.DurationExtractor;
            NumberParser = config.NumberParser;
            DateParser = config.DateParser;
            TimeParser = config.TimeParser;
            DurationParser = config.DurationParser;
            DateTimeParser = config.DateTimeParser;
            PureNumberFromToRegex = EnglishTimePeriodExtractorConfiguration.PureNumFromTo;
            PureNumberBetweenAndRegex = EnglishTimePeriodExtractorConfiguration.PureNumBetweenAnd;
            SpecificTimeOfDayRegex = EnglishDateTimeExtractorConfiguration.SpecificTimeOfDayRegex;
            TimeOfDayRegex = EnglishDateTimeExtractorConfiguration.TimeOfDayRegex;
            PastRegex = EnglishDatePeriodExtractorConfiguration.PastPrefixRegex;
            FutureRegex = EnglishDatePeriodExtractorConfiguration.NextPrefixRegex;
            NumberCombinedWithUnitRegex = EnglishDateTimePeriodExtractorConfiguration.TimeNumberCombinedWithUnit;
            UnitRegex = EnglishTimePeriodExtractorConfiguration.TimeUnitRegex;
            PeriodTimeOfDayWithDateRegex = EnglishDateTimePeriodExtractorConfiguration.PeriodTimeOfDayWithDateRegex;
            RelativeTimeUnitRegex = EnglishDateTimePeriodExtractorConfiguration.RelativeTimeUnitRegex;
            UnitMap = config.UnitMap;
            Numbers = config.Numbers;
        }

        public static readonly Regex MorningStartEndRegex = new Regex(DateTimeDefinitions.MorningStartEndRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex AfternoonStartEndRegex = new Regex(DateTimeDefinitions.AfternoonStartEndRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex EveningStartEndRegex = new Regex(DateTimeDefinitions.EveningStartEndRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex NightStartEndRegex = new Regex(DateTimeDefinitions.NightStartEndRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public bool GetMatchedTimeRange(string text, out string timeStr, out int beginHour, out int endHour, out int endMin)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            beginHour = 0;
            endHour = 0;
            endMin = 0;
            if (MorningStartEndRegex.IsMatch(trimedText))
            {
                timeStr = "TMO";
                beginHour = 8;
                endHour = 12;
            }
            else if (AfternoonStartEndRegex.IsMatch(trimedText))
            {
                timeStr = "TAF";
                beginHour = 12;
                endHour = 16;
            }
            else if (EveningStartEndRegex.IsMatch(trimedText))
            {
                timeStr = "TEV";
                beginHour = 16;
                endHour = 20;
            }
            else if (NightStartEndRegex.IsMatch(trimedText))
            {
                timeStr = "TNI";
                beginHour = 20;
                endHour = 23;
                endMin = 59;
            }
            else
            {
                timeStr = null;
                return false;
            }
            return true;
        }

        public int GetSwiftPrefix(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            var swift = 0;
            if (trimedText.StartsWith("next"))
            {
                swift = 1;
            }
            else if (trimedText.StartsWith("last"))
            {
                swift = -1;
            }
            return swift;
        }
    }
}
