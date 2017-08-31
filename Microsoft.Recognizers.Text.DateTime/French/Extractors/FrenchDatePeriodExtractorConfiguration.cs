using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Number.French;


namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchDatePeriodExtractorConfiguration : IDatePeriodExtractorConfiguration
    {
        // base regexes
        public static readonly Regex TillRegex = new Regex(@"(?<till>jusqu'[aà]|avant|--|-|—|——)", // until 
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AndRegex = new Regex(@"(?<and>et||et\s*la|--|-|—|——)", // and 
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DayRegex =
            new Regex(
                @"(?<day>01|02|03|04|05|06|07|08|09|10th|10|11|12|13|14|15|16|17|18|19|1er|20|21|22|23|24|25|26|27|28|29|2|30|31|3|4|5|6|7|8|9)",
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

        public static readonly Regex RelativeMonthRegex = new Regex(@"(?<relmonth>(ce\s+mois-ci)|(le\s+mois)\s+(prochain|dernier)", // this month, next month, last month
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex EngMonthRegex =
            new Regex(
                @"(?<month>Avril|Avr|Août|Decembre|Déc.|Février|Fév|Janvier|Janv|Juillet|Juil|Juin|Mars|Mai|Novembere|Nov.|Octobre|Oct.|Septembre|Sept.)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthSuffixRegex =
            new Regex($@"(?<msuf>(dans\s+|de\s+)?({RelativeMonthRegex}|{EngMonthRegex}))", // in, of, no "on"...
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DateUnitRegex = new Regex(@"(?<unit>ann[eé]es|ann[eé]e|mois|semaines|la\s+semaine|journ[eé]es|la\s+journ[eé]e|jour)\b", // year, month, week, day
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PastRegex = new Regex(@"(?<past>\b(pass[eé]|dernier|pr[eé]c[eé]dente)\b)", // past, last, previous
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex FutureRegex = new Regex(@"(?<past>\b(prochain|dans)\b)", // next, in
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // composite regexes
        public static readonly Regex SimpleCasesRegex =
            new Regex(
                $@"\b((de\s+le|du|des)\s+)?({DayRegex})\s*{TillRegex}\s*({DayRegex})\s+{MonthSuffixRegex}((\s+|\s*,\s*){YearRegex})?\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthFrontSimpleCasesRegex = 
            new Regex(
                $@"\b{MonthSuffixRegex}\s+((entre|de)\s+)?({DayRegex})\s*{TillRegex}\s*({DayRegex})((\s+|\s*,\s*){YearRegex})?\b", // between 'x' until 'y', from 'x' until 'y'
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthFrontBetweenRegex =
            new Regex(
                string.Format(@"\b{2}\s+((entre|entre\s+le)\s+)({0})\s*{1}\s*({0})((\s+|\s*,\s*){3})?\b", DayRegex, AndRegex,
                    MonthSuffixRegex, YearRegex),
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex BetweenRegex =
            new Regex(
                string.Format(@"\b((entre|entre\s+le)\s+)({0})\s*{1}\s*({0})\s+{2}((\s+|\s*,\s*){3})?\b", DayRegex, AndRegex,
                    MonthSuffixRegex, YearRegex),
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthWithYear =
            new Regex(
                $@"\b((?<month>Avril|Avr.|Ao[uû]t|Decembre|Dec.|Fevrier|Fev.|Janvier|Jan.|Juillet|Juil.|Juin|Mars|Mai|Novembre|Nov.|Octobre|Oct.|Septembre|Sept.),?(\s+de)?\s+({YearRegex}|(?<order>prochain|dernier|cette)\s+ann[eé]e))",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex OneWordPeriodRegex =
            new Regex(
                @"\b((([aà]\s+c[oô]t[eé]\s+de?|cette|derni[eè]re?|en)\s+)?(?<month>Avril|Avr.|Ao[uû]t|Decembre|Dec.|Fevrier|Fev.|Janvier|Jan.|Juillet|Juil.|Juin|Mars|Mai|Novembre|Nov.|Octobre|Oct.|Septembre|Sept.)|(?<=\b(de|le)\s+)?([aà]\s+c[oô]t[eé]\s+de\s?|derni[èe]re?|est(et|[àa]))?\s+(fin de semanaine|semaine|mes|año)|fin de la semaine|(mois|ann[ée]es)? [àa] la date)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline); // a cote de - 'next to', cette - 'this', dernier - 'last' (always after the noun, i.e annee dernier - 'last year'  

        public static readonly Regex MonthNumWithYear =
            new Regex($@"({YearRegex}[/\-\.]{MonthNumRegex})|({MonthNumRegex}[/\-]{YearRegex})",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);


        public static readonly Regex WeekOfMonthRegex =
            new Regex(
                $@"(?<wom>(l[ae]\s+)?(?<cardinal>premier|1er|seconde|2|troisi[eè]me|3|quatri[eè]me|4|cinqi[eè]me|5|derni[eè]re)\s+semaine\s+{
                    MonthSuffixRegex})", RegexOptions.IgnoreCase | RegexOptions.Singleline); // le/la - masc/fem 'the'

        public static readonly Regex WeekOfYearRegex =
            new Regex(
                $@"(?<woy>(l[ae]\s+)?(?<cardinal>premier|1er|seconde|2|troisi[eè]me|3|quatri[eè]me|4|cinqi[eè]me|5|derni[eè]re)\s+semaine\s+({
                    YearRegex}|(?<order>prochain|derni[eè]re|cette)\s+ann[eè]e))", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex FollowedDateUnit = new Regex($@"^\s*{DateUnitRegex}",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NumberCombinedWithDateUnit =
            new Regex($@"\b(?<num>\d+(\.\d*)?){DateUnitRegex}", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex QuarterRegex =
            new Regex(
                $@"(l[ae]\s+)?(?<cardinal>premier|1er|deuxi[eè]me|2|troisi[eè]me|3|quatri[eè]me|4)\s+trimestre(\s+de|\s*,\s*)?\s+({
                    YearRegex}|(cette\s+ann[eè])|(l'ann[eè]+?<order>prochain|derni[eè]re)\s)", // 1st quarter of this year, 2nd quarter of next/last year, etc 
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex QuarterRegexYearFront =
            new Regex(
                $@"({YearRegex
                    }|((cette\s+ann[eè]e)?(l'ann[eè]e\s+(?<order>derni[èe]re|prochaine)))\s+(l[ea]\s+)?(?<cardinal>premier|1er|deuxi[eè]me|2|troisi[eè]me|3|quatri[eè]me|4)\s+trimestre",
                RegexOptions.IgnoreCase | RegexOptions.Singleline); 

        public static readonly Regex SeasonRegex =
            new Regex(
                $@"\b(?<season>(ce|cet|l'|le\s+)?(?<seas>printemps|été|automne|hiver)((\s+d[ue]|\s*,\s*)?\s+({   // the summer of(du/de) last/next year
                    YearRegex}|(cette+ann[ée]e)|(l'ann[ée]e+?<order>prochain|derni[èe]re))?)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WhichWeekRegex =
            new Regex(
                $@"(semaine)(\s*)(?<number>\d\d|\d|0\d)", 
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekOfRegex =
            new Regex(
                $@"(semaine)(\s)(de|du|d')",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthOfRegex =
            new Regex(
                $@"(mois)(\s)(de|du)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //TODO: add this two regex
        public static readonly Regex RangeUnitRegex =
            new Regex(
                @"\b(?<unit>an|ann[ée]e|mois|mes|semaines|semaine)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex InConnectorRegex =
            new Regex(
                @"\b(dans)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex FromRegex = new Regex(@"((de|du)?)$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex ConnectorAndRegex = new Regex(@"(et\s*(le|la(s)?)?)$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex BeforeRegex = new Regex(@"(entre\s*(le|la(s)?)?)", RegexOptions.IgnoreCase | RegexOptions.Singleline);


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

        public FrenchDatePeriodExtractorConfiguration()
        {
            DatePointExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration());
            CardinalExtractor = new Number.French.CardinalExtractor.GetInstance();
            DurationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration());
        }

        public IExtractor DatePointExtractor { get; }

        public IExtractor CardinalExtractor { get; }

        public IExtractor DurationExtractor { get; }

        IEnumerable<Regex> IDatePeriodExtractorConfiguration.SimpleCasesRegexes => SimpleCasesRegexes;

        Regex IDatePeriodExtractorConfiguration.TillRegex => TillRegex;

        Regex IDatePeriodExtractorConfiguration.DateUnitRegex => DateUnitRegex;

        Regex IDatePeriodExtractorConfiguration.FollowedDateUnit => FollowedDateUnit;

        Regex IDatePeriodExtractorConfiguration.NumberCombinedWithDateUnit => NumberCombinedWithDateUnit;

        Regex IDatePeriodExtractorConfiguration.PastRegex => PastRegex;

        Regex IDatePeriodExtractorConfiguration.FutureRegex => FutureRegex;

        Regex IDatePeriodExtractorConfiguration.WeekOfRegex => WeekOfRegex;

        Regex IDatePeriodExtractorConfiguration.MonthOfRegex => MonthOfRegex;

        Regex IDatePeriodExtractorConfiguration.RangeUnitRegex => RangeUnitRegex;

        Regex IDatePeriodExtractorConfiguration.InConnectorRegex => InConnectorRegex;

        public bool GetFromTokenIndex(string text, out int index)
        {
            index = -1;
            var fromMatch = FromRegex.Match(text);
            if (fromMatch.Success)
            {
                index = fromMatch.Index;
            }
            return fromMatch.Success;
        }

        public bool GetBetweenTokenIndex(string text, out int index)
        {
            index = -1;
            var beforeMatch = BeforeRegex.Match(text);
            if (beforeMatch.Success)
            {
                index = beforeMatch.Index;
            }
            return beforeMatch.Success;
        }

        public bool HasConnectorToken(string text)
        {
            return ConnectorAndRegex.IsMatch(text);
        }
    }
}
