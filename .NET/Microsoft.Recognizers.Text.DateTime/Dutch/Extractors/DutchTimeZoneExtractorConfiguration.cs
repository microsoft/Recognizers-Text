﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Dutch;
using Microsoft.Recognizers.Text.Matcher;
using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Dutch
{
    public class DutchTimeZoneExtractorConfiguration : BaseDateTimeOptionsConfiguration, ITimeZoneExtractorConfiguration
    {
        public static readonly Regex DirectUtcRegex =
            new Regex(TimeZoneDefinitions.DirectUtcRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly List<string> AbbreviationsList =
            new List<string>(TimeZoneDefinitions.AbbreviationsList);

        public static readonly List<string> FullNameList =
            new List<string>(TimeZoneDefinitions.FullNameList);

        public static readonly StringMatcher TimeZoneMatcher =
            TimeZoneUtility.BuildMatcherFromLists(AbbreviationsList, FullNameList);

        public static readonly Regex LocationTimeSuffixRegex =
            new Regex(TimeZoneDefinitions.LocationTimeSuffixRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly StringMatcher LocationMatcher = new StringMatcher();

        public static readonly List<string> AmbiguousTimezoneList = TimeZoneDefinitions.AmbiguousTimezoneList.ToList();

        public DutchTimeZoneExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            if ((Options & DateTimeOptions.EnablePreview) != 0)
            {
                LocationMatcher.Init(TimeZoneDefinitions.MajorLocations.Select(o => QueryProcessor.RemoveDiacritics(o.ToLowerInvariant())));
            }
        }

        Regex ITimeZoneExtractorConfiguration.DirectUtcRegex => DirectUtcRegex;

        Regex ITimeZoneExtractorConfiguration.LocationTimeSuffixRegex => LocationTimeSuffixRegex;

        StringMatcher ITimeZoneExtractorConfiguration.LocationMatcher => LocationMatcher;

        StringMatcher ITimeZoneExtractorConfiguration.TimeZoneMatcher => TimeZoneMatcher;

        List<string> ITimeZoneExtractorConfiguration.AmbiguousTimezoneList => AmbiguousTimezoneList;
    }
}