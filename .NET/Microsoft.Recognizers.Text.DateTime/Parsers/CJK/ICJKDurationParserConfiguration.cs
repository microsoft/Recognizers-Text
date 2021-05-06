using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ICJKDurationParserConfiguration : IDateTimeOptionsConfiguration
    {
        IDateTimeExtractor DurationExtractor { get; }

        IParser InternalParser { get; }

        Regex YearRegex { get; }

        Regex DurationUnitRegex { get; }

        Regex DurationConnectorRegex { get; }

        IImmutableDictionary<string, string> UnitMap { get; }

        IImmutableDictionary<string, long> UnitValueMap { get; }

    }
}
