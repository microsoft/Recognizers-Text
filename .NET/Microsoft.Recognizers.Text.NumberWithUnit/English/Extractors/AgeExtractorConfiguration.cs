using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class AgeExtractorConfiguration : EnglishNumberWithUnitExtractorConfiguration
    {
        public AgeExtractorConfiguration() : this(new CultureInfo(Culture.English)) { }

        public AgeExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => AgeSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => null;

        public override string ExtractType => Constants.SYS_UNIT_AGE;

        public static readonly ImmutableDictionary<string, string> AgeSuffixList = NumbersWithUnitDefinitions.AgeSuffixList.ToImmutableDictionary();
    }
}