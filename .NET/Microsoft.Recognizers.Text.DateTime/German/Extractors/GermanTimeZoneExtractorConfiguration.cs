﻿using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.German;
using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    class GermanTimeZoneExtractorConfiguration : BaseOptionsConfiguration, ITimeZoneExtractorConfiguration
    {
        public static readonly Regex[] TimeZoneRegexList =
        {
        };

        public GermanTimeZoneExtractorConfiguration() : base(DateTimeOptions.None)
        {
        }

        public IEnumerable<Regex> TimeZoneRegexes => TimeZoneRegexList;
        public Regex CityTimeSuffixRegex { get; }
        public StringMatcher CityMatcher { get; }
    }
}