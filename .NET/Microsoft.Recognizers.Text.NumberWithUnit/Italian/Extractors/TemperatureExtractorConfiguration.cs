using System.Collections.Immutable;
using System.Globalization;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Italian;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Italian
{
    public class TemperatureExtractorConfiguration : ItalianNumberWithUnitExtractorConfiguration
    {
        public static readonly ImmutableSortedDictionary<string, string> TemperatureSuffixList =
            NumbersWithUnitDefinitions.TemperatureSuffixList.ToImmutableSortedDictionary();

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex AmbiguousUnitMultiplierRegex =
            new Regex(BaseUnits.AmbiguousUnitNumberMultiplierRegex, RegexFlags);

        public TemperatureExtractorConfiguration()
            : this(new CultureInfo(Culture.Italian))
        {
        }

        public TemperatureExtractorConfiguration(CultureInfo ci)
            : base(ci)
        {
        }

        public override ImmutableSortedDictionary<string, string> SuffixList => TemperatureSuffixList;

        public override ImmutableSortedDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => null;

        public override string ExtractType => Constants.SYS_UNIT_TEMPERATURE;

        public override Regex AmbiguousUnitNumberMultiplierRegex => AmbiguousUnitMultiplierRegex;
    }
}