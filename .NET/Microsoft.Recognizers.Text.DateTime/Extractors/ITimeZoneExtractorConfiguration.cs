using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ITimeZoneExtractorConfiguration : IOptionsConfiguration
    {
        IEnumerable<Regex> TimeZoneRegexes { get; }
    }
}
