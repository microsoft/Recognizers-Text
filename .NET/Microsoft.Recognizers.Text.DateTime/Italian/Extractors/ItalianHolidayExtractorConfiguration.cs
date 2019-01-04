using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Italian;

namespace Microsoft.Recognizers.Text.DateTime.Italian
{
    public class ItalianHolidayExtractorConfiguration : BaseOptionsConfiguration, IHolidayExtractorConfiguration
    {
        public static readonly Regex YearRegex =
            new Regex(
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

        // added to include more options, "fete des meres" mothers day, etc
        // public static readonly Regex H4 =
        //    new Regex(
        //        DateTimeDefinitions.HolidayRegex4,
        //        RegexOptions.Singleline);
        public static readonly Regex[] HolidayRegexList =
        {
            H1,
            H2,
            H3,

            // H4
        };

        public ItalianHolidayExtractorConfiguration(IOptionsConfiguration config)
            : base(config)
        {
        }

        public IEnumerable<Regex> HolidayRegexes => HolidayRegexList;
    }
}
