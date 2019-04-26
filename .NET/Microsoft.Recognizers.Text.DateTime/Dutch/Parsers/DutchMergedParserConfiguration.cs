using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime.Dutch
{
    public sealed class DutchMergedParserConfiguration : DutchCommonDateTimeParserConfiguration, IMergedParserConfiguration
    {
        public DutchMergedParserConfiguration(IOptionsConfiguration config)
            : base(config)
        {
            BeforeRegex = DutchMergedExtractorConfiguration.BeforeRegex;
            AfterRegex = DutchMergedExtractorConfiguration.AfterRegex;
            SinceRegex = DutchMergedExtractorConfiguration.SinceRegex;
            AroundRegex = DutchMergedExtractorConfiguration.AroundRegex;
            SuffixAfter = DutchMergedExtractorConfiguration.SuffixAfterRegex;
            YearRegex = DutchDatePeriodExtractorConfiguration.YearRegex;
            SuperfluousWordMatcher = DutchMergedExtractorConfiguration.SuperfluousWordMatcher;

            DatePeriodParser = new BaseDatePeriodParser(new DutchDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new DutchTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new DutchDateTimePeriodParserConfiguration(this));
            SetParser = new BaseSetParser(new DutchSetParserConfiguration(this));
            HolidayParser = new BaseHolidayParser(new DutchHolidayParserConfiguration(this));
            TimeZoneParser = new BaseTimeZoneParser();
        }

        public Regex BeforeRegex { get; }

        public Regex AfterRegex { get; }

        public Regex SinceRegex { get; }

        public Regex AroundRegex { get; }

        public Regex SuffixAfter { get; }

        public Regex YearRegex { get; }

        public IDateTimeParser SetParser { get; }

        public IDateTimeParser HolidayParser { get; }

        public StringMatcher SuperfluousWordMatcher { get; }
    }
}
