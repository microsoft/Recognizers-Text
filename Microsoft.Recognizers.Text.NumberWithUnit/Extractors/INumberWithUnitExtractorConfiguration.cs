using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public interface INumberWithUnitExtractorConfiguration
    {
        ImmutableDictionary<string, string> SuffixList { get; }
        ImmutableDictionary<string, string> PrefixList { get; }

        string ExtractType { get; }
        CultureInfo CultureInfo { get; }
        IExtractor UnitNumExtractor { get; }
        string BuildPrefix { get; }
        string BuildSuffix { get; }
        string ConnectorToken { get; }
    }
}
