using System.Collections.Immutable;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.NumberWithUnit;
using Microsoft.Recognizers.Text.NumberWithUnit.Chinese;
using static Microsoft.Recognizers.Text.DateTime.Chinese.ChineseDurationExtractorConfiguration;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class ChineseDurationParserConfiguration : BaseDateTimeOptionsConfiguration, ICJKDurationParserConfiguration
    {

        public ChineseDurationParserConfiguration(ICJKCommonDateTimeParserConfiguration config)
            : base(config)
        {
            InternalParser = new NumberWithUnitParser(new DurationParserConfiguration());

            var durationConfig = new BaseDateTimeOptionsConfiguration(config.Culture, DateTimeOptions.None);
            DurationExtractor = new BaseCJKDurationExtractor(new ChineseDurationExtractorConfiguration(durationConfig), false);

            YearRegex = ChineseDurationExtractorConfiguration.YearRegex;
            DurationUnitRegex = ChineseDurationExtractorConfiguration.DurationUnitRegex;
            DurationConnectorRegex = ChineseDurationExtractorConfiguration.DurationConnectorRegex;

            UnitMap = config.UnitMap;
            UnitValueMap = config.UnitValueMap;
        }

        public IDateTimeExtractor DurationExtractor { get; }

        public IParser InternalParser { get; }

        public Regex YearRegex { get; }

        public Regex DurationUnitRegex { get; }

        public Regex DurationConnectorRegex { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IImmutableDictionary<string, long> UnitValueMap { get; }

        internal class DurationParserConfiguration : ChineseNumberWithUnitParserConfiguration
        {
            public DurationParserConfiguration()
                : base(new CultureInfo(Text.Culture.Chinese))
            {
                this.BindDictionary(DurationExtractorConfiguration.DurationSuffixList);
            }
        }
    }
}