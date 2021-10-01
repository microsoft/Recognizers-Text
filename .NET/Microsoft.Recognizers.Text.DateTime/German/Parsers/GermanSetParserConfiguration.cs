// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanSetParserConfiguration : BaseDateTimeOptionsConfiguration, ISetParserConfiguration
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
            var trimmedText = text.Trim();

            // @TODO move hardcoded values to resources file

            if (trimmedText.Equals("täglich", StringComparison.Ordinal) ||
                trimmedText.Equals("täglicher", StringComparison.Ordinal) ||
                trimmedText.Equals("tägliches", StringComparison.Ordinal) ||
                trimmedText.Equals("tägliche", StringComparison.Ordinal) ||
                trimmedText.Equals("täglichen", StringComparison.Ordinal) ||
                trimmedText.Equals("alltäglich", StringComparison.Ordinal) ||
                trimmedText.Equals("alltäglicher", StringComparison.Ordinal) ||
                trimmedText.Equals("alltägliches", StringComparison.Ordinal) ||
                trimmedText.Equals("alltägliche", StringComparison.Ordinal) ||
                trimmedText.Equals("alltäglichen", StringComparison.Ordinal) ||
                trimmedText.Equals("jeden tag", StringComparison.Ordinal))
            {
                timex = "P1D";
            }
            else if (trimmedText.Equals("wöchentlich", StringComparison.Ordinal) ||
                     trimmedText.Equals("wöchentlicher", StringComparison.Ordinal) ||
                     trimmedText.Equals("wöchentliches", StringComparison.Ordinal) ||
                     trimmedText.Equals("wöchentliche", StringComparison.Ordinal) ||
                     trimmedText.Equals("wöchentlichen", StringComparison.Ordinal) ||
                     trimmedText.Equals("allwöchentlich", StringComparison.Ordinal) ||
                     trimmedText.Equals("allwöchentlicher", StringComparison.Ordinal) ||
                     trimmedText.Equals("allwöchentliches", StringComparison.Ordinal) ||
                     trimmedText.Equals("allwöchentliche", StringComparison.Ordinal) ||
                     trimmedText.Equals("allwöchentlichen", StringComparison.Ordinal))
            {
                timex = "P1W";
            }
            else if (trimmedText.Equals("monatlich", StringComparison.Ordinal) ||
                     trimmedText.Equals("monatlicher", StringComparison.Ordinal) ||
                     trimmedText.Equals("monatliches", StringComparison.Ordinal) ||
                     trimmedText.Equals("monatliche", StringComparison.Ordinal) ||
                     trimmedText.Equals("monatlichen", StringComparison.Ordinal) ||
                     trimmedText.Equals("allmonatlich", StringComparison.Ordinal) ||
                     trimmedText.Equals("allmonatlicher", StringComparison.Ordinal) ||
                     trimmedText.Equals("allmonatliches", StringComparison.Ordinal) ||
                     trimmedText.Equals("allmonatliche", StringComparison.Ordinal) ||
                     trimmedText.Equals("allmonatlichen", StringComparison.Ordinal))
            {
                timex = "P1M";
            }
            else if (trimmedText.Equals("jährlich", StringComparison.Ordinal) ||
                     trimmedText.Equals("jährlicher", StringComparison.Ordinal) ||
                     trimmedText.Equals("jährliches", StringComparison.Ordinal) ||
                     trimmedText.Equals("jährliche", StringComparison.Ordinal) ||
                     trimmedText.Equals("jährlichen", StringComparison.Ordinal) ||
                     trimmedText.Equals("alljährlich", StringComparison.Ordinal) ||
                     trimmedText.Equals("alljährlicher", StringComparison.Ordinal) ||
                     trimmedText.Equals("alljährliches", StringComparison.Ordinal) ||
                     trimmedText.Equals("alljährliche", StringComparison.Ordinal) ||
                     trimmedText.Equals("alljährlichen", StringComparison.Ordinal))
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

            // @TODO move hardcoded values to resources file

            if (trimmedText.Equals("tag", StringComparison.Ordinal))
            {
                timex = "P1D";
            }
            else if (trimmedText.Equals("woche", StringComparison.Ordinal))
            {
                timex = "P1W";
            }
            else if (trimmedText.Equals("monat", StringComparison.Ordinal))
            {
                timex = "P1M";
            }
            else if (trimmedText.Equals("jahr", StringComparison.Ordinal))
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