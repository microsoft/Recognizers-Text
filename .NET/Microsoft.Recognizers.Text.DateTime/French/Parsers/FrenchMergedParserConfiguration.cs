using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public sealed class FrenchMergedParserConfiguration : FrenchCommonDateTimeParserConfiguration, IMergedParserConfiguration
    {

        public Regex BeforeRegex { get; }

        public Regex AfterRegex { get; }

        public Regex SinceRegex { get; }

        public Regex AroundRegex { get; }

        public Regex YearAfterRegex { get; }

        public Regex YearRegex { get; }

        public IDateTimeParser SetParser { get; }

        public IDateTimeParser HolidayParser { get; }

        public IDateTimeParser TimeZoneParser { get; }

        public StringMatcher SuperfluousWordMatcher { get; }

        public FrenchMergedParserConfiguration(IOptionsConfiguration config) : base(config)
        {
            BeforeRegex = FrenchMergedExtractorConfiguration.BeforeRegex;
            AfterRegex = FrenchMergedExtractorConfiguration.AfterRegex;
            SinceRegex = FrenchMergedExtractorConfiguration.SinceRegex;
            AroundRegex = FrenchMergedExtractorConfiguration.AroundRegex;
            YearAfterRegex = FrenchMergedExtractorConfiguration.YearAfterRegex;
            YearRegex = FrenchDatePeriodExtractorConfiguration.YearRegex;
            SuperfluousWordMatcher = FrenchMergedExtractorConfiguration.SuperfluousWordMatcher;
            SetParser = new BaseSetParser(new FrenchSetParserConfiguration(this));
            HolidayParser = new BaseHolidayParser(new FrenchHolidayParserConfiguration(this));
            TimeZoneParser = new BaseTimeZoneParser();
        }
    }
}
