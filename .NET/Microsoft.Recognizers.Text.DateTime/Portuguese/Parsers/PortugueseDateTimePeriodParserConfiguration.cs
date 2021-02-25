using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Portuguese;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class PortugueseDateTimePeriodParserConfiguration : BaseDateTimeOptionsConfiguration, IDateTimePeriodParserConfiguration
    {
        public PortugueseDateTimePeriodParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;
            TokenBeforeTime = DateTimeDefinitions.TokenBeforeTime;

            DateExtractor = config.DateExtractor;
            TimeExtractor = config.TimeExtractor;
            DateTimeExtractor = config.DateTimeExtractor;
            TimePeriodExtractor = config.TimePeriodExtractor;
            CardinalExtractor = config.CardinalExtractor;
            DurationExtractor = config.DurationExtractor;
            NumberParser = config.NumberParser;
            DateParser = config.DateParser;
            TimeParser = config.TimeParser;
            DateTimeParser = config.DateTimeParser;
            TimePeriodParser = config.TimePeriodParser;
            DurationParser = config.DurationParser;
            TimeZoneParser = config.TimeZoneParser;

            PureNumberFromToRegex = PortugueseTimePeriodExtractorConfiguration.PureNumFromTo;
            HyphenDateRegex = PortugueseDateTimePeriodExtractorConfiguration.HyphenDateRegex;
            PureNumberBetweenAndRegex = PortugueseTimePeriodExtractorConfiguration.PureNumBetweenAnd;
            SpecificTimeOfDayRegex = PortugueseDateTimeExtractorConfiguration.SpecificTimeOfDayRegex;
            TimeOfDayRegex = PortugueseDateTimeExtractorConfiguration.TimeOfDayRegex;
            PreviousPrefixRegex = PortugueseDatePeriodExtractorConfiguration.PastRegex;
            FutureRegex = PortugueseDatePeriodExtractorConfiguration.FutureRegex;
            FutureSuffixRegex = PortugueseDatePeriodExtractorConfiguration.FutureSuffixRegex;
            NumberCombinedWithUnitRegex = PortugueseDateTimePeriodExtractorConfiguration.NumberCombinedWithUnit;
            UnitRegex = PortugueseTimePeriodExtractorConfiguration.UnitRegex;
            PeriodTimeOfDayWithDateRegex = PortugueseDateTimePeriodExtractorConfiguration.PeriodTimeOfDayWithDateRegex;
            RelativeTimeUnitRegex = PortugueseDateTimePeriodExtractorConfiguration.RelativeTimeUnitRegex;
            RestOfDateTimeRegex = PortugueseDateTimePeriodExtractorConfiguration.RestOfDateTimeRegex;
            AmDescRegex = PortugueseDateTimePeriodExtractorConfiguration.AmDescRegex;
            PmDescRegex = PortugueseDateTimePeriodExtractorConfiguration.PmDescRegex;
            WithinNextPrefixRegex = PortugueseDateTimePeriodExtractorConfiguration.WithinNextPrefixRegex;
            PrefixDayRegex = PortugueseDateTimePeriodExtractorConfiguration.PrefixDayRegex;
            BeforeRegex = PortugueseDateTimePeriodExtractorConfiguration.BeforeRegex;
            AfterRegex = PortugueseDateTimePeriodExtractorConfiguration.AfterRegex;
            UnitMap = config.UnitMap;
            Numbers = config.Numbers;
        }

        public string TokenBeforeDate { get; }

        public string TokenBeforeTime { get; }

        public IDateExtractor DateExtractor { get; }

        public IDateTimeExtractor TimeExtractor { get; }

        public IDateTimeExtractor DateTimeExtractor { get; }

        public IDateTimeExtractor TimePeriodExtractor { get; }

        public IExtractor CardinalExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IParser NumberParser { get; }

        public IDateTimeParser DateParser { get; }

        public IDateTimeParser TimeParser { get; }

        public IDateTimeParser DateTimeParser { get; }

        public IDateTimeParser TimePeriodParser { get; }

        public IDateTimeParser DurationParser { get; }

        public IDateTimeParser TimeZoneParser { get; }

        public Regex PureNumberFromToRegex { get; }

        public Regex HyphenDateRegex { get; }

        public Regex PureNumberBetweenAndRegex { get; }

        public Regex SpecificTimeOfDayRegex { get; }

        public Regex TimeOfDayRegex { get; }

        public Regex PreviousPrefixRegex { get; }

        public Regex FutureRegex { get; }

        public Regex FutureSuffixRegex { get; }

        public Regex NumberCombinedWithUnitRegex { get; }

        public Regex UnitRegex { get; }

        public Regex PeriodTimeOfDayWithDateRegex { get; }

        public Regex RelativeTimeUnitRegex { get; }

        public Regex RestOfDateTimeRegex { get; }

        public Regex AmDescRegex { get; }

        public Regex PmDescRegex { get; }

        public Regex WithinNextPrefixRegex { get; }

        public Regex PrefixDayRegex { get; }

        public Regex BeforeRegex { get; }

        public Regex AfterRegex { get; }

        bool IDateTimePeriodParserConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public bool GetMatchedTimeRange(string text, out string timeStr, out int beginHour, out int endHour, out int endMin)
        {
            var trimmedText = text.Trim().Normalized(DateTimeDefinitions.SpecialCharactersEquivalent);

            beginHour = 0;
            endHour = 0;
            endMin = 0;

            if (DateTimeDefinitions.EarlyMorningTermList.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal)) || DateTimeDefinitions.EarlyMorningTermList.Any(o => trimmedText.StartsWith(o, StringComparison.Ordinal)))
            {
                timeStr = Constants.EarlyMorning;
                beginHour = 4;
                endHour = 8;
            }
            else if (DateTimeDefinitions.MorningTermList.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal)) || DateTimeDefinitions.MorningTermList.Any(o => trimmedText.StartsWith(o, StringComparison.Ordinal)))
            {
                timeStr = Constants.Morning;
                beginHour = 8;
                endHour = Constants.HalfDayHourCount;
            }
            else if (DateTimeDefinitions.AfternoonTermList.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal)) || DateTimeDefinitions.AfternoonTermList.Any(o => trimmedText.StartsWith(o, StringComparison.Ordinal)))
            {
                timeStr = Constants.Afternoon;
                beginHour = Constants.HalfDayHourCount;
                endHour = 16;
            }
            else if (DateTimeDefinitions.EveningTermList.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal)) || DateTimeDefinitions.EveningTermList.Any(o => trimmedText.StartsWith(o, StringComparison.Ordinal)))
            {
                timeStr = Constants.Evening;
                beginHour = 16;
                endHour = 20;
            }
            else if (DateTimeDefinitions.NightTermList.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal)) || DateTimeDefinitions.NightTermList.Any(o => trimmedText.StartsWith(o, StringComparison.Ordinal)))
            {
                timeStr = Constants.Night;
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
            var trimmedText = text.Trim();
            var swift = 0;

            // @TODO move hardcoded values to resources file
            if (PortugueseDatePeriodParserConfiguration.PreviousPrefixRegex.IsMatch(trimmedText) ||
                trimmedText.Equals("anoche", StringComparison.Ordinal))
            {
                swift = -1;
            }
            else if (PortugueseDatePeriodParserConfiguration.NextPrefixRegex.IsMatch(trimmedText))
            {
                swift = 1;
            }

            return swift;
        }
    }
}
