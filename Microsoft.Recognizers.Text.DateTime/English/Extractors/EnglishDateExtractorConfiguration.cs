using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.English;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishDateExtractorConfiguration : IDateExtractorConfiguration
    {
        public static readonly Regex MonthRegex =
            new Regex(
                @"(?<month>April|Apr|August|Aug|December|Dec|February|Feb|January|Jan|July|Jul|June|Jun|March|Mar|May|November|Nov|October|Oct|September|Sept|Sep)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DayRegex =
            new Regex(
                @"(?<day>01|02|03|04|05|06|07|08|09|10th|10|11th|11st|11|12nd|12th|12|13rd|13th|13|14th|14|15th|15|16th|16|17th|17|18th|18|19th|19|1st|1|20th|20|21st|21|22nd|22|23rd|23|24th|24|25th|25|26th|26|27th|27|28th|28|29th|29|2nd|2|30th|30|31st|31|3rd|3|4th|4|5th|5|6th|6|7th|7|8th|8|9th|9)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthNumRegex =
            new Regex(@"(?<month>01|02|03|04|05|06|07|08|09|10|11|12|1|2|3|4|5|6|7|8|9)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex YearRegex = new Regex(@"(?<year>19\d{2}|20\d{2}|9\d|0\d|1\d|2\d)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekDayRegex =
            new Regex(
                @"(?<weekday>Sunday|Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Mon|Tue|Wedn|Weds|Wed|Thurs|Thur|Thu|Fri|Sat|Sun)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex OnRegex = new Regex($@"(?<=\bon\s+)({DayRegex}s?)\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex RelaxedOnRegex =
            new Regex(
                $@"(?<=\b(on|at|in)\s+)((?<day>10th|11th|11st|12nd|12th|13rd|13th|14th|15th|16th|17th|18th|19th|1st|20th|21st|22nd|23rd|24th|25th|26th|27th|28th|29th|2nd|30th|31st|3rd|4th|5th|6th|7th|8th|9th)s?)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex ThisRegex = new Regex($@"\b((this(\s*week)?\s+){WeekDayRegex})|({WeekDayRegex}(\s+this\s*week))\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex LastRegex = new Regex($@"\b(last(\s*week)?\s+{WeekDayRegex})|({WeekDayRegex}(\s+last\s*week))\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NextRegex = new Regex($@"\b(next(\s*week)?\s+{WeekDayRegex})|({WeekDayRegex}(\s+next\s*week))\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SpecialDayRegex =
            new Regex(
                @"\b((the\s+)?day before yesterday|(the\s+)?day after (tomorrow|tmr)|(the\s)?next day|(the\s+)?last day|the day|yesterday|tomorrow|tmr|today)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex StrictWeekDay =
            new Regex(
                @"\b(?<weekday>Sunday|Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Mon|Tue|Wedn|Weds|Wed|Thurs|Thur|Fri|Sat)s?\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekDayOfMonthRegex =
            new Regex(
                $@"(?<wom>(the\s+)?(?<cardinal>first|1st|second|2nd|third|3rd|fourth|4th|fifth|5th|last)\s+{WeekDayRegex
                    }\s+{EnglishDatePeriodExtractorConfiguration.MonthSuffixRegex})", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SpecialDate = new Regex($@"(?<=\b(on|at)\s+the\s+){DayRegex}\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex[] DateRegexList =
        {
            // (Sunday,)? April 5
            new Regex(
                string.Format(@"\b({2}(\s+|\s*,\s*))?{0}\s*[/\\\.\-]?\s*{1}\b", MonthRegex, DayRegex, WeekDayRegex),
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (Sunday,)? April 5, 2016
            new Regex(
                string.Format(@"\b({3}(\s+|\s*,\s*))?{0}\s*[\.\-]?\s*{1}(\s+|\s*,\s*|\s+of\s+){2}\b", MonthRegex,
                    DayRegex,
                    YearRegex, WeekDayRegex), RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (Sunday,)? 6th of April
            new Regex(
                string.Format(@"\b({2}(\s+|\s*,\s*))?{0}(\s+|\s*,\s*|\s+of\s+|\s*-\s*){1}((\s+|\s*,\s*){3})?\b",
                    DayRegex, MonthRegex, WeekDayRegex, YearRegex), RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 3-23-2017
            new Regex($@"\b{MonthNumRegex}\s*[/\\\-]\s*{DayRegex}\s*[/\\\-]\s*{YearRegex}",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 23-3-2015
            new Regex(string.Format(@"\b{1}\s*[/\\\-]\s*{0}\s*[/\\\-]\s*{2}", MonthNumRegex, DayRegex, YearRegex),
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // on 1.3
            new Regex($@"(?<=\b(on|in|at)\s+){MonthNumRegex}[\-\.]{DayRegex}\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 7/23
            new Regex($@"\b{MonthNumRegex}\s*/\s*{DayRegex}((\s+|\s*,\s*|\s+of\s+){YearRegex})?\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // on 24-12
            new Regex(string.Format(@"(?<=\b(on|in|at)\s+){1}[\\\-]{0}\b", MonthNumRegex, DayRegex),
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 23/7
            new Regex($@"\b{DayRegex}\s*/\s*{MonthNumRegex}((\s+|\s*,\s*|\s+of\s+){YearRegex})?\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 2015-12-23
            new Regex($@"\b{YearRegex}\s*[/\\\-]\s*{MonthNumRegex}\s*[/\\\-]\s*{DayRegex}",
                RegexOptions.IgnoreCase | RegexOptions.Singleline)
        };


        public static readonly Regex[] ImplicitDateList =
        {
            OnRegex, RelaxedOnRegex, SpecialDayRegex, ThisRegex, LastRegex, NextRegex,
            StrictWeekDay, WeekDayOfMonthRegex, SpecialDate
        };

        public static readonly Regex OfMonth = new Regex(@"^\s*of\s*" + MonthRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthEnd = new Regex(MonthRegex + @"\s*(the)?\s*$",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public EnglishDateExtractorConfiguration()
        {
            IntegerExtractor = new IntegerExtractor();
            OrdinalExtractor = new OrdinalExtractor();
            NumberParser = new BaseNumberParser(new EnglishNumberParserConfiguration());
        }

        public IExtractor IntegerExtractor { get; }

        public IExtractor OrdinalExtractor { get; }

        public IParser NumberParser { get; }

        IEnumerable<Regex> IDateExtractorConfiguration.DateRegexList => DateRegexList;

        IEnumerable<Regex> IDateExtractorConfiguration.ImplicitDateList => ImplicitDateList;

        Regex IDateExtractorConfiguration.OfMonth => OfMonth;

        Regex IDateExtractorConfiguration.MonthEnd => MonthEnd;
    }
}