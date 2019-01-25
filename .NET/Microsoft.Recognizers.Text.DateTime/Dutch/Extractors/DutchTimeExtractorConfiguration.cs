using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Dutch;

namespace Microsoft.Recognizers.Text.DateTime.Dutch
{
    public class DutchTimeExtractorConfiguration : BaseOptionsConfiguration, ITimeExtractorConfiguration
    {
        // handle "half past ..." "a quarter to ..."
        // rename 'min' group to 'deltamin'
        public static readonly Regex LessThanOneHour =
            new Regex(DateTimeDefinitions.LessThanOneHour, RegexOptions.Singleline);

        public static readonly Regex BasicTime =
            new Regex(DateTimeDefinitions.BasicTime, RegexOptions.Singleline);

        // handle special time such as 'at midnight', 'midnight', 'midday'
        public static readonly Regex MidnightRegex =
            new Regex(DateTimeDefinitions.MidnightRegex, RegexOptions.Singleline);

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
            // (three min past)? seven|7|(senven thirty) pm
            new Regex(DateTimeDefinitions.TimeRegex1, RegexOptions.Singleline),

            // (three min past)? 3:00(:00)? (pm)?
            new Regex(DateTimeDefinitions.TimeRegex2, RegexOptions.Singleline),

            // (three min past)? 3.00 (pm)
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

            // (three min past)? 3h00 (pm)?
            new Regex(DateTimeDefinitions.TimeRegex10, RegexOptions.Singleline),

            // at 2.30, "at" prefix is required here
            // 3.30pm, "am/pm" suffix is required here
            new Regex(DateTimeDefinitions.TimeRegex11, RegexOptions.Singleline),

            // 340pm
            ConnectNumRegex,
        };

        IEnumerable<Regex> ITimeExtractorConfiguration.TimeRegexList => TimeRegexList;

        Regex ITimeExtractorConfiguration.AtRegex => AtRegex;

        Regex ITimeExtractorConfiguration.IshRegex => IshRegex;

        Regex ITimeExtractorConfiguration.TimeBeforeAfterRegex => TimeBeforeAfterRegex;

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor TimeZoneExtractor { get; }

        public DutchTimeExtractorConfiguration(IOptionsConfiguration config)
            : base(config)
        {
            DurationExtractor = new BaseDurationExtractor(new DutchDurationExtractorConfiguration(this));
            TimeZoneExtractor = new BaseTimeZoneExtractor(new DutchTimeZoneExtractorConfiguration(this));
        }
    }
}