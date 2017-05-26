using Microsoft.Recognizers.Text.DateTime.Parsers;
using Microsoft.Recognizers.Text.DateTime.Spanish.Extractors;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Parsers
{
    public class SpanishHolidayParserConfiguration : BaseHolidayParserConfiguration
    {
        public SpanishHolidayParserConfiguration() : base()
        {
            this.HolidayRegexList = SpanishHolidayExtractorConfiguration.HolidayRegexList;
            this.HolidayNames = InitHolidayNames().ToImmutableDictionary();
        }
        
        private IDictionary<string, IEnumerable<string>> InitHolidayNames()
        {
            return new Dictionary<string, IEnumerable<string>>
            {
                { "fathers", new string[]{ "diadelpadre"} },
                { "mothers", new string[]{ "diadelamadre" } },
                { "thanksgiving", new string[]{ "diadegracias", "diadeacciondegracias" } },
                { "martinlutherking", new string[]{ "diademartinlutherking" } },
                { "washingtonsbirthday", new string[]{ "diadelpresidente" } },
                { "labour", new string[]{ "diadeltrabajadorusa" } },
                { "columbus", new string[]{ "diadelaraza", "diadeladiversidadcultural" } },
                { "memorial", new string[]{ "diadelamemoria" } },
            };
        }

        public override int GetSwiftYear(string text)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            var swift = -10;
            if (trimedText.StartsWith("proximo") || trimedText.StartsWith("próximo") ||
                trimedText.StartsWith("proxima") || trimedText.StartsWith("próxima"))
            {
                swift = 1;
            }
            if (trimedText.StartsWith("ultimo") || trimedText.StartsWith("último") ||
                trimedText.StartsWith("ultima") || trimedText.StartsWith("última"))
            {
                swift = -1;
            }
            else if (trimedText.StartsWith("este") || trimedText.StartsWith("esta"))
            {
                swift = 0;
            }
            return swift;
        }

        public override string SanitizeHolidayToken(string holiday)
        {
            return holiday
                .Replace(" ", "")
                .Replace("á", "a")
                .Replace("é", "e")
                .Replace("í", "i")
                .Replace("ó", "o")
                .Replace("ú", "u");
        }
    }
}
