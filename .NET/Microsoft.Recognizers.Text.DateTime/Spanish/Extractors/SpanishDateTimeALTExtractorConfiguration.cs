using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Spanish;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishDateTimeALTExtractorConfiguration : IDateTimeALTExtractorConfiguration
    {
        public SpanishDateTimeALTExtractorConfiguration()
        {
            DateExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration());
            DatePeriodExtractor = new BaseDatePeriodExtractor(new SpanishDatePeriodExtractorConfiguration());
        }

        public IDateTimeExtractor DateExtractor { get; }
        public IDateTimeExtractor DatePeriodExtractor { get; }

        private static readonly Regex OrRegex =
            new Regex(DateTimeDefinitions.OrRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex ThisPrefixRegex =
            new Regex(DateTimeDefinitions.ThisPrefixRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PastPrefixRegex =
            new Regex(DateTimeDefinitions.PastPrefixRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NextPrefixRegex =
            new Regex(DateTimeDefinitions.NextPrefixRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AmRegex =
            new Regex(DateTimeDefinitions.AmRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PmRegex =
            new Regex(DateTimeDefinitions.PmRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex[] RelativePrefixList =
        {
            ThisPrefixRegex, PastPrefixRegex, NextPrefixRegex,
        };

        public static readonly Regex[] AmPmRegexList =
        {
            AmRegex, PmRegex,
        };

        IEnumerable<Regex> IDateTimeALTExtractorConfiguration.RelativePrefixList => RelativePrefixList;

        IEnumerable<Regex> IDateTimeALTExtractorConfiguration.AmPmRegexList => AmPmRegexList;

        Regex IDateTimeALTExtractorConfiguration.OrRegex => OrRegex;
    }
}