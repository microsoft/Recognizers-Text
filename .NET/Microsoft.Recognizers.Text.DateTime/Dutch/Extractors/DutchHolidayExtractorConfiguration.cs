using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Dutch;

namespace Microsoft.Recognizers.Text.DateTime.Dutch
{
    public class DutchHolidayExtractorConfiguration : BaseOptionsConfiguration, IHolidayExtractorConfiguration
    {
        public static readonly Regex YearRegex =
            new Regex(DateTimeDefinitions.YearRegex, RegexFlags);

        public static readonly Regex H1 =
            new Regex(DateTimeDefinitions.HolidayRegex1, RegexFlags);

        public static readonly Regex H2 =
            new Regex(DateTimeDefinitions.HolidayRegex2, RegexFlags);

        public static readonly Regex H3 =
            new Regex(DateTimeDefinitions.HolidayRegex3, RegexFlags);

        public static readonly Regex[] HolidayRegexList =
        {
            H1,
            H2,
            H3,
        };

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public DutchHolidayExtractorConfiguration(IOptionsConfiguration config)
            : base(config)
        {
        }

        public IEnumerable<Regex> HolidayRegexes => HolidayRegexList;
    }
}