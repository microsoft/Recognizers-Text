﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateTimePeriodParserConfiguration : IDateTimeOptionsConfiguration
    {
        string TokenBeforeDate { get; }

        string TokenBeforeTime { get; }

        IDateExtractor DateExtractor { get; }

        IDateTimeExtractor TimeExtractor { get; }

        IDateTimeExtractor DateTimeExtractor { get; }

        IDateTimeExtractor TimePeriodExtractor { get; }

        IDateTimeExtractor DurationExtractor { get; }

        IExtractor CardinalExtractor { get; }

        IParser NumberParser { get; }

        IDateTimeParser DateParser { get; }

        IDateTimeParser TimeParser { get; }

        IDateTimeParser DateTimeParser { get; }

        IDateTimeParser TimePeriodParser { get; }

        IDateTimeParser DurationParser { get; }

        IDateTimeParser TimeZoneParser { get; }

        Regex PureNumberFromToRegex { get; }

        Regex HyphenDateRegex { get; }

        Regex PureNumberBetweenAndRegex { get; }

        Regex SpecificTimeOfDayRegex { get; }

        Regex TimeOfDayRegex { get; }

        Regex PreviousPrefixRegex { get; }

        Regex FutureRegex { get; }

        Regex FutureSuffixRegex { get; }

        Regex NumberCombinedWithUnitRegex { get; }

        Regex UnitRegex { get; }

        Regex PeriodTimeOfDayWithDateRegex { get; }

        Regex RelativeTimeUnitRegex { get; }

        Regex RestOfDateTimeRegex { get; }

        Regex AmDescRegex { get; }

        Regex PmDescRegex { get; }

        Regex WithinNextPrefixRegex { get; }

        Regex PrefixDayRegex { get; }

        Regex BeforeRegex { get; }

        Regex AfterRegex { get; }

        bool CheckBothBeforeAfter { get; }

        IImmutableDictionary<string, string> UnitMap { get; }

        IImmutableDictionary<string, int> Numbers { get; }

        bool GetMatchedTimeRange(string text, out string todSymbol, out int beginHour, out int endHour, out int endMin);

        int GetSwiftPrefix(string text);
    }
}
