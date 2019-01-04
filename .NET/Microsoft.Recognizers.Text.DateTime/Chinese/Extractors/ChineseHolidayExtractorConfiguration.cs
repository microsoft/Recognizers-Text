using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class ChineseHolidayExtractorConfiguration : BaseOptionsConfiguration, IHolidayExtractorConfiguration
    {
        public static readonly Regex LunarHolidayRegex = new Regex(DateTimeDefinitions.LunarHolidayRegex, RegexOptions.Singleline);

        public static readonly Regex[] HolidayRegexList =
        {
            new Regex(DateTimeDefinitions.HolidayRegexList1, RegexOptions.Singleline),
            new Regex(DateTimeDefinitions.HolidayRegexList2, RegexOptions.Singleline),
            LunarHolidayRegex,
        };

        public ChineseHolidayExtractorConfiguration()
            : base()
        {
        }

        public IEnumerable<Regex> HolidayRegexes => HolidayRegexList;
    }
}