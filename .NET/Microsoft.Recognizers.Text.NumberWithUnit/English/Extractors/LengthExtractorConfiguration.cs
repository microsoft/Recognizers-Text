using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class LengthExtractorConfiguration : EnglishNumberWithUnitExtractorConfiguration
    {
        public LengthExtractorConfiguration() : base(new CultureInfo(Culture.English)) { }

        public LengthExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => LenghtSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_LENGTH;

        public static readonly ImmutableDictionary<string, string> LenghtSuffixList = NumbersWithUnitDefinitions.LenghtSuffixList.ToImmutableDictionary();

        private static readonly ImmutableList<string> AmbiguousValues = NumbersWithUnitDefinitions.AmbiguousLengthUnitList.ToImmutableList();
    }
}