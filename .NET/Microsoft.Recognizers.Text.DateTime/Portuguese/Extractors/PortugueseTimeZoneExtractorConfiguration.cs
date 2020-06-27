using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    internal class PortugueseTimeZoneExtractorConfiguration : BaseDateTimeOptionsConfiguration, ITimeZoneExtractorConfiguration
    {
        public PortugueseTimeZoneExtractorConfiguration(IDateTimeOptionsConfiguration config)
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