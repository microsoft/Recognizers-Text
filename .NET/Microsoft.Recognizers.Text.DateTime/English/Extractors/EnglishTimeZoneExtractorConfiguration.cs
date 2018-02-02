using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishTimeZoneExtractorConfiguration : BaseOptionsConfiguration, ITimeZoneExtractorConfiguration
    {
        public static readonly Regex H1 =
            new Regex(
                TimeZoneDefinitions.DirectUTCRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex H2 =
            new Regex(
                TimeZoneDefinitions.AbbrRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex H3 =
            new Regex(
                TimeZoneDefinitions.FullRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex[] TimeZoneRegexList =
        {
            H1,
            H2,
            H3
        };

        public EnglishTimeZoneExtractorConfiguration() : base(DateTimeOptions.None)
        {
        }

        public IEnumerable<Regex> TimeZoneRegexes => TimeZoneRegexList;
    }
}