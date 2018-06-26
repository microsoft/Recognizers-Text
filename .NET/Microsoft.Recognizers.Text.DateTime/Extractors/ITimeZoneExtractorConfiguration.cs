using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ITimeZoneExtractorConfiguration : IOptionsConfiguration
    {
        IEnumerable<Regex> TimeZoneRegexes { get; }

        Regex LocationTimeSuffixRegex { get; }

        StringMatcher CityMatcher { get; }

        List<string> AmbiguousTimezoneList { get; }
    }
}
