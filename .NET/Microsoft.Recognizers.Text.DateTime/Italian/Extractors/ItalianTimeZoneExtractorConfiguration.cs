using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime.Italian
{
    public class ItalianTimeZoneExtractorConfiguration : BaseOptionsConfiguration, ITimeZoneExtractorConfiguration
    {
        public static readonly Regex[] TimeZoneRegexList =
        {
        };

        public ItalianTimeZoneExtractorConfiguration(IOptionsConfiguration config)
            : base(config)
        {
        }

        public IEnumerable<Regex> TimeZoneRegexes => TimeZoneRegexList;

        public Regex LocationTimeSuffixRegex { get; } = null;

        public StringMatcher LocationMatcher { get; } = new StringMatcher();

        public List<string> AmbiguousTimezoneList => new List<string>();
    }
}