using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Japanese;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class TimeExtractorJap : BaseDateTimeExtractor<TimeType>
    {
        internal sealed override ImmutableDictionary<Regex, TimeType> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_DATETIME_TIME; // "Fraction";

        public static readonly string HourNumRegex = DateTimeDefinitions.TimeHourNumRegex;

        public static readonly string MinuteNumRegex = DateTimeDefinitions.TimeMinuteNumRegex;

        public static readonly string SecondNumRegex = DateTimeDefinitions.TimeSecondNumRegex;

        public static readonly string HourJapRegex = DateTimeDefinitions.TimeHourJapRegex;

        public static readonly string MinuteJapRegex = DateTimeDefinitions.TimeMinuteJapRegex;

        public static readonly string SecondJapRegex = DateTimeDefinitions.TimeSecondJapRegex;

        public static readonly string ClockDescRegex = DateTimeDefinitions.TimeClockDescRegex;

        public static readonly string MinuteDescRegex = DateTimeDefinitions.TimeMinuteDescRegex;

        public static readonly string SecondDescRegex = DateTimeDefinitions.TimeSecondDescRegex;

        public static readonly string BanHourPrefixRegex = DateTimeDefinitions.TimeBanHourPrefixRegex;

        //e.g: 12点, 十二点, 十二点整
        public static readonly string HourRegex = DateTimeDefinitions.TimeHourRegex;

        public static readonly string MinuteRegex = DateTimeDefinitions.TimeMinuteRegex;

        public static readonly string SecondRegex = DateTimeDefinitions.TimeSecondRegex;

        public static readonly string HalfRegex = DateTimeDefinitions.TimeHalfRegex;

        public static readonly string QuarterRegex = DateTimeDefinitions.TimeQuarterRegex;

        //e.g: 十二点五十八分|半|一刻
        public static readonly string JapaneseTimeRegex = DateTimeDefinitions.TimeJapaneseTimeRegex;

        //e.g: 12:23
        public static readonly string DigitTimeRegex = DateTimeDefinitions.TimeDigitTimeRegex;

        //e.g: 早上九点
        public static readonly string DayDescRegex = DateTimeDefinitions.TimeDayDescRegex;

        public static readonly string ApproximateDescPreffixRegex = DateTimeDefinitions.TimeApproximateDescPreffixRegex;

        public static readonly string ApproximateDescSuffixRegex = DateTimeDefinitions.TimeApproximateDescSuffixRegex;

        public TimeExtractorJap()
        {
            var regexes = new Dictionary<Regex, TimeType>
            {
                {
                    new Regex(DateTimeDefinitions.TimeRegexes1, RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    TimeType.JapaneseTime
                },
                {
                    new Regex(DateTimeDefinitions.TimeRegexes2, RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    TimeType.DigitTime
                },
                {
                    new Regex(DateTimeDefinitions.TimeRegexes3, RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    TimeType.LessTime
                }
            };
            Regexes = regexes.ToImmutableDictionary();
        }
    }

    public enum TimeType
    {
        //十二点二十三分五十八秒,12点23分53秒
        JapaneseTime,
        //差五分十二点
        LessTime,
        //大约早上10:00
        DigitTime
    }
}