using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.French.Utilities;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.French;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchDateExtractorConfiguration : IDateExtractorConfiguration
    {
        public static readonly Regex MonthRegex =
            new Regex(
                @"(?<month>Avril|Avr|Ao[uû]t|D[ée]cembre|D[ée]c|F[ée]vrier|F[ée]v|Janvier|Jan|Juillet|Juin|Jun|Mars|Mai|Novembre|Nov|Octobre|Oct|Septembre|Sept|Sep)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DayRegex =
            new Regex(
                @"(?<day>01|02|03|04|05|06|07|08|09|10e|10|11e|11e|11|12e|12th|12|13rd|13th|13|14th|14|15th|15|16th|16|17th|17|18th|18|19th|19|1st|1|20th|20|21st|21|22nd|22|23rd|23|24th|24|25th|25|26th|26|27th|27|28th|28|29th|29|2nd|2|30th|30|31st|31|3rd|3|4th|4|5th|5|6th|6|7th|7|8th|8|9th|9)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthNumRegex =
            new Regex(@"(?<month>01|02|03|04|05|06|07|08|09|10|11|12|1|2|3|4|5|6|7|8|9)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex YearRegex = new Regex(@"(?<year>19\d{2}|20\d{2}|9\d|0\d|1\d|2\d)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekDayRegex =
            new Regex(
                @"(?<weekday>Dimanche|Lundi|Mardi|Mecredi|Jeudi|Vendredi|Samedi|Dim|Lun|Mar|Mer|Jeu|Ven|Sam)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex OnRegex = new Regex($@"(?<=\bon\s+)({DayRegex}s?)\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex RelaxedOnRegex =
            new Regex(
                $@"(?<=\b(on|at|in)\s+)((?<day>10[ée]me|11[ée]me|11e|12[ée]me|12e|13[ée]me|13e|14[ée]me|14e|15[ée]me|15e|16[ée]me|16e|17[ée]me|17e|18[ée]me|18e|19[ée]me|19e|1er|20[ée]me|20e|21[ée]me|21e|22[ée]me|22e|23[ée]me|23e|24[ée]me|24e|25[ée]me|25e|26[ée]me|26e|27[ée]me|27e|28[ée]me|28e|29[ée]me|29e|2[ée]me|30[ée]me|30e|31[ée]me|31e|3[ée]me|4e|5[ée]me|6[ée]me|7[ée]me|8[ée]meh|9[ée]me)s?)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex ThisRegex = new Regex($@"\b((this(\s*week)?\s+){WeekDayRegex})|({WeekDayRegex}(\s+this\s*semaine))\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex LastRegex = new Regex($@"\b(last(\s*week)?\s+{WeekDayRegex})|({WeekDayRegex}(\s+last\s*semaine))\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NextRegex = new Regex($@"\b(next(\s*week)?\s+{WeekDayRegex})|({WeekDayRegex}(\s+next\s*semaine))\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex UnitRegex = new Regex(@"(?<unit>ann[eé]es|ann[eé]e|mois|semaines|semaine|journ[eé]s|journ[eé]e)\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // day before yesterday, day after tomorrow, next day, last day, the day yesterday, the day tomorrow
        public static readonly Regex SpecialDayRegex =
            new Regex(
                @"\b((le\s+)?avant hier|(le\s+)?apr[èe]s (demain|-demain)|(le\s)? lendemain|(le\s)? jour d'apr[èe]s|(le\s+)?dernier jour|le jour|hier|demain|aujourd'hui)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DateUnitRegex =
            new Regex(
                @"(?<unit>ann[eé]es|ann[eé]e|mois|semaines|semaine|journ[eé]es|journ[eé]e)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex StrictWeekDay =
            new Regex(
                @"\b(?<weekday>Dimanche|Lundi|Mardi|Mecredi|Jeudi|Vendredi|Samedi|Dim|Lun|Mar|Mer|Jeu|Ven|Sam)s?\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekDayOfMonthRegex =
            new Regex(
                $@"(?<wom>(the\s+)?(?<cardinal>premier|1er|seconde|2[ée]me|troisi[ée]me|3[ée]me|quatri[ée]me|4[ée]me|cinqui[ée]me|5[ée]me|dernier)\s+{WeekDayRegex
                    }\s+{FrenchDatePeriodExtractorConfiguration.MonthSuffixRegex})", RegexOptions.IgnoreCase | RegexOptions.Singleline);

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

        public static readonly Regex NonDateUnitRegex = new Regex(@"(?<unit>heure|heures|hrs|secondes|seconde|secs|sec|minutes|minute|mins)\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public FrenchDateExtractorConfiguration()
        {
            IntegerExtractor = new IntegerExtractor();
            OrdinalExtractor = new OrdinalExtractor();
            NumberParser = new BaseNumberParser(new FrenchNumberParserConfiguration());
            DurationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration());
            UtilityConfiguration = new FrenchDatetimeUtilityConfiguration();
        }

        public IExtractor IntegerExtractor { get; }

        public IExtractor OrdinalExtractor { get; }

        public IParser NumberParser { get; }

        public IExtractor DurationExtractor { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        IEnumerable<Regex> IDateExtractorConfiguration.DateRegexList => DateRegexList;

        IEnumerable<Regex> IDateExtractorConfiguration.ImplicitDateList => ImplicitDateList;

        Regex IDateExtractorConfiguration.OfMonth => OfMonth;

        Regex IDateExtractorConfiguration.MonthEnd => MonthEnd;

        Regex IDateExtractorConfiguration.DateUnitRegex => DateUnitRegex;
    }
}
