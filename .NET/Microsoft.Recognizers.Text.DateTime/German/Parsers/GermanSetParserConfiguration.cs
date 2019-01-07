using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanSetParserConfiguration : BaseOptionsConfiguration, ISetParserConfiguration
    {
        public GermanSetParserConfiguration(ICommonDateTimeParserConfiguration config)
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

            EachPrefixRegex = GermanSetExtractorConfiguration.EachPrefixRegex;
            PeriodicRegex = GermanSetExtractorConfiguration.PeriodicRegex;
            EachUnitRegex = GermanSetExtractorConfiguration.EachUnitRegex;
            EachDayRegex = GermanSetExtractorConfiguration.EachDayRegex;
            SetWeekDayRegex = GermanSetExtractorConfiguration.SetWeekDayRegex;
            SetEachRegex = GermanSetExtractorConfiguration.SetEachRegex;
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
            if (trimmedText.Equals("täglich") || trimmedText.Equals("täglicher") || trimmedText.Equals("tägliches") || trimmedText.Equals("tägliche") || trimmedText.Equals("täglichen") ||
                trimmedText.Equals("alltäglich") || trimmedText.Equals("alltäglicher") || trimmedText.Equals("alltägliches") || trimmedText.Equals("alltägliche") || trimmedText.Equals("alltäglichen") ||
                trimmedText.Equals("jeden tag"))
            {
                timex = "P1D";
            }
            else if (trimmedText.Equals("wöchentlich") || trimmedText.Equals("wöchentlicher") || trimmedText.Equals("wöchentliches") || trimmedText.Equals("wöchentliche") || trimmedText.Equals("wöchentlichen")
                || trimmedText.Equals("allwöchentlich") || trimmedText.Equals("allwöchentlicher") || trimmedText.Equals("allwöchentliches") || trimmedText.Equals("allwöchentliche") || trimmedText.Equals("allwöchentlichen"))
            {
                timex = "P1W";
            }
            else if (trimmedText.Equals("monatlich") || trimmedText.Equals("monatlicher") || trimmedText.Equals("monatliches") || trimmedText.Equals("monatliche") || trimmedText.Equals("monatlichen")
                || trimmedText.Equals("allmonatlich") || trimmedText.Equals("allmonatlicher") || trimmedText.Equals("allmonatliches") || trimmedText.Equals("allmonatliche") || trimmedText.Equals("allmonatlichen"))
            {
                timex = "P1M";
            }
            else if (trimmedText.Equals("jährlich") || trimmedText.Equals("jährlicher") || trimmedText.Equals("jährliches") || trimmedText.Equals("jährliche") || trimmedText.Equals("jährlichen")
                || trimmedText.Equals("alljährlich") || trimmedText.Equals("alljährlicher") || trimmedText.Equals("alljährliches") || trimmedText.Equals("alljährliche") || trimmedText.Equals("alljährlichen"))
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
            var trimmedText = text.Trim() /*.ToLowerInvariant()*/;
            if (trimmedText.Equals("tag") || trimmedText.Equals("Tag"))
            {
                timex = "P1D";
            }
            else if (trimmedText.Equals("woche"))
            {
                timex = "P1W";
            }
            else if (trimmedText.Equals("monat"))
            {
                timex = "P1M";
            }
            else if (trimmedText.Equals("jahr"))
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