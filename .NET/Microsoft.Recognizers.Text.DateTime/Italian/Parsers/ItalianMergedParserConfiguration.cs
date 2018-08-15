using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Italian
{
    public sealed class ItalianMergedParserConfiguration : ItalianCommonDateTimeParserConfiguration, IMergedParserConfiguration
    {

        public Regex BeforeRegex { get; }

        public Regex AfterRegex { get; }

        public Regex SinceRegex { get; }

        public Regex YearAfterRegex { get; }

        public Regex YearRegex { get; }

        public IDateTimeParser GetParser { get; }

        public IDateTimeParser HolidayParser { get; }

        public ItalianMergedParserConfiguration(DateTimeOptions options) : base(options)
        {
            BeforeRegex = ItalianMergedExtractorConfiguration.BeforeRegex;
            AfterRegex = ItalianMergedExtractorConfiguration.AfterRegex;
            SinceRegex = ItalianMergedExtractorConfiguration.SinceRegex;
            YearAfterRegex = ItalianMergedExtractorConfiguration.YearAfterRegex;
            YearRegex = ItalianDatePeriodExtractorConfiguration.YearRegex;
            DatePeriodParser = new BaseDatePeriodParser(new ItalianDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new ItalianTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new ItalianDateTimePeriodParserConfiguration(this));
            GetParser = new BaseSetParser(new ItalianSetParserConfiguration(this));
            HolidayParser = new BaseHolidayParser(new ItalianHolidayParserConfiguration());
        }
    }
}
