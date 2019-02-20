using System.Collections.Immutable;
using Microsoft.Recognizers.Definitions.German;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    public class GermanHolidayParserConfiguration : BaseHolidayParserConfiguration
    {
        public GermanHolidayParserConfiguration(IOptionsConfiguration config)
            : base(config)
        {
            this.HolidayRegexList = GermanHolidayExtractorConfiguration.HolidayRegexList;
            this.HolidayNames = DateTimeDefinitions.HolidayNames.ToImmutableDictionary();
        }

        public override int GetSwiftYear(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            var swift = -10;
            if (trimmedText.StartsWith("nächster") || trimmedText.StartsWith("nächstes") || trimmedText.StartsWith("nächsten") || trimmedText.StartsWith("nächste"))
            {
                swift = 1;
            }
            else if (trimmedText.StartsWith("letzter") || trimmedText.StartsWith("letztes") || trimmedText.StartsWith("letzten") || trimmedText.StartsWith("letzte"))
            {
                swift = -1;
            }
            else if (trimmedText.StartsWith("dieser") || trimmedText.StartsWith("dieses") || trimmedText.StartsWith("diesen") || trimmedText.StartsWith("diese"))
            {
                swift = 0;
            }

            return swift;
        }

        public override string SanitizeHolidayToken(string holiday)
        {
            return holiday
                .Replace(" ", string.Empty)
                .Replace("'", string.Empty);
        }
    }
}
