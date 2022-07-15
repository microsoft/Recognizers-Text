// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ICJKTimePeriodExtractorConfiguration : IDateTimeOptionsConfiguration
    {
        ImmutableDictionary<Regex, PeriodType> Regexes { get; }

        Dictionary<Regex, Regex> AmbiguityTimePeriodFiltersDict { get; }

    }
}