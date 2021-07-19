﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ITimeParserConfiguration : IDateTimeOptionsConfiguration
    {
        string TimeTokenPrefix { get; }

        Regex AtRegex { get; }

        IEnumerable<Regex> TimeRegexes { get; }

        IImmutableDictionary<string, int> Numbers { get; }

        IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        IDateTimeParser TimeZoneParser { get; }

        void AdjustByPrefix(string prefix, ref int hour, ref int min, ref bool hasMin);

        void AdjustBySuffix(string suffix, ref int hour, ref int min, ref bool hasMin, ref bool hasAm, ref bool hasPm);
    }
}
