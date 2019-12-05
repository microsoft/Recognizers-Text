using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishSetParserConfiguration : BaseDateTimeOptionsConfiguration, ISetParserConfiguration
    {
        public EnglishSetParserConfiguration(ICommonDateTimeParserConfiguration config)
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

            EachPrefixRegex = EnglishSetExtractorConfiguration.EachPrefixRegex;
            PeriodicRegex = EnglishSetExtractorConfiguration.PeriodicRegex;
            EachUnitRegex = EnglishSetExtractorConfiguration.EachUnitRegex;
            EachDayRegex = EnglishSetExtractorConfiguration.EachDayRegex;
            SetWeekDayRegex = EnglishSetExtractorConfiguration.SetWeekDayRegex;
            SetEachRegex = EnglishSetExtractorConfiguration.SetEachRegex;
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

            if (trimmedText.Equals("daily"))
            {
                timex = "P1D";
            }
            else if (trimmedText.Equals("weekly"))
            {
                timex = "P1W";
            }
            else if (trimmedText.Equals("biweekly"))
            {
                timex = "P2W";
            }
            else if (trimmedText.Equals("monthly"))
            {
                timex = "P1M";
            }
            else if (trimmedText.Equals("quarterly"))
            {
                timex = "P3M";
            }
            else if (trimmedText.Equals("yearly") || trimmedText.Equals("annually") || trimmedText.Equals("annual"))
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

            if (trimmedText.Equals("day"))
            {
                timex = "P1D";
            }
            else if (trimmedText.Equals("week"))
            {
                timex = "P1W";
            }
            else if (trimmedText.Equals("month"))
            {
                timex = "P1M";
            }
            else if (trimmedText.Equals("year"))
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