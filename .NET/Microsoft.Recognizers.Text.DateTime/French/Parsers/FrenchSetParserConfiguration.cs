using System;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchSetParserConfiguration : BaseDateTimeOptionsConfiguration, ISetParserConfiguration
    {
        public FrenchSetParserConfiguration(ICommonDateTimeParserConfiguration config)
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

            EachPrefixRegex = FrenchSetExtractorConfiguration.EachPrefixRegex;
            PeriodicRegex = FrenchSetExtractorConfiguration.PeriodicRegex;
            EachUnitRegex = FrenchSetExtractorConfiguration.EachUnitRegex;
            EachDayRegex = FrenchSetExtractorConfiguration.EachDayRegex;
            SetWeekDayRegex = FrenchSetExtractorConfiguration.SetWeekDayRegex;
            SetEachRegex = FrenchSetExtractorConfiguration.SetEachRegex;
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

            // @TODO move hardcoded values to resources file

            if (trimmedText.Equals("quotidien", StringComparison.Ordinal) ||
                trimmedText.Equals("quotidienne", StringComparison.Ordinal) ||
                trimmedText.Equals("jours", StringComparison.Ordinal) ||
                trimmedText.Equals("journellement", StringComparison.Ordinal))
            {
                // daily
                timex = "P1D";
            }
            else if (trimmedText is "hebdomadaire")
            {
                // weekly
                timex = "P1W";
            }
            else if (trimmedText is "bihebdomadaire")
            {
                // bi weekly
                timex = "P2W";
            }
            else if (trimmedText is "mensuel" or "mensuelle")
            {
                // monthly
                timex = "P1M";
            }
            else if (trimmedText is "annuel" or "annuellement")
            {
                // yearly/annually
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

            // @TODO move hardcoded values to resources file

            if (trimmedText is "jour" or "journee")
            {
                timex = "P1D";
            }
            else if (trimmedText is "semaine")
            {
                timex = "P1W";
            }
            else if (trimmedText is "mois")
            {
                timex = "P1M";
            }
            else if (trimmedText is "an" or "annee")
            {
                // year
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
