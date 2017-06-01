using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Extractors
{
    public interface IDatePeriodExtractorConfiguration
    {
        IEnumerable<Regex> SimpleCasesRegexes { get; }
        Regex TillRegex { get; }
        Regex FollowedUnit { get; }
        Regex NumberCombinedWithUnit { get; }
        Regex PastRegex { get; }
        Regex FutureRegex { get; }

        IExtractor DatePointExtractor { get; }
        IExtractor CardinalExtractor { get; }

        bool GetFromTokenIndex(string text, out int index);
        bool HasConnectorToken(string text);
        bool GetBetweenTokenIndex(string text, out int index);
    }
}