using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Choice
{
    public interface IChoiceExtractorConfiguration
    {
        IDictionary<Regex, string> MapRegexes { get; }

        Regex TokenRegex { get; }

        bool AllowPartialMatch { get; }

        int MaxDistance { get; }

        bool OnlyTopMatch { get; }
    }
}
