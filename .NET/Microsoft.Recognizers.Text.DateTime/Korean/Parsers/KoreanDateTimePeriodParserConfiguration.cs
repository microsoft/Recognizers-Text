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

            SpecificTimeOfDayRegex = KoreanDateTimePeriodExtractorConfiguration.SpecificTimeOfDayRegex;
            TimeOfDayRegex = KoreanDateTimePeriodExtractorConfiguration.TimeOfDayRegex;
            NextRegex = KoreanDateTimePeriodExtractorConfiguration.NextRegex;
            LastRegex = KoreanDateTimePeriodExtractorConfiguration.LastRegex;
            PastRegex = KoreanDateTimePeriodExtractorConfiguration.PastRegex;
            FutureRegex = KoreanDateTimePeriodExtractorConfiguration.FutureRegex;
            UnitRegex = KoreanDateTimePeriodExtractorConfiguration.UnitRegex;
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

        public Regex SpecificTimeOfDayRegex { get; }

        public Regex TimeOfDayRegex { get; }

        public Regex NextRegex { get; }

        public Regex LastRegex { get; }

        public Regex PastRegex { get; }

        public Regex FutureRegex { get; }

        public Regex UnitRegex { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public bool GetMatchedTimeRangeAndSwift(string text, out string timeStr, out int beginHour, out int endHour, out int swift)
        {
            var trimmedText = text.Trim();

            // @TODO move hardcoded values to resources file
            beginHour = 0;
            endHour = 0;
            swift = 0;
            switch (trimmedText)
            {
                case "今晚":
                    swift = 0;
                    timeStr = "TEV";
                    beginHour = 16;
                    endHour = 20;
                    break;
                case "今早":
                case "今晨":
                    swift = 0;
                    timeStr = "TMO";
                    beginHour = 8;
                    endHour = Constants.HalfDayHourCount;
                    break;
                case "明晚":
                    swift = 1;
                    timeStr = "TEV";
                    beginHour = 16;
                    endHour = 20;
                    break;
                case "明早":
                case "明晨":
                    swift = 1;
                    timeStr = "TMO";
                    beginHour = 8;
                    endHour = Constants.HalfDayHourCount;
                    break;
                case "昨晚":
                    swift = -1;
                    timeStr = "TEV";
                    beginHour = 16;
                    endHour = 20;
                    break;
                default:
                    timeStr = null;
                    return false;
            }

            return true;
        }

        public bool GetMatchedTimeRange(string text, out string timeStr, out int beginHour, out int endHour, out int endMin)
        {
            var trimmedText = text.Trim();

            beginHour = 0;
            endHour = 0;
            endMin = 0;
            if (MORegex.IsMatch(trimmedText))
            {
                timeStr = "TMO";
                beginHour = 8;
                endHour = Constants.HalfDayHourCount;
            }
            else if (MIRegex.IsMatch(trimmedText))
            {
                timeStr = "TMI";
                beginHour = 11;
                endHour = 13;
            }
            else if (AFRegex.IsMatch(trimmedText))
            {
                timeStr = "TAF";
                beginHour = Constants.HalfDayHourCount;
                endHour = 16;
            }
            else if (EVRegex.IsMatch(trimmedText))
            {
                timeStr = "TEV";
                beginHour = 16;
                endHour = 20;
            }
            else if (NIRegex.IsMatch(trimmedText))
            {
                timeStr = "TNI";
                beginHour = 20;
                endHour = 23;
                endMin = 59;
            }
            else
            {
                timeStr = null;
                return false;
            }

            return true;
        }
    }
}