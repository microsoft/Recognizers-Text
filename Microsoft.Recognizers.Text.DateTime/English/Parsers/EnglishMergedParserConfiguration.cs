using Microsoft.Recognizers.Resources.English;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public sealed class EnglishMergedParserConfiguration : EnglishCommonDateTimeParserConfiguration, IMergedParserConfiguration
    {
        public Regex BeforeRegex { get; }

        public Regex AfterRegex { get; }

        public IDateTimeParser GetParser { get; }

        public IDateTimeParser HolidayParser { get; }

        public EnglishMergedParserConfiguration() : base()
        {
            BeforeRegex = new Regex(DateTimeDefinition.BeforeRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            AfterRegex = new Regex(DateTimeDefinition.AfterRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            DatePeriodParser = new BaseDatePeriodParser(new EnglishDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new EnglishTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new EnglishDateTimePeriodParserConfiguration(this));
            GetParser = new BaseSetParser(new EnglishSetParserConfiguration(this));
            HolidayParser = new BaseHolidayParser(new EnglishHolidayParserConfiguration());
        }
    }
}
