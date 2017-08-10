using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using Microsoft.Recognizers.Resources.English;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class TemperatureExtractorConfiguration : EnglishNumberWithUnitExtractorConfiguration
    {
        public TemperatureExtractorConfiguration() : this(new CultureInfo(Culture.English)) { }

        public TemperatureExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => TemperatureSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_TEMPERATURE;

        public static readonly ImmutableDictionary<string, string> TemperatureSuffixList = NumericWithUnit.TemperatureSuffixList.ToImmutableDictionary();

        private static readonly ImmutableList<string> AmbiguousValues = NumericWithUnit.AmbiguousTemperatureUnitList.ToImmutableList();
    }
}