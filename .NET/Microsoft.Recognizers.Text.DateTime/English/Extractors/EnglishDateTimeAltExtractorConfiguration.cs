using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishDateTimeAltExtractorConfiguration : BaseDateTimeOptionsConfiguration, IDateTimeAltExtractorConfiguration
    {
        public static readonly Regex ThisPrefixRegex =
            RegexCache.Get(DateTimeDefinitions.ThisPrefixRegex, RegexFlags);

        public static readonly Regex PreviousPrefixRegex =
            RegexCache.Get(DateTimeDefinitions.PreviousPrefixRegex, RegexFlags);

        public static readonly Regex NextPrefixRegex =
            RegexCache.Get(DateTimeDefinitions.NextPrefixRegex, RegexFlags);

        public static readonly Regex AmRegex =
            RegexCache.Get(DateTimeDefinitions.AmRegex, RegexFlags);

        public static readonly Regex PmRegex =
            RegexCache.Get(DateTimeDefinitions.PmRegex, RegexFlags);

        public static readonly Regex RangePrefixRegex =
            RegexCache.Get(DateTimeDefinitions.RangePrefixRegex, RegexFlags);

        public static readonly Regex[] RelativePrefixList =
        {
            ThisPrefixRegex, PreviousPrefixRegex, NextPrefixRegex,
        };

        public static readonly Regex[] AmPmRegexList =
        {
            AmRegex, PmRegex,
        };

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex OrRegex =
            RegexCache.Get(DateTimeDefinitions.OrRegex, RegexFlags);

        private static readonly Regex DayRegex =
            RegexCache.Get(DateTimeDefinitions.DayRegex, RegexFlags);

        public EnglishDateTimeAltExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            DateExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new EnglishDatePeriodExtractorConfiguration(this));
        }

        IEnumerable<Regex> IDateTimeAltExtractorConfiguration.RelativePrefixList => RelativePrefixList;

        IEnumerable<Regex> IDateTimeAltExtractorConfiguration.AmPmRegexList => AmPmRegexList;

        Regex IDateTimeAltExtractorConfiguration.OrRegex => OrRegex;

        Regex IDateTimeAltExtractorConfiguration.ThisPrefixRegex => ThisPrefixRegex;

        Regex IDateTimeAltExtractorConfiguration.DayRegex => DayRegex;

        Regex IDateTimeAltExtractorConfiguration.RangePrefixRegex => RangePrefixRegex;

        public IDateExtractor DateExtractor { get; }

        public IDateTimeExtractor DatePeriodExtractor { get; }
    }
}