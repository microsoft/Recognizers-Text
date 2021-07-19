﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateExtractorConfiguration : IDateTimeOptionsConfiguration
    {
        IEnumerable<Regex> DateRegexList { get; }

        IEnumerable<Regex> ImplicitDateList { get; }

        bool CheckBothBeforeAfter { get; }

        Regex OfMonth { get; }

        Regex MonthEnd { get; }

        Regex WeekDayEnd { get; }

        Regex WeekDayStart { get; }

        Regex DateUnitRegex { get; }

        Regex ForTheRegex { get; }

        Regex WeekDayAndDayOfMonthRegex { get; }

        Regex WeekDayAndDayRegex { get; }

        Regex RelativeMonthRegex { get; }

        Regex StrictRelativeRegex { get; }

        Regex WeekDayRegex { get; }

        Regex PrefixArticleRegex { get; }

        Regex YearSuffix { get; }

        Regex MoreThanRegex { get; }

        Regex LessThanRegex { get; }

        Regex InConnectorRegex { get; }

        Regex SinceYearSuffixRegex { get; }

        Regex RangeUnitRegex { get; }

        Regex RangeConnectorSymbolRegex { get; }

        Regex BeforeAfterRegex { get; }

        IExtractor IntegerExtractor { get; }

        IExtractor OrdinalExtractor { get; }

        IParser NumberParser { get; }

        IDateTimeExtractor DurationExtractor { get; }

        IDateTimeExtractor HolidayExtractor { get; }

        IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        IImmutableDictionary<string, int> DayOfWeek { get; }

        IImmutableDictionary<string, int> MonthOfYear { get; }
    }
}