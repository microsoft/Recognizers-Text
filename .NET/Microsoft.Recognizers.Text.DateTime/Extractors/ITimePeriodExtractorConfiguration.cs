﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ITimePeriodExtractorConfiguration : IDateTimeOptionsConfiguration
    {
        string TokenBeforeDate { get; }

        IExtractor IntegerExtractor { get; }

        IEnumerable<Regex> SimpleCasesRegex { get; }

        IEnumerable<Regex> PureNumberRegex { get; }

        bool CheckBothBeforeAfter { get; }

        Regex TillRegex { get; }

        Regex TimeOfDayRegex { get; }

        Regex GeneralEndingRegex { get; }

        IDateTimeExtractor SingleTimeExtractor { get; }

        IDateTimeExtractor TimeZoneExtractor { get; }

        bool GetFromTokenIndex(string text, out int index);

        bool IsConnectorToken(string text);

        bool GetBetweenTokenIndex(string text, out int index);

        List<ExtractResult> ApplyPotentialPeriodAmbiguityHotfix(string text, List<ExtractResult> timePeriodErs);
    }
}