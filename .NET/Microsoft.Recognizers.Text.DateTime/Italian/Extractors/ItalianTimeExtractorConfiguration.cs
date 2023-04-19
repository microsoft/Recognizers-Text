// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Italian;

namespace Microsoft.Recognizers.Text.DateTime.Italian
{
    public class ItalianTimeExtractorConfiguration : BaseDateTimeOptionsConfiguration, ITimeExtractorConfiguration
    {
        // part 1: smallest component
        // --------------------------------
        public static readonly Regex DescRegex =
            new Regex(DateTimeDefinitions.DescRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex HourNumRegex =
            new Regex(DateTimeDefinitions.HourNumRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex MinuteNumRegex =
            new Regex(DateTimeDefinitions.MinuteNumRegex, RegexFlags, RegexTimeOut);

        // part 2: middle level component
        // --------------------------------------
        // handle "... heures (o'clock, en punto)"
        public static readonly Regex OclockRegex =
            new Regex(DateTimeDefinitions.OclockRegex, RegexFlags, RegexTimeOut);

        // handle "... après midi (afternoon, tarde)"
        public static readonly Regex PmRegex =
            new Regex(DateTimeDefinitions.PmRegex, RegexFlags, RegexTimeOut);

        // handle "... dans la matinee (in the morning)"
        public static readonly Regex AmRegex =
            new Regex(DateTimeDefinitions.AmRegex, RegexFlags, RegexTimeOut);

        // handle "half past ..." "a quarter to ..."
        // rename 'min' group to 'deltamin'
        public static readonly Regex LessThanOneHour =
            new Regex(DateTimeDefinitions.LessThanOneHour, RegexFlags, RegexTimeOut);

        public static readonly Regex FrTimeRegex =
            new Regex(DateTimeDefinitions.EngTimeRegex, RegexFlags, RegexTimeOut);

        // TODO - will have change below
        // handle "six heures et demie" (six thirty), "six heures et vingt-et-un" (six twenty one)
        public static readonly Regex TimePrefix =
            new Regex(DateTimeDefinitions.TimePrefix, RegexFlags, RegexTimeOut);

        public static readonly Regex TimeSuffix =
            new Regex(DateTimeDefinitions.TimeSuffix, RegexFlags, RegexTimeOut);

        public static readonly Regex BasicTime =
            new Regex(DateTimeDefinitions.BasicTime, RegexFlags, RegexTimeOut);

        // handle special time such as 'at midnight', 'midnight', 'midday'
        // midnight - le minuit, la zero heure
        // midday - midi
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

            // (three min past)? 3.00 (pm)?
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

            // 340pm
            ConnectNumRegex,
        };

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public ItalianTimeExtractorConfiguration(IDateTimeOptionsConfiguration config)
           : base(config)
        {
            DurationExtractor = new BaseDurationExtractor(new ItalianDurationExtractorConfiguration(this));
            TimeZoneExtractor = new BaseTimeZoneExtractor(new ItalianTimeZoneExtractorConfiguration(this));
        }

        Regex ITimeExtractorConfiguration.IshRegex => IshRegex;

        IEnumerable<Regex> ITimeExtractorConfiguration.TimeRegexList => TimeRegexList;

        Regex ITimeExtractorConfiguration.AtRegex => AtRegex;

        Regex ITimeExtractorConfiguration.TimeBeforeAfterRegex => TimeBeforeAfterRegex;

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor TimeZoneExtractor { get; }

        public string TimeTokenPrefix => DateTimeDefinitions.TimeTokenPrefix;

        public Dictionary<Regex, Regex> AmbiguityFiltersDict => null;
    }
}
