using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public interface INumberWithUnitExtractorConfiguration
    {
        ImmutableDictionary<string, string> SuffixList { get; }

        ImmutableDictionary<string, string> PrefixList { get; }

        ImmutableList<string> AmbiguousUnitList { get; }

        string ExtractType { get; }

        CultureInfo CultureInfo { get; }

        IExtractor UnitNumExtractor { get; }

        string BuildPrefix { get; }

        string BuildSuffix { get; }

        string ConnectorToken { get; }

        Regex CompoundUnitConnectorRegex { get; }

        Regex NonUnitRegex { get; }

        Regex AmbiguousUnitNumberMultiplierRegex { get; }

        Dictionary<Regex, Regex> AmbiguityFiltersDict { get; }
    }
}
