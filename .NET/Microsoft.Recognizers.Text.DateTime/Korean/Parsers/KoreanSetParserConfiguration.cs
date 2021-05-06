using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Korean
{
    public class KoreanSetParserConfiguration : BaseDateTimeOptionsConfiguration, ICJKSetParserConfiguration
    {
        public KoreanSetParserConfiguration(ICJKCommonDateTimeParserConfiguration config)
            : base(config)
        {
            DurationExtractor = config.DurationExtractor;
            TimeExtractor = config.TimeExtractor;
            DateExtractor = config.DateExtractor;
            DateTimeExtractor = config.DateTimeExtractor;

            DurationParser = config.DurationParser;
            TimeParser = config.TimeParser;
            DateParser = config.DateParser;
            DateTimeParser = config.DateTimeParser;

            EachPrefixRegex = KoreanSetExtractorConfiguration.EachPrefixRegex;
            EachUnitRegex = KoreanSetExtractorConfiguration.EachUnitRegex;
            EachDayRegex = KoreanSetExtractorConfiguration.EachDayRegex;
            UnitMap = config.UnitMap;
        }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor TimeExtractor { get; }

        public IDateTimeExtractor DateExtractor { get; }

        public IDateTimeExtractor DateTimeExtractor { get; }

        public IDateTimeParser DurationParser { get; }

        public IDateTimeParser TimeParser { get; }

        public IDateTimeParser DateParser { get; }

        public IDateTimeParser DateTimeParser { get; }

        public Regex EachPrefixRegex { get; }

        public Regex EachUnitRegex { get; }

        public Regex EachDayRegex { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public bool GetMatchedUnitTimex(string text, out string timex)
        {
            var trimmedText = text.Trim();

            // @TODO move hardcoded values to resources file
            if (trimmedText.Equals("天", StringComparison.Ordinal) ||
                trimmedText.Equals("日", StringComparison.Ordinal))
            {
                timex = "P1D";
            }
            else if (trimmedText.Equals("周", StringComparison.Ordinal) ||
                     trimmedText.Equals("星期", StringComparison.Ordinal))
            {
                timex = "P1W";
            }
            else if (trimmedText.Equals("月", StringComparison.Ordinal))
            {
                timex = "P1M";
            }
            else if (trimmedText.Equals("年", StringComparison.Ordinal))
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
    }
}