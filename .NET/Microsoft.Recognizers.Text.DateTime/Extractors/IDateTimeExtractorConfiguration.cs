// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateTimeExtractorConfiguration : IDateTimeOptionsConfiguration
    {
        Regex NowRegex { get; }

        Regex SuffixRegex { get; }

        Regex TimeOfTodayAfterRegex { get; }

        Regex SimpleTimeOfTodayAfterRegex { get; }

        Regex TimeOfTodayBeforeRegex { get; }

        Regex SimpleTimeOfTodayBeforeRegex { get; }

        Regex TimeOfDayRegex { get; }

        Regex SpecificEndOfRegex { get; }

        Regex UnspecificEndOfRegex { get; }

        Regex UnitRegex { get; }

        Regex NumberAsTimeRegex { get; }

        Regex DateNumberConnectorRegex { get; }

        Regex YearRegex { get; }

        Regex YearSuffix { get; }

        Regex SuffixAfterRegex { get; }

        IDateTimeExtractor DurationExtractor { get; }

        IDateExtractor DatePointExtractor { get; }

        IDateTimeExtractor TimePointExtractor { get; }

        IExtractor IntegerExtractor { get; }

        IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        IDateTimeExtractor HolidayPointExtractor { get; }

        bool IsConnector(string text);
    }
}