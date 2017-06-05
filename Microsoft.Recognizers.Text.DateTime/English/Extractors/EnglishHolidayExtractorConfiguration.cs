using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishHolidayExtractorConfiguration : IHolidayExtractorConfiguration
    {
        public static readonly Regex YearRegex = new Regex(@"\b(?<year>19\d{2}|20\d{2})\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex h1 =
            new Regex(
                $@"\b(?<holiday>clean monday|good friday|ash wednesday|mardi gras|washington's birthday|mao's birthday|chinese new Year|new years|mayday|yuan dan|april fools|christmas|xmas|thanksgiving|halloween|yuandan)(\s+(of\s+)?({
                    YearRegex}|(?<order>next|last|this)\s+year))?\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex h2 =
            new Regex(
                $@"\b(?<holiday>martin luther king|martin luther king jr|all saint's|tree planting day|white lover|st patrick|st george|cinco de mayo|independence|us independence|all hallow|all souls|guy fawkes)(\s+(of\s+)?({
                    YearRegex}|(?<order>next|last|this)\s+year))?\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex h3 =
            new Regex(
                $@"(?<holiday>(canberra|easter|columbus|thanks\s*giving|labour|mother's|mother|mothers|father's|father|fathers|female|single|teacher's|youth|children|arbor|girls|chsmilbuild|lover|labor|inauguration|groundhog|valentine's|baptiste|bastille|halloween|veterans|memorial|mid(-| )autumn|moon|new year|new years|spring|lantern|qingming|dragon boat)\s+(day))(\s+(of\s+)?({
                    YearRegex}|(?<order>next|last|this)\s+year))?",
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