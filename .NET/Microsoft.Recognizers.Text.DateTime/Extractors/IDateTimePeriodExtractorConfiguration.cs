﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateTimePeriodExtractorConfiguration : IDateTimeOptionsConfiguration
    {
        string TokenBeforeDate { get; }

        IEnumerable<Regex> SimpleCasesRegex { get; }

        Regex PrepositionRegex { get; }

        Regex TillRegex { get; }

        Regex SpecificTimeOfDayRegex { get; }

        Regex TimeOfDayRegex { get; }

        Regex FollowedUnit { get; }

        Regex NumberCombinedWithUnit { get; }

        Regex TimeUnitRegex { get; }

        Regex PreviousPrefixRegex { get; }

        Regex NextPrefixRegex { get; }

        Regex FutureSuffixRegex { get; }

        Regex WeekDayRegex { get; }

        Regex PeriodTimeOfDayWithDateRegex { get; }

        Regex RelativeTimeUnitRegex { get; }

        Regex RestOfDateTimeRegex { get; }

        Regex GeneralEndingRegex { get; }

        Regex MiddlePauseRegex { get; }

        Regex AmDescRegex { get; }

        Regex PmDescRegex { get; }

        Regex WithinNextPrefixRegex { get; }

        Regex DateUnitRegex { get; }

        Regex PrefixDayRegex { get; }

        Regex SuffixRegex { get; }

        Regex BeforeRegex { get; }

        Regex AfterRegex { get; }

        bool CheckBothBeforeAfter { get; }

        IExtractor CardinalExtractor { get; }

        IDateTimeExtractor SingleDateExtractor { get; }

        IDateTimeExtractor SingleTimeExtractor { get; }

        IDateTimeExtractor SingleDateTimeExtractor { get; }

        IDateTimeExtractor DurationExtractor { get; }

        IDateTimeExtractor TimePeriodExtractor { get; }

        IDateTimeExtractor TimeZoneExtractor { get; }

        bool GetFromTokenIndex(string text, out int index);

        bool HasConnectorToken(string text);

        bool GetBetweenTokenIndex(string text, out int index);
    }
}