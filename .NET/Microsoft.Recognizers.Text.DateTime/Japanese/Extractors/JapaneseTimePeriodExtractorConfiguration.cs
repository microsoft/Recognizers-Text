using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Japanese;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseTimePeriodExtractorConfiguration : BaseDateTimeOptionsConfiguration, ICJKTimePeriodExtractorConfiguration
    {
        public const string TimePeriodConnectWords = DateTimeDefinitions.TimePeriodTimePeriodConnectWords;

        // 五点十分四十八秒
        public static readonly string CJKTimeRegex = JapaneseTimeExtractorConfiguration.CJKTimeRegex;

        // 6時から9時| 6時から9時
        public static readonly string LeftCJKTimeRegex = DateTimeDefinitions.TimePeriodLeftCJKTimeRegex;

        public static readonly string RightCJKTimeRegex = DateTimeDefinitions.TimePeriodRightCJKTimeRegex;

        // 2:45
        public static readonly string DigitTimeRegex = JapaneseTimeExtractorConfiguration.DigitTimeRegex;

        public static readonly string LeftDigitTimeRegex = DateTimeDefinitions.TimePeriodLeftDigitTimeRegex;

        public static readonly string RightDigitTimeRegex = DateTimeDefinitions.TimePeriodRightDigitTimeRegex;

        public static readonly string ShortLeftCJKTimeRegex = DateTimeDefinitions.TimePeriodShortLeftCJKTimeRegex;

        public static readonly string ShortLeftDigitTimeRegex = DateTimeDefinitions.TimePeriodShortLeftDigitTimeRegex;

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public JapaneseTimePeriodExtractorConfiguration(IDateTimeOptionsConfiguration config)
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