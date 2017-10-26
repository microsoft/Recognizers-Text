using System.Collections.Immutable;
using System.Globalization;
using Microsoft.Recognizers.Definitions.French;
using Microsoft.Recognizers.Text.Number.French;

namespace Microsoft.Recognizers.Text.NumberWithUnit.French
{
    public abstract class FrenchNumberWithUnitExtractorConfiguration : INumberWithUnitExtractorConfiguration
    {
        protected FrenchNumberWithUnitExtractorConfiguration(CultureInfo ci)
        {
            this.CultureInfo = ci;
            this.UnitNumExtractor = new NumberExtractor();
            this.BuildPrefix = NumbersWithUnitDefinitions.BuildPrefix;
            this.BuildSuffix = NumbersWithUnitDefinitions.BuildSuffix;
            this.ConnectorToken = NumbersWithUnitDefinitions.ConnectorToken;
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