﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Spanish.Utilities;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Spanish;
using System.Collections.Immutable;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishDateExtractorConfiguration : IDateExtractorConfiguration
    {

        public static readonly Regex MonthRegex =
            new Regex(
                @"(?<month>Abril|Abr|Agosto|Ago|Diciembre|Dic|Febrero|Feb|Enero|Ene|Julio|Jul|Junio|Jun|Marzo|Mar|Mayo|May|Noviembre|Nov|Octubre|Oct|Septiembre|Setiembre|Sept|Set)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DayRegex =
            new Regex(
                @"(?<day>01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|1|20|21|22|23|24|25|26|27|28|29|2|30|31|3|4|5|6|7|8|9)(?=\b|t)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthNumRegex =
            new Regex(@"(?<month>01|02|03|04|05|06|07|08|09|10|11|12|1|2|3|4|5|6|7|8|9)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex YearRegex = new Regex(@"(?<year>19\d{2}|20\d{2}|9\d|0\d|1\d|2\d)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //TODO: please check the expression of WeekDayRegex, I found two regexes with the same function, this one covers more patterns
        public static readonly Regex WeekDayRegex =
            new Regex(
                @"\b(?<weekday>Domingos?|Lunes|Martes|Mi[eé]rcoles|Jueves|Viernes|S[aá]bados?|Lu|Ma|Mi|Ju|Vi|Sa|Do)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex OnRegex = new Regex($@"(?<=\ben\s+)({DayRegex}s?)\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex RelaxedOnRegex =
            new Regex(
                $@"(?<=\b(en|el|del)\s+)((?<day>10|11|12|13|14|15|16|17|18|19|1st|20|21|22|23|24|25|26|27|28|29|2|30|31|3|4|5|6|7|8|9)s?)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex ThisRegex = new Regex($@"\b((este\s*){WeekDayRegex})|({WeekDayRegex}\s*((de\s+)?esta\s+semana))\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex LastDateRegex = new Regex($@"\b(([uú]ltimo)\s*{WeekDayRegex})|({WeekDayRegex}(\s+((de\s+)?(esta|la)\s+([uú]ltima\s+)?semana)))\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //TODO: modify it according to the coresponding English regex
        public static readonly Regex NextDateRegex = new Regex($@"\b(((pr[oó]ximo|siguiente)\s*){WeekDayRegex})|({WeekDayRegex}(\s+(de\s+)?(la\s+)?(pr[oó]xima|siguiente)(\s*semana)))\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SpecialDayRegex =
            new Regex(
                @"\b((el\s+)?(d[ií]a\s+antes\s+de\s+ayer|anteayer)|((el\s+)?d[ií]a\s+(despu[eé]s\s+)?de\s+mañana|pasado\s+mañana)|(el\s)?d[ií]a siguiente|(el\s)?pr[oó]ximo\s+d[ií]a|(el\s+)?[uú]ltimo d[ií]a|(d)?el d[ií]a|ayer|mañana|hoy)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DateUnitRegex =
            new Regex(
                @"(?<unit>anos|ano|meses|mes|semanas|semana|d[íi]as|d[íi]a)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekDayOfMonthRegex =
            new Regex(
                $@"(?<wom>(el\s+)?(?<cardinal>primer|1er|segundo|2do|tercer|3er|cuarto|4to|quinto|5to|[uú]ltimo)\s+{WeekDayRegex
                    }\s+{SpanishDatePeriodExtractorConfiguration.MonthSuffixRegex})", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SpecialDate = new Regex($@"(?<=\b(en)\s+el\s+){DayRegex}\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //TODO: modify below regex according to the counterpart in English
        public static readonly Regex ForTheRegex = new Regex($@"^[.]",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //TODO: modify below regex according to the counterpart in English
        public static readonly Regex WeekDayAndDayOfMothRegex = new Regex($@"^[.]",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex[] DateRegexList =
        {
            // (domingo,)? 5 de Abril
            new Regex(
                $@"\b({WeekDayRegex}(\s+|\s*,\s*))?{DayRegex}?((\s*(de)|[/\\\.\-])\s*)?{MonthRegex}\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (domingo,)? 5 de Abril 5, 2016
            new Regex(
                string.Format(@"\b({3}(\s+|\s*,\s*))?{1}\s*([\.\-]|de)?\s*{0}?(\s*,\s*|\s*(del?)\s*){2}\b", MonthRegex,
                    DayRegex,
                    YearRegex, WeekDayRegex), RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (domingo,)? 6 de Abril
            new Regex(
                string.Format(@"\b({2}(\s+|\s*,\s*))?{0}(\s+|\s*,\s*|\s+de\s+|\s*-\s*){1}((\s+|\s*,\s*){3})?\b",
                    DayRegex, MonthRegex, WeekDayRegex, YearRegex), RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 3-23-2017
            new Regex($@"\b{MonthNumRegex}\s*[/\\\-]\s*{DayRegex}\s*[/\\\-]\s*{YearRegex}",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 23-3-2015
            new Regex(string.Format(@"\b{1}\s*[/\\\-]\s*{0}\s*[/\\\-]\s*{2}", MonthNumRegex, DayRegex, YearRegex),
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // on 1.3
            new Regex($@"(?<=\b(en|el)\s+){MonthNumRegex}[\-\.]{DayRegex}\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 7/23
            new Regex($@"\b{MonthNumRegex}\s*/\s*{DayRegex}((\s+|\s*,\s*|\s+de\s+){YearRegex})?\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // on 24-12
            new Regex(string.Format(@"(?<=\b(en|el)\s+){1}[\\\-]{0}\b", MonthNumRegex, DayRegex),
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 23/7
            new Regex($@"\b{DayRegex}\s*/\s*{MonthNumRegex}((\s+|\s*,\s*|\s+de\s+){YearRegex})?\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 2015-12-23
            new Regex($@"\b{YearRegex}\s*[/\\\-]\s*{MonthNumRegex}\s*[/\\\-]\s*{DayRegex}",
                RegexOptions.IgnoreCase | RegexOptions.Singleline)
        };

        public static readonly Regex[] ImplicitDateList =
        {
            OnRegex, RelaxedOnRegex, SpecialDayRegex, ThisRegex, LastDateRegex, NextDateRegex,
            WeekDayRegex, WeekDayOfMonthRegex, SpecialDate
        };

        public static readonly Regex OfMonth = new Regex($@"^\s*de\s*{SpanishDatePeriodExtractorConfiguration.MonthSuffixRegex}",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthEnd = new Regex(MonthRegex + @"\s*(el)?\s*$",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly ImmutableDictionary<string, int> DayOfWeek = InitDayOfWeek();

        public SpanishDateExtractorConfiguration()
        {
            IntegerExtractor = new IntegerExtractor();
            OrdinalExtractor = new OrdinalExtractor();
            NumberParser = new BaseNumberParser(new SpanishNumberParserConfiguration());
            DurationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration());
            UtilityConfiguration = new SpanishDatetimeUtilityConfiguration();
        }

        public IExtractor IntegerExtractor { get; }

        public IExtractor OrdinalExtractor { get; }

        public IParser NumberParser { get; }

        public IExtractor DurationExtractor { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        IEnumerable<Regex> IDateExtractorConfiguration.DateRegexList => DateRegexList;

        IEnumerable<Regex> IDateExtractorConfiguration.ImplicitDateList => ImplicitDateList;

        IImmutableDictionary<string, int> IDateExtractorConfiguration.DayOfWeek => DayOfWeek;

        Regex IDateExtractorConfiguration.OfMonth => OfMonth;

        Regex IDateExtractorConfiguration.MonthEnd => MonthEnd;

        Regex IDateExtractorConfiguration.DateUnitRegex => DateUnitRegex;

        Regex IDateExtractorConfiguration.ForTheRegex => ForTheRegex;

        Regex IDateExtractorConfiguration.WeekDayAndDayOfMothRegex => WeekDayAndDayOfMothRegex;

        private static ImmutableDictionary<string, int> InitDayOfWeek()
        {
            return new Dictionary<string, int>
            {
                {"lunes", 1},
                {"martes", 2},
                {"miercoles", 3},
                {"miércoles", 3},
                {"jueves", 4},
                {"viernes", 5},
                {"sabado", 6},
                {"domingo", 0},
                {"lu", 1},
                {"ma", 2},
                {"mi", 3},
                {"ju", 4},
                {"vi", 5},
                {"sa", 6},
                {"do", 0}
            }.ToImmutableDictionary();
        }
    }
}
