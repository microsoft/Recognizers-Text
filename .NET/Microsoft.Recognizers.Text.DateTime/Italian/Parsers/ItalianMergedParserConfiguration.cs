using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Italian;
using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime.Italian
{
    public sealed class ItalianMergedParserConfiguration : ItalianCommonDateTimeParserConfiguration, IMergedParserConfiguration
    {
        public ItalianMergedParserConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            BeforeRegex = ItalianMergedExtractorConfiguration.BeforeRegex;
            AfterRegex = ItalianMergedExtractorConfiguration.AfterRegex;
            SinceRegex = ItalianMergedExtractorConfiguration.SinceRegex;
            AroundRegex = ItalianMergedExtractorConfiguration.AroundRegex;
            EqualRegex = ItalianMergedExtractorConfiguration.EqualRegex;
            SuffixAfter = ItalianMergedExtractorConfiguration.SuffixAfterRegex;
            YearRegex = ItalianDatePeriodExtractorConfiguration.YearRegex;

            SuperfluousWordMatcher = ItalianMergedExtractorConfiguration.SuperfluousWordMatcher;
            DateParser = new BaseDateParser(new ItalianDateParserConfiguration(this));
            DatePeriodParser = new BaseDatePeriodParser(new ItalianDatePeriodParserConfiguration(this));
            DateTimeParser = new BaseDateTimeParser(new ItalianDateTimeParserConfiguration(this));
            DurationParser = new BaseDurationParser(new ItalianDurationParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new ItalianTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new ItalianDateTimePeriodParserConfiguration(this));
            SetParser = new BaseSetParser(new ItalianSetParserConfiguration(this));
            HolidayParser = new BaseHolidayParser(new ItalianHolidayParserConfiguration(this));
            TimeZoneParser = new DummyTimeZoneParser();
        }

        public Regex BeforeRegex { get; }

        public Regex AfterRegex { get; }

        public Regex SinceRegex { get; }

        public Regex AroundRegex { get; }

        public Regex EqualRegex { get; }

        public Regex SuffixAfter { get; }

        public Regex YearRegex { get; }

        public IDateTimeParser SetParser { get; }

        public IDateTimeParser HolidayParser { get; }

        public StringMatcher SuperfluousWordMatcher { get; }

        bool IMergedParserConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;
    }
}
