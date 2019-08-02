using System;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Italian
{
    public class ItalianSetParserConfiguration : BaseOptionsConfiguration, ISetParserConfiguration
    {
        public ItalianSetParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config.Options)
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
            if (trimmedText.Equals("quotidianamente") || trimmedText.Equals("quotidiano") || trimmedText.Equals("quotidiana") ||
                trimmedText.Equals("giornalmente") || trimmedText.Equals("giornaliero") || trimmedText.Equals("giornaliera"))
            {
                // daily
                timex = "P1D";
            }
            else if (trimmedText.Equals("settimanale") || trimmedText.Equals("settimanalmente"))
            {
                // weekly
                timex = "P1W";
            }
            else if (trimmedText.Equals("bisettimanale"))
            {
                // bi weekly
                timex = "P2W";
            }
            else if (trimmedText.Equals("mensile") || trimmedText.Equals("mensilmente"))
            {
                // monthly
                timex = "P1M";
            }
            else if (trimmedText.Equals("annuale") || trimmedText.Equals("annualmente"))
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
            if (trimmedText.Equals("giorno") || trimmedText.Equals("giornata") || trimmedText.Equals("giorni"))
            {
                timex = "P1D";
            }
            else if (trimmedText.Equals("settimana") || trimmedText.Equals("settimane"))
            {
                timex = "P1W";
            }
            else if (trimmedText.Equals("mese") || trimmedText.Equals("mesi"))
            {
                timex = "P1M";
            }
            else if (trimmedText.Equals("anno") || trimmedText.Equals("annata") || trimmedText.Equals("anni"))
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
            if (match.Groups["g0"].ToString() != string.Empty)
            {
                weekday = match.Groups["g0"].ToString() + "a";
            }
            else if (match.Groups["g1"].ToString() != string.Empty)
            {
                weekday = match.Groups["g1"].ToString() + "io";
            }
            else if (match.Groups["g2"].ToString() != string.Empty)
            {
                weekday = match.Groups["g2"].ToString() + "e";
            }
            else if (match.Groups["g3"].ToString() != string.Empty)
            {
                weekday = match.Groups["g3"].ToString() + "ì";
            }
            else if (match.Groups["g4"].ToString() != string.Empty)
            {
                weekday = match.Groups["g4"].ToString() + "a";
            }
            else if (match.Groups["g5"].ToString() != string.Empty)
            {
                weekday = match.Groups["g5"].ToString() + "o";
            }

            return weekday;
        }
    }
}
