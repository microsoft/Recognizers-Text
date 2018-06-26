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

        public IDateTimeParser GetParser { get; }

        public IDateTimeParser HolidayParser { get; }

        public IDateTimeParser TimeZoneParser { get; }

        public StringMatcher SuperfluousWordMatcher { get; }

        public EnglishMergedParserConfiguration(DateTimeOptions options) : base(options)
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
            GetParser = new BaseSetParser(new EnglishSetParserConfiguration(this));
            HolidayParser = new BaseHolidayParser(new EnglishHolidayParserConfiguration());
            TimeZoneParser = new BaseTimeZoneParser();
        }
    }
}
