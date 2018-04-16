using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.French;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchDateTimeAltExtractorConfiguration : IDateTimeAltExtractorConfiguration
    {
        public FrenchDateTimeAltExtractorConfiguration()
        {
            DateExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration());
            DatePeriodExtractor = new BaseDatePeriodExtractor(new FrenchDatePeriodExtractorConfiguration());
        }

        public IDateTimeExtractor DateExtractor { get; }
        public IDateTimeExtractor DatePeriodExtractor { get; }

        private static readonly Regex OrRegex =
            new Regex(DateTimeDefinitions.OrRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex DayRegex =
            new Regex(DateTimeDefinitions.DayRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

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

        IEnumerable<Regex> IDateTimeAltExtractorConfiguration.RelativePrefixList => RelativePrefixList;

        IEnumerable<Regex> IDateTimeAltExtractorConfiguration.AmPmRegexList => AmPmRegexList;

        Regex IDateTimeAltExtractorConfiguration.OrRegex => OrRegex;

        Regex IDateTimeAltExtractorConfiguration.DayRegex => DayRegex;
    }
}