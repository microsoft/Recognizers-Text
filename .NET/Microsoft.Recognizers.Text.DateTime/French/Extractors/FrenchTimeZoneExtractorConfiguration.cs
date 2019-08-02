using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchTimeZoneExtractorConfiguration : BaseDateTimeOptionsConfiguration, ITimeZoneExtractorConfiguration
    {
        public FrenchTimeZoneExtractorConfiguration(IDateTimeOptionsConfiguration config)
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