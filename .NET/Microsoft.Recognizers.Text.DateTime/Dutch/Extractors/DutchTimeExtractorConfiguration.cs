// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Dutch;
using Microsoft.Recognizers.Definitions.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Dutch
{
    public class DutchTimeExtractorConfiguration : BaseDateTimeOptionsConfiguration, ITimeExtractorConfiguration
    {
        // part 1: smallest component
        // --------------------------------------
        public static readonly Regex DescRegex =
            new Regex(DateTimeDefinitions.DescRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex HourNumRegex =
            new Regex(DateTimeDefinitions.HourNumRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex MinuteNumRegex =
            new Regex(DateTimeDefinitions.MinuteNumRegex, RegexFlags, RegexTimeOut);

        // part 2: middle level component
        // --------------------------------------
        // handle "... o'clock"
        public static readonly Regex OclockRegex =
            new Regex(DateTimeDefinitions.OclockRegex, RegexFlags, RegexTimeOut);

        // handle "... afternoon"
        public static readonly Regex PmRegex =
            new Regex(DateTimeDefinitions.PmRegex, RegexFlags, RegexTimeOut);

        // handle "... in the morning"
        public static readonly Regex AmRegex =
            new Regex(DateTimeDefinitions.AmRegex, RegexFlags, RegexTimeOut);

        // handle "half past ..." "a quarter to ..."
        // rename 'min' group to 'deltamin'
        public static readonly Regex LessThanOneHour =
            new Regex(DateTimeDefinitions.LessThanOneHour, RegexFlags, RegexTimeOut);

        // handle "six thirty", "six twenty one"
        public static readonly Regex WrittenTimeRegex =
            new Regex(DateTimeDefinitions.WrittenTimeRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex TimePrefix =
            new Regex(DateTimeDefinitions.TimePrefix, RegexFlags, RegexTimeOut);

        public static readonly Regex TimeSuffix =
            new Regex(DateTimeDefinitions.TimeSuffix, RegexFlags, RegexTimeOut);

        public static readonly Regex BasicTime =
            new Regex(DateTimeDefinitions.BasicTime, RegexFlags, RegexTimeOut);

        // handle special time such as 'at midnight', 'midnight', 'midday'
        public static readonly Regex MidnightRegex =
            new Regex(DateTimeDefinitions.MidnightRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex MidmorningRegex =
            new Regex(DateTimeDefinitions.MidmorningRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex MidafternoonRegex =
            new Regex(DateTimeDefinitions.MidafternoonRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex MiddayRegex =
            new Regex(DateTimeDefinitions.MiddayRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex MidTimeRegex =
            new Regex(DateTimeDefinitions.MidTimeRegex, RegexFlags, RegexTimeOut);

        // part 3: regex for time
        // --------------------------------------
        // handle "at four" "at 3"
        public static readonly Regex AtRegex =
            new Regex(DateTimeDefinitions.AtRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex IshRegex =
            new Regex(DateTimeDefinitions.IshRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex TimeUnitRegex =
            new Regex(DateTimeDefinitions.TimeUnitRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex ConnectNumRegex =
            new Regex(DateTimeDefinitions.ConnectNumRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex TimeBeforeAfterRegex =
            new Regex(DateTimeDefinitions.TimeBeforeAfterRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex[] TimeRegexList =
        {
            // (three min past)? seven|7|(seven thirty) pm
            new Regex(DateTimeDefinitions.TimeRegex1, RegexFlags, RegexTimeOut),

            // (three min past)? 3:00(:00)? (pm)?
            new Regex(DateTimeDefinitions.TimeRegex2, RegexFlags, RegexTimeOut),

            // (three min past)? 3.00 (pm)
            new Regex(DateTimeDefinitions.TimeRegex3, RegexFlags, RegexTimeOut),

            // (three min past) (five thirty|seven|7|7:00(:00)?) (pm)? (in the night)
            new Regex(DateTimeDefinitions.TimeRegex4, RegexFlags, RegexTimeOut),

            // (three min past) (five thirty|seven|7|7:00(:00)?) (pm)?
            new Regex(DateTimeDefinitions.TimeRegex5, RegexFlags, RegexTimeOut),

            // (five thirty|seven|7|7:00(:00)?) (pm)? (in the night)
            new Regex(DateTimeDefinitions.TimeRegex6, RegexFlags, RegexTimeOut),

            // (in the night) at (five thirty|seven|7|7:00(:00)?) (pm)?
            new Regex(DateTimeDefinitions.TimeRegex7, RegexFlags, RegexTimeOut),

            // (in the night) (five thirty|seven|7|7:00(:00)?) (pm)?
            new Regex(DateTimeDefinitions.TimeRegex8, RegexFlags, RegexTimeOut),

            new Regex(DateTimeDefinitions.TimeRegex9, RegexFlags, RegexTimeOut),

            // (three min past)? 3h00 (pm)?
            new Regex(DateTimeDefinitions.TimeRegex10, RegexFlags, RegexTimeOut),

            // at 2.30, "at" prefix is required here
            // 3.30pm, "am/pm" suffix is required here
            new Regex(DateTimeDefinitions.TimeRegex11, RegexFlags, RegexTimeOut),

            // 16 from "16 vandaag"
            new Regex(DateTimeDefinitions.TimeRegex12, RegexFlags, RegexTimeOut),

            // 340pm
            ConnectNumRegex,
        };

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public DutchTimeExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            DurationExtractor = new BaseDurationExtractor(new DutchDurationExtractorConfiguration(this));
            TimeZoneExtractor = new BaseTimeZoneExtractor(new DutchTimeZoneExtractorConfiguration(this));
        }

        IEnumerable<Regex> ITimeExtractorConfiguration.TimeRegexList => TimeRegexList;

        Regex ITimeExtractorConfiguration.AtRegex => AtRegex;

        Regex ITimeExtractorConfiguration.IshRegex => IshRegex;

        Regex ITimeExtractorConfiguration.TimeBeforeAfterRegex => TimeBeforeAfterRegex;

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor TimeZoneExtractor { get; }

        public string TimeTokenPrefix => DateTimeDefinitions.TimeTokenPrefix;

        public Dictionary<Regex, Regex> AmbiguityFiltersDict => DefinitionLoader.LoadAmbiguityFilters(DateTimeDefinitions.AmbiguityTimeFiltersDict);
    }
}