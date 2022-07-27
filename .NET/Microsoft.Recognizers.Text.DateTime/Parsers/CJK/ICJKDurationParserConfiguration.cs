// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ICJKDurationParserConfiguration : IDateTimeOptionsConfiguration
    {
        IDateTimeExtractor DurationExtractor { get; }

        IParser InternalParser { get; }

        Regex YearRegex { get; }

        Regex SomeRegex { get; }

        Regex MoreOrLessRegex { get; }

        Regex DurationUnitRegex { get; }

        Regex AnUnitRegex { get; }

        Regex DurationConnectorRegex { get; }

        IImmutableDictionary<string, string> UnitMap { get; }

        IImmutableDictionary<string, long> UnitValueMap { get; }

    }
}
