using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Japanese;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class TimePeriodExtractorJap : BaseDateTimeExtractor<PeriodType>
    {
        internal sealed override ImmutableDictionary<Regex, PeriodType> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_DATETIME_TIMEPERIOD;

        public const string TimePeriodConnectWords = DateTimeDefinitions.TimePeriodTimePeriodConnectWords;

        //五点十分四十八秒
        public static readonly string JapaneseTimeRegex = TimeExtractorJap.JapaneseTimeRegex;

        //六点 到 九点 | 六 到 九点
        public static readonly string LeftJapTimeRegex = DateTimeDefinitions.TimePeriodLeftJapTimeRegex;

        public static readonly string RightJapTimeRegex = DateTimeDefinitions.TimePeriodRightJapTimeRegex;

        //2:45
        public static readonly string DigitTimeRegex = TimeExtractorJap.DigitTimeRegex;

        public static readonly string LeftDigitTimeRegex = DateTimeDefinitions.TimePeriodLeftDigitTimeRegex;

        public static readonly string RightDigitTimeRegex = DateTimeDefinitions.TimePeriodRightDigitTimeRegex;

        public static readonly string ShortLeftJapTimeRegex = DateTimeDefinitions.TimePeriodShortLeftJapTimeRegex;

        public static readonly string ShortLeftDigitTimeRegex = DateTimeDefinitions.TimePeriodShortLeftDigitTimeRegex;

        public TimePeriodExtractorJap()
        {
            var regexes = new Dictionary<Regex, PeriodType>
            {
                {
                    new Regex(DateTimeDefinitions.TimePeriodRegexes1, RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    PeriodType.FullTime
                },
                {
                    new Regex(DateTimeDefinitions.TimePeriodRegexes2, RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    PeriodType.ShortTime
                },
                {
                    new Regex(DateTimeDefinitions.TimeOfDayRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline),
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