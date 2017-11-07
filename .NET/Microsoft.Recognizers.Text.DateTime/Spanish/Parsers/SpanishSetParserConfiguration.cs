using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishSetParserConfiguration : ISetParserConfiguration
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

        public SpanishSetParserConfiguration(ICommonDateTimeParserConfiguration config)
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

        public bool GetMatchedDailyTimex(string text, out string timex)
        {
            var trimedText = text.Trim().ToLowerInvariant();

            if (trimedText.EndsWith("diario") || trimedText.EndsWith("diariamente"))
            {
                timex = "P1D";
            }
            else if (trimedText.Equals("semanalmente"))
            {
                timex = "P1W";
            }
            else if (trimedText.Equals("quincenalmente"))
            {
                timex = "P2W";
            }
            else if (trimedText.Equals("mensualmente"))
            {
                timex = "P1M";
            }
            else if (trimedText.Equals("anualmente"))
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
            var trimedText = text.Trim().ToLowerInvariant();

            if (trimedText.Equals("día") || trimedText.Equals("dia") ||
                trimedText.Equals("días") || trimedText.Equals("dias"))
            {
                timex = "P1D";
            }
            else if (trimedText.Equals("semana") || trimedText.Equals("semanas"))
            {
                timex = "P1W";
            }
            else if (trimedText.Equals("mes") || trimedText.Equals("meses"))
            {
                timex = "P1M";
            }
            else if (trimedText.Equals("año") || trimedText.Equals("años"))
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