using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class TimePeriodExtractorChs : BaseDateTimeExtractor<PeriodType>
    {
        internal sealed override ImmutableDictionary<Regex, PeriodType> Regexes { get; }
        protected sealed override string ExtractType { get; } = Constants.SYS_DATETIME_TIMEPERIOD;

        public const string TimePeriodConnectWords = "(起|至|到|–|-|—|~|～)";
        //五点十分四十八秒
        public static readonly string ChineseTimeRegex = TimeExtractorChs.ChineseTimeRegex;
        //六点 到 九点 | 六 到 九点
        public static readonly string LeftChsTimeRegex = $"(从)?(?<left>{TimeExtractorChs.DayDescRegex}?" +
                                                         $"({ChineseTimeRegex}))";

        public static readonly string RightChsTimeRegex =
            $"{TimePeriodConnectWords}(?<right>{TimeExtractorChs.DayDescRegex}?" +
            $"{ChineseTimeRegex})(之间)?";

        //2:45
        public static readonly string DigitTimeRegex = TimeExtractorChs.DigitTimeRegex;

        public static readonly string LeftDigitTimeRegex = $"(从)?(?<left>{TimeExtractorChs.DayDescRegex}?" +
                                                           $"({DigitTimeRegex}))";

        public static readonly string RightDigitTimeRegex =
            $"{TimePeriodConnectWords}(?<right>{TimeExtractorChs.DayDescRegex}?" +
            $"{DigitTimeRegex})(之间)?";

        public static readonly string ShortLeftChsTimeRegex =
            $"(从)?(?<left>{TimeExtractorChs.DayDescRegex}?({TimeExtractorChs.HourChsRegex}))";

        public static readonly string ShortLeftDigitTimeRegex =
            $"(从)?(?<left>{TimeExtractorChs.DayDescRegex}?({TimeExtractorChs.HourNumRegex}))";

        public TimePeriodExtractorChs()
        {
            var _regexes = new Dictionary<Regex, PeriodType>
            {
                {
                    new Regex($@"({LeftDigitTimeRegex}{RightDigitTimeRegex}|{LeftChsTimeRegex}{RightChsTimeRegex})",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    PeriodType.FullTime
                },
                {
                    new Regex(
                        $@"({ShortLeftDigitTimeRegex}{RightDigitTimeRegex}|{ShortLeftChsTimeRegex}{RightChsTimeRegex})",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    PeriodType.ShortTime
                }
            };
            Regexes = _regexes.ToImmutableDictionary();
        }
    }

    public enum PeriodType
    {
        ShortTime,
        FullTime
    }
}