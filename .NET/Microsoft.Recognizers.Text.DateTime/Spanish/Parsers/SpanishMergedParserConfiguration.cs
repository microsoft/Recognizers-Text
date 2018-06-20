using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public sealed class SpanishMergedParserConfiguration : SpanishCommonDateTimeParserConfiguration, IMergedParserConfiguration
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

        public SpanishMergedParserConfiguration(DateTimeOptions options) : base(options)
        {
            BeforeRegex = SpanishMergedExtractorConfiguration.BeforeRegex;
            AfterRegex = SpanishMergedExtractorConfiguration.AfterRegex;
            SinceRegex = SpanishMergedExtractorConfiguration.SinceRegex;
            YearAfterRegex = SpanishMergedExtractorConfiguration.YearAfterRegex;
            YearRegex = SpanishDatePeriodExtractorConfiguration.YearRegex;
            SuperfluousWordMatcher = SpanishMergedExtractorConfiguration.SuperfluousWordMatcher;

            DatePeriodParser = new BaseDatePeriodParser(new SpanishDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new SpanishTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new DateTimePeriodParser(new SpanishDateTimePeriodParserConfiguration(this));
            GetParser = new BaseSetParser(new SpanishSetParserConfiguration(this));
            HolidayParser = new BaseHolidayParser(new SpanishHolidayParserConfiguration());
            TimeZoneParser = new BaseTimeZoneParser();
        }
    }
}
