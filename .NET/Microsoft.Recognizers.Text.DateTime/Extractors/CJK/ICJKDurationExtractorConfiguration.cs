using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ICJKDurationExtractorConfiguration : IDateTimeOptionsConfiguration
    {
        Regex DurationUnitRegex { get; }

        Regex DurationConnectorRegex { get; }

        Regex YearRegex { get; }

        IExtractor InternalExtractor { get; }

        Dictionary<string, string> UnitMap { get; }

        Dictionary<string, long> UnitValueMap { get; }

    }
}