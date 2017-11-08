using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public sealed class EnglishMergedParserConfiguration : EnglishCommonDateTimeParserConfiguration, IMergedParserConfiguration
    {
        public Regex BeforeRegex { get; }

        public Regex AfterRegex { get; }

        public Regex SinceRegex { get; }

        public IDateTimeParser GetParser { get; }

        public IDateTimeParser HolidayParser { get; }

        public EnglishMergedParserConfiguration() : base()
        {
            BeforeRegex = EnglishMergedExtractorConfiguration.BeforeRegex;
            AfterRegex = EnglishMergedExtractorConfiguration.AfterRegex;
            SinceRegex = EnglishMergedExtractorConfiguration.SinceRegex;
            DatePeriodParser = new BaseDatePeriodParser(new EnglishDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new EnglishTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new EnglishDateTimePeriodParserConfiguration(this));
            GetParser = new BaseSetParser(new EnglishSetParserConfiguration(this));
            HolidayParser = new BaseHolidayParser(new EnglishHolidayParserConfiguration());
        }
    }
}
