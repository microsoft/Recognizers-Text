using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanTimeZoneExtractorConfiguration : BaseOptionsConfiguration, ITimeZoneExtractorConfiguration
    {
        public GermanTimeZoneExtractorConfiguration(IOptionsConfiguration config)
            : base(config)
        {
        }

        public Regex DirectUtcRegex { get; }

        public Regex LocationTimeSuffixRegex { get; }

        public StringMatcher LocationMatcher { get; }

        public StringMatcher TimeZoneMatcher { get; }

        public List<string> AmbiguousTimezoneList => new List<string>();
    }
}