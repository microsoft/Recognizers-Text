using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Japanese;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseTimePeriodExtractorConfiguration : JapaneseBaseDateTimeExtractorConfiguration<PeriodType>
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public JapaneseTimePeriodExtractorConfiguration()
        {
            var regexes = new Dictionary<Regex, PeriodType>
            {
                {
                    RegexCache.Get(DateTimeDefinitions.TimePeriodRegexes1, RegexFlags),
                    PeriodType.FullTime
                },
                {
                    RegexCache.Get(DateTimeDefinitions.TimePeriodRegexes2, RegexFlags),
                    PeriodType.ShortTime
                },
                {
                    RegexCache.Get(DateTimeDefinitions.TimeOfDayRegex, RegexFlags),
                    PeriodType.ShortTime
                },
            };

            Regexes = regexes.ToImmutableDictionary();
        }

        internal sealed override ImmutableDictionary<Regex, PeriodType> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_DATETIME_TIMEPERIOD;
    }
}