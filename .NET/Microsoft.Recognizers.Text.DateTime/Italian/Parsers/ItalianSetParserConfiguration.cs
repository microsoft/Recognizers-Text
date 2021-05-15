using System;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Italian
{
    public class ItalianSetParserConfiguration : BaseDateTimeOptionsConfiguration, ISetParserConfiguration
    {
        public ItalianSetParserConfiguration(ICommonDateTimeParserConfiguration config)
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

            EachPrefixRegex = ItalianSetExtractorConfiguration.EachPrefixRegex;
            PeriodicRegex = ItalianSetExtractorConfiguration.PeriodicRegex;
            EachUnitRegex = ItalianSetExtractorConfiguration.EachUnitRegex;
            EachDayRegex = ItalianSetExtractorConfiguration.EachDayRegex;
            SetWeekDayRegex = ItalianSetExtractorConfiguration.SetWeekDayRegex;
            SetEachRegex = ItalianSetExtractorConfiguration.SetEachRegex;
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

            if (trimmedText is "quotidianamente"
                            or "quotidiano"
                            or "quotidiana"
                            or "giornalmente"
                            or "giornaliero"
                            or "giornaliera")
            {
                // daily
                timex = "P1D";
            }
            else if (trimmedText is "settimanale"
                                 or "settimanalmente")
            {
                // weekly
                timex = "P1W";
            }
            else if (trimmedText is "bisettimanale")
            {
                // bi weekly
                timex = "P2W";
            }
            else if (trimmedText is "mensile"
                                 or "mensilmente")
            {
                // monthly
                timex = "P1M";
            }
            else if (trimmedText is "annuale"
                                 or "annualmente")
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

            if (trimmedText is "giorno"
                            or "giornata"
                            or "giorni")
            {
                timex = "P1D";
            }
            else if (trimmedText is "settimana"
                                 or "settimane")
            {
                timex = "P1W";
            }
            else if (trimmedText is "mese" or "mesi")
            {
                timex = "P1M";
            }
            else if (trimmedText is "anno" or "annata" or "anni")
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

        public string WeekDayGroupMatchString(Match match)
        {
            string weekday = string.Empty;

            if (match.Groups["g0"].Length != 0)
            {
                weekday = match.Groups["g0"] + "a";
            }
            else if (match.Groups["g1"].Length != 0)
            {
                weekday = match.Groups["g1"] + "io";
            }
            else if (match.Groups["g2"].Length != 0)
            {
                weekday = match.Groups["g2"] + "e";
            }
            else if (match.Groups["g3"].Length != 0)
            {
                weekday = match.Groups["g3"] + "ì";
            }
            else if (match.Groups["g4"].Length != 0)
            {
                weekday = match.Groups["g4"] + "a";
            }
            else if (match.Groups["g5"].Length != 0)
            {
                weekday = match.Groups["g5"] + "o";
            }

            return weekday;
        }
    }
}
