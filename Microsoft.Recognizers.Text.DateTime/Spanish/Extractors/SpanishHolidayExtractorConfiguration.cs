using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishHolidayExtractorConfiguration : IHolidayExtractorConfiguration
    {
        public static readonly Regex YearRegex = new Regex(@"\b(?<year>19\d{2}|20\d{2})\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex H1 =
            new Regex(
                $@"\b(?<holiday>viernes santo|mi[eé]rcoles de ceniza|martes de carnaval|d[ií]a (de|de los) presidentes?|clebraci[oó]n de mao|año nuevo chino|año nuevo|(festividad de )?los mayos|d[ií]a de los inocentes|navidad|d[ií]a de acci[oó]n de gracias|halloween|noches de brujas)(\s+(del?\s+)?({
                    YearRegex}|(?<order>(pr[oó]xim[oa]?|est[ea]|[uú]ltim[oa]?|en))\s+año))?\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex H2 =
            new Regex(
                $@"\b(?<holiday>(d[ií]a( del?( la)?)? )?(martin luther king|todos los santos|blanco|san patricio|san valent[ií]n|san jorge|cinco de mayo|independencia|raza|trabajador))(\s+(del?\s+)?({
                    YearRegex}|(?<order>(pr[oó]xim[oa]?|est[ea]|[uú]ltim[oa]?|en))\s+año))?\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex H3 =
            new Regex(
                $@"\b(?<holiday>(d[ií]a( del?( las?)?)? )(trabajador|madres?|padres?|[aá]rbol|mujer(es)?|solteros?|niños?|marmota))(\s+(del?\s+)?({
                    YearRegex}|(?<order>(pr[oó]xim[oa]?|est[ea]|[uú]ltim[oa]?|en))\s+año))?\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex[] HolidayRegexList =
        {
            H1,
            H2,
            H3
        };

        public IEnumerable<Regex> HolidayRegexes => HolidayRegexList;
    }
}
