using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Korean;

namespace Microsoft.Recognizers.Text.DateTime.Korean
{
    public class KoreanTimePeriodExtractorConfiguration : BaseDateTimeOptionsConfiguration, ICJKTimePeriodExtractorConfiguration
    {
        public const string TimePeriodConnectWords = DateTimeDefinitions.TimePeriodTimePeriodConnectWords;

        // 다섯 점 충분히 마흔 여덟 초
        public static readonly string CJKTimeRegex = KoreanTimeExtractorConfiguration.CJKTimeRegex;

        // 6 ~ 9시 | 6 ~ 9시
        public static readonly string LeftCJKTimeRegex = DateTimeDefinitions.TimePeriodLeftCJKTimeRegex;

        public static readonly string RightCJKTimeRegex = DateTimeDefinitions.TimePeriodRightCJKTimeRegex;

        // 2:45
        public static readonly string DigitTimeRegex = KoreanTimeExtractorConfiguration.DigitTimeRegex;

        public static readonly string LeftDigitTimeRegex = DateTimeDefinitions.TimePeriodLeftDigitTimeRegex;

        public static readonly string RightDigitTimeRegex = DateTimeDefinitions.TimePeriodRightDigitTimeRegex;

        public static readonly string ShortLeftCJKTimeRegex = DateTimeDefinitions.TimePeriodShortLeftCJKTimeRegex;

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