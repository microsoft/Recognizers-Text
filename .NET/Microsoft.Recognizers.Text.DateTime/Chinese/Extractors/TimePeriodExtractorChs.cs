using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class TimePeriodExtractorChs : BaseDateTimeExtractor<PeriodType>
    {
        // 五点十分四十八秒
        public static readonly string ChineseTimeRegex = TimeExtractorChs.ChineseTimeRegex;

        // 2:45
        public static readonly string DigitTimeRegex = TimeExtractorChs.DigitTimeRegex;

        public TimePeriodExtractorChs()
        {
            var regexes = new Dictionary<Regex, PeriodType>
            {
                {
                    new Regex(DateTimeDefinitions.TimePeriodRegexes1, RegexOptions.Singleline),
                    PeriodType.FullTime
                },
                {
                    new Regex(DateTimeDefinitions.TimePeriodRegexes2, RegexOptions.Singleline),
                    PeriodType.ShortTime
                },
                {
                    new Regex(DateTimeDefinitions.TimeOfDayRegex, RegexOptions.Singleline),
                    PeriodType.ShortTime
                },
            };

            Regexes = regexes.ToImmutableDictionary();
        }

        internal sealed override ImmutableDictionary<Regex, PeriodType> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_DATETIME_TIMEPERIOD;
    }
}