using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Spanish;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishHolidayExtractorConfiguration : IHolidayExtractorConfiguration
    {
        public static readonly Regex[] HolidayRegexList =
        {
            new Regex(DateTimeDefinitions.HolidayRegex1, RegexOptions.IgnoreCase | RegexOptions.Singleline),
            new Regex(DateTimeDefinitions.HolidayRegex2, RegexOptions.IgnoreCase | RegexOptions.Singleline),
            new Regex(DateTimeDefinitions.HolidayRegex3, RegexOptions.IgnoreCase | RegexOptions.Singleline)
        };

        public IEnumerable<Regex> HolidayRegexes => HolidayRegexList;
    }
}
