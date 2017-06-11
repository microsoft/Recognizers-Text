using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Number.English;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishDatePeriodExtractorConfiguration : IDatePeriodExtractorConfiguration
    {
        // base regexes
        public static readonly Regex TillRegex = new Regex(@"(?<till>to|till|until|thru|through|--|-|—|——)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AndRegex = new Regex(@"(?<and>and|--|-|—|——)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DayRegex =
            new Regex(
                @"(?<day>01|02|03|04|05|06|07|08|09|10th|10|11th|11st|11|12nd|12th|12|13rd|13th|13|14th|14|15th|15|16th|16|17th|17|18th|18|19th|19|1st|1|20th|20|21st|21|22nd|22|23rd|23|24th|24|25th|25|26th|26|27th|27|28th|28|29th|29|2nd|2|30th|30|31st|31|3rd|3|4th|4|5th|5|6th|6|7th|7|8th|8|9th|9)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthNumRegex =
            new Regex(@"(?<month>01|02|03|04|05|06|07|08|09|10|11|12|1|2|3|4|5|6|7|8|9)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex YearRegex = new Regex(@"\b(?<year>19\d{2}|20\d{2})\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekDayRegex =
            new Regex(
                @"(?<weekday>Sunday|Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Mon|Tue|Wedn|Weds|Wed|Thurs|Thur|Thu|Fri|Sat|Sun)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex RelativeMonthRegex = new Regex(@"(?<relmonth>(this|next|last)\s+month)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex EngMonthRegex =
            new Regex(
                @"(?<month>April|Apr|August|Aug|December|Dec|February|Feb|January|Jan|July|Jul|June|Jun|March|Mar|May|November|Nov|October|Oct|September|Sept|Sep)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthSuffixRegex =
            new Regex($@"(?<msuf>(in\s+|of\s+|on\s+)?({RelativeMonthRegex}|{EngMonthRegex}))",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex UnitRegex = new Regex(@"(?<unit>years|year|months|month|weeks|week|days|day)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PastRegex = new Regex(@"(?<past>\b(past|last|previous)\b)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex FutureRegex = new Regex(@"(?<past>\b(next|in)\b)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // composite regexes
        public static readonly Regex SimpleCasesRegex =
            new Regex(
                string.Format(@"\b(from\s+)?({0})\s*{1}\s*({0})\s+{2}((\s+|\s*,\s*){3})?\b", DayRegex, TillRegex,
                    MonthSuffixRegex, YearRegex),
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthFrontSimpleCasesRegex =
            new Regex(
                string.Format(@"\b{2}\s+(from\s+)?({0})\s*{1}\s*({0})((\s+|\s*,\s*){3})?\b", DayRegex, TillRegex,
                    MonthSuffixRegex, YearRegex),
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthFrontBetweenRegex =
            new Regex(
                string.Format(@"\b{2}\s+(between\s+)({0})\s*{1}\s*({0})((\s+|\s*,\s*){3})?\b", DayRegex, AndRegex,
                    MonthSuffixRegex, YearRegex),
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex BetweenRegex =
            new Regex(
                string.Format(@"\b(between\s+)({0})\s*{1}\s*({0})\s+{2}((\s+|\s*,\s*){3})?\b", DayRegex, AndRegex,
                    MonthSuffixRegex, YearRegex),
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthWithYear =
            new Regex(
                $@"\b((?<month>April|Apr|August|Aug|December|Dec|February|Feb|January|Jan|July|Jul|June|Jun|March|Mar|May|November|Nov|October|Oct|September|Sep|Sept),?(\s+of)?\s+({YearRegex}|(?<order>next|last|this)\s+year))",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex OneWordPeriodRegex =
            new Regex(
                @"\b(((next|this|last)\s+)?(?<month>April|Apr|August|Aug|December|Dec|February|Feb|January|Jan|July|Jul|June|Jun|March|Mar|May|November|Nov|October|Oct|September|Sep|Sept)|(next|last|this)\s+(weekend|week|month|year)|weekend|(month|year) to date)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
        
        public static readonly Regex MonthNumWithYear =
            new Regex($@"({YearRegex}[/\-\.]{MonthNumRegex})|({MonthNumRegex}[/\-]{YearRegex})",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);


        public static readonly Regex WeekOfMonthRegex =
            new Regex(
                $@"(?<wom>(the\s+)?(?<cardinal>first|1st|second|2nd|third|3rd|fourth|4th|fifth|5th|last)\s+week\s+{
                    MonthSuffixRegex})", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekOfYearRegex =
            new Regex(
                $@"(?<woy>(the\s+)?(?<cardinal>first|1st|second|2nd|third|3rd|fourth|4th|fifth|5th|last)\s+week(\s+of)?\s+({
                    YearRegex}|(?<order>next|last|this)\s+year))", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex FollowedUnit = new Regex($@"^\s*{UnitRegex}\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NumberCombinedWithUnit =
            new Regex($@"\b(?<num>\d+(\.\d*)?){UnitRegex}\b", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex QuarterRegex =
            new Regex(
                $@"(the\s+)?(?<cardinal>first|1st|second|2nd|third|3rd|fourth|4th)\s+quarter(\s+of|\s*,\s*)?\s+({
                    YearRegex}|(?<order>next|last|this)\s+year)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex QuarterRegexYearFront =
            new Regex(
                $@"({YearRegex
                    }|(?<order>next|last|this)\s+year)\s+(the\s+)?(?<cardinal>first|1st|second|2nd|third|3rd|fourth|4th)\s+quarter",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SeasonRegex =
            new Regex(
                $@"\b(?<season>((last|this|the|next)\s+)?(?<seas>spring|summer|fall|autumn|winter)((\s+of|\s*,\s*)?\s+({
                    YearRegex}|(?<order>next|last|this)\s+year))?)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex[] SimpleCasesRegexes =
        {
            SimpleCasesRegex,
            BetweenRegex,
            OneWordPeriodRegex,
            MonthWithYear,
            MonthNumWithYear,
            YearRegex,
            WeekOfMonthRegex,
            WeekOfYearRegex,
            MonthFrontBetweenRegex,
            MonthFrontSimpleCasesRegex,
            QuarterRegex,
            QuarterRegexYearFront,
            SeasonRegex
        };

        public EnglishDatePeriodExtractorConfiguration()
        {
            DatePointExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
            CardinalExtractor = new CardinalExtractor();
        }

        public IExtractor DatePointExtractor { get; }

        public IExtractor CardinalExtractor { get; }

        IEnumerable<Regex> IDatePeriodExtractorConfiguration.SimpleCasesRegexes => SimpleCasesRegexes;

        Regex IDatePeriodExtractorConfiguration.TillRegex => TillRegex;

        Regex IDatePeriodExtractorConfiguration.FollowedUnit => FollowedUnit;

        Regex IDatePeriodExtractorConfiguration.NumberCombinedWithUnit => NumberCombinedWithUnit;

        Regex IDatePeriodExtractorConfiguration.PastRegex => PastRegex;

        Regex IDatePeriodExtractorConfiguration.FutureRegex => FutureRegex;

        public bool GetFromTokenIndex(string text, out int index)
        {
            index = -1;
            if (text.EndsWith("from"))
            {
                index = text.LastIndexOf("from");
                return true;
            }
            return false;
        }

        public bool GetBetweenTokenIndex(string text, out int index)
        {
            index = -1;
            if (text.EndsWith("between"))
            {
                index = text.LastIndexOf("between");
                return true;
            }
            return false ;
        }

        public bool HasConnectorToken(string text)
        {
            return text.Equals("and");
        }        
    }
}