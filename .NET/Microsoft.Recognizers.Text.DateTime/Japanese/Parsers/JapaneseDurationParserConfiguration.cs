using System.Collections.Immutable;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.NumberWithUnit;
using Microsoft.Recognizers.Text.NumberWithUnit.Japanese;
using static Microsoft.Recognizers.Text.DateTime.Japanese.JapaneseDurationExtractorConfiguration;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseDurationParserConfiguration : BaseDateTimeOptionsConfiguration, ICJKDurationParserConfiguration
    {

        public JapaneseDurationParserConfiguration(ICJKCommonDateTimeParserConfiguration config)
            : base(config)
        {
            InternalParser = new NumberWithUnitParser(new DurationParserConfiguration());

            var durationConfig = new BaseDateTimeOptionsConfiguration(config.Culture, DateTimeOptions.None);
            DurationExtractor = new BaseCJKDurationExtractor(new JapaneseDurationExtractorConfiguration(durationConfig), false);

            YearRegex = JapaneseDurationExtractorConfiguration.YearRegex;
            DurationUnitRegex = JapaneseDurationExtractorConfiguration.DurationUnitRegex;
            DurationConnectorRegex = JapaneseDurationExtractorConfiguration.DurationConnectorRegex;

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

        internal class DurationParserConfiguration : JapaneseNumberWithUnitParserConfiguration
        {
            public DurationParserConfiguration()
                : base(new CultureInfo(Text.Culture.Japanese))
            {
                this.BindDictionary(DurationExtractorConfiguration.DurationSuffixList);
            }
        }
    }
}