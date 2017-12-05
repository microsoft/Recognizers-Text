using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class ChineseHolidayExtractorConfiguration : BaseOptionsConfiguration, IHolidayExtractorConfiguration
    {
        public static readonly Regex LunarHolidayRegex = new Regex(DateTimeDefinitions.LunarHolidayRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex[] HolidayRegexList =
        {
            new Regex(DateTimeDefinitions.HolidayRegexList1, RegexOptions.IgnoreCase | RegexOptions.Singleline),
            new Regex(DateTimeDefinitions.HolidayRegexList2, RegexOptions.IgnoreCase | RegexOptions.Singleline),
            LunarHolidayRegex
        };

        public IEnumerable<Regex> HolidayRegexes => HolidayRegexList;

        public ChineseHolidayExtractorConfiguration() : base(DateTimeOptions.None)
        {
        }

    }
}