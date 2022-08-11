// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishDateTimePeriodParserConfiguration : BaseDateTimeOptionsConfiguration, IDateTimePeriodParserConfiguration
    {
        public static readonly Regex MorningStartEndRegex =
            new Regex(DateTimeDefinitions.MorningStartEndRegex, RegexFlags);

        public static readonly Regex AfternoonStartEndRegex =
            new Regex(DateTimeDefinitions.AfternoonStartEndRegex, RegexFlags);

        public static readonly Regex EveningStartEndRegex =
            new Regex(DateTimeDefinitions.EveningStartEndRegex, RegexFlags);

        public static readonly Regex NightStartEndRegex =
            new Regex(DateTimeDefinitions.NightStartEndRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public EnglishDateTimePeriodParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;
            TokenBeforeTime = DateTimeDefinitions.TokenBeforeTime;

            DateExtractor = config.DateExtractor;
            TimeExtractor = config.TimeExtractor;
            DateTimeExtractor = config.DateTimeExtractor;
            TimePeriodExtractor = config.TimePeriodExtractor;
            CardinalExtractor = config.CardinalExtractor;
            DurationExtractor = config.DurationExtractor;
            NumberParser = config.NumberParser;
            DateParser = config.DateParser;
            TimeParser = config.TimeParser;
            TimePeriodParser = config.TimePeriodParser;
            DurationParser = config.DurationParser;
            DateTimeParser = config.DateTimeParser;
            TimeZoneParser = config.TimeZoneParser;
            HolidayExtractor = config.HolidayExtractor;
            HolidayTimeParser = config.HolidayTimeParser;

            PureNumberFromToRegex = EnglishTimePeriodExtractorConfiguration.PureNumFromTo;
            HyphenDateRegex = EnglishDateTimePeriodExtractorConfiguration.HyphenDateRegex;
            PureNumberBetweenAndRegex = EnglishTimePeriodExtractorConfiguration.PureNumBetweenAnd;
            SpecificTimeOfDayRegex = EnglishDateTimePeriodExtractorConfiguration.PeriodSpecificTimeOfDayRegex;
            TimeOfDayRegex = EnglishDateTimeExtractorConfiguration.TimeOfDayRegex;
            PreviousPrefixRegex = EnglishDatePeriodExtractorConfiguration.PreviousPrefixRegex;
            FutureRegex = EnglishDatePeriodExtractorConfiguration.NextPrefixRegex;
            FutureSuffixRegex = EnglishDatePeriodExtractorConfiguration.FutureSuffixRegex;
            NumberCombinedWithUnitRegex = EnglishDateTimePeriodExtractorConfiguration.TimeNumberCombinedWithUnit;
            UnitRegex = EnglishTimePeriodExtractorConfiguration.TimeUnitRegex;
            PeriodTimeOfDayWithDateRegex = EnglishDateTimePeriodExtractorConfiguration.PeriodTimeOfDayWithDateRegex;
            RelativeTimeUnitRegex = EnglishDateTimePeriodExtractorConfiguration.RelativeTimeUnitRegex;
            RestOfDateTimeRegex = EnglishDateTimePeriodExtractorConfiguration.RestOfDateTimeRegex;
            AmDescRegex = EnglishDateTimePeriodExtractorConfiguration.AmDescRegex;
            PmDescRegex = EnglishDateTimePeriodExtractorConfiguration.PmDescRegex;
            WithinNextPrefixRegex = EnglishDateTimePeriodExtractorConfiguration.WithinNextPrefixRegex;
            PrefixDayRegex = EnglishDateTimePeriodExtractorConfiguration.PrefixDayRegex;
            BeforeRegex = EnglishDateTimePeriodExtractorConfiguration.BeforeRegex;
            AfterRegex = EnglishDateTimePeriodExtractorConfiguration.AfterRegex;
            UnitMap = config.UnitMap;
            Numbers = config.Numbers;

            TasksmodeMealTimeofDayRegex = EnglishDateTimePeriodExtractorConfiguration.TasksmodeMealTimeofDayRegex;
        }

        public string TokenBeforeDate { get; }

        public string TokenBeforeTime { get; }

        public IDateExtractor DateExtractor { get; }

        public IDateTimeExtractor TimeExtractor { get; }

        public IDateTimeExtractor DateTimeExtractor { get; }

        public IDateTimeExtractor TimePeriodExtractor { get; }

        public IExtractor CardinalExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IParser NumberParser { get; }

        public IDateTimeParser DateParser { get; }

        public IDateTimeParser TimeParser { get; }

        public IDateTimeParser DateTimeParser { get; }

        public IDateTimeParser TimePeriodParser { get; }

        public IDateTimeParser DurationParser { get; }

        public IDateTimeParser TimeZoneParser { get; }

        public Regex PureNumberFromToRegex { get; }

        public Regex HyphenDateRegex { get; }

        public Regex PureNumberBetweenAndRegex { get; }

        public Regex SpecificTimeOfDayRegex { get; }

        public Regex TimeOfDayRegex { get; }

        public Regex PreviousPrefixRegex { get; }

        public Regex FutureRegex { get; }

        public Regex FutureSuffixRegex { get; }

        public Regex NumberCombinedWithUnitRegex { get; }

        public Regex UnitRegex { get; }

        public Regex PeriodTimeOfDayWithDateRegex { get; }

        public Regex RelativeTimeUnitRegex { get; }

        public Regex RestOfDateTimeRegex { get; }

        public Regex AmDescRegex { get; }

        public Regex PmDescRegex { get; }

        public Regex WithinNextPrefixRegex { get; }

        public Regex PrefixDayRegex { get; }

        public Regex BeforeRegex { get; }

        public Regex AfterRegex { get; }

        public Regex TasksmodeMealTimeofDayRegex { get; }

        bool IDateTimePeriodParserConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public IDateTimeExtractor HolidayExtractor { get; }

        public IDateTimeParser HolidayTimeParser { get; }

        // @TODO Move time range resolution to common policy

        public bool GetMatchedTimeRange(string text, out string todSymbol, out int beginHour, out int endHour, out int endMin)
        {
            var trimmedText = text.Trim();
            beginHour = 0;
            endHour = 0;
            endMin = 0;

            if (MorningStartEndRegex.IsMatch(trimmedText))
            {
                todSymbol = "TMO";
                beginHour = 8;
                endHour = Constants.HalfDayHourCount;
            }
            else if (AfternoonStartEndRegex.IsMatch(trimmedText))
            {
                todSymbol = "TAF";
                beginHour = Constants.HalfDayHourCount;
                endHour = 16;
            }
            else if (EveningStartEndRegex.IsMatch(trimmedText))
            {
                todSymbol = "TEV";
                beginHour = 16;
                endHour = 20;
            }
            else if (NightStartEndRegex.IsMatch(trimmedText))
            {
                todSymbol = "TNI";
                beginHour = 20;
                endHour = 23;
                endMin = 59;

            }
            else if (((Options & DateTimeOptions.TasksMode) != 0) && DateTimeDefinitions.MealtimeBreakfastTermList.Any(o => trimmedText.Contains(o)))
            {
                todSymbol = Constants.MealtimeBreakfast;
                beginHour = Constants.MealtimeBreakfastBeginHour;
                endHour = Constants.MealtimeBreakfastEndHour;
            }
            else if (((Options & DateTimeOptions.TasksMode) != 0) && DateTimeDefinitions.MealtimeBrunchTermList.Any(o => trimmedText.Contains(o)))
            {
                todSymbol = Constants.MealtimeBrunch;
                beginHour = Constants.MealtimeBrunchBeginHour;
                endHour = Constants.MealtimeBrunchEndHour;
            }
            else if (((Options & DateTimeOptions.TasksMode) != 0) && DateTimeDefinitions.MealtimeLunchTermList.Any(o => trimmedText.Contains(o)))
            {
                todSymbol = Constants.MealtimeLunch;
                beginHour = Constants.MealtimeLunchBeginHour;
                endHour = Constants.MealtimeLunchEndHour;
            }
            else if (((Options & DateTimeOptions.TasksMode) != 0) && DateTimeDefinitions.MealtimeDinnerTermList.Any(o => trimmedText.Contains(o)))
            {
                todSymbol = Constants.MealtimeDinner;
                beginHour = Constants.MealtimeDinnerBeginHour;
                endHour = Constants.MealtimeDinnerEndHour;
            }
            else if (((Options & DateTimeOptions.TasksMode) != 0) && DateTimeDefinitions.MealtimeBreakfastTermList.Any(o => trimmedText.Contains(o)))
            {
                todSymbol = Constants.MealtimeBreakfast;
                beginHour = Constants.MealtimeBreakfastBeginHour;
                endHour = Constants.MealtimeBreakfastEndHour;
            }
            else if (((Options & DateTimeOptions.TasksMode) != 0) && DateTimeDefinitions.MealtimeBrunchTermList.Any(o => trimmedText.Contains(o)))
            {
                todSymbol = Constants.MealtimeBrunch;
                beginHour = Constants.MealtimeBrunchBeginHour;
                endHour = Constants.MealtimeBrunchEndHour;
            }
            else if (((Options & DateTimeOptions.TasksMode) != 0) && DateTimeDefinitions.MealtimeLunchTermList.Any(o => trimmedText.Contains(o)))
            {
                todSymbol = Constants.MealtimeLunch;
                beginHour = Constants.MealtimeLunchBeginHour;
                endHour = Constants.MealtimeLunchEndHour;
            }
            else if (((Options & DateTimeOptions.TasksMode) != 0) && DateTimeDefinitions.MealtimeDinnerTermList.Any(o => trimmedText.Contains(o)))
            {
                todSymbol = Constants.MealtimeDinner;
                beginHour = Constants.MealtimeDinnerBeginHour;
                endHour = Constants.MealtimeDinnerEndHour;
            }
            else
            {
                todSymbol = null;
                return false;
            }

            // TasksMode modifies the values of Ambiguous time refrences like morning, lunchtime etc.
            if ((Options & DateTimeOptions.TasksMode) != 0)
            {
                return TasksModeProcessing.GetMatchedTimeRangeForTasksMode(text, todSymbol,
                          out beginHour, out endHour, out endMin);
            }

            return true;
        }

        public int GetSwiftPrefix(string text)
        {
            var trimmedText = text.Trim();

            // @TODO Move hardcoded terms to resource file

            var swift = 0;
            if (trimmedText.StartsWith("next", StringComparison.Ordinal))
            {
                swift = 1;
            }
            else if (trimmedText.StartsWith("last", StringComparison.Ordinal))
            {
                swift = -1;
            }

            return swift;
        }
    }
}
