using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Sequence
{
    public interface IURLExtractorConfiguration : ISequenceConfiguration
    {
        Regex IpUrlRegex { get; }

        Regex UrlRegex { get; }

        Regex UrlRegex2 { get; }
    }

}