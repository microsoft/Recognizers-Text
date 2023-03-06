// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Portuguese;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese.Utilities
{
    public class PortugueseDatetimeUtilityConfiguration : BaseDatetimeUtilityConfiguration
    {
        public PortugueseDatetimeUtilityConfiguration()
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
                  DateTimeDefinitions.CheckBothBeforeAfter)
        {
        }
    }
}
