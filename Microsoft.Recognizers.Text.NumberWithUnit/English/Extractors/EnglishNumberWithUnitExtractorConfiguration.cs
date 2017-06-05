using System.Collections.Immutable;
using System.Globalization;
using Microsoft.Recognizers.Text.Number.English;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public abstract class EnglishNumberWithUnitExtractorConfiguration : INumberWithUnitExtractorConfiguration
    {
        protected EnglishNumberWithUnitExtractorConfiguration(CultureInfo ci)
        {
            this.CultureInfo = ci;
            this.UnitNumExtractor = new NumberExtractor();
            this.BuildPrefix = @"(?<=(\s|^|\W))";
            this.BuildSuffix = @"(?=(\s|\W|$))";
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
