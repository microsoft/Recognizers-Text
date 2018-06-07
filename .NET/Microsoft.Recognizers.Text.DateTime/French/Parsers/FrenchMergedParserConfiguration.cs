using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public sealed class FrenchMergedParserConfiguration : FrenchCommonDateTimeParserConfiguration, IMergedParserConfiguration
    {

        public Regex BeforeRegex { get; }

        public Regex AfterRegex { get; }

        public Regex SinceRegex { get; }

        public Regex YearAfterRegex { get; }

        public Regex YearRegex { get; }

        public IDateTimeParser GetParser { get; }

        public IDateTimeParser HolidayParser { get; }

        public IDateTimeParser TimeZoneParser { get; }

        public FrenchMergedParserConfiguration(DateTimeOptions options) : base(options)
        {
            BeforeRegex = FrenchMergedExtractorConfiguration.BeforeRegex;
            AfterRegex = FrenchMergedExtractorConfiguration.AfterRegex;
            SinceRegex = FrenchMergedExtractorConfiguration.SinceRegex;
            YearAfterRegex = FrenchMergedExtractorConfiguration.YearAfterRegex;
            YearRegex = FrenchDatePeriodExtractorConfiguration.YearRegex;
            DatePeriodParser = new BaseDatePeriodParser(new FrenchDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new FrenchTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new FrenchDateTimePeriodParserConfiguration(this));
            GetParser = new BaseSetParser(new FrenchSetParserConfiguration(this));
            HolidayParser = new BaseHolidayParser(new FrenchHolidayParserConfiguration());
            TimeZoneParser = new BaseTimeZoneParser();
        }
    }
}
