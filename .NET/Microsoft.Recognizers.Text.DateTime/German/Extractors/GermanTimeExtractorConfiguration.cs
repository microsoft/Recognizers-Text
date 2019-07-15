using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.German;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanTimeExtractorConfiguration : BaseOptionsConfiguration, ITimeExtractorConfiguration
    {
        // part 1: smallest component
        // --------------------------------------
        public static readonly Regex DescRegex =
            new Regex(DateTimeDefinitions.DescRegex, RegexFlags);

        public static readonly Regex HourNumRegex =
            new Regex(DateTimeDefinitions.HourNumRegex, RegexFlags);

        public static readonly Regex MinuteNumRegex =
            new Regex(DateTimeDefinitions.MinuteNumRegex, RegexFlags);

        // part 2: middle level component
        // --------------------------------------
        // handle "... o'clock"
        public static readonly Regex OclockRegex =
            new Regex(DateTimeDefinitions.OclockRegex, RegexFlags);

        // handle "... afternoon"
        public static readonly Regex PmRegex =
            new Regex(DateTimeDefinitions.PmRegex, RegexFlags);

        // handle "... in the morning"
        public static readonly Regex AmRegex =
            new Regex(DateTimeDefinitions.AmRegex, RegexFlags);

        // handle "half past ..." "a quarter to ..."
        // rename 'min' group to 'deltamin'
        public static readonly Regex LessThanOneHour =
            new Regex(DateTimeDefinitions.LessThanOneHour, RegexFlags);

        // handle "six thirty", "six twenty one"
        public static readonly Regex WrittenTimeRegex =
            new Regex(DateTimeDefinitions.WrittenTimeRegex, RegexFlags);

        public static readonly Regex TimePrefix =
            new Regex(DateTimeDefinitions.TimePrefix, RegexFlags);

        public static readonly Regex TimeSuffix =
            new Regex(DateTimeDefinitions.TimeSuffix, RegexFlags);

        public static readonly Regex BasicTime =
            new Regex(DateTimeDefinitions.BasicTime, RegexFlags);

        // handle special time such as 'at midnight', 'midnight', 'midday'
        public static readonly Regex MidnightRegex =
            new Regex(DateTimeDefinitions.MidnightRegex, RegexFlags);

        public static readonly Regex MidmorningRegex =
            new Regex(DateTimeDefinitions.MidmorningRegex, RegexFlags);

        public static readonly Regex MidafternoonRegex =
            new Regex(DateTimeDefinitions.MidafternoonRegex, RegexFlags);

        public static readonly Regex MiddayRegex =
            new Regex(DateTimeDefinitions.MiddayRegex, RegexFlags);

        public static readonly Regex MidTimeRegex =
            new Regex(DateTimeDefinitions.MidTimeRegex, RegexFlags);

        // part 3: regex for time
        // --------------------------------------
        // handle "at four" "at 3"
        public static readonly Regex AtRegex =
            new Regex(DateTimeDefinitions.AtRegex, RegexFlags);

        public static readonly Regex IshRegex =
            new Regex(DateTimeDefinitions.IshRegex, RegexFlags);

        public static readonly Regex TimeUnitRegex =
            new Regex(DateTimeDefinitions.TimeUnitRegex, RegexFlags);

        public static readonly Regex ConnectNumRegex =
            new Regex(DateTimeDefinitions.ConnectNumRegex, RegexFlags);

        public static readonly Regex TimeBeforeAfterRegex =
            new Regex(DateTimeDefinitions.TimeBeforeAfterRegex, RegexFlags);

        public static readonly Regex[] TimeRegexList =
        {
            // (three min past)? seven|7|(senven thirty) pm
            new Regex(DateTimeDefinitions.TimeRegex1, RegexFlags),

            // (three min past)? 3:00(:00)? (pm)?
            new Regex(DateTimeDefinitions.TimeRegex2, RegexFlags),

            // (three min past)? 3.00 (pm)
            new Regex(DateTimeDefinitions.TimeRegex3, RegexFlags),

            // (three min past) (five thirty|seven|7|7:00(:00)?) (pm)? (in the night)
            new Regex(DateTimeDefinitions.TimeRegex4, RegexFlags),

            // (three min past) (five thirty|seven|7|7:00(:00)?) (pm)?
            new Regex(DateTimeDefinitions.TimeRegex5, RegexFlags),

            // (five thirty|seven|7|7:00(:00)?) (pm)? (in the night)
            new Regex(DateTimeDefinitions.TimeRegex6, RegexFlags),

            // (in the night) at (five thirty|seven|7|7:00(:00)?) (pm)?
            new Regex(DateTimeDefinitions.TimeRegex7, RegexFlags),

            // (in the night) (five thirty|seven|7|7:00(:00)?) (pm)?
            new Regex(DateTimeDefinitions.TimeRegex8, RegexFlags),

            new Regex(DateTimeDefinitions.TimeRegex9, RegexFlags),

            // (three min past)? 3h00 (pm)?
            new Regex(DateTimeDefinitions.TimeRegex10, RegexFlags),

            // 340pm
            ConnectNumRegex,
        };

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public GermanTimeExtractorConfiguration(IOptionsConfiguration config)
            : base(config)
        {
            DurationExtractor = new BaseDurationExtractor(new GermanDurationExtractorConfiguration(this));
            TimeZoneExtractor = new BaseTimeZoneExtractor(new GermanTimeZoneExtractorConfiguration(this));
        }

        IEnumerable<Regex> ITimeExtractorConfiguration.TimeRegexList => TimeRegexList;

        Regex ITimeExtractorConfiguration.AtRegex => AtRegex;

        Regex ITimeExtractorConfiguration.IshRegex => IshRegex;

        Regex ITimeExtractorConfiguration.TimeBeforeAfterRegex => TimeBeforeAfterRegex;

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor TimeZoneExtractor { get; }
    }
}