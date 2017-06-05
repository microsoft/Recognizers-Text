using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class TimeExtractorChs : BaseDateTimeExtractor<TimeType>
    {
        internal sealed override ImmutableDictionary<Regex, TimeType> Regexes { get; }
        protected sealed override string ExtractType { get; } = Constants.SYS_DATETIME_TIME; // "Fraction";

        public static readonly string HourNumRegex =
            @"(00|01|02|03|04|05|06|07|08|09|0|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|1|2|3|4|5|6|7|8|9)";

        public static readonly string MinuteNumRegex =
            @"(00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|32|33|34|35|36|37|38|39|40|41|42|43|44|45|46|47|48|49|50|51|52|53|54|55|56|57|58|59|0|1|2|3|4|5|6|7|8|9)";

        public static readonly string SecondNumRegex =
            @"(00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|32|33|34|35|36|37|38|39|40|41|42|43|44|45|46|47|48|49|50|51|52|53|54|55|56|57|58|59|0|1|2|3|4|5|6|7|8|9)";

        public static readonly string HourChsRegex =
            @"([零〇一二两三四五六七八九]|二十[一二三四]?|十[一二三四五六七八九]?)";

        public static readonly string MinuteChsRegex =
            @"([二三四五]?十[一二三四五六七八九]?|六十|[零〇一二三四五六七八九])";

        public static readonly string SecondChsRegex = MinuteChsRegex;

        public static readonly string ClockDescRegex = @"(点\s*整|点\s*钟|点|时)";

        public static readonly string MinuteDescRegex = @"(分钟|分|)";

        public static readonly string SecondDescRegex = @"(秒钟|秒)";

        public static readonly string BanHourPrefixRegex = @"(第)";
        //e.g: 12点, 十二点, 十二点整
        public static readonly string HourRegex =
            $@"(?<!{BanHourPrefixRegex})(?<hour>{HourChsRegex}|{HourNumRegex}){ClockDescRegex}";

        public static readonly string MinuteRegex =
            $@"(?<min>{MinuteChsRegex}|{MinuteNumRegex}){MinuteDescRegex}";

        public static readonly string SecondRegex =
            $@"(?<sec>{SecondChsRegex}|{SecondNumRegex}){SecondDescRegex}";

        public static readonly string HalfRegex = @"(?<half>过半|半)";

        public static readonly string QuareterRegex = @"(?<quarter>[一两二三四1-4])\s*(刻钟|刻)";

        //e.g: 十二点五十八分|半|一刻
        public static readonly string ChineseTimeRegex =
            $@"{HourRegex}({QuareterRegex}|{HalfRegex}|((过|又)?{MinuteRegex})({SecondRegex})?)?";

        //e.g: 12:23
        public static readonly string DigitTimeRegex =
            $@"(?<hour>{HourNumRegex}):(?<min>{MinuteNumRegex})(:(?<sec>{SecondNumRegex}))?";

        //e.g: 早上九点
        public static readonly string DayDescRegex =
            @"(?<daydesc>凌晨|清晨|早上|早|上午|中午|下午|午后|晚上|夜里|夜晚|半夜|午夜|夜间|深夜|傍晚|晚)";

        public static readonly string ApproximateDescPreffixRegex =
            @"(大[约概]|差不多|可能|也许|约|不超过|不多[于过]|最[多长少]|少于|[超短长多]过|几乎要|将近|差点|快要|接近|至少|起码|超出|不到)";

        public static readonly string ApproximateDescSuffixRegex =
            @"(之前|以前|以后|之后|前|后|左右)";

        public TimeExtractorChs()
        {
            var _regexes = new Dictionary<Regex, TimeType>
            {
                {
                    new Regex(
                        $@"{ApproximateDescPreffixRegex}?{DayDescRegex}?{ChineseTimeRegex}{ApproximateDescSuffixRegex}?",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    TimeType.ChineseTime
                },
                {
                    new Regex(
                        $@"{ApproximateDescPreffixRegex}?{DayDescRegex}?{DigitTimeRegex}{ApproximateDescSuffixRegex}?",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    TimeType.DigitTime
                },
                {
                    new Regex($@"差{MinuteRegex}{ChineseTimeRegex}", RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    TimeType.LessTime
                }
            };
            Regexes = _regexes.ToImmutableDictionary();
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