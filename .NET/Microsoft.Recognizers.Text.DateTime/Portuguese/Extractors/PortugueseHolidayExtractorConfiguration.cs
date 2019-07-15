using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Portuguese;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class PortugueseHolidayExtractorConfiguration : BaseOptionsConfiguration, IHolidayExtractorConfiguration
    {
        public static readonly Regex[] HolidayRegexList =
        {
            new Regex(DateTimeDefinitions.HolidayRegex1, RegexFlags),
            new Regex(DateTimeDefinitions.HolidayRegex2, RegexFlags),
            new Regex(DateTimeDefinitions.HolidayRegex3, RegexFlags),
        };

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public PortugueseHolidayExtractorConfiguration(IOptionsConfiguration config)
            : base(config)
        {
        }

        public IEnumerable<Regex> HolidayRegexes => HolidayRegexList;
    }
}
