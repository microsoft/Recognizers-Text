using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Number.Spanish;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishDatePeriodExtractorConfiguration : IDatePeriodExtractorConfiguration
    {
        // base regexes
        public static readonly Regex TillRegex = new Regex(@"(?<till>hasta|al|a|--|-|—|——)(\s+(el|la(s)?))?",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AndRegex = new Regex(@"(?<and>y|y\s*el|--|-|—|——)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DayRegex =
            new Regex(
                @"(?<day>01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|1ro|1|20|21|22|23|24|25|26|27|28|29|2|30|31|3|4|5|6|7|8|9)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthNumRegex =
            new Regex(@"(?<month>01|02|03|04|05|06|07|08|09|10|11|12|1|2|3|4|5|6|7|8|9)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex YearRegex = new Regex(@"\b(?<year>19\d{2}|20\d{2})\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex RelativeMonthRegex = new Regex(@"(?<relmonth>(este|pr[oó]ximo|[uú]ltimo)\s+mes)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex EngMonthRegex =
            new Regex(
                @"(?<month>Abril|Abr|Agosto|Ago|Diciembre|Dic|Enero|Ene|Febrero|Feb|Julio|Jul|Junio|Jun|Marzo|Mar|Mayo|May|Noviembre|Nov|Octubre|Oct|Septiembre|Setiembre|Sept|Set)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthSuffixRegex =
            new Regex($@"(?<msuf>(en\s+|del\s+|de\s+)?({RelativeMonthRegex}|{EngMonthRegex}))",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex UnitRegex = new Regex(@"(?<unit>años|año|meses|mes|semanas|semana|d[ií]a(s)?)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PastRegex = new Regex(@"(?<past>\b(pasad(a|o)(s)?|[uú]ltim[oa](s)?|anterior(es)?|previo(s)?)\b)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex FutureRegex = new Regex(@"(?<past>\b(siguiente(s)?|pr[oó]xim[oa](s)?|dentro\s+de|en)\b)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //// composite regexes
        public static readonly Regex SimpleCasesRegex =
            new Regex(
                $@"\b((desde\s+el|desde|del)\s+)?({DayRegex})\s*{TillRegex}\s*({DayRegex})\s+{MonthSuffixRegex}((\s+|\s*,\s*){YearRegex})?\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthFrontSimpleCasesRegex =
            new Regex(
                $@"\b{MonthSuffixRegex}\s+((desde\s+el|desde|del)\s+)?({DayRegex})\s*{TillRegex}\s*({DayRegex})((\s+|\s*,\s*){YearRegex})?\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthFrontBetweenRegex =
            new Regex(
                string.Format(@"\b{2}\s+((entre|entre\s+el)\s+)({0})\s*{1}\s*({0})((\s+|\s*,\s*){3})?\b", DayRegex, AndRegex,
                    MonthSuffixRegex, YearRegex),
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex BetweenRegex =
            new Regex(
                string.Format(@"\b((entre|entre\s+el)\s+)({0})\s*{1}\s*({0})\s+{2}((\s+|\s*,\s*){3})?\b", DayRegex, AndRegex,
                    MonthSuffixRegex, YearRegex),
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex OneWordPeriodRegex =
            new Regex(
                @"\b(((pr[oó]xim[oa]?|est[ea]|[uú]ltim[oa]?|en)\s+)?(?<month>Abril|Abr|Agosto|Ago|Diciembre|Dic|Enero|Ene|Febrero|Feb|Julio|Jul|Junio|Jun|Marzo|Mar|Mayo|May|Noviembre|Nov|Octubre|Oct|Septiembre|Setiembre|Sept|Set)|(?<=\b(del|de la|el|la)\s+)?(pr[oó]xim[oa](s)?|[uú]ltim[oa]?|est(e|a))?\s+(fin de semana|semana|mes|año)|fin de semana|(mes|años)? a la fecha)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthWithYear =
            new Regex(
                $@"\b(((pr[oó]xim[oa](s)?|este|esta|[uú]ltim[oa]?|en)\s+)?(?<month>Abril|Abr|Agosto|Ago|Diciembre|Dic|Enero|Ene|Febrero|Feb|Julio|Jul|Junio|Jun|Marzo|Mar|Mayo|May|Noviembre|Nov|Octubre|Oct|Septiembre|Setiembre|Sept|Set)\s+((de|del|de la)\s+)?({
                    YearRegex}|(?<order>pr[oó]ximo(s)?|[uú]ltimo?|este)\s+año))\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MonthNumWithYear =
            new Regex($@"({YearRegex}[/\-\.]{MonthNumRegex})|({MonthNumRegex}[/\-]{YearRegex})",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);


        public static readonly Regex WeekOfMonthRegex =
            new Regex(
                $@"(?<wom>(la\s+)?(?<cardinal>primera?|1ra|segunda|2da|tercera?|3ra|cuarta|4ta|quinta|5ta|[uú]ltima)\s+semana\s+{
                    MonthSuffixRegex})", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex WeekOfYearRegex =
            new Regex(
                $@"(?<woy>(la\s+)?(?<cardinal>primera?|1ra|segunda|2da|tercera?|3ra|cuarta|4ta|quinta|5ta|[uú]ltima?)\s+semana(\s+del?)?\s+({
                    YearRegex}|(?<order>pr[oó]ximo|[uú]ltimo|este)\s+año))", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex FollowedUnit = new Regex($@"^\s*{UnitRegex}\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NumberCombinedWithUnit =
            new Regex($@"\b(?<num>\d+(\.\d*)?){UnitRegex}\b", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex QuarterRegex =
            new Regex(
                $@"(el\s+)?(?<cardinal>primer|1er|segundo|2do|tercer|3ro|cuarto|4to)\s+cuatrimestre(\s+de|\s*,\s*)?\s+({
                    YearRegex}|(?<order>pr[oó]ximo(s)?|[uú]ltimo?|este)\s+año)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex QuarterRegexYearFront =
            new Regex(
                $@"({YearRegex
                    }|(?<order>pr[oó]ximo(s)?|[uú]ltimo?|este)\s+año)\s+(el\s+)?(?<cardinal>(primer|primero)|1er|segundo|2do|(tercer|terceo)|3ro|cuarto|4to)\s+cuatrimestre",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SeasonRegex =
            new Regex(
                $@"\b(?<season>(([uú]ltim[oa]|est[ea]|el|la|(pr[oó]xim[oa]s?|siguiente))\s+)?(?<seas>primavera|verano|otoño|invierno)((\s+del?|\s*,\s*)?\s+({
                    YearRegex}|(?<order>pr[oó]ximo|[uú]ltimo|este)\s+año))?)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex fromRegex = new Regex(@"((desde|de)(\s*la(s)?)?)$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex andRegex = new Regex(@"(y\s*(la(s)?)?)$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex beforeRegex = new Regex(@"(entre\s*(la(s)?)?)", RegexOptions.IgnoreCase | RegexOptions.Singleline);

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

        public SpanishDatePeriodExtractorConfiguration()
        {
            DatePointExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration());
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
            var fromMatch = fromRegex.Match(text);
            if (fromMatch.Success)
            {
                index = fromMatch.Index;
            }
            return fromMatch.Success;
        }

        public bool GetBetweenTokenIndex(string text, out int index)
        {
            index = -1;
            var beforeMatch = beforeRegex.Match(text);
            if (beforeMatch.Success)
            {
                index = beforeMatch.Index;
            }
            return beforeMatch.Success;
        }

        public bool HasConnectorToken(string text)
        {
            return andRegex.IsMatch(text);
        }
    }
}
