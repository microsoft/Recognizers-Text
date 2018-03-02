using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishTimeZoneExtractorConfiguration : BaseOptionsConfiguration, ITimeZoneExtractorConfiguration
    {
        public static readonly Regex DirectUtcRegex =
            new Regex(
                TimeZoneDefinitions.DirectUtcRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AbbreviationRegex =
            new Regex(
                TimeZoneDefinitions.AbbreviationsRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex StandardTimeRegex =
            new Regex(
                TimeZoneDefinitions.FullNameRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex[] TimeZoneRegexList =
        {
            DirectUtcRegex,
            AbbreviationRegex,
            StandardTimeRegex
        };

        public EnglishTimeZoneExtractorConfiguration() : base(DateTimeOptions.None)
        {
        }

        public IEnumerable<Regex> TimeZoneRegexes => TimeZoneRegexList;
    }
}