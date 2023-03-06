// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Arabic;

namespace Microsoft.Recognizers.Text.DateTime.Arabic
{
    public class ArabicDateTimeAltExtractorConfiguration : BaseDateTimeOptionsConfiguration, IDateTimeAltExtractorConfiguration
    {
        public static readonly Regex ThisPrefixRegex =
            new Regex(DateTimeDefinitions.ThisPrefixRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex PreviousPrefixRegex =
            new Regex(DateTimeDefinitions.PreviousPrefixRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex NextPrefixRegex =
            new Regex(DateTimeDefinitions.NextPrefixRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex AmRegex =
            new Regex(DateTimeDefinitions.AmRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex PmRegex =
            new Regex(DateTimeDefinitions.PmRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex RangePrefixRegex =
            new Regex(DateTimeDefinitions.RangePrefixRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex[] RelativePrefixList =
        {
            ThisPrefixRegex, PreviousPrefixRegex, NextPrefixRegex,
        };

        public static readonly Regex[] AmPmRegexList =
        {
            AmRegex, PmRegex,
        };

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.RightToLeft;

        private static readonly Regex OrRegex =
            new Regex(DateTimeDefinitions.OrRegex, RegexFlags, RegexTimeOut);

        private static readonly Regex DayRegex =
            new Regex(DateTimeDefinitions.DayRegex, RegexFlags, RegexTimeOut);

        public ArabicDateTimeAltExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            DateExtractor = new BaseDateExtractor(new ArabicDateExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new ArabicDatePeriodExtractorConfiguration(this));
        }

        IEnumerable<Regex> IDateTimeAltExtractorConfiguration.RelativePrefixList => RelativePrefixList;

        IEnumerable<Regex> IDateTimeAltExtractorConfiguration.AmPmRegexList => AmPmRegexList;

        Regex IDateTimeAltExtractorConfiguration.OrRegex => OrRegex;

        Regex IDateTimeAltExtractorConfiguration.ThisPrefixRegex => ThisPrefixRegex;

        Regex IDateTimeAltExtractorConfiguration.DayRegex => DayRegex;

        Regex IDateTimeAltExtractorConfiguration.RangePrefixRegex => RangePrefixRegex;

        public IDateExtractor DateExtractor { get; }

        public IDateTimeExtractor DatePeriodExtractor { get; }
    }
}