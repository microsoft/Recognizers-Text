using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Hindi;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Hindi
{
   public class HindiSetParserConfiguration : BaseDateTimeOptionsConfiguration, ISetParserConfiguration
    {
        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex DateUnitRegex =
            new Regex(DateTimeDefinitions.DateUnitRegex, RegexFlags);

        public HindiSetParserConfiguration(ICommonDateTimeParserConfiguration config)
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

            EachPrefixRegex = HindiSetExtractorConfiguration.EachPrefixRegex;
            PeriodicRegex = HindiSetExtractorConfiguration.PeriodicRegex;
            EachUnitRegex = HindiSetExtractorConfiguration.EachUnitRegex;
            EachDayRegex = HindiSetExtractorConfiguration.EachDayRegex;
            SetWeekDayRegex = HindiSetExtractorConfiguration.SetWeekDayRegex;
            SetEachRegex = HindiSetExtractorConfiguration.SetEachRegex;
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
            var match = PeriodicRegex.MatchExact(trimmedText, trim: true);

            if (match.Groups["daily"].Success)
            {
                timex = "P1D";
            }
            else if (match.Groups["weekly"].Success)
            {
                timex = "P1W";
            }
            else if (match.Groups["biweekly"].Success)
            {
                timex = "P2W";
            }
            else if (match.Groups["monthly"].Success)
            {
                timex = "P1M";
            }
            else if (match.Groups["yearly"].Success)
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
            var match = DateUnitRegex.MatchExact(trimmedText, trim: true);

            if (match.Groups["day"].Success)
            {
                timex = "P1D";
            }
            else if (match.Groups["week"].Success)
            {
                timex = "P1W";
            }
            else if (match.Groups["month"].Success)
            {
                timex = "P1M";
            }
            else if (match.Groups["year"].Success)
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
