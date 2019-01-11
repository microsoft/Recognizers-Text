using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Chinese;
using Microsoft.Recognizers.Text.Number.Chinese;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Chinese
{
    public abstract class ChineseNumberWithUnitExtractorConfiguration : INumberWithUnitExtractorConfiguration
    {
        private static readonly Regex CompoundUnitConnRegex =
            new Regex(NumbersWithUnitDefinitions.CompoundUnitConnectorRegex, RegexOptions.None);

        private static readonly Regex NonUnitsRegex =
            new Regex(BaseUnits.PmNonUnitRegex, RegexOptions.None);

        protected ChineseNumberWithUnitExtractorConfiguration(CultureInfo ci)
        {
            this.CultureInfo = ci;
            this.UnitNumExtractor = new NumberExtractor(ChineseNumberExtractorMode.ExtractAll);
            this.BuildPrefix = NumbersWithUnitDefinitions.BuildPrefix;
            this.BuildSuffix = NumbersWithUnitDefinitions.BuildSuffix;
            this.ConnectorToken = NumbersWithUnitDefinitions.ConnectorToken;
        }

        public Regex CompoundUnitConnectorRegex => CompoundUnitConnRegex;

        public Regex NonUnitRegex => NonUnitsRegex;

        public virtual Regex AmbiguousUnitNumberMultiplierRegex => null;

        public abstract string ExtractType { get; }

        public CultureInfo CultureInfo { get; }

        public IExtractor UnitNumExtractor { get; }

        public string BuildPrefix { get; }

        public string BuildSuffix { get; }

        public string ConnectorToken { get; }

        public IExtractor IntegerExtractor { get; }

        public Dictionary<Regex, Regex> AmbiguityFiltersDict { get; } = null;

        public abstract ImmutableDictionary<string, string> SuffixList { get; }

        public abstract ImmutableDictionary<string, string> PrefixList { get; }

        public abstract ImmutableList<string> AmbiguousUnitList { get; }
    }
}
