using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Extractors;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Extractors
{
    public class SpanishHolidayExtractorConfiguration : IHolidayExtractorConfiguration
    {
        public static readonly Regex YearRegex = new Regex(@"\b(?<year>19\d{2}|20\d{2})\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex h1 =
            new Regex(
                $@"\b(?<holiday>viernes santo|mi[eé]rcoles de ceniza|martes de carnaval|d[ií]a (de|de los) presidentes?|clebraci[oó]n de mao|año nuevo chino|año nuevo|(festividad de )?los mayos|d[ií]a de los inocentes|navidad|d[ií]a de acci[oó]n de gracias|halloween|noches de brujas)(\s+(del?\s+)?({
                    YearRegex}|(?<order>(pr[oó]xim[oa]?|est[ea]|[uú]ltim[oa]?|en))\s+año))?\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex h2 =
            new Regex(
                $@"\b(?<holiday>(d[ií]a( del?( la)?)? )?(martin luther king|todos los santos|blanco|san patricio|san valent[ií]n|san jorge|cinco de mayo|independencia|raza|trabajador))(\s+(del?\s+)?({
                    YearRegex}|(?<order>(pr[oó]xim[oa]?|est[ea]|[uú]ltim[oa]?|en))\s+año))?\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex h3 =
            new Regex(
                $@"\b(?<holiday>(d[ií]a( del?( las?)?)? )(trabajador|madres?|padres?|[aá]rbol|mujer(es)?|solteros?|niños?|marmota))(\s+(del?\s+)?({
                    YearRegex}|(?<order>(pr[oó]xim[oa]?|est[ea]|[uú]ltim[oa]?|en))\s+año))?\b",
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
