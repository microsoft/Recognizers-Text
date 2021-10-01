// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateTimeAltExtractorConfiguration
    {
        IDateExtractor DateExtractor { get; }

        IDateTimeExtractor DatePeriodExtractor { get; }

        IEnumerable<Regex> RelativePrefixList { get; }

        IEnumerable<Regex> AmPmRegexList { get; }

        Regex OrRegex { get; }

        Regex ThisPrefixRegex { get; }

        Regex DayRegex { get; }

        Regex RangePrefixRegex { get; }
    }
}