using Microsoft.Recognizers.Resources.English;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishTimeExtractorConfiguration : ITimeExtractorConfiguration
    {
        // part 1: smallest component
        // --------------------------------------
        public static readonly Regex DescRegex = new Regex(DateTimeDefinition.DescRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex HourNumRegex =
            new Regex(DateTimeDefinition.HourNumRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MinuteNumRegex =
            new Regex(
                DateTimeDefinition.MinuteNumRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // part 2: middle level component
        // --------------------------------------
        // handle "... o'clock"
        public static readonly Regex OclockRegex = new Regex(DateTimeDefinition.OclockRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // handle "... afternoon"
        public static readonly Regex PmRegex =
            new Regex(DateTimeDefinition.PmRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // handle "... in the morning"
        public static readonly Regex AmRegex = new Regex(DateTimeDefinition.AmRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // handle "half past ..." "a quarter to ..."
        // rename 'min' group to 'deltamin'
        public static readonly Regex LessThanOneHour =
            new Regex(
                DateTimeDefinition.LessThanOneHour, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // handle "six thirty", "six twenty one" 
        public static readonly Regex EngTimeRegex =
            new Regex(
                DateTimeDefinition.EngTimeRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimePrefix =
            new Regex(DateTimeDefinition.TimePrefix,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeSuffix =
            new Regex(DateTimeDefinition.TimeSuffix, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex BasicTime =
            new Regex(
                DateTimeDefinition.BasicTime, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // part 3: regex for time
        // --------------------------------------
        // handle "at four" "at 3"
        public static readonly Regex AtRegex =
            new Regex(DateTimeDefinition.AtRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex IshRegex = new Regex(DateTimeDefinition.IshRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeUnitRegex = new Regex(DateTimeDefinition.TimeUnitRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex ConnectNumRegex =
            new Regex(
                DateTimeDefinition.ConnectNumRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex[] TimeRegexList =
        {
            // (three min past)? seven|7|(senven thirty) pm
            new Regex(
                DateTimeDefinition.TimeRegex1, RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (three min past)? 3:00(:00)? (pm)?
            new Regex(
                DateTimeDefinition.TimeRegex2, RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (three min past)? 3.00 (pm)?
            new Regex(DateTimeDefinition.TimeRegex3,
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (three min past) (five thirty|seven|7|7:00(:00)?) (pm)? (in the night)
            new Regex(DateTimeDefinition.TimeRegex4,
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (three min past) (five thirty|seven|7|7:00(:00)?) (pm)?
            new Regex(DateTimeDefinition.TimeRegex5,
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (five thirty|seven|7|7:00(:00)?) (pm)? (in the night)
            new Regex(DateTimeDefinition.TimeRegex6,
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (in the night) at (five thirty|seven|7|7:00(:00)?) (pm)?
            new Regex(DateTimeDefinition.TimeRegex7,
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (in the night) (five thirty|seven|7|7:00(:00)?) (pm)?
            new Regex(DateTimeDefinition.TimeRegex8,
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 340pm
            ConnectNumRegex
        };

        IEnumerable<Regex> ITimeExtractorConfiguration.TimeRegexList => TimeRegexList;

        Regex ITimeExtractorConfiguration.AtRegex => AtRegex;

        Regex ITimeExtractorConfiguration.IshRegex => IshRegex;

        public IExtractor DurationExtractor { get; }

        public EnglishTimeExtractorConfiguration()
        {
            DurationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
        }
    }
}