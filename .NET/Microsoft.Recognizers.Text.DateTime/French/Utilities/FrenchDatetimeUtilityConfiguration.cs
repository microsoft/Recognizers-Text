// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.French;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.French.Utilities
{
    public class FrenchDatetimeUtilityConfiguration : BaseDatetimeUtilityConfiguration
    {
        public FrenchDatetimeUtilityConfiguration()
            : base(
                  DateTimeDefinitions.AgoRegex,
                  DateTimeDefinitions.LaterRegex,
                  DateTimeDefinitions.InConnectorRegex,
                  DateTimeDefinitions.SinceYearSuffixRegex,
                  DateTimeDefinitions.WithinNextPrefixRegex,
                  DateTimeDefinitions.AmDescRegex,
                  DateTimeDefinitions.PmDescRegex,
                  DateTimeDefinitions.AmPmDescRegex,
                  DateTimeDefinitions.RangeUnitRegex,
                  DateTimeDefinitions.TimeUnitRegex,
                  DateTimeDefinitions.DateUnitRegex,
                  DateTimeDefinitions.CommonDatePrefixRegex,
                  DateTimeDefinitions.RangePrefixRegex,
                  RegexOptions.Singleline | RegexOptions.ExplicitCapture,
                  true)
        {
            // CheckBothBeforeAfter normally gets its value from DateTimeDefinitions.CheckBothBeforeAfter which however for French is false.
            // It only needs to be true here to extract 'ago/later' in prefixes (e.g. 'il y a 30 minutes').
        }
    }
}
