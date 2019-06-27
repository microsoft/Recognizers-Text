using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Japanese;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseHolidayExtractorConfiguration : BaseOptionsConfiguration, IHolidayExtractorConfiguration
    {
        public static readonly Regex LunarHolidayRegex =
            new Regex(DateTimeDefinitions.LunarHolidayRegex, RegexFlags);

        public static readonly Regex[] HolidayRegexList =
        {
            new Regex(DateTimeDefinitions.HolidayRegexList1, RegexFlags),
            new Regex(DateTimeDefinitions.HolidayRegexList2, RegexFlags),
            LunarHolidayRegex,
        };

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public JapaneseHolidayExtractorConfiguration()
            : base()
        {
        }

        public IEnumerable<Regex> HolidayRegexes => HolidayRegexList;
    }
}