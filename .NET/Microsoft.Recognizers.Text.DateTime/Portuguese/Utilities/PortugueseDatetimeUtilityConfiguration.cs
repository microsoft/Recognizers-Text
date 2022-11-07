// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Portuguese;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese.Utilities
{
    public class PortugueseDatetimeUtilityConfiguration : IDateTimeUtilityConfiguration
    {
        public static readonly Regex AgoRegex =
            new Regex(DateTimeDefinitions.AgoRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex LaterRegex =
            new Regex(DateTimeDefinitions.LaterRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex InConnectorRegex =
            new Regex(DateTimeDefinitions.InConnectorRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex SinceYearSuffixRegex =
            new Regex(DateTimeDefinitions.SinceYearSuffixRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex WithinNextPrefixRegex =
            new Regex(DateTimeDefinitions.WithinNextPrefixRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex AmDescRegex =
            new Regex(DateTimeDefinitions.AmDescRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex PmDescRegex =
            new Regex(DateTimeDefinitions.PmDescRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex AmPmDescRegex =
            new Regex(DateTimeDefinitions.AmPmDescRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex RangeUnitRegex =
            new Regex(DateTimeDefinitions.RangeUnitRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex TimeUnitRegex =
            new Regex(DateTimeDefinitions.TimeUnitRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex DateUnitRegex =
            new Regex(DateTimeDefinitions.DateUnitRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex CommonDatePrefixRegex =
            new Regex(DateTimeDefinitions.CommonDatePrefixRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex RangePrefixRegex =
            new Regex(DateTimeDefinitions.RangePrefixRegex, RegexFlags, RegexTimeOut);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        Regex IDateTimeUtilityConfiguration.LaterRegex => LaterRegex;

        Regex IDateTimeUtilityConfiguration.AgoRegex => AgoRegex;

        Regex IDateTimeUtilityConfiguration.InConnectorRegex => InConnectorRegex;

        Regex IDateTimeUtilityConfiguration.SinceYearSuffixRegex => SinceYearSuffixRegex;

        Regex IDateTimeUtilityConfiguration.WithinNextPrefixRegex => WithinNextPrefixRegex;

        Regex IDateTimeUtilityConfiguration.AmDescRegex => AmDescRegex;

        Regex IDateTimeUtilityConfiguration.PmDescRegex => PmDescRegex;

        Regex IDateTimeUtilityConfiguration.AmPmDescRegex => AmPmDescRegex;

        Regex IDateTimeUtilityConfiguration.RangeUnitRegex => RangeUnitRegex;

        Regex IDateTimeUtilityConfiguration.TimeUnitRegex => TimeUnitRegex;

        Regex IDateTimeUtilityConfiguration.DateUnitRegex => DateUnitRegex;

        Regex IDateTimeUtilityConfiguration.CommonDatePrefixRegex => CommonDatePrefixRegex;

        Regex IDateTimeUtilityConfiguration.RangePrefixRegex => RangePrefixRegex;

        bool IDateTimeUtilityConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;

        protected static TimeSpan RegexTimeOut => DateTimeRecognizer.GetTimeout(MethodBase.GetCurrentMethod().DeclaringType);
    }
}
