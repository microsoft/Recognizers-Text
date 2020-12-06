using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.French;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchHolidayExtractorConfiguration : BaseDateTimeOptionsConfiguration, IHolidayExtractorConfiguration
    {
        public static readonly Regex YearRegex =
            RegexCache.Get(DateTimeDefinitions.YearRegex, RegexFlags);

        public static readonly Regex H1 =
            RegexCache.Get(DateTimeDefinitions.HolidayRegex1, RegexFlags);

        public static readonly Regex H2 =
            RegexCache.Get(DateTimeDefinitions.HolidayRegex2, RegexFlags);

        public static readonly Regex H3 =
            RegexCache.Get(DateTimeDefinitions.HolidayRegex3, RegexFlags);

        // added to include more options, "fete des meres" mothers day, etc
        public static readonly Regex H4 =
            RegexCache.Get(DateTimeDefinitions.HolidayRegex4, RegexFlags);

        public static readonly Regex[] HolidayRegexList =
        {
            H1,
            H2,
            H3,
            H4,
        };

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public FrenchHolidayExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
        }

        public IEnumerable<Regex> HolidayRegexes => HolidayRegexList;
    }
}
