using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime.Italian
{
    public class ItalianTimeZoneExtractorConfiguration : BaseOptionsConfiguration, ITimeZoneExtractorConfiguration
    {
        public ItalianTimeZoneExtractorConfiguration(IOptionsConfiguration config)
            : base(config)
        {
        }

        public Regex DirectUtcRegex { get; }

        public Regex LocationTimeSuffixRegex { get; } = null;

        public StringMatcher LocationMatcher { get; } = new StringMatcher();

        public StringMatcher TimeZoneMatcher { get; }

        public List<string> AmbiguousTimezoneList => new List<string>();
    }
}