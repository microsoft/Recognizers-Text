// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Utilities
{
    public abstract class BaseDatetimeUtilityConfiguration : IDateTimeUtilityConfiguration
    {
        public BaseDatetimeUtilityConfiguration(
            string agoRegex,
            string laterRegex,
            string inConnectorRegex,
            string sinceYearSuffixRegex,
            string withinNextPrefixRegex,
            string amDescRegex,
            string pmDescRegex,
            string amPmDescRegex,
            string rangeUnitRegex,
            string timeUnitRegex,
            string dateUnitRegex,
            string commonDatePrefixRegex,
            string rangePrefixRegex,
            RegexOptions options,
            bool checkBothBeforeAfter)
        {
            this.AgoRegex = new Regex(agoRegex, options, RegexTimeOut);
            this.LaterRegex = new Regex(laterRegex, options, RegexTimeOut);
            this.InConnectorRegex = new Regex(inConnectorRegex, options, RegexTimeOut);
            this.SinceYearSuffixRegex = new Regex(sinceYearSuffixRegex, options, RegexTimeOut);
            this.WithinNextPrefixRegex = new Regex(withinNextPrefixRegex, options, RegexTimeOut);
            this.AmDescRegex = new Regex(amDescRegex, options, RegexTimeOut);
            this.PmDescRegex = new Regex(pmDescRegex, options, RegexTimeOut);
            this.AmPmDescRegex = new Regex(amPmDescRegex, options, RegexTimeOut);
            this.RangeUnitRegex = new Regex(rangeUnitRegex, options, RegexTimeOut);
            this.TimeUnitRegex = new Regex(timeUnitRegex, options, RegexTimeOut);
            this.DateUnitRegex = new Regex(dateUnitRegex, options, RegexTimeOut);
            this.CommonDatePrefixRegex = new Regex(commonDatePrefixRegex, options, RegexTimeOut);
            this.RangePrefixRegex = new Regex(rangePrefixRegex, options, RegexTimeOut);
            this.CheckBothBeforeAfter = checkBothBeforeAfter;
        }

        public static TimeSpan RegexTimeOut => DateTimeRecognizer.GetTimeout(MethodBase.GetCurrentMethod().DeclaringType);

        public Regex AgoRegex { get; set; }

        public Regex LaterRegex { get; set; }

        public Regex InConnectorRegex { get; set; }

        public Regex SinceYearSuffixRegex { get; set; }

        public Regex WithinNextPrefixRegex { get; set; }

        public Regex RangeUnitRegex { get; set; }

        public Regex TimeUnitRegex { get; set; }

        public Regex DateUnitRegex { get; set; }

        public Regex AmDescRegex { get; set; }

        public Regex PmDescRegex { get; set; }

        public Regex AmPmDescRegex { get; set; }

        public Regex CommonDatePrefixRegex { get; set; }

        public Regex RangePrefixRegex { get; set; }

        public bool CheckBothBeforeAfter { get; set; }
    }
}
