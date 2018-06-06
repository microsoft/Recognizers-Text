using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ITimeExtractorConfiguration : IOptionsConfiguration
    {
        IDateTimeExtractor TimeZoneExtractor { get; }

        IEnumerable<Regex> TimeRegexList { get; }

        Regex AtRegex { get; }

        Regex IshRegex { get; }

        Regex TimeBeforeAfterRegex { get; }
    }
}