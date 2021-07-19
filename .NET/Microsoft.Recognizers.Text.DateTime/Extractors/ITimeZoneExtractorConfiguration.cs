// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ITimeZoneExtractorConfiguration : IDateTimeOptionsConfiguration
    {
        Regex DirectUtcRegex { get; }

        Regex LocationTimeSuffixRegex { get; }

        StringMatcher LocationMatcher { get; }

        StringMatcher TimeZoneMatcher { get; }

        List<string> AmbiguousTimezoneList { get; }
    }
}
