using Microsoft.Recognizers.Text.Number.Chinese.Extractors;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Chinese.Extractors
{
    public abstract class ChineseNumberWithUnitExtractorConfiguration : INumberWithUnitExtractorConfiguration
    {
        protected ChineseNumberWithUnitExtractorConfiguration(CultureInfo ci)
        {
            this.CultureInfo = ci;
            this.UnitNumExtractor = new NumberExtractor(ChineseNumberMode.ExtractAll);
            this.BuildPrefix = @"";
            this.BuildSuffix = @"";
            this.ConnectorToken = string.Empty;
        }

        public abstract string ExtractType { get; }

        public CultureInfo CultureInfo { get; }

        public IExtractor UnitNumExtractor { get; }

        public string BuildPrefix { get; }

        public string BuildSuffix { get; }

        public string ConnectorToken { get; }

        public abstract ImmutableDictionary<string, string> SuffixList { get; }
        public abstract ImmutableDictionary<string, string> PrefixList { get; }
        public abstract ImmutableList<string> AmbiguousUnitList { get; }
    }
}
