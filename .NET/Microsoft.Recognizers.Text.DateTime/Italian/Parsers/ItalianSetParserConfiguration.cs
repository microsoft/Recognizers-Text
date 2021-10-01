// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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

            if (trimmedText.Equals("quotidianamente", StringComparison.Ordinal) ||
                trimmedText.Equals("quotidiano", StringComparison.Ordinal) ||
                trimmedText.Equals("quotidiana", StringComparison.Ordinal) ||
                trimmedText.Equals("giornalmente", StringComparison.Ordinal) ||
                trimmedText.Equals("giornaliero", StringComparison.Ordinal) ||
                trimmedText.Equals("giornaliera", StringComparison.Ordinal))
            {
                // daily
                timex = "P1D";
            }
            else if (trimmedText.Equals("settimanale", StringComparison.Ordinal) ||
                     trimmedText.Equals("settimanalmente", StringComparison.Ordinal))
            {
                // weekly
                timex = "P1W";
            }
            else if (trimmedText.Equals("bisettimanale", StringComparison.Ordinal))
            {
                // bi weekly
                timex = "P2W";
            }
            else if (trimmedText.Equals("mensile", StringComparison.Ordinal) ||
                     trimmedText.Equals("mensilmente", StringComparison.Ordinal))
            {
                // monthly
                timex = "P1M";
            }
            else if (trimmedText.Equals("annuale", StringComparison.Ordinal) ||
                     trimmedText.Equals("annualmente", StringComparison.Ordinal))
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

            if (trimmedText.Equals("giorno", StringComparison.Ordinal) ||
                trimmedText.Equals("giornata", StringComparison.Ordinal) ||
                trimmedText.Equals("giorni", StringComparison.Ordinal))
            {
                timex = "P1D";
            }
            else if (trimmedText.Equals("settimana", StringComparison.Ordinal) ||
                     trimmedText.Equals("settimane", StringComparison.Ordinal))
            {
                timex = "P1W";
            }
            else if (trimmedText.Equals("mese", StringComparison.Ordinal) ||
                     trimmedText.Equals("mesi", StringComparison.Ordinal))
            {
                timex = "P1M";
            }
            else if (trimmedText.Equals("anno", StringComparison.Ordinal) ||
                     trimmedText.Equals("annata", StringComparison.Ordinal) ||
                     trimmedText.Equals("anni", StringComparison.Ordinal))
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
