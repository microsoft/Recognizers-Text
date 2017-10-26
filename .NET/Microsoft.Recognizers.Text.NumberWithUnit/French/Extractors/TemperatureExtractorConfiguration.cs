using System.Collections.Immutable;
using System.Globalization;
using Microsoft.Recognizers.Definitions.French;

namespace Microsoft.Recognizers.Text.NumberWithUnit.French
{
    public class TemperatureExtractorConfiguration : FrenchNumberWithUnitExtractorConfiguration
    {
        public TemperatureExtractorConfiguration() : this(new CultureInfo(Culture.French)) { }

        public TemperatureExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => TemperatureSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => null;

        public override string ExtractType => Constants.SYS_UNIT_TEMPERATURE;

        public static readonly ImmutableDictionary<string, string> TemperatureSuffixList = NumbersWithUnitDefinitions.TemperatureSuffixList.ToImmutableDictionary();
    }
}