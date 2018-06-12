using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishTimeExtractorConfiguration : BaseOptionsConfiguration, ITimeExtractorConfiguration
    {
        // part 1: smallest component
        // --------------------------------------
        public static readonly Regex DescRegex = new Regex(DateTimeDefinitions.DescRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex HourNumRegex =
            new Regex(DateTimeDefinitions.HourNumRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MinuteNumRegex =
            new Regex(
                DateTimeDefinitions.MinuteNumRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // part 2: middle level component
        // --------------------------------------
        // handle "... o'clock"
        public static readonly Regex OclockRegex = new Regex(DateTimeDefinitions.OclockRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // handle "... afternoon"
        public static readonly Regex PmRegex =
            new Regex(DateTimeDefinitions.PmRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // handle "... in the morning"
        public static readonly Regex AmRegex = new Regex(DateTimeDefinitions.AmRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // handle "half past ..." "a quarter to ..."
        // rename 'min' group to 'deltamin'
        public static readonly Regex LessThanOneHour =
            new Regex(
                DateTimeDefinitions.LessThanOneHour, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // handle "six thirty", "six twenty one" 
        public static readonly Regex WrittenTimeRegex =
            new Regex(
                DateTimeDefinitions.WrittenTimeRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimePrefix =
            new Regex(DateTimeDefinitions.TimePrefix,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeSuffix =
            new Regex(DateTimeDefinitions.TimeSuffix, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex BasicTime =
            new Regex(
                DateTimeDefinitions.BasicTime, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // handle special time such as 'at midnight', 'midnight', 'midday'
        public static readonly Regex MidnightRegex =
            new Regex(
                DateTimeDefinitions.MidnightRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MidmorningRegex =
            new Regex(
                DateTimeDefinitions.MidmorningRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MidafternoonRegex =
            new Regex(
                DateTimeDefinitions.MidafternoonRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MiddayRegex =
            new Regex(
                DateTimeDefinitions.MiddayRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MidTimeRegex =
            new Regex(
                DateTimeDefinitions.MidTimeRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // part 3: regex for time
        // --------------------------------------
        // handle "at four" "at 3"
        public static readonly Regex AtRegex =
            new Regex(DateTimeDefinitions.AtRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex IshRegex = new Regex(DateTimeDefinitions.IshRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeUnitRegex = new Regex(DateTimeDefinitions.TimeUnitRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex ConnectNumRegex =
            new Regex(
                DateTimeDefinitions.ConnectNumRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeBeforeAfterRegex =
            new Regex(
                DateTimeDefinitions.TimeBeforeAfterRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex[] TimeRegexList =
        {
            // (three min past)? seven|7|(senven thirty) pm
            new Regex(
                DateTimeDefinitions.TimeRegex1, RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (three min past)? 3:00(:00)? (pm)?
            new Regex(
                DateTimeDefinitions.TimeRegex2, RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (three min past)? 3.00 (pm)
            new Regex(DateTimeDefinitions.TimeRegex3,
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (three min past) (five thirty|seven|7|7:00(:00)?) (pm)? (in the night)
            new Regex(DateTimeDefinitions.TimeRegex4,
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (three min past) (five thirty|seven|7|7:00(:00)?) (pm)?
            new Regex(DateTimeDefinitions.TimeRegex5,
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (five thirty|seven|7|7:00(:00)?) (pm)? (in the night)
            new Regex(DateTimeDefinitions.TimeRegex6,
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (in the night) at (five thirty|seven|7|7:00(:00)?) (pm)?
            new Regex(DateTimeDefinitions.TimeRegex7,
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (in the night) (five thirty|seven|7|7:00(:00)?) (pm)?
            new Regex(DateTimeDefinitions.TimeRegex8,
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            new Regex(DateTimeDefinitions.TimeRegex9,
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (three min past)? 3h00 (pm)?
            new Regex(DateTimeDefinitions.TimeRegex10,
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 340pm
            ConnectNumRegex
        };

        IEnumerable<Regex> ITimeExtractorConfiguration.TimeRegexList => TimeRegexList;

        Regex ITimeExtractorConfiguration.AtRegex => AtRegex;

        Regex ITimeExtractorConfiguration.IshRegex => IshRegex;

        Regex ITimeExtractorConfiguration.TimeBeforeAfterRegex => TimeBeforeAfterRegex;

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor TimeZoneExtractor { get; }

        public EnglishTimeExtractorConfiguration(DateTimeOptions options = DateTimeOptions.None) : base(options)
        {
            DurationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
            TimeZoneExtractor = new BaseTimeZoneExtractor(new EnglishTimeZoneExtractorConfiguration(options));
        }
    }
}