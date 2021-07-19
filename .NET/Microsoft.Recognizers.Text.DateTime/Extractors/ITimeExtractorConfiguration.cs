// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ITimeExtractorConfiguration : IDateTimeOptionsConfiguration
    {
        IDateTimeExtractor TimeZoneExtractor { get; }

        IEnumerable<Regex> TimeRegexList { get; }

        Regex AtRegex { get; }

        Regex IshRegex { get; }

        Regex TimeBeforeAfterRegex { get; }

        string TimeTokenPrefix { get; }

        Dictionary<Regex, Regex> AmbiguityFiltersDict { get; }

    }
}