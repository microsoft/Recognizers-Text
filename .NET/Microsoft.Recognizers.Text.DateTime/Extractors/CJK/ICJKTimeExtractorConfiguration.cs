// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ICJKTimeExtractorConfiguration : IDateTimeOptionsConfiguration
    {
        ImmutableDictionary<Regex, TimeType> Regexes { get; }

        Dictionary<Regex, Regex> AmbiguityTimeFiltersDict { get; }
    }
}