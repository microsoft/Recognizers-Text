using System.Collections.Immutable;
using System.Globalization;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Chinese
{
    public class TemperatureExtractorConfiguration : ChineseNumberWithUnitExtractorConfiguration
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex AmbiguousUnitMultiplierRegex =
            new Regex(BaseUnits.AmbiguousUnitNumberMultiplierRegex, RegexFlags);

        public TemperatureExtractorConfiguration()
            : this(new CultureInfo(Culture.Chinese))
        {
        }

        public TemperatureExtractorConfiguration(CultureInfo ci)
            : base(ci)
        {
        }

        public override ImmutableSortedDictionary<string, string> SuffixList =>
            NumbersWithUnitDefinitions.TemperatureSuffixList.ToImmutableSortedDictionary();

        public override ImmutableSortedDictionary<string, string> PrefixList =>
            NumbersWithUnitDefinitions.TemperaturePrefixList.ToImmutableSortedDictionary();

        public override ImmutableList<string> AmbiguousUnitList =>
            NumbersWithUnitDefinitions.TemperatureAmbiguousValues.ToImmutableList();

        public override string ExtractType => Constants.SYS_UNIT_TEMPERATURE;

        public override Regex AmbiguousUnitNumberMultiplierRegex => AmbiguousUnitMultiplierRegex;
    }
}