using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.English;
using Microsoft.Recognizers.Text.Matcher;
using Microsoft.Recognizers.Text.Utilities;

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

        public static readonly Regex CityTimeSuffixRegex = new Regex(TimeZoneDefinitions.CityTimeSuffixRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly StringMatcher CityMatcher = new StringMatcher();

        public static readonly List<string> AmbiguousTimezoneList = TimeZoneDefinitions.AmbiguousTimezoneList.ToList();

        public EnglishTimeZoneExtractorConfiguration(DateTimeOptions options = DateTimeOptions.None) : base(options)
        {
            if ((options & DateTimeOptions.EnablePreview) != 0)
            {
                CityMatcher.Init(TimeZoneDefinitions.MajorCities.Select(o => FormatUtility.RemoveDiacritics(o.ToLowerInvariant())));
            }
        }

        IEnumerable<Regex> ITimeZoneExtractorConfiguration.TimeZoneRegexes => TimeZoneRegexList;

        Regex ITimeZoneExtractorConfiguration.CityTimeSuffixRegex => CityTimeSuffixRegex;

        StringMatcher ITimeZoneExtractorConfiguration.CityMatcher => CityMatcher;

        List<string> ITimeZoneExtractorConfiguration.AmbiguousTimezoneList => AmbiguousTimezoneList;
    }
}