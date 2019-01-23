using System.Collections.Immutable;
using System.Globalization;
using Microsoft.Recognizers.Definitions.Japanese;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Japanese
{
    public class AgeExtractorConfiguration : JapaneseNumberWithUnitExtractorConfiguration
    {
        public AgeExtractorConfiguration()
            : this(new CultureInfo(Culture.Japanese))
        {
        }

        public AgeExtractorConfiguration(CultureInfo ci)
            : base(ci)
        {
        }

        public override ImmutableDictionary<string, string> SuffixList => NumbersWithUnitDefinitions.AgeSuffixList.ToImmutableDictionary();

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => NumbersWithUnitDefinitions.AgeAmbiguousValues.ToImmutableList();

        public override string ExtractType => Constants.SYS_UNIT_AGE;
    }
}
