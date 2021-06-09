using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Arabic;
using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime.Arabic
{
    public sealed class ArabicMergedParserConfiguration : ArabicCommonDateTimeParserConfiguration, IMergedParserConfiguration
    {
        public ArabicMergedParserConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            BeforeRegex = ArabicMergedExtractorConfiguration.BeforeRegex;
            AfterRegex = ArabicMergedExtractorConfiguration.AfterRegex;
            SinceRegex = (config.Options & DateTimeOptions.ExperimentalMode) != 0 ? ArabicMergedExtractorConfiguration.SinceRegexExp :
                ArabicMergedExtractorConfiguration.SinceRegex;
            AroundRegex = ArabicMergedExtractorConfiguration.AroundRegex;
            EqualRegex = ArabicMergedExtractorConfiguration.EqualRegex;
            SuffixAfter = ArabicMergedExtractorConfiguration.SuffixAfterRegex;
            YearRegex = ArabicDatePeriodExtractorConfiguration.YearRegex;

            SuperfluousWordMatcher = ArabicMergedExtractorConfiguration.SuperfluousWordMatcher;

            DatePeriodParser = new BaseDatePeriodParser(new ArabicDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new ArabicTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new ArabicDateTimePeriodParserConfiguration(this));
            SetParser = new BaseSetParser(new ArabicSetParserConfiguration(this));
            HolidayParser = new BaseHolidayParser(new ArabicHolidayParserConfiguration(this));
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