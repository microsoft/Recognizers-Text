using System.Collections.Immutable;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.NumberWithUnit;
using Microsoft.Recognizers.Text.NumberWithUnit.Korean;
using static Microsoft.Recognizers.Text.DateTime.Korean.KoreanDurationExtractorConfiguration;

namespace Microsoft.Recognizers.Text.DateTime.Korean
{
    public class KoreanDurationParserConfiguration : BaseDateTimeOptionsConfiguration, ICJKDurationParserConfiguration
    {

        public KoreanDurationParserConfiguration(ICJKCommonDateTimeParserConfiguration config)
            : base(config)
        {
            InternalParser = new NumberWithUnitParser(new DurationParserConfiguration());

            var durationConfig = new BaseDateTimeOptionsConfiguration(config.Culture, DateTimeOptions.None);
            DurationExtractor = new BaseCJKDurationExtractor(new KoreanDurationExtractorConfiguration(durationConfig), false);

            YearRegex = KoreanDurationExtractorConfiguration.YearRegex;
            DurationUnitRegex = KoreanDurationExtractorConfiguration.DurationUnitRegex;
            DurationConnectorRegex = KoreanDurationExtractorConfiguration.DurationConnectorRegex;

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

        internal class DurationParserConfiguration : KoreanNumberWithUnitParserConfiguration
        {
            public DurationParserConfiguration()
                : base(new CultureInfo(Text.Culture.Korean))
            {
                this.BindDictionary(DurationExtractorConfiguration.DurationSuffixList);
            }
        }
    }
}