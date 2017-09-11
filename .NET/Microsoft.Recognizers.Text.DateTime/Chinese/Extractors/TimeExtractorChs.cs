using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class TimeExtractorChs : BaseDateTimeExtractor<TimeType>
    {
        internal sealed override ImmutableDictionary<Regex, TimeType> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_DATETIME_TIME; // "Fraction";

        public static readonly string HourNumRegex = DateTimeDefinitions.Time_HourNumRegex;

        public static readonly string MinuteNumRegex = DateTimeDefinitions.Time_MinuteNumRegex;

        public static readonly string SecondNumRegex = DateTimeDefinitions.Time_SecondNumRegex;

        public static readonly string HourChsRegex = DateTimeDefinitions.Time_HourChsRegex;

        public static readonly string MinuteChsRegex = DateTimeDefinitions.Time_MinuteChsRegex;

        public static readonly string SecondChsRegex = DateTimeDefinitions.Time_SecondChsRegex;

        public static readonly string ClockDescRegex = DateTimeDefinitions.Time_ClockDescRegex;

        public static readonly string MinuteDescRegex = DateTimeDefinitions.Time_MinuteDescRegex;

        public static readonly string SecondDescRegex = DateTimeDefinitions.Time_SecondDescRegex;

        public static readonly string BanHourPrefixRegex = DateTimeDefinitions.Time_BanHourPrefixRegex;
        //e.g: 12点, 十二点, 十二点整
        public static readonly string HourRegex = DateTimeDefinitions.Time_HourRegex;

        public static readonly string MinuteRegex = DateTimeDefinitions.Time_MinuteRegex;

        public static readonly string SecondRegex = DateTimeDefinitions.Time_SecondRegex;

        public static readonly string HalfRegex = DateTimeDefinitions.Time_HalfRegex;

        public static readonly string QuarterRegex = DateTimeDefinitions.Time_QuarterRegex;

        //e.g: 十二点五十八分|半|一刻
        public static readonly string ChineseTimeRegex = DateTimeDefinitions.Time_ChineseTimeRegex;

        //e.g: 12:23
        public static readonly string DigitTimeRegex = DateTimeDefinitions.Time_DigitTimeRegex;

        //e.g: 早上九点
        public static readonly string DayDescRegex = DateTimeDefinitions.Time_DayDescRegex;

        public static readonly string ApproximateDescPreffixRegex = DateTimeDefinitions.Time_ApproximateDescPreffixRegex;

        public static readonly string ApproximateDescSuffixRegex = DateTimeDefinitions.Time_ApproximateDescSuffixRegex;

        public TimeExtractorChs()
        {
            var regexes = new Dictionary<Regex, TimeType>
            {
                {
                    new Regex(DateTimeDefinitions.Time_Regexes1, RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    TimeType.ChineseTime
                },
                {
                    new Regex(DateTimeDefinitions.Time_Regexes2, RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    TimeType.DigitTime
                },
                {
                    new Regex(DateTimeDefinitions.Time_Regexes3, RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    TimeType.LessTime
                }
            };
            Regexes = regexes.ToImmutableDictionary();
        }
    }

    public enum TimeType
    {
        //十二点二十三分五十八秒,12点23分53秒
        ChineseTime,
        //差五分十二点
        LessTime,
        //大约早上10:00
        DigitTime
    }
}