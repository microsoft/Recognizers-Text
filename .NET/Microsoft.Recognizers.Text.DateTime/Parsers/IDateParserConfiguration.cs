﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateParserConfiguration : IDateTimeOptionsConfiguration
    {
        string DateTokenPrefix { get; }

        IExtractor IntegerExtractor { get; }

        IExtractor OrdinalExtractor { get; }

        IExtractor CardinalExtractor { get; }

        IParser NumberParser { get; }

        IDateTimeExtractor DurationExtractor { get; }

        IDateExtractor DateExtractor { get; }

        IDateTimeParser DurationParser { get; }

        IDateTimeParser HolidayParser { get; }

        IEnumerable<Regex> DateRegexes { get; }

        Regex OnRegex { get; }

        Regex SpecialDayRegex { get; }

        Regex SpecialDayWithNumRegex { get; }

        Regex NextRegex { get; }

        Regex ThisRegex { get; }

        Regex LastRegex { get; }

        Regex UnitRegex { get; }

        Regex UpcomingPrefixRegex { get; }

        Regex PastPrefixRegex { get; }

        Regex WeekDayRegex { get; }

        Regex MonthRegex { get; }

        Regex WeekDayOfMonthRegex { get; }

        Regex ForTheRegex { get; }

        Regex WeekDayAndDayOfMothRegex { get; }

        Regex WeekDayAndDayRegex { get; }

        Regex RelativeMonthRegex { get; }

        Regex StrictRelativeRegex { get; }

        Regex YearSuffix { get; }

        Regex RelativeWeekDayRegex { get; }

        Regex RelativeDayRegex { get; }

        Regex NextPrefixRegex { get; }

        Regex PreviousPrefixRegex { get; }

        Regex BeforeAfterRegex { get; }

        IImmutableDictionary<string, string> UnitMap { get; }

        IImmutableDictionary<string, int> DayOfMonth { get; }

        IImmutableDictionary<string, int> DayOfWeek { get; }

        IImmutableDictionary<string, int> MonthOfYear { get; }

        IImmutableDictionary<string, int> CardinalMap { get; }

        IImmutableList<string> SameDayTerms { get; }

        IImmutableList<string> PlusOneDayTerms { get; }

        IImmutableList<string> MinusOneDayTerms { get; }

        IImmutableList<string> PlusTwoDayTerms { get; }

        IImmutableList<string> MinusTwoDayTerms { get; }

        bool CheckBothBeforeAfter { get; }

        IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        int GetSwiftMonthOrYear(string text);

        bool IsCardinalLast(string text);

        string Normalize(string text);
    }
}
