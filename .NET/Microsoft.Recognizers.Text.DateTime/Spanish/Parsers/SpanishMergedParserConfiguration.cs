using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public sealed class SpanishMergedParserConfiguration : SpanishCommonDateTimeParserConfiguration, IMergedParserConfiguration
    {
        public Regex BeforeRegex { get; }

        public Regex AfterRegex { get; }

        public Regex SinceRegex { get; }

        public IDateTimeParser GetParser { get; }

        public IDateTimeParser HolidayParser { get; }

        public SpanishMergedParserConfiguration() : base()
        {
            BeforeRegex = SpanishMergedExtractorConfiguration.BeforeRegex;
            AfterRegex = SpanishMergedExtractorConfiguration.AfterRegex;
            SinceRegex = SpanishMergedExtractorConfiguration.SinceRegex;

            DatePeriodParser = new BaseDatePeriodParser(new SpanishDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new SpanishTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new DateTimePeriodParser(new SpanishDateTimePeriodParserConfiguration(this));
            GetParser = new BaseSetParser(new SpanishSetParserConfiguration(this));
            HolidayParser = new BaseHolidayParser(new SpanishHolidayParserConfiguration());
        }
    }
}
