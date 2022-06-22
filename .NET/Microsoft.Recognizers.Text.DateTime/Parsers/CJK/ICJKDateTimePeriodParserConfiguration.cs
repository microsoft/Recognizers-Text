// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ICJKDateTimePeriodParserConfiguration : IDateTimeOptionsConfiguration
    {
        IDateTimeExtractor DateExtractor { get; }

        IDateTimeExtractor TimeExtractor { get; }

        IDateTimeExtractor DateTimeExtractor { get; }

        IDateTimeExtractor TimePeriodExtractor { get; }

        IDateTimeExtractor DurationExtractor { get; }

        IDateTimeParser DurationParser { get; }

        IExtractor CardinalExtractor { get; }

        IParser CardinalParser { get; }

        IDateTimeParser DateParser { get; }

        IDateTimeParser TimeParser { get; }

        IDateTimeParser DateTimeParser { get; }

        IDateTimeParser TimePeriodParser { get; }

        Regex SpecificTimeOfDayRegex { get; }

        Regex TimeOfDayRegex { get; }

        Regex NextRegex { get; }

        Regex LastRegex { get; }

        Regex PastRegex { get; }

        Regex FutureRegex { get; }

        Regex TimePeriodLeftRegex { get; }

        Regex UnitRegex { get; }

        Regex RestOfDateRegex { get; }

        Regex AmPmDescRegex { get; }

        IImmutableDictionary<string, string> UnitMap { get; }

        bool GetMatchedTimeRange(string text, out string todSymbol, out int beginHour, out int endHour, out int endMinute);

        bool GetMatchedTimeRangeAndSwift(string text, out string todSymbol, out int beginHour, out int endHour, out int endMinute, out int swift);
    }
}
