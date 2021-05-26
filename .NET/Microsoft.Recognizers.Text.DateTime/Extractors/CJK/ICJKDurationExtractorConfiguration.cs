using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ICJKDurationExtractorConfiguration : IDateTimeOptionsConfiguration
    {
        Regex DurationUnitRegex { get; }

        Regex DurationConnectorRegex { get; }

        Regex YearRegex { get; }

        Regex AllRegex { get; }

        Regex HalfRegex { get; }

        Regex RelativeDurationUnitRegex { get; }

        Regex DuringRegex { get; }

        Regex SomeRegex { get; }

        Regex MoreOrLessRegex { get; }

        IExtractor InternalExtractor { get; }

        Dictionary<string, string> UnitMap { get; }

        Dictionary<string, long> UnitValueMap { get; }

    }
}