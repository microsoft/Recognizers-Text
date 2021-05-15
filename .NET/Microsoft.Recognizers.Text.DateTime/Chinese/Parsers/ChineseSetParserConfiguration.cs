using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class ChineseSetParserConfiguration : BaseDateTimeOptionsConfiguration, ICJKSetParserConfiguration
    {
        public ChineseSetParserConfiguration(ICJKCommonDateTimeParserConfiguration config)
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

            EachPrefixRegex = ChineseSetExtractorConfiguration.EachPrefixRegex;
            EachUnitRegex = ChineseSetExtractorConfiguration.EachUnitRegex;
            EachDayRegex = ChineseSetExtractorConfiguration.EachDayRegex;
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
            if (trimmedText is "天" or "日")
            {
                timex = "P1D";
            }
            else if (trimmedText is "周" or "星期")
            {
                timex = "P1W";
            }
            else if (trimmedText is "月")
            {
                timex = "P1M";
            }
            else if (trimmedText is "年")
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