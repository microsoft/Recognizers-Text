using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Hindi;
using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime.Hindi
{
    public class HindiMergedParserConfiguration : HindiCommonDateTimeParserConfiguration, IMergedParserConfiguration
    {
        public HindiMergedParserConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            BeforeRegex = HindiMergedExtractorConfiguration.BeforeRegex;
            AfterRegex = HindiMergedExtractorConfiguration.AfterRegex;
            SinceRegex = HindiMergedExtractorConfiguration.SinceRegex;
            AroundRegex = HindiMergedExtractorConfiguration.AroundRegex;
            EqualRegex = HindiMergedExtractorConfiguration.EqualRegex;
            SuffixAfter = HindiMergedExtractorConfiguration.SuffixAfterRegex;
            YearRegex = HindiDatePeriodExtractorConfiguration.YearRegex;

            SuperfluousWordMatcher = HindiMergedExtractorConfiguration.SuperfluousWordMatcher;

            DatePeriodParser = new BaseDatePeriodParser(new HindiDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new HindiTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new HindiDateTimePeriodParserConfiguration(this));
            SetParser = new BaseSetParser(new HindiSetParserConfiguration(this));
            HolidayParser = new BaseHolidayParser(new HindiHolidayParserConfiguration(this));
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
