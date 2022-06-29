// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Korean;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Korean;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Korean
{
    public class KoreanDateTimePeriodParserConfiguration : BaseDateTimeOptionsConfiguration, ICJKDateTimePeriodParserConfiguration
    {

        public static readonly Regex MORegex = new Regex(DateTimeDefinitions.DateTimePeriodMORegex, RegexFlags);

        public static readonly Regex MIRegex = new Regex(DateTimeDefinitions.DateTimePeriodMIRegex, RegexFlags);

        public static readonly Regex AFRegex = new Regex(DateTimeDefinitions.DateTimePeriodAFRegex, RegexFlags);

        public static readonly Regex EVRegex = new Regex(DateTimeDefinitions.DateTimePeriodEVRegex, RegexFlags);

        public static readonly Regex NIRegex = new Regex(DateTimeDefinitions.DateTimePeriodNIRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public KoreanDateTimePeriodParserConfiguration(ICJKCommonDateTimeParserConfiguration config)
             : base(config)
        {

            var numOptions = NumberOptions.None;
            if ((config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                numOptions = NumberOptions.NoProtoCache;
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, numOptions);

            CardinalExtractor = config.CardinalExtractor;
            CardinalParser = AgnosticNumberParserFactory.GetParser(
                AgnosticNumberParserType.Cardinal, new KoreanNumberParserConfiguration(numConfig));

            DateExtractor = config.DateExtractor;
            DurationExtractor = config.DurationExtractor;
            DateTimeExtractor = config.DateTimeExtractor;
            TimeExtractor = config.TimeExtractor;
            TimePeriodExtractor = config.TimePeriodExtractor;
            DateParser = config.DateParser;
            TimeParser = config.TimeParser;
            DateTimeParser = config.DateTimeParser;
            TimePeriodParser = config.TimePeriodParser;
            DurationParser = config.DurationParser;

            SpecificTimeOfDayRegex = KoreanDateTimePeriodExtractorConfiguration.SpecificTimeOfDayRegex;
            TimeOfDayRegex = KoreanDateTimePeriodExtractorConfiguration.TimeOfDayRegex;
            NextRegex = KoreanDateTimePeriodExtractorConfiguration.NextRegex;
            LastRegex = KoreanDateTimePeriodExtractorConfiguration.LastRegex;
            PastRegex = KoreanDateTimePeriodExtractorConfiguration.PastRegex;
            FutureRegex = KoreanDateTimePeriodExtractorConfiguration.FutureRegex;
            WeekDayRegex = KoreanDateTimePeriodExtractorConfiguration.WeekDayRegex;
            TimePeriodLeftRegex = KoreanDateTimePeriodExtractorConfiguration.TimePeriodLeftRegex;
            UnitRegex = KoreanDateTimePeriodExtractorConfiguration.UnitRegex;
            RestOfDateRegex = KoreanDateTimePeriodExtractorConfiguration.RestOfDateRegex;
            AmPmDescRegex = KoreanDateTimePeriodExtractorConfiguration.AmPmDescRegex;
            UnitMap = config.UnitMap;
        }

        public IDateTimeExtractor DateExtractor { get; }

        public IDateTimeExtractor TimeExtractor { get; }

        public IDateTimeExtractor DateTimeExtractor { get; }

        public IDateTimeExtractor TimePeriodExtractor { get; }

        public IExtractor CardinalExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IParser CardinalParser { get; }

        public IDateTimeParser DateParser { get; }

        public IDateTimeParser TimeParser { get; }

        public IDateTimeParser DateTimeParser { get; }

        public IDateTimeParser TimePeriodParser { get; }

        public IDateTimeParser DurationParser { get; }

        public Regex SpecificTimeOfDayRegex { get; }

        public Regex TimeOfDayRegex { get; }

        public Regex NextRegex { get; }

        public Regex LastRegex { get; }

        public Regex PastRegex { get; }

        public Regex FutureRegex { get; }

        public Regex WeekDayRegex { get; }

        public Regex TimePeriodLeftRegex { get; }

        public Regex UnitRegex { get; }

        public Regex RestOfDateRegex { get; }

        public Regex AmPmDescRegex { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public bool GetMatchedTimeRangeAndSwift(string text, out string todSymbol, out int beginHour, out int endHour, out int endMinute, out int swift)
        {
            var trimmedText = text.Trim();

            // @TODO move hardcoded values to resources file
            beginHour = 0;
            endHour = 0;
            endMinute = 0;
            swift = 0;

            var tod = string.Empty;

            switch (trimmedText)
            {
                case "今晚":
                    swift = 0;
                    tod = Constants.Evening;
                    break;
                case "今早":
                case "今晨":
                    swift = 0;
                    tod = Constants.Morning;
                    break;
                case "明晚":
                    swift = 1;
                    tod = Constants.Evening;
                    break;
                case "明早":
                case "明晨":
                    swift = 1;
                    tod = Constants.Morning;
                    break;
                case "昨晚":
                    swift = -1;
                    tod = Constants.Evening;
                    break;
            }

            if (MORegex.IsMatch(trimmedText))
            {
                tod = Constants.Morning;
            }
            else if (MIRegex.IsMatch(trimmedText))
            {
                tod = Constants.MidDay;
            }
            else if (AFRegex.IsMatch(trimmedText))
            {
                tod = Constants.Afternoon;
            }
            else if (EVRegex.IsMatch(trimmedText))
            {
                tod = Constants.Evening;
            }
            else if (NIRegex.IsMatch(trimmedText))
            {
                tod = Constants.Night;
            }
            else if (string.IsNullOrEmpty(tod))
            {
                todSymbol = null;
                return false;
            }

            var parseResult = TimexUtility.ResolveTimeOfDay(tod);
            todSymbol = parseResult.Timex;
            beginHour = parseResult.BeginHour;
            endHour = parseResult.EndHour;
            endMinute = parseResult.EndMin;

            return true;
        }

        public bool GetMatchedTimeRange(string text, out string todSymbol, out int beginHour, out int endHour, out int endMin)
        {
            return GetMatchedTimeRangeAndSwift(text, out todSymbol, out beginHour, out endHour, out endMin, out int swift);
        }
    }
}