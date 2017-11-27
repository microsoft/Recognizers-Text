using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateTimeALTExtractorConfiguration
    {
        IDateTimeExtractor DateExtractor { get; }

        IDateTimeExtractor DatePeriodExtractor { get; }

        IEnumerable<Regex> RelativePrefixList { get; }

        IEnumerable<Regex> AmPmRegexList { get; }

        Regex OrRegex { get; }
    }
}