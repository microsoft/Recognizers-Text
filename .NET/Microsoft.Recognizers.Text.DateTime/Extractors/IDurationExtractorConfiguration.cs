﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDurationExtractorConfiguration : IDateTimeOptionsConfiguration
    {
        Regex FollowedUnit { get; }

        Regex NumberCombinedWithUnit { get; }

        Regex AnUnitRegex { get; }

        Regex DuringRegex { get; }

        Regex AllRegex { get; }

        Regex HalfRegex { get; }

        Regex SuffixAndRegex { get; }

        Regex ConjunctionRegex { get; }

        Regex InexactNumberRegex { get; }

        Regex InexactNumberUnitRegex { get; }

        Regex RelativeDurationUnitRegex { get; }

        Regex DurationUnitRegex { get; }

        Regex DurationConnectorRegex { get; }

        Regex LessThanRegex { get; }

        Regex MoreThanRegex { get; }

        Regex ModPrefixRegex { get; }

        Regex ModSuffixRegex { get; }

        Regex SpecialNumberUnitRegex { get; }

        bool CheckBothBeforeAfter { get; }

        IExtractor CardinalExtractor { get; }

        IImmutableDictionary<string, string> UnitMap { get; }

        IImmutableDictionary<string, long> UnitValueMap { get; }

    }
}