using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Korean;
using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime.Korean
{
    public sealed class KoreanMergedParserConfiguration : KoreanCommonDateTimeParserConfiguration, IMergedParserConfiguration
    {
        public KoreanMergedParserConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            BeforeRegex = KoreanMergedExtractorConfiguration.BeforeRegex;
            AfterRegex = KoreanMergedExtractorConfiguration.AfterRegex;
            SinceRegex = (config.Options & DateTimeOptions.ExperimentalMode) != 0 ? KoreanMergedExtractorConfiguration.SinceRegexExp :
                KoreanMergedExtractorConfiguration.SinceRegex;
            AroundRegex = KoreanMergedExtractorConfiguration.AroundRegex;
            EqualRegex = KoreanMergedExtractorConfiguration.EqualRegex;
            SuffixAfter = KoreanMergedExtractorConfiguration.SuffixAfterRegex;
            YearRegex = KoreanDatePeriodExtractorConfiguration.YearRegex;

            SuperfluousWordMatcher = KoreanMergedExtractorConfiguration.SuperfluousWordMatcher;

            DatePeriodParser = new BaseDatePeriodParser(new KoreanDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new KoreanTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new KoreanDateTimePeriodParserConfiguration(this));
            SetParser = new BaseSetParser(new KoreanSetParserConfiguration(this));
            HolidayParser = new BaseHolidayParser(new KoreanHolidayParserConfiguration(this));
            TimeZoneParser = new BaseTimeZoneParser();
        }

        public Regex BeforeRegex { get; }

        public Regex AfterRegex { get; }

        public Regex SinceRegex { get; }

        public Regex AroundRegex { get; }

        public Regex EqualRegex { get; }

        public Regex SuffixAfter { get; }

        public Regex YearRegex { get; }

        public IDateTimeParser SetParser { get; }

        public IDateTimeParser HolidayParser { get; }

        public StringMatcher SuperfluousWordMatcher { get; }

        bool IMergedParserConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;
    }
}