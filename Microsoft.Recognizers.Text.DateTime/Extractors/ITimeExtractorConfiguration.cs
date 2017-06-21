using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ITimeExtractorConfiguration
    {
        IEnumerable<Regex> TimeRegexList { get; }
        Regex AtRegex { get; }
        Regex IshRegex { get; }
    }
}