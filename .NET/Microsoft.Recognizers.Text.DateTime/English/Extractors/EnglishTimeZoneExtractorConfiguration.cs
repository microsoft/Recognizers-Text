using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.English;
using Microsoft.Recognizers.Text.Matcher;

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

        public EnglishTimeZoneExtractorConfiguration(DateTimeOptions options = DateTimeOptions.None) : base(options)
        {
            if ((options & DateTimeOptions.EnablePreview) != 0)
            {
                CityMatcher.Init(TimeZoneDefinitions.MajorCities.Select(o => o.ToLowerInvariant()));
            }
        }

        public IEnumerable<Regex> TimeZoneRegexes => TimeZoneRegexList;

        public Regex CityTimeSuffixRegex { get; } = new Regex(TimeZoneDefinitions.CityTimeSuffixRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public StringMatcher CityMatcher { get; } = new StringMatcher();
    }
}