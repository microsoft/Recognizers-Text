using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.French;
using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    class FrenchTimeZoneExtractorConfiguration : BaseOptionsConfiguration, ITimeZoneExtractorConfiguration
    {
        public static readonly Regex[] TimeZoneRegexList =
        {
        };

        public FrenchTimeZoneExtractorConfiguration(IOptionsConfiguration config) : base(config)
        {
        }

        public IEnumerable<Regex> TimeZoneRegexes => TimeZoneRegexList;
        public Regex LocationTimeSuffixRegex { get; }
        public StringMatcher CityMatcher { get; }
        public List<string> AmbiguousTimezoneList => new List<string>();
    }
}