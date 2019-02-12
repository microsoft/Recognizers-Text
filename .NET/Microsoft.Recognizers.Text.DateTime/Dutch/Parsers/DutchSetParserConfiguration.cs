using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Dutch
{
    public class DutchSetParserConfiguration : BaseOptionsConfiguration, ISetParserConfiguration
    {
        public DutchSetParserConfiguration(ICommonDateTimeParserConfiguration config)
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

            EachPrefixRegex = DutchSetExtractorConfiguration.EachPrefixRegex;
            PeriodicRegex = DutchSetExtractorConfiguration.PeriodicRegex;
            EachUnitRegex = DutchSetExtractorConfiguration.EachUnitRegex;
            EachDayRegex = DutchSetExtractorConfiguration.EachDayRegex;
            SetWeekDayRegex = DutchSetExtractorConfiguration.SetWeekDayRegex;
            SetEachRegex = DutchSetExtractorConfiguration.SetEachRegex;
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
            var trimmedText = text.Trim().ToLowerInvariant();
            if (trimmedText.Equals("dagelijks"))
            {
                timex = "P1D";
            }
            else if (trimmedText.Equals("wekelijks"))
            {
                timex = "P1W";
            }
            else if (trimmedText.Equals("tweewekelijks"))
            {
                timex = "P2W";
            }
            else if (trimmedText.Equals("maandelijks"))
            {
                timex = "P1M";
            }
            else if (trimmedText.Equals("elk jaar") || trimmedText.Equals("jaarlijks"))
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
            var trimmedText = text.Trim().ToLowerInvariant();
            if (trimmedText.Equals("dag"))
            {
                timex = "P1D";
            }
            else if (trimmedText.Equals("week"))
            {
                timex = "P1W";
            }
            else if (trimmedText.Equals("maand"))
            {
                timex = "P1M";
            }
            else if (trimmedText.Equals("jaar"))
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
    }
}