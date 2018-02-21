using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.Recognizers.Text.Options.Extractors
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
