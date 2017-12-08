using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public sealed class PortugueseMergedParserConfiguration : PortugueseCommonDateTimeParserConfiguration, IMergedParserConfiguration
    {

        public Regex BeforeRegex { get; }

        public Regex AfterRegex { get; }

        public Regex SinceRegex { get; }

        public IDateTimeParser GetParser { get; }

        public IDateTimeParser HolidayParser { get; }

        public PortugueseMergedParserConfiguration(DateTimeOptions options) : base(options)
        {
            BeforeRegex = PortugueseMergedExtractorConfiguration.BeforeRegex;
            AfterRegex = PortugueseMergedExtractorConfiguration.AfterRegex;
            SinceRegex = PortugueseMergedExtractorConfiguration.SinceRegex;

            DatePeriodParser = new BaseDatePeriodParser(new PortugueseDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new PortugueseTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new DateTimePeriodParser(new PortugueseDateTimePeriodParserConfiguration(this));
            GetParser = new BaseSetParser(new PortugueseSetParserConfiguration(this));
            HolidayParser = new BaseHolidayParser(new PortugueseHolidayParserConfiguration());
        }
    }
}
