using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime.Turkish
{
    public sealed class TurkishMergedParserConfiguration : TurkishCommonDateTimeParserConfiguration, IMergedParserConfiguration
    {
        public TurkishMergedParserConfiguration(IOptionsConfiguration config)
            : base(config)
        {
            BeforeRegex = TurkishMergedExtractorConfiguration.BeforeRegex;
            AfterRegex = TurkishMergedExtractorConfiguration.AfterRegex;
            SinceRegex = TurkishMergedExtractorConfiguration.SinceRegex;
            AroundRegex = TurkishMergedExtractorConfiguration.AroundRegex;
            EqualRegex = TurkishMergedExtractorConfiguration.EqualRegex;
            SuffixAfter = TurkishMergedExtractorConfiguration.SuffixAfterRegex;
            YearRegex = TurkishDatePeriodExtractorConfiguration.YearRegex;

            SuperfluousWordMatcher = TurkishMergedExtractorConfiguration.SuperfluousWordMatcher;

            DatePeriodParser = new BaseDatePeriodParser(new TurkishDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new TurkishTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new TurkishDateTimePeriodParserConfiguration(this));
            SetParser = new BaseSetParser(new TurkishSetParserConfiguration(this));
            HolidayParser = new BaseHolidayParser(new TurkishHolidayParserConfiguration(this));
            TimeZoneParser = new BaseTimeZoneParser();
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
    }
}