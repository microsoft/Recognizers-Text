using System.Collections.Immutable;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Dutch;
using Microsoft.Recognizers.Text.Number.Dutch;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Dutch
{
    public abstract class DutchNumberWithUnitExtractorConfiguration : INumberWithUnitExtractorConfiguration
    {
        protected DutchNumberWithUnitExtractorConfiguration(CultureInfo ci)
        {
            this.CultureInfo = ci;
            this.UnitNumExtractor = NumberExtractor.GetInstance();
            this.BuildPrefix = NumbersWithUnitDefinitions.BuildPrefix;
            this.BuildSuffix = NumbersWithUnitDefinitions.BuildSuffix;
            this.ConnectorToken = string.Empty;
            this.CompoundUnitConnectorRegex = new Regex(NumbersWithUnitDefinitions.CompoundUnitConnectorRegex, RegexOptions.IgnoreCase);
            this.PmNonUnitRegex = new Regex(BaseUnits.PmNonUnitRegex, RegexOptions.IgnoreCase);
        }

        public abstract string ExtractType { get; }

        public CultureInfo CultureInfo { get; }

        public IExtractor UnitNumExtractor { get; }

        public string BuildPrefix { get; }

        public string BuildSuffix { get; }

        public string ConnectorToken { get; }

        public Regex CompoundUnitConnectorRegex { get; set; }

        public Regex PmNonUnitRegex { get; set; }

        public abstract ImmutableDictionary<string, string> SuffixList { get; }

        public abstract ImmutableDictionary<string, string> PrefixList { get; }

        public abstract ImmutableList<string> AmbiguousUnitList { get; }
    }
}