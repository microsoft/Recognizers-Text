using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.German;

namespace Microsoft.Recognizers.Text.NumberWithUnit.German
{
    public class LengthExtractorConfiguration : GermanNumberWithUnitExtractorConfiguration
    {
        public LengthExtractorConfiguration() : base(new CultureInfo(Culture.German)) { }

        public LengthExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => LenghtSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_LENGTH;

        public static readonly ImmutableDictionary<string, string> LenghtSuffixList = NumbersWithUnitDefinitions.LenghtSuffixList.ToImmutableDictionary();

        private static readonly ImmutableList<string> AmbiguousValues = NumbersWithUnitDefinitions.AmbiguousLengthUnitList.ToImmutableList();
    }
}