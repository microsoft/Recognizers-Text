// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Turkish;

namespace Microsoft.Recognizers.Text.DateTime.Turkish
{
    public class TurkishDateTimePeriodParserConfiguration : BaseDateTimeOptionsConfiguration, IDateTimePeriodParserConfiguration
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

        public TurkishDateTimePeriodParserConfiguration(ICommonDateTimeParserConfiguration config)
            : base(config)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenListBeforeDate;
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

            PureNumberFromToRegex = TurkishTimePeriodExtractorConfiguration.PureNumFromTo;
            HyphenDateRegex = TurkishDateTimePeriodExtractorConfiguration.HyphenDateRegex;
            PureNumberBetweenAndRegex = TurkishTimePeriodExtractorConfiguration.PureNumBetweenAnd;
            SpecificTimeOfDayRegex = TurkishDateTimeExtractorConfiguration.SpecificTimeOfDayRegex;
            TimeOfDayRegex = TurkishDateTimeExtractorConfiguration.TimeOfDayRegex;
            PreviousPrefixRegex = TurkishDatePeriodExtractorConfiguration.PreviousPrefixRegex;
            FutureRegex = TurkishDatePeriodExtractorConfiguration.NextPrefixRegex;
            FutureSuffixRegex = TurkishDatePeriodExtractorConfiguration.FutureSuffixRegex;
            NumberCombinedWithUnitRegex = TurkishDateTimePeriodExtractorConfiguration.TimeNumberCombinedWithUnit;
            UnitRegex = TurkishTimePeriodExtractorConfiguration.TimeUnitRegex;
            PeriodTimeOfDayWithDateRegex = TurkishDateTimePeriodExtractorConfiguration.PeriodTimeOfDayWithDateRegex;
            RelativeTimeUnitRegex = TurkishDateTimePeriodExtractorConfiguration.RelativeTimeUnitRegex;
            RestOfDateTimeRegex = TurkishDateTimePeriodExtractorConfiguration.RestOfDateTimeRegex;
            AmDescRegex = TurkishDateTimePeriodExtractorConfiguration.AmDescRegex;
            PmDescRegex = TurkishDateTimePeriodExtractorConfiguration.PmDescRegex;
            WithinNextPrefixRegex = TurkishDateTimePeriodExtractorConfiguration.WithinNextPrefixRegex;
            PrefixDayRegex = TurkishDateTimePeriodExtractorConfiguration.PrefixDayRegex;
            BeforeRegex = TurkishDateTimePeriodExtractorConfiguration.BeforeRegex;
            AfterRegex = TurkishDateTimePeriodExtractorConfiguration.AfterRegex;

            UnitMap = config.UnitMap;
            Numbers = config.Numbers;
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

        bool IDateTimePeriodParserConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;

        Regex IDateTimePeriodParserConfiguration.TasksmodeMealTimeofDayRegex => null;

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IImmutableDictionary<string, int> Numbers { get; }

        public IDateTimeExtractor HolidayExtractor { get; }

        public IDateTimeParser HolidayTimeParser { get; }

        public bool GetMatchedTimeRange(string text, out string todSymbol, out int beginHour, out int endHour, out int endMin)
        {
            var trimmedText = text.Trim();

            beginHour = 0;
            endHour = 0;
            endMin = 0;

            if (MorningStartEndRegex.IsMatch(trimmedText))
            {
                todSymbol = Constants.Morning;
            }
            else if (AfternoonStartEndRegex.IsMatch(trimmedText))
            {
                todSymbol = Constants.Afternoon;
            }
            else if (EveningStartEndRegex.IsMatch(trimmedText))
            {
                todSymbol = Constants.Evening;
            }
            else if (NightStartEndRegex.IsMatch(trimmedText))
            {
                todSymbol = Constants.Night;
            }
            else
            {
                todSymbol = null;
                return false;
            }

            var parseResult = TimexUtility.ResolveTimeOfDay(todSymbol);
            todSymbol = parseResult.Timex;
            beginHour = parseResult.BeginHour;
            endHour = parseResult.EndHour;
            endMin = parseResult.EndMin;

            return true;
        }

        public int GetSwiftPrefix(string text)
        {
            var trimmedText = text.Trim();

            var swift = 0;
            if (FutureRegex.IsMatch(trimmedText))
            {
                swift = 1;
            }
            else if (PreviousPrefixRegex.IsMatch(trimmedText))
            {
                swift = -1;
            }

            return swift;
        }
    }
}
