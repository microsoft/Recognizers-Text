// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ICJKDateTimeExtractorConfiguration : IDateTimeOptionsConfiguration
    {
        Regex NowRegex { get; }

        Regex PrepositionRegex { get; }

        Regex NightRegex { get; }

        Regex TimeOfSpecialDayRegex { get; }

        Regex TimeOfDayRegex { get; }

        Regex BeforeRegex { get; }

        Regex AfterRegex { get; }

        Regex ConnectorRegex { get; }

        IDateTimeExtractor DurationExtractor { get; }

        IDateTimeExtractor DatePointExtractor { get; }

        IDateTimeExtractor TimePointExtractor { get; }

        Dictionary<Regex, Regex> AmbiguityDateTimeFiltersDict { get; }

    }
}