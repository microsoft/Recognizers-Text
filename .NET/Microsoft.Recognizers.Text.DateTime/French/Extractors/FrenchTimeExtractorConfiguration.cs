using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.French;
using Microsoft.Recognizers.Definitions.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchTimeExtractorConfiguration : BaseDateTimeOptionsConfiguration, ITimeExtractorConfiguration
    {
        // part 1: smallest component
        // --------------------------------
        public static readonly Regex DescRegex =
            RegexCache.Get(DateTimeDefinitions.DescRegex, RegexFlags);

        public static readonly Regex HourNumRegex =
            RegexCache.Get(DateTimeDefinitions.HourNumRegex, RegexFlags);

        public static readonly Regex MinuteNumRegex =
            RegexCache.Get(DateTimeDefinitions.MinuteNumRegex, RegexFlags);

        // part 2: middle level component
        // --------------------------------------
        // handle "... heures (o'clock, en punto)"
        public static readonly Regex OclockRegex =
            RegexCache.Get(DateTimeDefinitions.OclockRegex, RegexFlags);

        // handle "... après midi (afternoon, tarde)"
        public static readonly Regex PmRegex =
            RegexCache.Get(DateTimeDefinitions.PmRegex, RegexFlags);

        // handle "... dans la matinee (in the morning)"
        public static readonly Regex AmRegex =
            RegexCache.Get(DateTimeDefinitions.AmRegex, RegexFlags);

        // handle "half past ..." "a quarter to ..."
        // rename 'min' group to 'deltamin'
        public static readonly Regex LessThanOneHour =
            RegexCache.Get(DateTimeDefinitions.LessThanOneHour, RegexFlags);

        public static readonly Regex WrittenTimeRegex =
            RegexCache.Get(DateTimeDefinitions.WrittenTimeRegex, RegexFlags);

        // TODO - will have change below
        // handle "six heures et demie" (six thirty), "six heures et vingt-et-un" (six twenty one)
        public static readonly Regex TimePrefix =
            RegexCache.Get(DateTimeDefinitions.TimePrefix, RegexFlags);

        public static readonly Regex TimeSuffix =
            RegexCache.Get(DateTimeDefinitions.TimeSuffix, RegexFlags);

        public static readonly Regex BasicTime =
            RegexCache.Get(DateTimeDefinitions.BasicTime, RegexFlags);

        // handle special time such as 'at midnight', 'midnight', 'midday'
        // midnight - le minuit, la zero heure
        // midday - midi
        public static readonly Regex MidnightRegex =
            RegexCache.Get(DateTimeDefinitions.MidnightRegex, RegexFlags);

        public static readonly Regex MidmorningRegex =
            RegexCache.Get(DateTimeDefinitions.MidmorningRegex, RegexFlags);

        public static readonly Regex MidafternoonRegex =
            RegexCache.Get(DateTimeDefinitions.MidafternoonRegex, RegexFlags);

        public static readonly Regex MiddayRegex =
            RegexCache.Get(DateTimeDefinitions.MiddayRegex, RegexFlags);

        public static readonly Regex MidTimeRegex =
            RegexCache.Get(DateTimeDefinitions.MidTimeRegex, RegexFlags);

        // part 3: regex for time
        // --------------------------------------
        // handle "at four" "at 3"
        public static readonly Regex AtRegex =
            RegexCache.Get(DateTimeDefinitions.AtRegex, RegexFlags);

        public static readonly Regex IshRegex =
            RegexCache.Get(DateTimeDefinitions.IshRegex, RegexFlags);

        public static readonly Regex TimeUnitRegex =
            RegexCache.Get(DateTimeDefinitions.TimeUnitRegex, RegexFlags);

        public static readonly Regex ConnectNumRegex =
            RegexCache.Get(DateTimeDefinitions.ConnectNumRegex, RegexFlags);

        public static readonly Regex TimeBeforeAfterRegex =
            RegexCache.Get(DateTimeDefinitions.TimeBeforeAfterRegex, RegexFlags);

        public static readonly Regex[] TimeRegexList =
        {
            // (three min past)? seven|7|(seven thirty) pm
            RegexCache.Get(DateTimeDefinitions.TimeRegex1, RegexFlags),

            // (three min past)? 3:00(:00)? (pm)?
            RegexCache.Get(DateTimeDefinitions.TimeRegex2, RegexFlags),

            // (three min past)? 3.00 (pm)?
            RegexCache.Get(DateTimeDefinitions.TimeRegex3, RegexFlags),

            // (three min past) (five thirty|seven|7|7:00(:00)?) (pm)? (in the night)
            RegexCache.Get(DateTimeDefinitions.TimeRegex4, RegexFlags),

            // (three min past) (five thirty|seven|7|7:00(:00)?) (pm)?
            RegexCache.Get(DateTimeDefinitions.TimeRegex5, RegexFlags),

            // (five thirty|seven|7|7:00(:00)?) (pm)? (in the night)
            RegexCache.Get(DateTimeDefinitions.TimeRegex6, RegexFlags),

            // (in the night) at (five thirty|seven|7|7:00(:00)?) (pm)?
            RegexCache.Get(DateTimeDefinitions.TimeRegex7, RegexFlags),

            // (in the night) (five thirty|seven|7|7:00(:00)?) (pm)?
            RegexCache.Get(DateTimeDefinitions.TimeRegex8, RegexFlags),

            RegexCache.Get(DateTimeDefinitions.TimeRegex9, RegexFlags),

            RegexCache.Get(DateTimeDefinitions.TimeRegex10, RegexFlags),

            // 340pm
            ConnectNumRegex,
        };

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public FrenchTimeExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            DurationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration(this));
            TimeZoneExtractor = new BaseTimeZoneExtractor(new FrenchTimeZoneExtractorConfiguration(this));
        }

        Regex ITimeExtractorConfiguration.IshRegex => IshRegex;

        IEnumerable<Regex> ITimeExtractorConfiguration.TimeRegexList => TimeRegexList;

        Regex ITimeExtractorConfiguration.AtRegex => AtRegex;

        Regex ITimeExtractorConfiguration.TimeBeforeAfterRegex => TimeBeforeAfterRegex;

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor TimeZoneExtractor { get; }

        public string TimeTokenPrefix => DateTimeDefinitions.TimeTokenPrefix;

        public Dictionary<Regex, Regex> AmbiguityFiltersDict => DefinitionLoader.LoadAmbiguityFilters(DateTimeDefinitions.AmbiguityTimeFiltersDict);
    }
}
