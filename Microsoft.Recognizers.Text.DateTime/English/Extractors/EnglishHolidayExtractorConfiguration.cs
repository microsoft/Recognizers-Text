using Microsoft.Recognizers.Resources.English;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishHolidayExtractorConfiguration : IHolidayExtractorConfiguration
    {
        public static readonly Regex YearRegex = new Regex(DateTimeDefinition.YearRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex h1 =
            new Regex(
                DateTimeDefinition.HolidayRegex1,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex h2 =
            new Regex(
                DateTimeDefinition.HolidayRegex2,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex h3 =
            new Regex(
                DateTimeDefinition.HolidayRegex3,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex[] HolidayRegexList =
        {
            h1,
            h2,
            h3
        };

        public IEnumerable<Regex> HolidayRegexes => HolidayRegexList;
    }
}