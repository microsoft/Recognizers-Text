using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.French;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchDateTimeALTExtractorConfiguration : IDateTimeALTExtractorConfiguration
    {
        public FrenchDateTimeALTExtractorConfiguration()
        {
            DateExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration());
            DatePeriodExtractor = new BaseDatePeriodExtractor(new FrenchDatePeriodExtractorConfiguration());
        }

        public IDateTimeExtractor DateExtractor { get; }
        public IDateTimeExtractor DatePeriodExtractor { get; }

        private static readonly Regex OrRegex =
            new Regex(DateTimeDefinitions.OrRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex ThisPrefixRegex =
            new Regex(DateTimeDefinitions.ThisPrefixRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AmRegex =
            new Regex(DateTimeDefinitions.AmRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PmRegex =
            new Regex(DateTimeDefinitions.PmRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex[] RelativePrefixList =
        {
            ThisPrefixRegex
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