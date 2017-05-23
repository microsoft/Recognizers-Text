using Microsoft.Recognizers.Text.Number.Utilities;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English.Extractors
{
    public class AreaExtractorConfiguration : EnglishNumberWithUnitExtractorConfiguration
    {
        public AreaExtractorConfiguration() : this(new CultureInfo(Culture.English)) { }

        public AreaExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => AreaSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => null;

        public override string ExtractType => Constants.SYS_UNIT_AREA;

        public static readonly ImmutableDictionary<string, string> AreaSuffixList = new Dictionary<string, string>
        {
            {"sq m", "sq m|square meter|square meters|square metre|square metres|acre|acres"}
        }.ToImmutableDictionary();
    }
}
