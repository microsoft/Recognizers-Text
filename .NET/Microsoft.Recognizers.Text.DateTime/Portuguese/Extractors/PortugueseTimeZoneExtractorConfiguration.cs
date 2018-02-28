using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Portuguese;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    class PortugueseTimeZoneExtractorConfiguration : BaseOptionsConfiguration, ITimeZoneExtractorConfiguration
    {
        public static readonly Regex[] TimeZoneRegexList =
        {
        };

        public PortugueseTimeZoneExtractorConfiguration() : base(DateTimeOptions.None)
        {
        }

        public IEnumerable<Regex> TimeZoneRegexes => TimeZoneRegexList;
    }
}