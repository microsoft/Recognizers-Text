using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public sealed class GermanMergedParserConfiguration : GermanCommonDateTimeParserConfiguration, IMergedParserConfiguration
    {
        public GermanMergedParserConfiguration(IOptionsConfiguration config)
            : base(config)
        {
            BeforeRegex = GermanMergedExtractorConfiguration.BeforeRegex;
            AfterRegex = GermanMergedExtractorConfiguration.AfterRegex;
            SinceRegex = GermanMergedExtractorConfiguration.SinceRegex;
            AroundRegex = GermanMergedExtractorConfiguration.AroundRegex;
            SuffixAfter = GermanMergedExtractorConfiguration.SuffixAfterRegex;
            YearRegex = GermanDatePeriodExtractorConfiguration.YearRegex;
            SuperfluousWordMatcher = GermanMergedExtractorConfiguration.SuperfluousWordMatcher;
            DatePeriodParser = new BaseDatePeriodParser(new GermanDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new GermanTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new GermanDateTimePeriodParserConfiguration(this));
            SetParser = new BaseSetParser(new GermanSetParserConfiguration(this));
            HolidayParser = new HolidayParserGer(new GermanHolidayParserConfiguration(this));
            TimeZoneParser = new DummyTimeZoneParser();
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
