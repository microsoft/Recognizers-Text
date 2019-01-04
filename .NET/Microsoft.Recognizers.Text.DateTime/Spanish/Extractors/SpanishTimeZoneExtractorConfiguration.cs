using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Spanish;
using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    internal class SpanishTimeZoneExtractorConfiguration : BaseOptionsConfiguration, ITimeZoneExtractorConfiguration
    {
        public static readonly Regex[] TimeZoneRegexList =
        {
        };

        public SpanishTimeZoneExtractorConfiguration(IOptionsConfiguration config)
            : base(config)
        {
        }

        public IEnumerable<Regex> TimeZoneRegexes => TimeZoneRegexList;

        public Regex LocationTimeSuffixRegex { get; }

        public StringMatcher LocationMatcher { get; }

        public List<string> AmbiguousTimezoneList => new List<string>();
    }
}