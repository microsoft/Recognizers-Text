using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class ChineseTimeExtractorConfiguration : ChineseBaseDateTimeExtractorConfiguration<TimeType>
    {
        public static readonly string HourNumRegex = DateTimeDefinitions.TimeHourNumRegex;

        public static readonly string MinuteNumRegex = DateTimeDefinitions.TimeMinuteNumRegex;

        public static readonly string SecondNumRegex = DateTimeDefinitions.TimeSecondNumRegex;

        public static readonly string HourChsRegex = DateTimeDefinitions.TimeHourChsRegex;

        public static readonly string MinuteChsRegex = DateTimeDefinitions.TimeMinuteChsRegex;

        public static readonly string SecondChsRegex = DateTimeDefinitions.TimeSecondChsRegex;

        public static readonly string ClockDescRegex = DateTimeDefinitions.TimeClockDescRegex;

        public static readonly string MinuteDescRegex = DateTimeDefinitions.TimeMinuteDescRegex;

        public static readonly string SecondDescRegex = DateTimeDefinitions.TimeSecondDescRegex;

        public static readonly string BanHourPrefixRegex = DateTimeDefinitions.TimeBanHourPrefixRegex;

        // e.g: 12点, 十二点, 十二点整
        public static readonly string HourRegex = DateTimeDefinitions.TimeHourRegex;

        public static readonly string MinuteRegex = DateTimeDefinitions.TimeMinuteRegex;

        public static readonly string SecondRegex = DateTimeDefinitions.TimeSecondRegex;

        public static readonly string HalfRegex = DateTimeDefinitions.TimeHalfRegex;

        public static readonly string QuarterRegex = DateTimeDefinitions.TimeQuarterRegex;

        // e.g: 十二点五十八分|半|一刻
        public static readonly string ChineseTimeRegex = DateTimeDefinitions.TimeChineseTimeRegex;

        // e.g: 12:23
        public static readonly string DigitTimeRegex = DateTimeDefinitions.TimeDigitTimeRegex;

        // e.g: 早上九点
        public static readonly string DayDescRegex = DateTimeDefinitions.TimeDayDescRegex;

        public static readonly string ApproximateDescPreffixRegex = DateTimeDefinitions.TimeApproximateDescPreffixRegex;

        public static readonly string ApproximateDescSuffixRegex = DateTimeDefinitions.TimeApproximateDescSuffixRegex;

        public ChineseTimeExtractorConfiguration()
        {
            var regexes = new Dictionary<Regex, TimeType>
            {
                {
                    new Regex(DateTimeDefinitions.TimeRegexes1, RegexOptions.Singleline),
                    TimeType.CjkTime
                },
                {
                    new Regex(DateTimeDefinitions.TimeRegexes2, RegexOptions.Singleline),
                    TimeType.DigitTime
                },
                {
                    new Regex(DateTimeDefinitions.TimeRegexes3, RegexOptions.Singleline),
                    TimeType.LessTime
                },
            };
            Regexes = regexes.ToImmutableDictionary();
        }

        internal sealed override ImmutableDictionary<Regex, TimeType> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_DATETIME_TIME; // "Fraction";
    }
}