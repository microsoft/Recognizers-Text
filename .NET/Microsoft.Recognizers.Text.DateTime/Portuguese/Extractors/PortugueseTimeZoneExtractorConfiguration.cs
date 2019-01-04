using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Portuguese;
using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    internal class PortugueseTimeZoneExtractorConfiguration : BaseOptionsConfiguration, ITimeZoneExtractorConfiguration
    {
        public static readonly Regex[] TimeZoneRegexList =
        {
        };

        public PortugueseTimeZoneExtractorConfiguration(IOptionsConfiguration config)
            : base(config)
        {
        }

        public IEnumerable<Regex> TimeZoneRegexes => TimeZoneRegexList;

        public Regex LocationTimeSuffixRegex { get; }

        public StringMatcher LocationMatcher { get; }

        public List<string> AmbiguousTimezoneList => new List<string>();
    }
}