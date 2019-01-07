using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Italian;

namespace Microsoft.Recognizers.Text.DateTime.Italian
{
    public class ItalianTimeExtractorConfiguration : BaseOptionsConfiguration, ITimeExtractorConfiguration
    {
        // part 1: smallest component
        // --------------------------------
        public static readonly Regex DescRegex =
            new Regex(DateTimeDefinitions.DescRegex, RegexOptions.Singleline);

        public static readonly Regex HourNumRegex =
            new Regex(DateTimeDefinitions.HourNumRegex, RegexOptions.Singleline);

        public static readonly Regex MinuteNumRegex =
            new Regex(DateTimeDefinitions.MinuteNumRegex, RegexOptions.Singleline);

        // part 2: middle level component
        // --------------------------------------
        // handle "... heures (o'clock, en punto)"
        public static readonly Regex OclockRegex =
            new Regex(DateTimeDefinitions.OclockRegex, RegexOptions.Singleline);

        // handle "... après midi (afternoon, tarde)"
        public static readonly Regex PmRegex =
            new Regex(DateTimeDefinitions.PmRegex, RegexOptions.Singleline);

        // handle "... dans la matinee (in the morning)"
        public static readonly Regex AmRegex =
            new Regex(DateTimeDefinitions.AmRegex, RegexOptions.Singleline);

        // handle "half past ..." "a quarter to ..."
        // rename 'min' group to 'deltamin'
        public static readonly Regex LessThanOneHour =
            new Regex(DateTimeDefinitions.LessThanOneHour, RegexOptions.Singleline);

        public static readonly Regex FrTimeRegex =
            new Regex(DateTimeDefinitions.EngTimeRegex, RegexOptions.Singleline);

        // TODO - will have change below
        // handle "six heures et demie" (six thirty), "six heures et vingt-et-un" (six twenty one)
        public static readonly Regex TimePrefix =
            new Regex(DateTimeDefinitions.TimePrefix, RegexOptions.Singleline);

        public static readonly Regex TimeSuffix =
            new Regex(DateTimeDefinitions.TimeSuffix, RegexOptions.Singleline);

        public static readonly Regex BasicTime =
            new Regex(DateTimeDefinitions.BasicTime, RegexOptions.Singleline);

        // handle special time such as 'at midnight', 'midnight', 'midday'
        // midnight - le minuit, la zero heure
        // midday - midi
        public static readonly Regex MidnightRegex =
            new Regex(DateTimeDefinitions.MidnightRegex, RegexOptions.Singleline);

        public static readonly Regex MidmorningRegex =
            new Regex(DateTimeDefinitions.MidmorningRegex, RegexOptions.Singleline);

        public static readonly Regex MidafternoonRegex =
            new Regex(DateTimeDefinitions.MidafternoonRegex, RegexOptions.Singleline);

        public static readonly Regex MiddayRegex =
            new Regex(DateTimeDefinitions.MiddayRegex, RegexOptions.Singleline);

        public static readonly Regex MidTimeRegex =
            new Regex(DateTimeDefinitions.MidTimeRegex, RegexOptions.Singleline);

        // part 3: regex for time
        // --------------------------------------
        // handle "at four" "at 3"
        public static readonly Regex AtRegex =
            new Regex(DateTimeDefinitions.AtRegex, RegexOptions.Singleline);

        public static readonly Regex IshRegex =
            new Regex(DateTimeDefinitions.IshRegex, RegexOptions.Singleline);

        public static readonly Regex TimeUnitRegex =
            new Regex(DateTimeDefinitions.TimeUnitRegex, RegexOptions.Singleline);

        public static readonly Regex ConnectNumRegex =
            new Regex(DateTimeDefinitions.ConnectNumRegex, RegexOptions.Singleline);

        public static readonly Regex TimeBeforeAfterRegex =
            new Regex(DateTimeDefinitions.TimeBeforeAfterRegex, RegexOptions.Singleline);

        public static readonly Regex[] TimeRegexList =
        {
            // (three min past)? seven|7|(seven thirty) pm
            new Regex(DateTimeDefinitions.TimeRegex1, RegexOptions.Singleline),

            // (three min past)? 3:00(:00)? (pm)?
            new Regex(DateTimeDefinitions.TimeRegex2, RegexOptions.Singleline),

            // (three min past)? 3.00 (pm)?
            new Regex(DateTimeDefinitions.TimeRegex3, RegexOptions.Singleline),

            // (three min past) (five thirty|seven|7|7:00(:00)?) (pm)? (in the night)
            new Regex(DateTimeDefinitions.TimeRegex4, RegexOptions.Singleline),

            // (three min past) (five thirty|seven|7|7:00(:00)?) (pm)?
            new Regex(DateTimeDefinitions.TimeRegex5, RegexOptions.Singleline),

            // (five thirty|seven|7|7:00(:00)?) (pm)? (in the night)
            new Regex(DateTimeDefinitions.TimeRegex6, RegexOptions.Singleline),

            // (in the night) at (five thirty|seven|7|7:00(:00)?) (pm)?
            new Regex(DateTimeDefinitions.TimeRegex7, RegexOptions.Singleline),

            // (in the night) (five thirty|seven|7|7:00(:00)?) (pm)?
            new Regex(DateTimeDefinitions.TimeRegex8, RegexOptions.Singleline),

            new Regex(DateTimeDefinitions.TimeRegex9, RegexOptions.Singleline),

            // 340pm
            ConnectNumRegex,
        };

        public ItalianTimeExtractorConfiguration(IOptionsConfiguration config)
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
    }
}
