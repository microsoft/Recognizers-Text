using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public sealed class PortugueseMergedParserConfiguration : PortugueseCommonDateTimeParserConfiguration, IMergedParserConfiguration
    {

        public Regex BeforeRegex { get; }

        public Regex AfterRegex { get; }

        public Regex SinceRegex { get; }

        public Regex YearAfterRegex { get; }

        public Regex YearRegex { get; }

        public IDateTimeParser GetParser { get; }

        public IDateTimeParser HolidayParser { get; }

        public IDateTimeParser TimeZoneParser { get; }

        public StringMatcher SuperfluousWordMatcher { get; }

        public PortugueseMergedParserConfiguration(DateTimeOptions options) : base(options)
        {
            BeforeRegex = PortugueseMergedExtractorConfiguration.BeforeRegex;
            AfterRegex = PortugueseMergedExtractorConfiguration.AfterRegex;
            SinceRegex = PortugueseMergedExtractorConfiguration.SinceRegex;
            YearAfterRegex = PortugueseMergedExtractorConfiguration.YearAfterRegex;
            YearRegex = PortugueseDatePeriodExtractorConfiguration.YearRegex;
            SuperfluousWordMatcher = PortugueseMergedExtractorConfiguration.SuperfluousWordMatcher;

            DatePeriodParser = new BaseDatePeriodParser(new PortugueseDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new PortugueseTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new DateTimePeriodParser(new PortugueseDateTimePeriodParserConfiguration(this));
            GetParser = new BaseSetParser(new PortugueseSetParserConfiguration(this));
            HolidayParser = new BaseHolidayParser(new PortugueseHolidayParserConfiguration());
            TimeZoneParser = new BaseTimeZoneParser();
        }
    }
}
