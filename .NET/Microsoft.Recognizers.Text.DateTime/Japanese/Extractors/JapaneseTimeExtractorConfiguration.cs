using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Japanese;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseTimeExtractorConfiguration : BaseDateTimeOptionsConfiguration, ICJKTimeExtractorConfiguration
    {
        public static readonly string HourNumRegex = DateTimeDefinitions.TimeHourNumRegex;

        public static readonly string MinuteNumRegex = DateTimeDefinitions.TimeMinuteNumRegex;

        public static readonly string SecondNumRegex = DateTimeDefinitions.TimeSecondNumRegex;

        public static readonly string HourCJKRegex = DateTimeDefinitions.TimeHourCJKRegex;

        public static readonly string MinuteCJKRegex = DateTimeDefinitions.TimeMinuteCJKRegex;

        public static readonly string SecondCJKRegex = DateTimeDefinitions.TimeSecondCJKRegex;

        public static readonly string ClockDescRegex = DateTimeDefinitions.TimeClockDescRegex;

        public static readonly string MinuteDescRegex = DateTimeDefinitions.TimeMinuteDescRegex;

        public static readonly string SecondDescRegex = DateTimeDefinitions.TimeSecondDescRegex;

        public static readonly string BanHourPrefixRegex = DateTimeDefinitions.TimeBanHourPrefixRegex;

        // e.g: 12時
        public static readonly string HourRegex = DateTimeDefinitions.TimeHourRegex;

        public static readonly string MinuteRegex = DateTimeDefinitions.TimeMinuteRegex;

        public static readonly string SecondRegex = DateTimeDefinitions.TimeSecondRegex;

        public static readonly string HalfRegex = DateTimeDefinitions.TimeHalfRegex;

        public static readonly string QuarterRegex = DateTimeDefinitions.TimeQuarterRegex;

        // e.g: 十二五十から八|半分|瞬間
        public static readonly string CJKTimeRegex = DateTimeDefinitions.TimeCJKTimeRegex;

        // e.g: 12:23
        public static readonly string DigitTimeRegex = DateTimeDefinitions.TimeDigitTimeRegex;

        // e.g: 朝の9時
        public static readonly string DayDescRegex = DateTimeDefinitions.TimeDayDescRegex;

        public static readonly string ApproximateDescPreffixRegex = DateTimeDefinitions.TimeApproximateDescPreffixRegex;

        public static readonly string ApproximateDescSuffixRegex = DateTimeDefinitions.TimeApproximateDescSuffixRegex;

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public JapaneseTimeExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            var regexes = new Dictionary<Regex, TimeType>
            {
                {
                    new Regex(DateTimeDefinitions.TimeRegexes1, RegexFlags),
                    TimeType.CjkTime
                },
                {
                    new Regex(DateTimeDefinitions.TimeRegexes2, RegexFlags),
                    TimeType.DigitTime
                },
                {
                    new Regex(DateTimeDefinitions.TimeRegexes3, RegexFlags),
                    TimeType.LessTime
                },
            };
            Regexes = regexes.ToImmutableDictionary();
        }

        public ImmutableDictionary<Regex, TimeType> Regexes { get; }
    }
}