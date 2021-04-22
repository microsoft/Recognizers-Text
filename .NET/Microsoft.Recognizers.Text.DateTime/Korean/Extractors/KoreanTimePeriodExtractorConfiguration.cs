using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Korean;

namespace Microsoft.Recognizers.Text.DateTime.Korean
{
    public class KoreanTimePeriodExtractorConfiguration : BaseDateTimeOptionsConfiguration, ICJKTimePeriodExtractorConfiguration
    {
        public const string TimePeriodConnectWords = DateTimeDefinitions.TimePeriodTimePeriodConnectWords;

        // 五点十分四十八秒
        public static readonly string CJKTimeRegex = KoreanTimeExtractorConfiguration.CJKTimeRegex;

        // 六点 到 九点 | 六 到 九点
        public static readonly string LeftChsTimeRegex = DateTimeDefinitions.TimePeriodLeftChsTimeRegex;

        public static readonly string RightChsTimeRegex = DateTimeDefinitions.TimePeriodRightChsTimeRegex;

        // 2:45
        public static readonly string DigitTimeRegex = KoreanTimeExtractorConfiguration.DigitTimeRegex;

        public static readonly string LeftDigitTimeRegex = DateTimeDefinitions.TimePeriodLeftDigitTimeRegex;

        public static readonly string RightDigitTimeRegex = DateTimeDefinitions.TimePeriodRightDigitTimeRegex;

        public static readonly string ShortLeftChsTimeRegex = DateTimeDefinitions.TimePeriodShortLeftChsTimeRegex;

        public static readonly string ShortLeftDigitTimeRegex = DateTimeDefinitions.TimePeriodShortLeftDigitTimeRegex;

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public KoreanTimePeriodExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            var regexes = new Dictionary<Regex, PeriodType>
            {
                {
                    new Regex(DateTimeDefinitions.TimePeriodRegexes1, RegexFlags),
                    PeriodType.FullTime
                },
                {
                    new Regex(DateTimeDefinitions.TimePeriodRegexes2, RegexFlags),
                    PeriodType.ShortTime
                },
                {
                    new Regex(DateTimeDefinitions.TimeOfDayRegex, RegexFlags),
                    PeriodType.ShortTime
                },
            };

            Regexes = regexes.ToImmutableDictionary();
        }

        public ImmutableDictionary<Regex, PeriodType> Regexes { get; }
    }
}