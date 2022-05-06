// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ICJKSetExtractorConfiguration : IDateTimeOptionsConfiguration
    {
        Regex LastRegex { get; }

        Regex EachPrefixRegex { get; }

        Regex EachSuffixRegex { get; }

        Regex EachUnitRegex { get; }

        Regex UnitRegex { get; }

        Regex EachDayRegex { get; }

        Regex EachDateUnitRegex { get; }

        IDateTimeExtractor DurationExtractor { get; }

        IDateTimeExtractor TimeExtractor { get; }

        IDateTimeExtractor DateExtractor { get; }

        IDateTimeExtractor DateTimeExtractor { get; }

        IDateTimeExtractor DatePeriodExtractor { get; }

        IDateTimeExtractor TimePeriodExtractor { get; }

        IDateTimeExtractor DateTimePeriodExtractor { get; }
    }
}