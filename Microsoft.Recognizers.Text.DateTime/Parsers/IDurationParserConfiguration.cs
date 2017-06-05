using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDurationParserConfiguration
    {
        IExtractor CardinalExtractor { get; }
        IParser NumberParser { get; }

        Regex NumberCombinedWithUnit { get; }
        Regex AnUnitRegex { get; }
        Regex AllDateUnitRegex { get; }
        
        IImmutableDictionary<string, string> UnitMap { get; }

        IImmutableDictionary<string, long> UnitValueMap { get; }
    }
}
