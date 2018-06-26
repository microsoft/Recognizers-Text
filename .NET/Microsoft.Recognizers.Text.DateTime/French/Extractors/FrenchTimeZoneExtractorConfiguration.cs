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

        public FrenchTimeZoneExtractorConfiguration() : base(DateTimeOptions.None)
        {
        }

        public IEnumerable<Regex> TimeZoneRegexes => TimeZoneRegexList;
        public Regex CityTimeSuffixRegex { get; }
        public StringMatcher CityMatcher { get; }
        public List<string> AmbiguousTimezoneList => new List<string>();
    }
}