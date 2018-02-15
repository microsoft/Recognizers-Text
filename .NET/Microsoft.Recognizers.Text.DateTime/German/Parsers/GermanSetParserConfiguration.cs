using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanSetParserConfiguration : BaseOptionsConfiguration,ISetParserConfiguration
    {
        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeParser DurationParser { get; }

        public IDateTimeExtractor TimeExtractor { get; }

        public IDateTimeParser TimeParser { get; }

        public IDateTimeExtractor DateExtractor { get; }

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


        public GermanSetParserConfiguration(ICommonDateTimeParserConfiguration config): base(config.Options)
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

        public bool GetMatchedDailyTimex(string text, out string timex)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            if (trimedText.Equals("täglich") || trimedText.Equals("täglicher") || trimedText.Equals("tägliches") || trimedText.Equals("tägliche") || trimedText.Equals("täglichen") ||
                trimedText.Equals("alltäglich") || trimedText.Equals("alltäglicher") || trimedText.Equals("alltägliches") || trimedText.Equals("alltägliche") || trimedText.Equals("alltäglichen") ||
                trimedText.Equals("jeden tag")
                )
            {
                timex = "P1D";
            }
            else if (trimedText.Equals("wöchentlich") || trimedText.Equals("wöchentlicher") || trimedText.Equals("wöchentliches") || trimedText.Equals("wöchentliche") || trimedText.Equals("wöchentlichen")
                || trimedText.Equals("allwöchentlich") || trimedText.Equals("allwöchentlicher") || trimedText.Equals("allwöchentliches") || trimedText.Equals("allwöchentliche") || trimedText.Equals("allwöchentlichen")
                )
            {
                timex = "P1W";
            }
            else if (trimedText.Equals("monatlich") || trimedText.Equals("monatlicher") || trimedText.Equals("monatliches") || trimedText.Equals("monatliche") || trimedText.Equals("monatlichen")
                || trimedText.Equals("allmonatlich") || trimedText.Equals("allmonatlicher") || trimedText.Equals("allmonatliches") || trimedText.Equals("allmonatliche") || trimedText.Equals("allmonatlichen")
                )
            {
                timex = "P1M";
            }
            else if (trimedText.Equals("jährlich") || trimedText.Equals("jährlicher") || trimedText.Equals("jährliches") || trimedText.Equals("jährliche") || trimedText.Equals("jährlichen")
                || trimedText.Equals("alljährlich") || trimedText.Equals("alljährlicher") || trimedText.Equals("alljährliches") || trimedText.Equals("alljährliche") || trimedText.Equals("alljährlichen")
                )
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
            var trimedText = text.Trim()/*.ToLowerInvariant()*/;
            if (trimedText.Equals("tag") || trimedText.Equals("Tag"))
            {
                timex = "P1D";
            }
            else if (trimedText.Equals("woche"))
            {
                timex = "P1W";
            }
            else if (trimedText.Equals("monat"))
            {
                timex = "P1M";
            }
            else if (trimedText.Equals("jahr"))
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