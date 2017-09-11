using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class TimePeriodExtractorChs : BaseDateTimeExtractor<PeriodType>
    {
        internal sealed override ImmutableDictionary<Regex, PeriodType> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_DATETIME_TIMEPERIOD;

        public const string TimePeriodConnectWords = DateTimeDefinitions.TimePeriod_TimePeriodConnectWords;

        //五点十分四十八秒
        public static readonly string ChineseTimeRegex = TimeExtractorChs.ChineseTimeRegex;

        //六点 到 九点 | 六 到 九点
        public static readonly string LeftChsTimeRegex = DateTimeDefinitions.TimePeriod_LeftChsTimeRegex;

        public static readonly string RightChsTimeRegex = DateTimeDefinitions.TimePeriod_RightChsTimeRegex;

        //2:45
        public static readonly string DigitTimeRegex = TimeExtractorChs.DigitTimeRegex;

        public static readonly string LeftDigitTimeRegex = DateTimeDefinitions.TimePeriod_LeftDigitTimeRegex;

        public static readonly string RightDigitTimeRegex = DateTimeDefinitions.TimePeriod_RightDigitTimeRegex;

        public static readonly string ShortLeftChsTimeRegex = DateTimeDefinitions.TimePeriod_ShortLeftChsTimeRegex;

        public static readonly string ShortLeftDigitTimeRegex = DateTimeDefinitions.TimePeriod_ShortLeftDigitTimeRegex;

        public TimePeriodExtractorChs()
        {
            var regexes = new Dictionary<Regex, PeriodType>
            {
                {
                    new Regex(DateTimeDefinitions.TimePeriod_Regexes1, RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    PeriodType.FullTime
                },
                {
                    new Regex(DateTimeDefinitions.TimePeriod_Regexes2, RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    PeriodType.ShortTime
                }
            };

            Regexes = regexes.ToImmutableDictionary();
        }
    }

    public enum PeriodType
    {
        ShortTime,
        FullTime
    }
}