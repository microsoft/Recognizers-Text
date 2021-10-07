﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDatePeriodExtractorConfiguration : IDateTimeOptionsConfiguration
    {
        IEnumerable<Regex> SimpleCasesRegexes { get; }

        Regex IllegalYearRegex { get; }

        Regex YearRegex { get; }

        Regex TillRegex { get; }

        Regex DateUnitRegex { get; }

        Regex TimeUnitRegex { get; }

        Regex FollowedDateUnit { get; }

        Regex NumberCombinedWithDateUnit { get; }

        Regex PreviousPrefixRegex { get; }

        Regex FutureRegex { get; }

        Regex FutureSuffixRegex { get; }

        Regex WeekOfRegex { get; }

        Regex MonthOfRegex { get; }

        Regex RangeUnitRegex { get; }

        Regex InConnectorRegex { get; }

        Regex WithinNextPrefixRegex { get; }

        Regex YearPeriodRegex { get; }

        Regex RelativeDecadeRegex { get; }

        Regex ComplexDatePeriodRegex { get; }

        Regex ReferenceDatePeriodRegex { get; }

        Regex AgoRegex { get; }

        Regex LaterRegex { get; }

        Regex LessThanRegex { get; }

        Regex MoreThanRegex { get; }

        Regex CenturySuffixRegex { get; }

        Regex MonthNumRegex { get; }

        Regex NowRegex { get; }

        bool CheckBothBeforeAfter { get; }

        IDateExtractor DatePointExtractor { get; }

        IExtractor CardinalExtractor { get; }

        IExtractor OrdinalExtractor { get; }

        IDateTimeExtractor DurationExtractor { get; }

        IParser NumberParser { get; }

        string[] DurationDateRestrictions { get; }

        bool GetFromTokenIndex(string text, out int index);

        bool HasConnectorToken(string text);

        bool GetBetweenTokenIndex(string text, out int index);
    }
}