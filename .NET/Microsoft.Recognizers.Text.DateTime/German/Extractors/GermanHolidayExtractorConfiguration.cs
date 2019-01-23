using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.German;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanHolidayExtractorConfiguration : BaseOptionsConfiguration, IHolidayExtractorConfiguration
    {
        public static readonly Regex YearRegex = new Regex(
            DateTimeDefinitions.YearRegex,
            RegexOptions.Singleline);

        public static readonly Regex H1 =
            new Regex(
                DateTimeDefinitions.HolidayRegex1,
                RegexOptions.Singleline);

        public static readonly Regex H2 =
            new Regex(
                DateTimeDefinitions.HolidayRegex2,
                RegexOptions.Singleline);

        public static readonly Regex H3 =
            new Regex(
                DateTimeDefinitions.HolidayRegex3,
                RegexOptions.Singleline);

        public static readonly Regex[] HolidayRegexList =
        {
            H1,
            H2,
            H3,
        };

        public GermanHolidayExtractorConfiguration(IOptionsConfiguration config)
            : base(config)
        {
        }

        public IEnumerable<Regex> HolidayRegexes => HolidayRegexList;
    }
}