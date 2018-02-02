using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishTimeZoneExtractorConfiguration : BaseOptionsConfiguration, ITimeZoneExtractorConfiguration
    {
        public static readonly Regex DirectUtc =
            new Regex(
                TimeZoneDefinitions.DirectUTCRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex Abbr =
            new Regex(
                TimeZoneDefinitions.AbbrRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex FullName =
            new Regex(
                TimeZoneDefinitions.FullRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex[] TimeZoneRegexList =
        {
            DirectUtc,
            Abbr,
            FullName
        };

        public EnglishTimeZoneExtractorConfiguration() : base(DateTimeOptions.None)
        {
        }

        public IEnumerable<Regex> TimeZoneRegexes => TimeZoneRegexList;
    }
}