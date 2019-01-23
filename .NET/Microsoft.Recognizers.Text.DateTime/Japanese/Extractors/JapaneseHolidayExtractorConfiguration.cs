using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Japanese;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseHolidayExtractorConfiguration : BaseOptionsConfiguration, IHolidayExtractorConfiguration
    {
        public static readonly Regex LunarHolidayRegex = new Regex(DateTimeDefinitions.LunarHolidayRegex, RegexOptions.Singleline);

        public static readonly Regex[] HolidayRegexList =
        {
            new Regex(DateTimeDefinitions.HolidayRegexList1, RegexOptions.Singleline),
            new Regex(DateTimeDefinitions.HolidayRegexList2, RegexOptions.Singleline),
            LunarHolidayRegex,
        };

        public JapaneseHolidayExtractorConfiguration()
            : base()
        {
        }

        public IEnumerable<Regex> HolidayRegexes => HolidayRegexList;
    }
}