using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ITimeExtractorConfiguration
    {
        IEnumerable<Regex> TimeRegexList { get; }

        IExtractor TimeExtractor { get; }

        IExtractor NumExtractor { get; }

        Regex AtRegex { get; }

        Regex IshRegex { get; }

        Regex SpecialTimePattern { get; }
    }
}