using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public sealed class EnglishMergedParserConfiguration : EnglishCommonDateTimeParserConfiguration, IMergedParserConfiguration
    {

        public Regex BeforeRegex { get; }

        public Regex AfterRegex { get; }

        public Regex SinceRegex { get; }

        public Regex YearAfterRegex { get; }

        public Regex YearRegex { get; }

        public IDateTimeParser SetParser { get; }

        public IDateTimeParser HolidayParser { get; }

        public IDateTimeParser TimeZoneParser { get; }

        public StringMatcher SuperfluousWordMatcher { get; }

        public EnglishMergedParserConfiguration(IOptionsConfiguration config) : base(config)
        {
            BeforeRegex = EnglishMergedExtractorConfiguration.BeforeRegex;
            AfterRegex = EnglishMergedExtractorConfiguration.AfterRegex;
            SinceRegex = EnglishMergedExtractorConfiguration.SinceRegex;
            YearAfterRegex = EnglishMergedExtractorConfiguration.YearAfterRegex;
            YearRegex = EnglishDatePeriodExtractorConfiguration.YearRegex;
            SuperfluousWordMatcher = EnglishMergedExtractorConfiguration.SuperfluousWordMatcher;

            DatePeriodParser = new BaseDatePeriodParser(new EnglishDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new EnglishTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new EnglishDateTimePeriodParserConfiguration(this));
            SetParser = new BaseSetParser(new EnglishSetParserConfiguration(this));
            HolidayParser = new BaseHolidayParser(new EnglishHolidayParserConfiguration(this));
            TimeZoneParser = new BaseTimeZoneParser();
        }
    }
}
