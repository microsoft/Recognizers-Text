using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Japanese;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseTimeExtractorConfiguration : JapaneseBaseDateTimeExtractorConfiguration<TimeType>
    {
        // e.g: 早上九点
        public static readonly string DayDescRegex = DateTimeDefinitions.TimeDayDescRegex;

        public JapaneseTimeExtractorConfiguration()
        {
            var regexes = new Dictionary<Regex, TimeType>
            {
                {
                    new Regex(DateTimeDefinitions.TimeRegexes1, RegexOptions.Singleline),
                    TimeType.CjkTime
                },
                {
                    new Regex(DateTimeDefinitions.TimeRegexes2, RegexOptions.Singleline),
                    TimeType.DigitTime
                },
                {
                    new Regex(DateTimeDefinitions.TimeRegexes3, RegexOptions.Singleline),
                    TimeType.LessTime
                },
            };
            Regexes = regexes.ToImmutableDictionary();
        }

        internal sealed override ImmutableDictionary<Regex, TimeType> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_DATETIME_TIME; // "Fraction";
    }
}