﻿using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Turkish
{
    public class TurkishSetParserConfiguration : BaseDateTimeOptionsConfiguration, ISetParserConfiguration
    {
        public TurkishSetParserConfiguration(ICommonDateTimeParserConfiguration config)
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

            EachPrefixRegex = TurkishSetExtractorConfiguration.EachPrefixRegex;
            PeriodicRegex = TurkishSetExtractorConfiguration.PeriodicRegex;
            EachUnitRegex = TurkishSetExtractorConfiguration.EachUnitRegex;
            EachDayRegex = TurkishSetExtractorConfiguration.EachDayRegex;
            SetWeekDayRegex = TurkishSetExtractorConfiguration.SetWeekDayRegex;
            SetEachRegex = TurkishSetExtractorConfiguration.SetEachRegex;
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

            if (trimmedText.Equals("günlük"))
            {
                timex = "P1D";
            }
            else if (trimmedText.Equals("haftalık"))
            {
                timex = "P1W";
            }
            else if (trimmedText.Equals("iki haftada bir"))
            {
                timex = "P2W";
            }
            else if (trimmedText.Equals("aylık"))
            {
                timex = "P1M";
            }
            else if (trimmedText.Equals("yıllık") || trimmedText.Equals("senelik"))
            {
                timex = "P1Y";
            }
            else
            {
                timex = null;
                return false;
            }

            return true;
        }

        public bool GetMatchedUnitTimex(string text, out string timex)
        {
            var trimmedText = text.Trim();

            if (trimmedText.Equals("gün"))
            {
                timex = "P1D";
            }
            else if (trimmedText.Equals("hafta"))
            {
                timex = "P1W";
            }
            else if (trimmedText.Equals("ay"))
            {
                timex = "P1M";
            }
            else if (trimmedText.Equals("yıl"))
            {
                timex = "P1Y";
            }
            else
            {
                timex = null;
                return false;
            }

            return true;
        }

        public string WeekDayGroupMatchString(Match match) => SetHandler.WeekDayGroupMatchString(match);
    }
}