﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Swedish;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Swedish
{
    public class SwedishSetParserConfiguration : BaseDateTimeOptionsConfiguration, ISetParserConfiguration
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex DoubleMultiplierRegex =
            new Regex(DateTimeDefinitions.DoubleMultiplierRegex, RegexFlags, RegexTimeOut);

        private static readonly Regex HalfMultiplierRegex =
            new Regex(DateTimeDefinitions.HalfMultiplierRegex, RegexFlags, RegexTimeOut);

        private static readonly Regex DayTypeRegex =
            new Regex(DateTimeDefinitions.DayTypeRegex, RegexFlags, RegexTimeOut);

        private static readonly Regex WeekTypeRegex =
            new Regex(DateTimeDefinitions.WeekTypeRegex, RegexFlags, RegexTimeOut);

        private static readonly Regex WeekendTypeRegex =
            new Regex(DateTimeDefinitions.WeekendTypeRegex, RegexFlags, RegexTimeOut);

        private static readonly Regex MonthTypeRegex =
            new Regex(DateTimeDefinitions.MonthTypeRegex, RegexFlags, RegexTimeOut);

        private static readonly Regex QuarterTypeRegex =
            new Regex(DateTimeDefinitions.QuarterTypeRegex, RegexFlags, RegexTimeOut);

        private static readonly Regex YearTypeRegex =
            new Regex(DateTimeDefinitions.YearTypeRegex, RegexFlags, RegexTimeOut);

        // pass FutureTerms as List to ReplaceValueInTextWithFutTerm function
        private static readonly List<string> ThisTerms = (List<string>)DateTimeDefinitions.FutureTerms;

        public SwedishSetParserConfiguration(ICommonDateTimeParserConfiguration config)
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

            EachPrefixRegex = SwedishSetExtractorConfiguration.EachPrefixRegex;
            PeriodicRegex = SwedishSetExtractorConfiguration.PeriodicRegex;
            EachUnitRegex = SwedishSetExtractorConfiguration.EachUnitRegex;
            EachDayRegex = SwedishSetExtractorConfiguration.EachDayRegex;
            SetWeekDayRegex = SwedishSetExtractorConfiguration.SetWeekDayRegex;
            SetEachRegex = SwedishSetExtractorConfiguration.SetEachRegex;
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

            float durationLength = 1; // Default value
            float multiplier = 1;
            string durationType;

            if (DoubleMultiplierRegex.IsMatch(trimmedText))
            {
                multiplier = 2;
            }
            else if (HalfMultiplierRegex.IsMatch(trimmedText))
            {
                multiplier = 0.5f;
            }

            if (DayTypeRegex.IsMatch(trimmedText))
            {
                durationType = "D";
            }
            else if (WeekTypeRegex.IsMatch(trimmedText))
            {
                durationType = "W";
            }
            else if (WeekendTypeRegex.IsMatch(trimmedText))
            {
                durationType = "WE";
            }
            else if (MonthTypeRegex.IsMatch(trimmedText))
            {
                durationType = "M";
            }
            else if (QuarterTypeRegex.IsMatch(trimmedText))
            {
                durationLength = 3;
                durationType = "M";
            }
            else if (YearTypeRegex.IsMatch(trimmedText))
            {
                durationType = "Y";
            }
            else
            {
                timex = null;
                return false;
            }

            timex = TimexUtility.GenerateSetTimex(durationType, durationLength, multiplier);

            return true;
        }

        public bool GetMatchedUnitTimex(string text, out string timex)
        {
            return GetMatchedDailyTimex(text, out timex);
        }

        public string WeekDayGroupMatchString(Match match) => SetHandler.WeekDayGroupMatchString(match);

        public string ReplaceValueInTextWithFutTerm(string text, string value) => TasksModeSetHandler.ReplaceValueInTextWithFutTerm(text, value, ThisTerms);
    }
}