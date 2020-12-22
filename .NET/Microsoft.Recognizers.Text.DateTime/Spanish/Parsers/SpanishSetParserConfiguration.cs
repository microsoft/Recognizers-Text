using System;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Spanish;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishSetParserConfiguration : BaseDateTimeOptionsConfiguration, ISetParserConfiguration
    {
        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex DoubleMultiplierRegex =
            new Regex(DateTimeDefinitions.DoubleMultiplierRegex, RegexFlags);

        private static readonly Regex DayTypeRegex =
            new Regex(DateTimeDefinitions.DayTypeRegex, RegexFlags);

        private static readonly Regex WeekTypeRegex =
            new Regex(DateTimeDefinitions.WeekTypeRegex, RegexFlags);

        private static readonly Regex BiWeekTypeRegex =
            new Regex(DateTimeDefinitions.BiWeekTypeRegex, RegexFlags);

        private static readonly Regex WeekendTypeRegex =
            new Regex(DateTimeDefinitions.WeekendTypeRegex, RegexFlags);

        private static readonly Regex MonthTypeRegex =
            new Regex(DateTimeDefinitions.MonthTypeRegex, RegexFlags);

        private static readonly Regex YearTypeRegex =
            new Regex(DateTimeDefinitions.YearTypeRegex, RegexFlags);

        public SpanishSetParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            DurationExtractor = config.DurationExtractor;
            TimeExtractor = config.TimeExtractor;
            DateExtractor = config.DateExtractor;
            DateTimeExtractor = config.DateTimeExtractor;
            DatePeriodExtractor = config.DatePeriodExtractor;
            TimePeriodExtractor = config.TimePeriodExtractor;
            DateTimePeriodExtractor = config.DateTimePeriodExtractor;

            DurationParser = config.DurationParser;
            TimeParser = config.TimeParser;
            DateParser = config.DateParser;
            DateTimeParser = config.DateTimeParser;
            DatePeriodParser = config.DatePeriodParser;
            TimePeriodParser = config.TimePeriodParser;
            DateTimePeriodParser = config.DateTimePeriodParser;
            UnitMap = config.UnitMap;

            EachPrefixRegex = SpanishSetExtractorConfiguration.EachPrefixRegex;
            PeriodicRegex = SpanishSetExtractorConfiguration.PeriodicRegex;
            EachUnitRegex = SpanishSetExtractorConfiguration.EachUnitRegex;
            EachDayRegex = SpanishSetExtractorConfiguration.EachDayRegex;
            SetWeekDayRegex = SpanishSetExtractorConfiguration.SetWeekDayRegex;
            SetEachRegex = SpanishSetExtractorConfiguration.SetEachRegex;
        }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeParser DurationParser { get; }

        public IDateTimeExtractor TimeExtractor { get; }

        public IDateTimeParser TimeParser { get; }

        public IDateExtractor DateExtractor { get; }

        public IDateTimeParser DateParser { get; }

        public IDateTimeExtractor DateTimeExtractor { get; }

        public IDateTimeParser DateTimeParser { get; }

        public IDateTimeExtractor DatePeriodExtractor { get; }

        public IDateTimeParser DatePeriodParser { get; }

        public IDateTimeExtractor TimePeriodExtractor { get; }

        public IDateTimeParser TimePeriodParser { get; }

        public IDateTimeExtractor DateTimePeriodExtractor { get; }

        public IDateTimeParser DateTimePeriodParser { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public Regex EachPrefixRegex { get; }

        public Regex PeriodicRegex { get; }

        public Regex EachUnitRegex { get; }

        public Regex EachDayRegex { get; }

        public Regex SetWeekDayRegex { get; }

        public Regex SetEachRegex { get; }

        public bool GetMatchedDailyTimex(string text, out string timex)
        {
            var trimmedText = text.Trim();

            float durationLength = 1; // Default value
            float multiplier = 1;
            string durationType;

            if (DoubleMultiplierRegex.IsMatch(trimmedText))
            {
                multiplier = 2;
            }

            if (DayTypeRegex.IsMatch(trimmedText))
            {
                durationType = "D";
            }
            else if (WeekTypeRegex.IsMatch(trimmedText))
            {
                durationType = "W";
            }
            else if (BiWeekTypeRegex.IsMatch(trimmedText))
            {
                durationType = "W";
                multiplier = 2;
            }
            else if (WeekendTypeRegex.IsMatch(trimmedText))
            {
                durationType = "WE";
            }
            else if (MonthTypeRegex.IsMatch(trimmedText))
            {
                durationType = "M";
            }
            else if (YearTypeRegex.IsMatch(trimmedText))
            {
                durationType = "Y";
            }
            else
            {
                timex = null;
                return false;
            }

            timex = TimexUtility.GenerateSetTimex(durationType, durationLength, multiplier);

            return true;
        }

        public bool GetMatchedUnitTimex(string text, out string timex)
        {
            return GetMatchedDailyTimex(text, out timex);
        }

        public string WeekDayGroupMatchString(Match match) => SetHandler.WeekDayGroupMatchString(match);
    }
}