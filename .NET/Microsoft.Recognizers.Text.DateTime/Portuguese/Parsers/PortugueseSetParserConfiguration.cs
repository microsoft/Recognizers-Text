// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Portuguese;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class PortugueseSetParserConfiguration : BaseDateTimeOptionsConfiguration, ISetParserConfiguration
    {
        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex DayTypeRegex =
            new Regex(DateTimeDefinitions.DayTypeRegex, RegexFlags);

        private static readonly Regex WeekTypeRegex =
            new Regex(DateTimeDefinitions.WeekTypeRegex, RegexFlags);

        private static readonly Regex BiWeekTypeRegex =
            new Regex(DateTimeDefinitions.BiWeekTypeRegex, RegexFlags);

        private static readonly Regex MonthTypeRegex =
            new Regex(DateTimeDefinitions.MonthTypeRegex, RegexFlags);

        private static readonly Regex BiMonthTypeRegex =
            new Regex(DateTimeDefinitions.BiMonthTypeRegex, RegexFlags);

        private static readonly Regex QuarterTypeRegex =
            new Regex(DateTimeDefinitions.QuarterTypeRegex, RegexFlags);

        private static readonly Regex SemiAnnualTypeRegex =
            new Regex(DateTimeDefinitions.SemiAnnualTypeRegex, RegexFlags);

        private static readonly Regex YearTypeRegex =
            new Regex(DateTimeDefinitions.YearTypeRegex, RegexFlags);

        public PortugueseSetParserConfiguration(ICommonDateTimeParserConfiguration config)
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

            EachPrefixRegex = PortugueseSetExtractorConfiguration.EachPrefixRegex;
            PeriodicRegex = PortugueseSetExtractorConfiguration.PeriodicRegex;
            EachUnitRegex = PortugueseSetExtractorConfiguration.EachUnitRegex;
            EachDayRegex = PortugueseSetExtractorConfiguration.EachDayRegex;
            SetWeekDayRegex = PortugueseSetExtractorConfiguration.SetWeekDayRegex;
            SetEachRegex = PortugueseSetExtractorConfiguration.SetEachRegex;
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
            var trimmedText = text.Trim().Normalized(DateTimeDefinitions.SpecialCharactersEquivalent);

            float durationLength = 1; // Default value
            float multiplier = 1;
            string durationType;

            if (DayTypeRegex.IsMatch(trimmedText))
            {
                durationType = Constants.TimexDay;
            }
            else if (WeekTypeRegex.IsMatch(trimmedText))
            {
                durationType = Constants.TimexWeek;
            }
            else if (BiWeekTypeRegex.IsMatch(trimmedText))
            {
                durationType = Constants.TimexWeek;
                multiplier = 2;
            }
            else if (MonthTypeRegex.IsMatch(trimmedText))
            {
                durationType = Constants.TimexMonth;
            }
            else if (BiMonthTypeRegex.IsMatch(trimmedText))
            {
                durationType = Constants.TimexMonth;
                multiplier = 2;
            }
            else if (QuarterTypeRegex.IsMatch(trimmedText))
            {
                durationType = Constants.TimexMonth;
                multiplier = 3;
            }
            else if (SemiAnnualTypeRegex.IsMatch(trimmedText))
            {
                durationType = Constants.TimexYear;
                multiplier = 0.5f;
            }
            else if (YearTypeRegex.IsMatch(trimmedText))
            {
                durationType = Constants.TimexYear;
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
            var trimmedText = text.Trim().Normalized(DateTimeDefinitions.SpecialCharactersEquivalent);

            // @TODO move hardcoded values to resources file
            if (trimmedText.Equals("dia", StringComparison.Ordinal) || trimmedText.Equals("dias", StringComparison.Ordinal))
            {
                timex = "P1D";
            }
            else if (trimmedText.Equals("semana", StringComparison.Ordinal) || trimmedText.Equals("semanas", StringComparison.Ordinal))
            {
                timex = "P1W";
            }
            else if (trimmedText.Equals("mes", StringComparison.Ordinal) || trimmedText.Equals("meses", StringComparison.Ordinal))
            {
                timex = "P1M";
            }
            else if (trimmedText.Equals("ano", StringComparison.Ordinal) || trimmedText.Equals("anos", StringComparison.Ordinal))
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